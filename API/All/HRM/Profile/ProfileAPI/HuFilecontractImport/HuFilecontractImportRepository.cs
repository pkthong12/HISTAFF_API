using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using ProfileDAL.ViewModels;
using API.All.SYSTEM.CoreAPI.Xlsx;
using Common.Extensions;

namespace API.Controllers.HuFilecontractImport
{
    public class HuFilecontractImportRepository : IHuFilecontractImportRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_FILECONTRACT_IMPORT, HuFilecontractImportDTO> _genericRepository;
        private readonly GenericReducer<HU_FILECONTRACT_IMPORT, HuFilecontractImportDTO> _genericReducer;
        private readonly List<string> XLSX_COLUMNS;

        public HuFilecontractImportRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_FILECONTRACT_IMPORT, HuFilecontractImportDTO>();
            _genericReducer = new();
            XLSX_COLUMNS = new() { "XLSX_USER_ID", "XLSX_EX_CODE", "XLSX_INSERT_ON", "XLSX_SESSION", "XLSX_FILE_NAME", "XLSX_ROW" };
        }

        public async Task<GenericPhaseTwoListResponse<HuFilecontractImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuFilecontractImportDTO> request)
        {
            var joined = from p in _dbContext.HuFilecontractImports.AsNoTracking()
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from ct in _dbContext.HuContracts.AsNoTracking().Where(e => e.ID == p.ID_CONTRACT).DefaultIfEmpty()
                         from cv1 in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => e.PROFILE_ID == cv.ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == e.ORG_ID).DefaultIfEmpty()
                         from t in _dbContext.HuPositions.Where(f => e.POSITION_ID == f.ID).DefaultIfEmpty()
                         from s in _dbContext.HuEmployees.AsNoTracking().Where(s => s.ID == p.SIGN_ID).DefaultIfEmpty()
                         from cv2 in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv2 => s.PROFILE_ID == cv2.ID).DefaultIfEmpty()
                         from t2 in _dbContext.HuPositions.Where(f => s.POSITION_ID == f.ID).DefaultIfEmpty()
                         from f in _dbContext.SysOtherLists.Where(c => c.ID == p.STATUS_ID).DefaultIfEmpty()
                         from l in _dbContext.SysContractTypes.Where(c => c.ID == p.APPEND_TYPEID).DefaultIfEmpty()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new HuFilecontractImportDTO
                         {
                             Id = p.XLSX_ROW,
                             XlsxUserId = p.XLSX_USER_ID,
                             XlsxExCode = p.XLSX_EX_CODE,
                             XlsxSession = p.XLSX_SESSION,
                             XlsxInsertOn = p.XLSX_INSERT_ON,
                             XlsxFileName = p.XLSX_FILE_NAME,
                             XlsxRow = p.XLSX_ROW,

                             EmployeeId = p.EMPLOYEE_ID,
                             EmployeeCode = e.CODE,
                             EmployeeName = cv1.FULL_NAME,
                             OrgName = o.NAME,
                             PositionName = t.NAME,
                             OrgId = o.ID,
                             ContractAppendixNo = p.CONTRACT_NO,
                             ContractTypeName = l.NAME,

                             StartDate = p.START_DATE,
                             ExpireDate = p.EXPIRE_DATE,
                             ContractNo = ct.CONTRACT_NO,
                             SignerName = cv2.FULL_NAME,
                             SignerPosition = t2.NAME,
                             SignDate = p.SIGN_DATE,
                             StatusName = f.NAME,
                             StatusId = f.ID,
                             Note = p.NOTE,
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
                var list = new List<HU_FILECONTRACT_IMPORT>
                    {
                        (HU_FILECONTRACT_IMPORT)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new HuFilecontractImportDTO
                              {
                                  Id = l.ID
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuFilecontractImportDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuFilecontractImportDTO> dtos, string sid)
        {
            var add = new List<HuFilecontractImportDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuFilecontractImportDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuFilecontractImportDTO> dtos, string sid, bool patchMode = true)
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

        public Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> Save(ImportQueryListBaseDTO request)
        {
            _uow.CreateTransaction();
            try
            {

                var now = DateTime.UtcNow;

                var tmp1 = await _dbContext.HuFilecontractImports.Where(x => x.XLSX_USER_ID == request.XlsxSid && x.XLSX_EX_CODE == request.XlsxExCode && x.XLSX_SESSION == request.XlsxSession).ToListAsync();
                var res = new HU_FILECONTRACT();
                var tmp1Type = typeof(HU_FILECONTRACT_IMPORT);
                var tmp1Properties = tmp1Type.GetProperties()?
                    .Where(x => !XLSX_COLUMNS.Contains(x.Name))
                    .ToList();

                var contract = typeof(HU_FILECONTRACT);
                var cvTypeProperties = contract.GetProperties().ToList();
                var getOtherList = (from t in _dbContext.SysOtherListTypes.Where(x => x.CODE == "STATUS")
                                    from o in _dbContext.SysOtherLists.Where(x => x.TYPE_ID == t.ID)
                                    where o.CODE == "CD"
                                    select new { Id = o.ID }).FirstOrDefault();
                tmp1.ForEach(tmpCv =>
                {
                    var obj1 = Activator.CreateInstance(typeof(HU_FILECONTRACT)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
                    var cv = (HU_FILECONTRACT)obj1;

                    tmp1Properties?.ForEach(tmp1Property =>
                    {
                        var tmp1Value = tmp1Property.GetValue(tmpCv);
                        var cvProperty = cvTypeProperties.SingleOrDefault(x => x.Name == tmp1Property.Name);
                        if (cvProperty != null)
                        {
                            if (tmp1Value != null)
                            {
                                cvProperty.SetValue(cv, tmp1Value);
                            };
                        }
                        else
                        {
                            if (tmp1Value != null)
                            {
                                throw new Exception($"{tmp1Property.Name} was not found in HU_FILECONTRACT");
                            }
                        }
                    });
                    if (cv.SIGN_DATE == null)
                    {
                        throw new Exception("SIGN_DATE_IS_NULL" + " " + cv.CONTRACT_NO);
                    }
                    //check HSL chua phe duyet
                    var checkWorking = _dbContext.HuWorkings.AsNoTracking().Where(p => p.ID == cv.WORKING_ID).FirstOrDefault();
                    if (checkWorking != null)
                    {
                        if (checkWorking.STATUS_ID == OtherConfig.STATUS_WAITING)
                        {
                            throw new Exception("WORKING_IS_NOT_APPROVE");
                        }

                        cv.SAL_BASIC = checkWorking.SAL_BASIC ?? 0;
                        cv.SAL_PERCENT = checkWorking.SAL_PERCENT ?? 100;
                    }
                    else
                    {
                        throw new Exception("WORKING_IS_NOT_EXIST");
                    }
                    //check ton tai so hop dong
                    var checkContractNo = _dbContext.HuFileContracts.AsNoTracking().Where(p => p.CONTRACT_NO.Trim().ToUpper() == cv.CONTRACT_NO.Trim().ToUpper()).Any();
                    if (checkContractNo)
                    {
                        throw new Exception("CONTRACT_APPENDIX_NO_IS_EXIST" + " " + cv.CONTRACT_NO.ToUpper());
                    }
                    //lay phong ban nhan vien
                    var emp = _dbContext.HuEmployees.AsNoTracking().Where(p => p.ID == cv.EMPLOYEE_ID).FirstOrDefault();
                    if (emp == null)
                    {
                        throw new Exception("EMPPLOYEE_NOT_EXIST");
                    }
                    cv.ORG_ID = emp!.ORG_ID;
                    //lay chuc danh nguoi ky
                    cv.POSITION_ID = emp!.POSITION_ID;
                    //lay ten nguoi ky
                    var signer = (from e in _dbContext.HuEmployees.AsNoTracking().Where(p => p.ID == cv.SIGN_ID)
                                  from cvs in _dbContext.HuEmployeeCvs.AsNoTracking().Where(p => p.ID == e.PROFILE_ID).DefaultIfEmpty()
                                  select cvs).FirstOrDefault();
                    cv.SIGNER_NAME = signer!.FULL_NAME ?? "";
                    //lay chuc danh nguoi ky
                    var signerPos = (from e in _dbContext.HuEmployees.AsNoTracking().Where(p => p.ID == cv.SIGN_ID)
                                     from p in _dbContext.HuPositions.AsNoTracking().Where(p => p.ID == e.POSITION_ID).DefaultIfEmpty()
                                     select p).FirstOrDefault();

                    cv.SIGNER_POSITION = signerPos == null ? null : (signerPos!.NAME ?? "");

                    cv.CREATED_DATE = now;
                    cv.CREATED_BY = request.XlsxSid;

                    _dbContext.HuFileContracts.Add(cv);
                    res = cv;
                });
                // Clear tmp
                _dbContext.HuFilecontractImports.RemoveRange(tmp1);
                _dbContext.SaveChanges();

                _uow.Commit();
                return new() { InnerBody = res, StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.CREATE_SUCCESS };

            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = ex.Message };
            }
        }
    }
}

