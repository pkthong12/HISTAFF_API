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
        private readonly FullDbContext _dbFullContext;
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
            IFileService fileService,
            FullDbContext fullContext
            )
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_FAMILY, HuFamilyDTO>();
            _genericReducer = new();
            _env = env;
            _appSettings = options.Value;
            _fileService = fileService;
            _dbFullContext = fullContext;
        }

        public async Task<GenericPhaseTwoListResponse<HuFamilyDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuFamilyDTO> request)
        {
            var joined = (from p in _dbFullContext.HuFamilys.AsNoTracking()
                                from e in _dbFullContext.HuEmployees.AsNoTracking().Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                                from t in _dbFullContext.HuPositions.AsNoTracking().Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                from o in _dbFullContext.HuOrganizations.AsNoTracking().Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                from j in _dbFullContext.HuJobs.AsNoTracking().Where(x => x.ID == t.JOB_ID).DefaultIfEmpty()
                                from r in _dbFullContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.RELATIONSHIP_ID).DefaultIfEmpty()
                                from g in _dbFullContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.GENDER).DefaultIfEmpty()
                                from n in _dbFullContext.HuNations.AsNoTracking().Where(x => x.ID == p.NATIONALITY).DefaultIfEmpty()
                                from pr in _dbFullContext.HuProvinces.AsNoTracking().Where(x => x.ID == p.BIRTH_CER_PROVINCE).DefaultIfEmpty()
                                from d in _dbFullContext.HuDistricts.AsNoTracking().Where(x => x.ID == p.BIRTH_CER_DISTRICT).DefaultIfEmpty()
                                from w in _dbFullContext.HuWards.AsNoTracking().Where(x => x.ID == p.BIRTH_CER_WARD).DefaultIfEmpty()
                                from s in _dbFullContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.STATUS_ID).DefaultIfEmpty()
                                orderby p.BIRTH_DATE ascending
                                select new HuFamilyDTO
                                {
                                    Id = p.ID,
                                    EmployeeName = e.Profile!.FULL_NAME,
                                    EmployeeCode = e.CODE,
                                    PositionName = t.NAME,
                                    OrgId = e.ORG_ID,
                                    OrgName = o.NAME,
                                    RelationshipId = p.RELATIONSHIP_ID,
                                    RelationshipName = r.NAME,
                                    Fullname = p.FULLNAME,
                                    Gender = p.GENDER,
                                    GenderName = g.NAME,
                                    BirthDate = p.BIRTH_DATE,
                                    PitCode = p.PIT_CODE,
                                    SameCompany = p.SAME_COMPANY,
                                    IsDead = p.IS_DEAD,
                                    IsDeduct = p.IS_DEDUCT,
                                    DeductFrom = p.DEDUCT_FROM,
                                    DeductTo = p.DEDUCT_TO,
                                    RegistDeductDate = p.REGIST_DEDUCT_DATE,
                                    IsHousehold = p.IS_HOUSEHOLD,
                                    IdNo = p.ID_NO,
                                    Career = p.CAREER,
                                    Nationality = p.NATIONALITY,
                                    NationalityName = n.NAME,
                                    BirthCerPlace = p.BIRTH_CER_PLACE,
                                    BirthCerProvince = p.BIRTH_CER_PROVINCE,
                                    BirthCerProvinceName = pr.NAME,
                                    BirthCerDistrict = p.BIRTH_CER_DISTRICT,
                                    BirthCerDistrictName = d.NAME,
                                    BirthCerWard = p.BIRTH_CER_WARD,
                                    BirthCerWardName = w.NAME,
                                    UploadFile = p.UPLOAD_FILE,
                                    StatusId = e.WORK_STATUS_ID,
                                    StatusName = s.NAME,
                                    CreatedBy = p.CREATED_BY,
                                    UpdatedBy = p.UPDATED_BY,
                                    CreatedDate = p.CREATED_DATE,
                                    UpdatedDate = p.UPDATED_DATE,
                                    Note = p.NOTE,
                                    JobOrderNum = (int)(j.ORDERNUM ?? 99)
                                });
            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
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

