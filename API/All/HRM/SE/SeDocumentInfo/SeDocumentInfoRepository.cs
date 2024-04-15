using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using API.Controllers.SeDocumentInfo;
using Azure;
using CORE.Services.File;
using Microsoft.Extensions.Options;

namespace API.Controllers.SeDocument
{
    public class SeDocumentInfoRepository : ISeDocumentInfoRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<SE_DOCUMENT_INFO, SeDocumentInfoDTO> _genericRepository;
        private readonly GenericReducer<SE_DOCUMENT_INFO, SeDocumentInfoDTO> _genericReducer;
        private readonly IWebHostEnvironment _env;
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;

        public SeDocumentInfoRepository(FullDbContext context, GenericUnitOfWork uow, IWebHostEnvironment env, IFileService fileService, IOptions<AppSettings> options)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<SE_DOCUMENT_INFO, SeDocumentInfoDTO>();
            _genericReducer = new();
            _env = env;
            _appSettings = options.Value;
            _fileService = fileService;
        }

        public async Task<GenericPhaseTwoListResponse<SeDocumentInfoDTO>> SinglePhaseQueryList(GenericQueryListDTO<SeDocumentInfoDTO> request)
        {
            var joined = from p in _dbContext.SeDocuments.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from d in _dbContext.SeDocumentInfos.AsNoTracking().Where(x => x.DOCUMENT_ID == p.ID && x.EMP_ID == request.Filter!.EmpId).DefaultIfEmpty()
                         from t in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.DOCUMENT_TYPE && x.IS_ACTIVE == true).DefaultIfEmpty()
                         where p.IS_ACTIVE == true
                         select new SeDocumentInfoDTO
                         {
                             Id = p.ID,
                             DocumentId = p.ID,
                             EmpId = request.Filter!.EmpId,
                             DocumentName = p.NAME,
                             DocumentType = t.NAME,
                             IsObligatory = p.IS_OBLIGATORY,
                             IsPermissveUpload = p.IS_PERMISSVE_UPLOAD,
                             SubDate = d.SUB_DATE,
                             IsSubmit = d.IS_SUBMIT,
                             Note = d.NOTE,
                             AttachedFile = d.ATTACHED_FILE
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, long value)
        {
            var resposne = await _genericRepository.ReadAllByKey(key, value);
            return new() { InnerBody = resposne };
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, string value)
        {
            var resposne = await _genericRepository.ReadAllByKey(key, value);
            return new() { InnerBody = resposne };
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var res = await _genericRepository.GetById(id);
            return new() { InnerBody = res };
        } 
        public async Task<FormatedResponse> GetIdEmp(long id, long empId)
        {
            var check = _dbContext.SeDocumentInfos.AsNoTracking().Where(p => p.DOCUMENT_ID == id && p.EMP_ID == empId).FirstOrDefault();
            var _check = _dbContext.SeDocuments.AsNoTracking().Where(x => x.ID == id).SingleOrDefault();
            if(check == null)
            {
                return new() { InnerBody = new { DocumentId = id, EmpId = empId, IsPermissveUpload = _check!.IS_PERMISSVE_UPLOAD } };
            }
            var res = (from s in _dbContext.SeDocumentInfos.AsNoTracking().Where(p => p.DOCUMENT_ID == id && p.EMP_ID == empId).DefaultIfEmpty()
                             from d in _dbContext.SeDocuments.AsNoTracking().Where(x=>x.ID == id).DefaultIfEmpty()
                             select new 
                             {
                                 Id = s.ID,
                                 EmpId = empId,
                                 DocumentId = id,
                                 IsPermissveUpload =  d.IS_PERMISSVE_UPLOAD,
                                 SubDate = s.SUB_DATE,
                                 Note = s.NOTE,
                                 IsSubmit = s.IS_SUBMIT,
                                 //AttachedFileBuffer = s.ATTACHED_FILE,
                                 AttachedFile = s.ATTACHED_FILE,
                             }).FirstOrDefault();
            if (res != null)
            {
                return new FormatedResponse() { InnerBody = res };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, SeDocumentInfoDTO dto, string sid)
        {
            _uow.CreateTransaction();
            List<UploadFileResponse> uploadFiles = new();
            try
            {
                if (dto.AttachedFileBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = dto.AttachedFileBuffer.ClientFileName,
                        ClientFileType = dto.AttachedFileBuffer.ClientFileType,
                        ClientFileData = dto.AttachedFileBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(SeDocumentInfoDTO).GetProperty("AttachedFile");

                    if (property != null)
                    {
                        property?.SetValue(dto, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": AttachmentFile" };
                    }
                    dto.IsSubmit = true;
                }
                
                var response = await _genericRepository.Create(_uow, dto, sid);
                _uow.Commit();
                return response;
            }
            catch (Exception ex)
            {
                uploadFiles.ForEach(async f =>
                {
                    await _fileService.DeleteAttachment(f.SavedAs);
                });

                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
            //var response = await _genericRepository.Create(_uow, dto, sid);
            //return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<SeDocumentInfoDTO> dtos, string sid)
        {
            var add = new List<SeDocumentInfoDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, SeDocumentInfoDTO dto, string sid, bool patchMode = true)
        {
            List<UploadFileResponse> uploadFiles = new();
            try
            {
                if (dto.AttachedFileBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = dto.AttachedFileBuffer.ClientFileName,
                        ClientFileType = dto.AttachedFileBuffer.ClientFileType,
                        ClientFileData = dto.AttachedFileBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(SeDocumentInfoDTO).GetProperty("AttachedFile");

                    if (property != null)
                    {
                        property?.SetValue(dto, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": AttachmentFile" };
                    }
                }
                var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
                _uow.Commit();
                return response;
            }
            catch (Exception ex)
            {
                uploadFiles.ForEach(async f =>
                {
                    await _fileService.DeleteAttachment(f.SavedAs);
                });

                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
            //var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            //return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<SeDocumentInfoDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(_uow, dtos, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, long id)
        {
            var response = await _genericRepository.Delete(_uow, id);
            return response;
        }

        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<long> ids)
        {
            var response = await _genericRepository.DeleteIds(_uow, ids);
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }

        public async Task<FormatedResponse> GetByIdEmp(long id)
        {
            var joined = (from e in _dbContext.HuEmployees.AsNoTracking().Where(x => x.ID == id).DefaultIfEmpty()
                          from o in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                          from s in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                          select new
                          {
                              Id = e.ID,
                              CodeEmp = e.CODE,
                              NameEmp = e.Profile!.FULL_NAME,
                              OrgName = o.NAME,
                              PosName = s.NAME,
                          }).FirstOrDefault();
            return new FormatedResponse { InnerBody = joined };
        }

        public async Task<FormatedResponse> GetListDocument()
        {
            var query = (from p in _dbContext.SeDocuments.AsNoTracking()
                               from i in _dbContext.SeDocumentInfos.AsNoTracking().Where(x => x.DOCUMENT_ID == p.ID || x.DOCUMENT_ID == null).DefaultIfEmpty()
                               from o in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.DOCUMENT_TYPE).DefaultIfEmpty()
                               //from e in _dbContext.HuEmployees.AsNoTracking().Where(x => x.ID == i.EMP_ID)
                               
                               select new
                               {
                                   Id = p.ID,
                                   Code = p.CODE,
                                   Name = p.NAME,
                                   DocumentType = p.DOCUMENT_TYPE,
                                   DocumentTypeName = o.NAME,
                                   IsObligatory = p.IS_OBLIGATORY,
                                   IsPermissveUpload = p.IS_PERMISSVE_UPLOAD,
                                   SubDate = i.SUB_DATE,
                                   IsSubmit = i.IS_SUBMIT,
                                   Note = i.NOTE,
                                   AttachedFile = i.ATTACHED_FILE
                               }).ToList();
            return new FormatedResponse { InnerBody = query };
        }

    }
}


