using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using Common.Extensions;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;

namespace API.Controllers.AtSignDefaultImport
{
    public class AtSignDefaultImportRepository : IAtSignDefaultImportRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private readonly GenericReducer<AT_SIGN_DEFAULT_IMPORT, AtSignDefaultImportDTO> _genericReducer;
        private readonly List<string> XLSX_COLUMNS;

        public AtSignDefaultImportRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericReducer = new();
            XLSX_COLUMNS = new() { "XLSX_USER_ID", "XLSX_EX_CODE", "XLSX_INSERT_ON", "XLSX_SESSION", "XLSX_FILE_NAME", "XLSX_ROW" };
        }

        public async Task<GenericPhaseTwoListResponse<AtSignDefaultImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtSignDefaultImportDTO> request)
        {

            var raw = from a in _dbContext.AtSignDefaultImports
                      from e in _dbContext.HuEmployees.Where(x => x.ID == a.EMPLOYEE_ID).DefaultIfEmpty()
                      from ecv in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                      from s in _dbContext.AtShifts.Where(c => c.ID == a.SIGN_DEFAULT).DefaultIfEmpty()
                      from p in _dbContext.HuPositions.AsNoTracking().Where(pos => pos.ID == e.POSITION_ID).DefaultIfEmpty()
                      from o in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == e.ORG_ID).DefaultIfEmpty()

                      select new AtSignDefaultImportDTO()
                      {
                          Id = a.XLSX_ROW,
                          // Phần thông tin có trong tất cả các
                          XlsxUserId = a.XLSX_USER_ID,
                          XlsxExCode = a.XLSX_EX_CODE,
                          XlsxSession = a.XLSX_SESSION,
                          XlsxInsertOn = a.XLSX_INSERT_ON,
                          XlsxFileName = a.XLSX_FILE_NAME,
                          XlsxRow = a.XLSX_ROW,

                          // Phần thông tin thô từ bảng gốc
                          EmployeeId = a.EMPLOYEE_ID,
                          EmployeeCode = e.CODE,
                          EmployeeName = e.Profile!.FULL_NAME,
                          PositionName = p.NAME,
                          OrgId = o.ID,
                          OrgName = o.NAME,
                          SignDefault = s.ID,
                          SignDefaultName = s.NAME,
                          EffectDateFrom = a.EFFECT_DATE_FROM,
                          EffectDateTo = a.EFFECT_DATE_TO,
                          Note = a.NOTE,

                      };
            var response = await _genericReducer.SinglePhaseReduce(raw, request);
            return response;
        }

        public async Task<FormatedResponse> Save(ImportQueryListBaseDTO request)
        {
            _uow.CreateTransaction();
            try
            {

                var now = DateTime.UtcNow;

                var signDefaultImport = await _dbContext.AtSignDefaultImports.Where(x => x.XLSX_USER_ID == request.XlsxSid && x.XLSX_EX_CODE == request.XlsxExCode && x.XLSX_SESSION == request.XlsxSession).ToListAsync();
                var res = new AT_SIGN_DEFAULT();
                var tmpType = typeof(AT_SIGN_DEFAULT_IMPORT);
                var tmpProperties = tmpType.GetProperties()?
                    .Where(x => !XLSX_COLUMNS.Contains(x.Name))
                    .ToList();

                var signDefaultType = typeof(AT_SIGN_DEFAULT);
                var signDefaultTypeProperties = signDefaultType.GetProperties().ToList();

                signDefaultImport.ForEach(tmpCv =>
                {
                    var obj = Activator.CreateInstance(typeof(AT_SIGN_DEFAULT)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
                    var signDefault = (AT_SIGN_DEFAULT)obj;

                    tmpProperties?.ForEach(tmpProperty =>
                    {
                        var tmpValue = tmpProperty.GetValue(tmpCv);
                        var signDefaultProperty = signDefaultTypeProperties.SingleOrDefault(x => x.Name == tmpProperty.Name);
                        if (signDefaultProperty != null)
                        {
                            if (tmpValue != null)
                            {
                                signDefaultProperty.SetValue(signDefault, tmpValue);
                            };
                        }
                        else
                        {
                            if (tmpValue != null)
                            {
                                throw new Exception($"{tmpProperty.Name} was not found in HU_EMPLOYEE_CV");
                            }
                        }
                    });
                    if (signDefault.EFFECT_DATE_FROM > signDefault.EFFECT_DATE_TO)
                    {
                        throw new Exception(CommonMessageCode.EXP_MUST_LESS_THAN_EFF);
                    }

                    // Check Employee
                    var employee = _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == signDefault.EMPLOYEE_ID).First();
                    if (employee.WORK_STATUS_ID == OtherConfig.EMP_STATUS_TERMINATE && (employee.TER_EFFECT_DATE <= signDefault.EFFECT_DATE_FROM || signDefault.EFFECT_DATE_TO <= employee.TER_EFFECT_DATE))
                    {
                        throw new Exception(CommonMessageCode.EMP_HAVE_NO_WORKING_STATUS);
                    }

                    // check wage
                    var latestWage = _dbContext.HuWorkings.Where(p => p.EMPLOYEE_ID == signDefault.EMPLOYEE_ID && p.IS_WAGE != 0 && p.STATUS_ID == OtherConfig.STATUS_APPROVE).OrderBy(p => p.EFFECT_DATE).FirstOrDefault();
                    if (latestWage == null)
                    {
                        throw new Exception(CommonMessageCode.EMP_NOT_HAVE_SALARY_PROFILE);
                    }
                    else
                    {
                        if ((latestWage.EFFECT_DATE!.Value.Date > signDefault.EFFECT_DATE_FROM!.Value.Date))
                        {
                            throw new Exception(CommonMessageCode.SALARY_PROFILE_EXPIRED);
                        }
                    }

                    // check date -- todo 
                    if (signDefault.EFFECT_DATE_TO != null)
                    {
                        var checkExist = _dbContext.AtSignDefaults.Where(p => p.EMPLOYEE_ID == signDefault.EMPLOYEE_ID
                                && (p.EFFECT_DATE_FROM!.Value.Date <= signDefault.EFFECT_DATE_TO!.Value.Date && signDefault.EFFECT_DATE_TO!.Value.Date <= p.EFFECT_DATE_TO!.Value.Date)
                                && p.IS_ACTIVE == true).Count();
                        if (checkExist != 0)
                        {
                            throw new Exception(CommonMessageCode.EMPLOYEE_HAVE_EXIST_SHIFT_DEFAULT);
                        }
                    }
                    else
                    {
                        var checkExist = _dbContext.AtSignDefaults.Where(p => p.EMPLOYEE_ID == signDefault.EMPLOYEE_ID
                                && ((p.EFFECT_DATE_FROM!.Value.Date <= signDefault.EFFECT_DATE_FROM!.Value.Date && signDefault.EFFECT_DATE_FROM!.Value.Date <= p.EFFECT_DATE_TO!.Value.Date))
                                && p.IS_ACTIVE == true).Count();
                        if (checkExist != 0)
                        {
                            throw new Exception(CommonMessageCode.EMPLOYEE_HAVE_EXIST_SHIFT_DEFAULT);
                        }
                        var checkSpecialCase = _dbContext.AtSignDefaults.Where(p => p.EMPLOYEE_ID == signDefault.EMPLOYEE_ID && (p.EFFECT_DATE_FROM!.Value.Date <= signDefault.EFFECT_DATE_FROM!.Value.Date && p.EFFECT_DATE_TO == null)).Count();
                        if (checkSpecialCase != 0)
                        {
                            var sign = _dbContext.AtSignDefaults.Where(p => p.EMPLOYEE_ID == signDefault.EMPLOYEE_ID && (p.EFFECT_DATE_FROM!.Value.Date <= signDefault.EFFECT_DATE_FROM!.Value.Date && p.EFFECT_DATE_TO == null)).OrderByDescending(p => p.ID).First();

                            sign.EFFECT_DATE_TO = signDefault.EFFECT_DATE_FROM.Value.AddDays(-1);
                            
                            _dbContext.Entry(sign).State = EntityState.Modified;
                            _dbContext.SaveChangesAsync();
                        }

                    }

                    if (signDefault.EMPLOYEE_ID == null)
                    {
                        throw new Exception("EMPLOYEE_ID_CANNOT_NULL" + " " + tmpCv.XLSX_ROW);
                    }

                    if (signDefault.EFFECT_DATE_FROM == null)
                    {
                        throw new Exception("EFFECT_DATE_FROM_CANNOT_NULL" + " " + tmpCv.XLSX_ROW);
                    }

                    if (signDefault.SIGN_DEFAULT == null)
                    {
                        throw new Exception("SIGN_DEFAULT_CANNOT_NULL" + " " + tmpCv.XLSX_ROW);
                    }

                    signDefault.CREATED_DATE = now;
                    signDefault.CREATED_BY = request.XlsxSid;
                    signDefault.IS_ACTIVE = true;
                    res = signDefault;
                    _dbContext.AtSignDefaults.Add(signDefault);
                    _dbContext.SaveChanges();

                });

                // Clear tmp
                _dbContext.AtSignDefaultImports.RemoveRange(signDefaultImport);
                _dbContext.SaveChanges();

                _uow.Commit();
                return new() { InnerBody = res, StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.CREATE_SUCCESS };

            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }
    }
}
