using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;

namespace API.All.HRM.Attendance.AttendanceAPI.List.AtSwipeDataImport
{
    public class AtSwipeDataImport : IAtSwipeDataImport
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private readonly GenericReducer<AT_SWIPE_DATA_IMPORT, AtSwipeDataImportDTO> _genericReducer;
        private readonly List<string> XLSX_COLUMNS;

        public AtSwipeDataImport(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericReducer = new();
            XLSX_COLUMNS = new() { "XLSX_USER_ID", "XLSX_EX_CODE", "XLSX_INSERT_ON", "XLSX_SESSION", "XLSX_FILE_NAME", "XLSX_ROW" };
        }

        public async Task<GenericPhaseTwoListResponse<AtSwipeDataImportDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtSwipeDataImportDTO> request)
        {
            var raw = from a in _dbContext.AtSwipeDataImports.AsNoTracking()
                      from b in _dbContext.AtTerminals.AsNoTracking().Where(x => x.ID == a.TERMINAL_ID).DefaultIfEmpty()


                      select new AtSwipeDataImportDTO()
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
                          TerminalCode = b.TERMINAL_CODE,
                          ItimeId = a.ITIME_ID,
                          WorkingDay = a.WORKING_DAY,
                          ValTime = a.VALTIME.Value.AddHours(-7), // Indochina Time for viewing
                          TimeOnly = a.TIME_ONLY
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

                var tmps = await _dbContext.AtSwipeDataImports.Where(x => x.XLSX_USER_ID == request.XlsxSid && x.XLSX_EX_CODE == request.XlsxExCode && x.XLSX_SESSION == request.XlsxSession).ToListAsync();

                var tmpType = typeof(AT_SWIPE_DATA_IMPORT);
                var tmpProperties = tmpType.GetProperties()?
                    .Where(x => !XLSX_COLUMNS.Contains(x.Name))
                    .ToList();

                var mainType = typeof(AT_SWIPE_DATA);
                var mainTypeProperties = mainType.GetProperties().ToList();

                var workingDayProperty = tmpProperties?.SingleOrDefault(x => x.Name == "WORKING_DAY");


                tmps.ForEach(tmp =>
                {
                    var obj1 = Activator.CreateInstance(typeof(AT_SWIPE_DATA)) ?? throw new Exception(CommonMessageCode.ACTIVATOR_CREATE_INSTANCE_RETURNS_NULL);
                    var swipe = (AT_SWIPE_DATA)obj1;

                    tmpProperties?.ForEach(tmpProperty =>
                    {
                        var tmpValue = tmpProperty.GetValue(tmp);
                        var mainProperty = mainTypeProperties.SingleOrDefault(x => x.Name == tmpProperty.Name);
                        var workingDay = workingDayProperty?.GetValue(tmp);
                        var valTimeProperty = mainTypeProperties.SingleOrDefault(x => x.Name == "VALTIME");

                        if (mainProperty != null)
                        {
                            if (mainProperty.Name == "TIME_ONLY")
                            {
                                var timeArr = tmpValue?.ToString()?.Split(":");
                                if (timeArr == null)
                                {
                                    throw new Exception($"TIME_ONLY was null or was in wrong format");
                                }
                                else
                                {
                                    var hh = timeArr[0];
                                    var mm = timeArr[1];
                                    var ss = "0";
                                    if (timeArr.Length == 3)
                                    {
                                        ss = timeArr[2];
                                    }
                                    var y = ((DateTime)workingDay!).Year;
                                    var M = ((DateTime)workingDay).Month;
                                    var d = ((DateTime)workingDay).Day;
                                    var h = int.Parse(hh);
                                    var m = int.Parse(mm);
                                    var s = int.Parse(ss);
                                    var valTime = new DateTime(y, M, d, h, m, s);
                                    valTimeProperty?.SetValue(swipe, valTime);
                                }
                            }
                            else if (mainProperty.Name != "VALTIME")
                            {
                                if (tmpValue != null)
                                {
                                    mainProperty.SetValue(swipe, tmpValue);
                                };
                            }
                        }
                        else
                        {
                            if (tmpValue != null)
                            {
                                throw new Exception($"{tmpProperty.Name} was not found in AT_SWIPE_DATA");
                            }
                        }
                    });

                    // Check ITIME_ID
                    var Check_ITIME_ID = _dbContext.HuEmployees.SingleOrDefault(x => x.ITIME_ID == swipe.ITIME_ID);

                    /* If needed to stop
                    if (Check_ITIME_ID == null)
                    {
                        throw new Exception($"No employee has ITIME_ID={swipe.ITIME_ID}");
                    }
                    */

                    swipe.CREATED_DATE = now;
                    swipe.CREATED_BY = request.XlsxSid;

                    _dbContext.AtSwipeDatas.Add(swipe);
                    _dbContext.SaveChanges();

                });

                // Clear tmp
                _dbContext.AtSwipeDataImports.RemoveRange(tmps);
                _dbContext.SaveChanges();

                _uow.Commit();
                return new() { InnerBody = true, StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.CREATE_SUCCESS };

            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }
    }
}
