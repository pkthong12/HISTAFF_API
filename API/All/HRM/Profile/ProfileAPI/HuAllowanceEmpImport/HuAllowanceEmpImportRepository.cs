using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using Common.Extensions;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace API.All.HRM.Profile.ProfileAPI.HuAllowanceEmpImport
{
    public class HuAllowanceEmpImportRepository : IHuAllowanceEmpImportRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private readonly GenericReducer<HU_ALLOWANCE_EMP_IMPORT, HuAllowanceEmpImportDTO> _genericReducer;
        private readonly List<string> XLSX_COLUMNS;

        public HuAllowanceEmpImportRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericReducer = new();
            XLSX_COLUMNS = new() { "XLSX_USER_ID", "XLSX_EX_CODE", "XLSX_INSERT_ON", "XLSX_SESSION", "XLSX_FILE_NAME", "XLSX_ROW" };
        }
        public async Task<GenericPhaseTwoListResponse<HuAllowanceEmpImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuAllowanceEmpImportDTO> request)
        {
            var raw = from p in _dbContext.HuAllowanceEmpImports
                      from g in _dbContext.HuAllowances.Where(c => c.ID == p.ALLOWANCE_ID).DefaultIfEmpty()
                      from e in _dbContext.HuEmployees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                      from o in _dbContext.HuOrganizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                      from t in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                      select new HuAllowanceEmpImportDTO()
                      {
                          Id = p.XLSX_ROW,
                          // Phần thông tin có trong tất cả các
                          XlsxUserId = p.XLSX_USER_ID,
                          XlsxExCode = p.XLSX_EX_CODE,
                          XlsxSession = p.XLSX_SESSION,
                          XlsxInsertOn = p.XLSX_INSERT_ON,
                          XlsxFileName = p.XLSX_FILE_NAME,
                          XlsxRow = p.XLSX_ROW,
                          EmployeeId = p.EMPLOYEE_ID,
                          EmployeeCode = e.CODE,
                          EmployeeName = e.Profile!.FULL_NAME,
                          Monney = p.MONNEY,
                          Coefficient = p.COEFFICIENT,
                          DateStart = p.DATE_START,
                          DateEnd = p.DATE_END,
                          AllowanceId = p.ALLOWANCE_ID,
                          AllowanceName = g.NAME,
                          Note = p.NOTE
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

                var tmp = await _dbContext.HuAllowanceEmpImports.Where(x => x.XLSX_USER_ID == request.XlsxSid && x.XLSX_EX_CODE == request.XlsxExCode && x.XLSX_SESSION == request.XlsxSession).ToListAsync();

                var tmpType = typeof(HU_ALLOWANCE_EMP_IMPORT);
                var tmpProperties = tmpType.GetProperties()?
                    .Where(x => !XLSX_COLUMNS.Contains(x.Name))
                    .ToList();

                var allowanceEmp = typeof(HU_ALLOWANCE_EMP);
                var allowanceEmpProperties = allowanceEmp.GetProperties().ToList();
                var res = new HU_ALLOWANCE_EMP();
                tmp.ForEach(item =>
                {
                    var obj = Activator.CreateInstance(typeof(HU_ALLOWANCE_EMP)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
                    var a = (HU_ALLOWANCE_EMP)obj;
                    if (item.DATE_START > item.DATE_END)
                    {
                        throw new Exception($"LINE {item.XLSX_ROW} - DATE START MUST LESS THAN DATE END");
                    }
                    tmpProperties?.ForEach(tmpProperty =>
                    {
                        var tmpValue = tmpProperty.GetValue(item);
                        var allowanceEmpProperty = allowanceEmpProperties.SingleOrDefault(x => x.Name == tmpProperty.Name);
                        if (allowanceEmpProperty != null)
                        {
                            if (tmpValue != null)
                            {
                                allowanceEmpProperty.SetValue(a, tmpValue);
                            };
                        }
                        else
                        {
                            if (tmpValue != null)
                            {
                                throw new Exception($"{tmpProperty.Name} was not found in HU_ALLOWANCE_EMP");
                            }
                        }
                    });

                    if(a?.EMPLOYEE_ID == null)
                    {
                        throw new Exception("EMPLOYEE_ID_CANNOT_NULL_IN_ROW" + " " + item.XLSX_ROW);
                    }

                    if (a?.ALLOWANCE_ID == null)
                    {
                        throw new Exception("ALLOWANCE_ID_CANNOT_NULL_IN_ROW" + " " + item.XLSX_ROW);
                    }

                    if (a?.DATE_START == null)
                    {
                        throw new Exception("DATE_START_CANNOT_NULL_IN_ROW" + " " + item.XLSX_ROW);
                    }

                    a.CREATED_DATE = now;
                    a.CREATED_BY = request.XlsxSid;
                    res = a;
                    _dbContext.HuAllowanceEmps.Add(a);
                    _dbContext.SaveChanges();

                });



                // Clear tmp
                _dbContext.HuAllowanceEmpImports.RemoveRange(tmp);
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
