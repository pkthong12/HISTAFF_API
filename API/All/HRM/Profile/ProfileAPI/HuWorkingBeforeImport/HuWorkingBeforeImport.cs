using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using System.Diagnostics;

namespace API.All.HRM.Profile.ProfileAPI.HuWorkingBeforeImport
{
    public class HuWorkingBeforeImport : IHuWorkingBeforeImport
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private readonly GenericReducer<HU_WORKING_BEFORE_IMPORT, HuWorkingBeforeImportDTO> _genericReducer;
        private readonly List<string> XLSX_COLUMNS;

        public HuWorkingBeforeImport(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericReducer = new();
            XLSX_COLUMNS = new() { "XLSX_USER_ID", "XLSX_EX_CODE", "XLSX_INSERT_ON", "XLSX_SESSION", "XLSX_FILE_NAME", "XLSX_ROW" };
        }
        public async Task<GenericPhaseTwoListResponse<HuWorkingBeforeImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuWorkingBeforeImportDTO> request)
        {
            /*
                var raw = from a in _dbContext.HuWorkingBeforeImports 
                join b in _dbContext.HuEmployeeCvImports 
                on new { a.XLSX_USER_ID, a.XLSX_EX_CODE, a.XLSX_SESSION, a.XLSX_ROW } equals new { b.XLSX_USER_ID, b.XLSX_EX_CODE, b.XLSX_SESSION, b.XLSX_ROW }

             */
            var raw = from a in _dbContext.HuWorkingBeforeImports
                      from reference_1 in _dbContext.HuEmployees.Where(x => x.ID == a.EMPLOYEE_ID).DefaultIfEmpty()
                      from reference_2 in _dbContext.HuEmployeeCvs.Where(x => x.ID == reference_1.PROFILE_ID).DefaultIfEmpty()
                      from reference_3 in _dbContext.HuOrganizations.Where(x => x.ID == reference_1.ORG_ID).DefaultIfEmpty()
                      from reference_4 in _dbContext.HuPositions.Where(x => x.ID == reference_1.POSITION_ID).DefaultIfEmpty()
                      select new HuWorkingBeforeImportDTO()
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
                          EmployeeCode = reference_1.CODE,
                          EmployeeName = reference_2.FULL_NAME,
                          OrgName = reference_3.NAME,
                          PositionName = reference_4.NAME,
                          FromDate = a.FROM_DATE,
                          EndDate = a.END_DATE,
                          FromDateStr = (a.FROM_DATE != null) ? a.FROM_DATE.Value.ToString("dd/MM/yyyy") : "",
                          EndDateStr = (a.END_DATE != null) ? a.END_DATE.Value.ToString("dd/MM/yyyy") : "",
                          CompanyName = a.COMPANY_NAME,
                          TitleName = a.TITLE_NAME,
                          Seniority = a.END_DATE.Value.Subtract(a.FROM_DATE.Value).Days.ToString() + " ngày",
                          TerReason = a.TER_REASON
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

                var tmp1 = await _dbContext.HuWorkingBeforeImports
                            .Where(x => x.XLSX_USER_ID == request.XlsxSid
                                    && x.XLSX_EX_CODE == request.XlsxExCode
                                    && x.XLSX_SESSION == request.XlsxSession)
                            .ToListAsync();

                var tmp1Type = typeof(HU_WORKING_BEFORE_IMPORT);
                var tmp1Properties = tmp1Type.GetProperties()?
                    .Where(x => !XLSX_COLUMNS.Contains(x.Name))
                    .ToList();


                var hu_working_before_Type = typeof(HU_WORKING_BEFORE);
                var hu_working_before_TypeProperties = hu_working_before_Type.GetProperties().ToList();


                foreach (var tmpHuWorkingBefore in tmp1)
                {
                    var obj1 = Activator.CreateInstance(typeof(HU_WORKING_BEFORE)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
                    var huWorkingBefore = (HU_WORKING_BEFORE)obj1;

                    tmp1Properties?.ForEach(tmp1Property =>
                    {
                        var tmp1Value = tmp1Property.GetValue(tmpHuWorkingBefore);
                        var huWorkingBeforeProperty = hu_working_before_TypeProperties.SingleOrDefault(x => x.Name == tmp1Property.Name);
                        if (huWorkingBeforeProperty != null)
                        {
                            if (tmp1Value != null)
                            {
                                huWorkingBeforeProperty.SetValue(huWorkingBefore, tmp1Value);
                            };
                        }
                        else
                        {
                            if (tmp1Value != null)
                            {
                                throw new Exception($"{tmp1Property.Name} was not found in HU_WORKING_BEFORE");
                            }
                        }
                    });

                    huWorkingBefore.CREATED_DATE = now;
                    huWorkingBefore.CREATED_BY = request.XlsxSid;

                    // kiểm tra ngày bắt đầu phải nhỏ hơn ngày kết thúc
                    if (tmpHuWorkingBefore.FROM_DATE > tmpHuWorkingBefore.END_DATE)
                    {
                        return new FormatedResponse()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            StatusCode = EnumStatusCode.StatusCode400,
                            MessageCode = "UI_FORM_CONTROL_ERROR_FROM_DATE_MUST_LESS_THAN_EQUAL_TO_DATE"
                        };
                    }

                    var get_status_id_by_code = (from item_sub in _dbContext.SysOtherListTypes.Where(x => x.CODE == "STATUS")
                                                 from item_main in _dbContext.SysOtherLists.Where(x => x.CODE == "DD" && x.TYPE_ID == item_sub.ID)
                                                 select item_main.ID
                                                 ).First();

                    // kiểm tra ngày kết thúc của quá trình công tác trước đây
                    // phải nhỏ hơn ngày bắt đầu của thời gian công tác hiện tại
                    var contractEntity = _uow.Context.Set<HU_CONTRACT>().AsNoTracking().AsQueryable();
                    var contract = (from p in contractEntity
                                    where p.EMPLOYEE_ID == tmpHuWorkingBefore.EMPLOYEE_ID
                                            && p.START_DATE <= tmpHuWorkingBefore.END_DATE
                                            && p.STATUS_ID == get_status_id_by_code
                                    select p).Any();

                    if (contract)
                    {
                        return new FormatedResponse() {
                            ErrorType = EnumErrorType.CATCHABLE,
                            StatusCode = EnumStatusCode.StatusCode519,
                            MessageCode = "UI_FORM_CONTROL_ERROR_WORKING_BEFORE_END_DATE_VIOLATION"
                        };
                    }

                    // tính thâm niên
                    var fromDate = tmpHuWorkingBefore.FROM_DATE.Value;
                    var endDate = tmpHuWorkingBefore.END_DATE.Value;
                    var seniority = Math.Round((endDate - fromDate).TotalDays / 365.25, 2).ToString() + " năm";
                    huWorkingBefore.SENIORITY = seniority;

                    _dbContext.HuWorkingBefores.Add(huWorkingBefore);
                    _dbContext.SaveChanges();
                }


                // Clear tmp
                _dbContext.HuWorkingBeforeImports.RemoveRange(tmp1);
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
