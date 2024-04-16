using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.Services.File;
using Microsoft.Extensions.Options;

namespace API.Controllers.TrCenter
{
    public class TrCenterRepository : ITrCenterRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<TR_CENTER, TrCenterDTO> _genericRepository;
        private readonly GenericReducer<TR_CENTER, TrCenterDTO> _genericReducer;
        private readonly IWebHostEnvironment _env;
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;
        public TrCenterRepository(FullDbContext context, GenericUnitOfWork uow,
            IWebHostEnvironment env,
            IOptions<AppSettings> options,
            IFileService fileService)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<TR_CENTER, TrCenterDTO>();
            _genericReducer = new();
            _env = env;
            _appSettings = options.Value;
            _fileService = fileService;
        }

        public async Task<GenericPhaseTwoListResponse<TrCenterDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrCenterDTO> request)
        {
            var joined = from p in _dbContext.TrCenters.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from c in _dbContext.SysUsers.AsNoTracking().Where(c => p.CREATED_BY == null ? false : c.ID == p.CREATED_BY).DefaultIfEmpty()
                         from u in _dbContext.SysUsers.AsNoTracking().Where(u => p.UPDATED_BY == null ? false : u.ID == p.UPDATED_BY).DefaultIfEmpty()
                         select new TrCenterDTO
                         {
                             Id = p.ID,
                             CodeCenter = p.CODE_CENTER,
                             NameCenter = p.NAME_CENTER,
                             TrainingField = p.TRAINING_FIELD,
                             Address = p.ADDRESS,
                             Phone = p.PHONE,
                             Representative = p.REPRESENTATIVE,
                             ContactPerson = p.CONTACT_PERSON,
                             PhoneContactPerson = p.PHONE_CONTACT_PERSON,
                             Website = p.WEBSITE,
                             Note = p.NOTE,
                             AttachedFile = p.ATTACHED_FILE,
                             Status = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
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
            var joined = (from l in _dbContext.TrCenters.AsNoTracking().Where(l => l.ID == id)
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                          select new TrCenterDTO
                          {
                              Id = l.ID,
                              CodeCenter = l.CODE_CENTER,
                              NameCenter = l.NAME_CENTER,
                              TrainingField = l.TRAINING_FIELD,
                              Address = l.ADDRESS,
                              Phone = l.PHONE,
                              Representative = l.REPRESENTATIVE,
                              ContactPerson = l.CONTACT_PERSON,
                              PhoneContactPerson = l.PHONE_CONTACT_PERSON,
                              Website = l.WEBSITE,
                              Note = l.NOTE,
                              AttachedFile = l.ATTACHED_FILE,
                          }).FirstOrDefault();


            if (joined != null)
            {
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, TrCenterDTO dto, string sid)
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
                    var property = typeof(TrCenterDTO).GetProperty("AttachedFile");

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

                dto.IsActive = true;
                string newcode = "";
                if (await _dbContext.TrCenters.CountAsync() == 0)
                {
                    newcode = "ĐVDT001";
                }
                else
                {
                    string lastestData = _dbContext.TrCenters.OrderByDescending(x => x.CODE_CENTER).First().CODE_CENTER!.ToString();
                    newcode = lastestData.Substring(0, 4) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
                }
                //var stt = await _dbContext.TrCenters.OrderByDescending(x => x.ID).FirstAsync();
                //dto.CodeCenter = "TT" + (stt.ID + 1).ToString("0##");
                dto.CodeCenter = newcode;
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
            //var stt = await _dbContext.TrCenters.OrderByDescending(x => x.ID).FirstAsync();
            //dto.CodeCenter = "TT" + (stt.ID + 1).ToString("0##");
            //var response = await _genericRepository.Create(_uow, dto, sid);
            //return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<TrCenterDTO> dtos, string sid)
        {
            var add = new List<TrCenterDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, TrCenterDTO dto, string sid, bool patchMode = true)
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
                    var property = typeof(TrCenterDTO).GetProperty("AttachedFile");

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
            //    var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            //return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<TrCenterDTO> dtos, string sid, bool patchMode = true)
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
            foreach (var item in ids)
            {
                var a = _dbContext.TrPlans.Where(x => x.CENTER_ID == item).Any();
                if (a)
                {
                    return new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = "CAN_NOT_DELETE_RECORD_IS_USING",
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
            }

            var checkAc = _dbContext.TrCenters.Where(x => ids.Contains(x.ID)).AsNoTracking().DefaultIfEmpty();
            foreach (var item in checkAc)
            {
                if (item?.IS_ACTIVE == true)
                {
                    return new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
            }
            var response = await _genericRepository.DeleteIds(_uow, ids);
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> CreateNewCode()
        {
            string newCode = "";
            if (await _dbContext.TrCenters.CountAsync() == 0)
            {
                newCode = "TT001";
            }
            else
            {
                string lastestData = _dbContext.TrCenters.OrderByDescending(t => t.CODE_CENTER).First().CODE_CENTER!.ToString();

                newCode = lastestData.Substring(0, 2) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
            }

            return new FormatedResponse() { InnerBody = new { Code = newCode } };

        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
           var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }
    }
}

