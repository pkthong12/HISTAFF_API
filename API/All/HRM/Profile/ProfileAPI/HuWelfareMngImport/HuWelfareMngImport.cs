using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using Common.Extensions;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using System.Diagnostics;

namespace API.All.HRM.Profile.ProfileAPI.HuWelfareMngImport
{
    public class HuWelfareMngImport : IHuWelfareMngImport
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private readonly GenericReducer<HU_WELFARE_MNG_IMPORT, HuWelfareMngImportDTO> _genericReducer;
        private readonly List<string> XLSX_COLUMNS;

        public HuWelfareMngImport(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericReducer = new();
            XLSX_COLUMNS = new() { "XLSX_USER_ID", "XLSX_EX_CODE", "XLSX_INSERT_ON", "XLSX_SESSION", "XLSX_FILE_NAME", "XLSX_ROW" };
        }
        public async Task<GenericPhaseTwoListResponse<HuWelfareMngImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuWelfareMngImportDTO> request)
        {
            /*
                var raw = from a in _dbContext.HuEmployeeImports 
                join b in _dbContext.HuEmployeeCvImports 
                on new { a.XLSX_USER_ID, a.XLSX_EX_CODE, a.XLSX_SESSION, a.XLSX_ROW } equals new { b.XLSX_USER_ID, b.XLSX_EX_CODE, b.XLSX_SESSION, b.XLSX_ROW }

             */
            var raw = from a in _dbContext.HuWelfareMngImports
                      from e in _dbContext.HuEmployees.Where(x => x.ID == a.EMPLOYEE_ID).DefaultIfEmpty()
                      from ecv in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                      from c in _dbContext.HuWelfares.Where(c => c.ID == a.WELFARE_ID).DefaultIfEmpty()
                      from p in _dbContext.HuPositions.AsNoTracking().Where(pos => pos.ID == e.POSITION_ID).DefaultIfEmpty()
                      from o in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == e.ORG_ID).DefaultIfEmpty()

                      select new HuWelfareMngImportDTO()
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
                          DepartmentName = o.NAME,
                          WelfareName = c.NAME,
                          DecisionCode = a.DECISION_CODE,
                          EffectDate = a.EFFECT_DATE,
                          Money = a.MONEY,

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
                var welfareImport = await _dbContext.HuWelfareMngImports.Where(x => x.XLSX_USER_ID == request.XlsxSid && x.XLSX_EX_CODE == request.XlsxExCode && x.XLSX_SESSION == request.XlsxSession).ToListAsync();

                var tmpType = typeof(HU_WELFARE_MNG_IMPORT);
                var tmpProperties = tmpType.GetProperties()?
                    .Where(x => !XLSX_COLUMNS.Contains(x.Name))
                    .ToList();

                var welfareMngType = typeof(HU_WELFARE_MNG);
                var welfareMngTypeProperties = welfareMngType.GetProperties().ToList();

                welfareImport.ForEach(tmpCv =>
                {
                    var obj = Activator.CreateInstance(typeof(HU_WELFARE_MNG)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
                    var welfareMng = (HU_WELFARE_MNG)obj;

                    tmpProperties?.ForEach(tmpProperties =>
                    {
                        var tmpValue = tmpProperties.GetValue(tmpCv);
                        var welfareMngProperty = welfareMngTypeProperties.SingleOrDefault(x => x.Name == tmpProperties.Name);
                        if (welfareMngProperty != null)
                        {
                            if (tmpValue != null)
                            {
                                welfareMngProperty.SetValue(welfareMng, tmpValue);
                            };
                        }
                        else
                        {
                            if (tmpValue != null)
                            {
                                throw new Exception($"{tmpProperties.Name} was not found in HU_EMPLOYEE_CV");
                            }
                        }
                    });
                    if (tmpCv.EFFECT_DATE > tmpCv.EXPIRE_DATE)
                    {
                        throw new Exception(CommonMessageCode.EXP_MUST_LESS_THAN_EFF);
                    }

                    welfareMng.CREATED_DATE = now;
                    welfareMng.CREATED_BY = request.XlsxSid;

                    _dbContext.HuWelfareMngs.Add(welfareMng);
                    _dbContext.SaveChanges();

                });

                // Clear tmp
                _dbContext.HuWelfareMngImports.RemoveRange(welfareImport);
                _dbContext.SaveChanges();

                _uow.Commit();
                return new() { InnerBody = true };

            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }
    }
}
