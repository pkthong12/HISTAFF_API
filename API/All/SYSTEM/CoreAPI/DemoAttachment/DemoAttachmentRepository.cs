using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.Services.File;
using Microsoft.Extensions.Options;

namespace API.Controllers.DemoAttachment
{
    public class DemoAttachmentRepository : IDemoAttachmentRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<DEMO_ATTACHMENT, DemoAttachmentDTO> _genericRepository;
        private readonly GenericReducer<DEMO_ATTACHMENT, DemoAttachmentDTO> _genericReducer;
        private readonly IWebHostEnvironment _env;
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;

        public DemoAttachmentRepository(
            FullDbContext context, 
            GenericUnitOfWork uow,
            IWebHostEnvironment env, 
            IOptions<AppSettings> options,
            IFileService fileService
            )
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<DEMO_ATTACHMENT, DemoAttachmentDTO>();
            _genericReducer = new();
            _env = env;
            _appSettings = options.Value;
            _fileService = fileService;
        }

        public async Task<GenericPhaseTwoListResponse<DemoAttachmentDTO>> SinglePhaseQueryList(GenericQueryListDTO<DemoAttachmentDTO> request)
        {
            var joined = from p in _dbContext.DemoAttachments.AsNoTracking()
                         from o in _dbContext.SysOtherLists.AsNoTracking().Where(o => p.STATUS_ID == o.ID).DefaultIfEmpty()
                         from c in _dbContext.SysUsers.AsNoTracking().Where(c => p.CREATED_BY == c.ID).DefaultIfEmpty()
                         select new DemoAttachmentDTO
                         {
                             Id = p.ID,
                             Name = p.NAME,
                             FirstAttachment = p.FIRST_ATTACHMENT,
                             SecondAttachment = p.SECOND_ATTACHMENT,
                             StatusId = p.STATUS_ID,
                             EffectDate = p.EFFECT_DATE,
                             CreatedDate = p.CREATED_DATE,
                             CreatedBy = p.CREATED_BY,
                             CreatedByUsername = c.FULLNAME ?? "",
                             //UpdatedBy = p.CREATED_BY,
                             //UpdatedByUsername = c.FULLNAME ?? "",

                             StatusName = o.NAME
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
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<DEMO_ATTACHMENT>
                    {
                        (DEMO_ATTACHMENT)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new DemoAttachmentDTO
                              {
                                  Id = l.ID,
                                  Name = l.NAME,
                                  FirstAttachment = l.FIRST_ATTACHMENT,
                                  SecondAttachment = l.SECOND_ATTACHMENT,
                                  EffectDate = l.EFFECT_DATE

                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, DemoAttachmentDTO dto, string sid)
        {

            _uow.CreateTransaction();
            List<UploadFileResponse> uploadFiles = new();
            try
            {
                // First of all we need to upload all the attachments
                if (dto.FirstAttachmentBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest() { 
                        ClientFileName = dto.FirstAttachmentBuffer.ClientFileName,
                        ClientFileType = dto.FirstAttachmentBuffer.ClientFileType,
                        ClientFileData = dto.FirstAttachmentBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(DemoAttachmentDTO).GetProperty("FirstAttachment");

                    if (property != null)
                    {
                        property?.SetValue(dto, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    } else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment" };
                    }

                }
                if (dto.SecondAttachmentBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = dto.SecondAttachmentBuffer.ClientFileName,
                        ClientFileType = dto.SecondAttachmentBuffer.ClientFileType,
                        ClientFileData = dto.SecondAttachmentBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(DemoAttachmentDTO).GetProperty("SecondAttachment");

                    if (property != null)
                    {
                        property?.SetValue(dto, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": SecondAttachment" };
                    }
                }

                var response = await _genericRepository.Create(_uow, dto, sid);
                _uow.Commit();
                return response;

            }
            catch (Exception ex)
            {
                // Try to delete uploaded files
                uploadFiles.ForEach(async f =>
                {
                    await _fileService.DeleteAttachment(f.SavedAs);
                });

                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }

        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<DemoAttachmentDTO> dtos, string sid)
        {
            var add = new List<DemoAttachmentDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, DemoAttachmentDTO dto, string sid, bool patchMode = true)
        {
            _uow.CreateTransaction();
            List<UploadFileResponse> uploadFiles = new();
            try
            {
                // First of all we need to upload all the attachments
                if (dto.FirstAttachmentBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = dto.FirstAttachmentBuffer.ClientFileName,
                        ClientFileType = dto.FirstAttachmentBuffer.ClientFileType,
                        ClientFileData = dto.FirstAttachmentBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(DemoAttachmentDTO).GetProperty("FirstAttachment");

                    if (property != null)
                    {
                        property?.SetValue(dto, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment" };
                    }

                }
                if (dto.SecondAttachmentBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = dto.SecondAttachmentBuffer.ClientFileName,
                        ClientFileType = dto.SecondAttachmentBuffer.ClientFileType,
                        ClientFileData = dto.SecondAttachmentBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(DemoAttachmentDTO).GetProperty("SecondAttachment");

                    if (property != null)
                    {
                        property?.SetValue(dto, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": SecondAttachment" };
                    }
                }

                var response = await _genericRepository.Update(_uow, dto, sid);
                _uow.Commit();
                return response;

            }
            catch (Exception ex)
            {
                // Try to delete uploaded files
                uploadFiles.ForEach(async f =>
                {
                    await _fileService.DeleteAttachment(f.SavedAs);
                });

                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<DemoAttachmentDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetAttachmentStatusList()
        {
            var list = await (from o in _dbContext.SysOtherLists.AsNoTracking()
                       from t in _dbContext.SysOtherListTypes.AsNoTracking().Where(t => o.TYPE_ID == t.ID).DefaultIfEmpty()
                       where t.CODE == "ATTACHMENT_STATUS"
                       orderby o.ORDERS
                       select new SysOtherListDTO()
                       {
                           Id = o.ID,
                           Name = o.NAME ?? string.Empty
                       }).ToListAsync();
            return new() { InnerBody = list };
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

    }
}

