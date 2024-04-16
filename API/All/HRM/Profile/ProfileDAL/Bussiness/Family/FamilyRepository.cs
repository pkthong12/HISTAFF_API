using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.Services.File;
using Microsoft.Extensions.Options;

namespace API.Controllers.Family
{
    public class FamilyRepository : IFamilyRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly CoreDbContext _dbContext;
        private IGenericRepository<HU_FAMILY, HuFamilyDTO> _genericRepository;
        private readonly GenericReducer<HU_FAMILY, HuFamilyDTO> _genericReducer;
        private readonly IWebHostEnvironment _env;
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;

        public FamilyRepository(
            CoreDbContext context, 
            GenericUnitOfWork uow,
            IWebHostEnvironment env, 
            IOptions<AppSettings> options,
            IFileService fileService
            )
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_FAMILY, HuFamilyDTO>();
            _genericReducer = new();
            _env = env;
            _appSettings = options.Value;
            _fileService = fileService;
        }
        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuFamilyDTO dto, string sid)
        {

            _uow.CreateTransaction();
            List<UploadFileResponse> uploadFiles = new();
            try
            {
                // First of all we need to upload all the attachments
                if (dto.UploadFileBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest() { 
                        ClientFileName = dto.UploadFileBuffer.ClientFileName,
                        ClientFileType = dto.UploadFileBuffer.ClientFileType,
                        ClientFileData = dto.UploadFileBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(HuFamilyDTO).GetProperty("UploadFile");

                    if (property != null)
                    {
                        property?.SetValue(dto, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    } else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": UploadFile" };
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
        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuFamilyDTO dto, string sid, bool patchMode = true)
        {
            _uow.CreateTransaction();
            List<UploadFileResponse> uploadFiles = new();
            try
            {
                // First of all we need to upload all the attachments
                if (dto.UploadFileBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = dto.UploadFileBuffer.ClientFileName,
                        ClientFileType = dto.UploadFileBuffer.ClientFileType,
                        ClientFileData = dto.UploadFileBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(HuFamilyDTO).GetProperty("UploadFile");

                    if (property != null)
                    {
                        property?.SetValue(dto, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": UploadFile" };
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

        public async Task<FormatedResponse> ReadAll()
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> ReadAllByKey(string key, long value)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> ReadAllByKey(string key, string value)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> GetById(long id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuFamilyDTO> dtos, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuFamilyDTO> dtos, string sid, bool patchMode = true)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, long id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<long> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

    }
}

