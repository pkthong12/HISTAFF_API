using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using System.Linq.Dynamic.Core;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Common.Interfaces;
using Common.DataAccess;
using System.Data;
using System.Data.SqlClient;
using API.All.HRM.Attendance.AttendanceAPI.Business.AtTimeTimesheetDaily;
using System.Linq;
using Common.Extensions;
using DocumentFormat.OpenXml.Spreadsheet;
using CORE.DataContract;

namespace API.Controllers.AtTimeTimesheetDaily
{
    public class AtTimeTimesheetDailyRepository : IAtTimeTimesheetDailyRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<AT_TIME_TIMESHEET_DAILY, AtTimeTimesheetDailyDTO> _genericRepository;
        private readonly GenericReducer<AT_TIME_TIMESHEET_DAILY, AtTimeTimesheetDailyDTO> _genericReducer;
        protected AbsQueryDataTemplate QueryData;


        public AtTimeTimesheetDailyRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<AT_TIME_TIMESHEET_DAILY, AtTimeTimesheetDailyDTO>();
            _genericReducer = new();
            QueryData = new SqlQueryDataTemplate(_dbContext);
        }

        public async Task<FormatedResponse> SinglePhaseQueryList(GenericQueryListDTO<AtTimeTimesheetDailyDTO> request)
        {
            var joined = from p in _dbContext.AtTimeTimesheetDailys
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(emp => emp.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == p.ORG_ID).DefaultIfEmpty()
                         from po in _dbContext.HuPositions.AsNoTracking().Where(po => po.ID == p.POSITION_ID).DefaultIfEmpty()
                         from j in _dbContext.HuJobs.AsNoTracking().Where(j => j.ID == po.JOB_ID).DefaultIfEmpty()
                         from s in _dbContext.AtShifts.AsNoTracking().Where(s => s.ID == p.SHIFT_ID).DefaultIfEmpty()
                         from sb in _dbContext.AtTimeTypes.AsNoTracking().Where(s => s.ID == p.MANUAL_ID).DefaultIfEmpty()
                         from pe in _dbContext.AtSalaryPeriods.AsNoTracking().Where(s => s.ID == p.PERIOD_ID).DefaultIfEmpty()
                         select new AtTimeTimesheetDailyDTO
                         {
                             Id = p.ID,
                             OrgId = p.ORG_ID,
                             EmployeeId = e.ID,
                             EmployeeCode = e.CODE, // Mã NV
                             EmployeeName = e.Profile.FULL_NAME,   // Họ tên
                             PositionName = po.NAME,    // Chức danh   
                             OrgName = o.NAME,  // Phòng/Ban
                             PeriodId = p.PERIOD_ID,
                             DateStart = pe.START_DATE, // Ngày bắt đầu kỳ công
                             DateEnd = pe.END_DATE, // Ngày kết thúc kỳ công
                             Workingday = p.WORKINGDAY, // Ngày làm việc
                             ShiftCode = p.SHIFT_CODE,  // Ca làm việc
                             ShiftStart = s.HOURS_START, // Giờ vào ca
                             ShiftStartString = s.HOURS_START! == null ? "" : s.HOURS_START.ToString("HH:mm"),
                             ShiftEnd = s.HOURS_STOP, // Giờ ra ca
                             ShiftEndString = s.HOURS_STOP! == null ? "" : s.HOURS_STOP.ToString("HH:mm"),
                             Valin1 = p.VALIN1, // Giờ vào gốc
                             Valin1String = p.VALIN1 == null ? "" : p.VALIN1!.Value.ToString("HH:mm"),
                             Valin4 = p.VALIN4, // Giờ ra gốc
                             Valin4String = p.VALIN4 == null ? "" : p.VALIN4!.Value.ToString("HH:mm"),
                             Workinghour = p.WORKINGHOUR,// Số giờ làm việc
                             ManualId = p.MANUAL_ID, // Mã công trong ngày
                             ManualCode = sb.CODE == "X" && p.WORKINGHOUR <= 5 && p.WORKINGHOUR >= 3 && p.LEAVE_ID == null ? sb.CODE + "/2" : sb.CODE,
                             // Kiểu công trong ngày
                             Late = p.LATE, // Số phút đi muộn
                             Comebackout = p.COMEBACKOUT,// Số phút về sớm
                             DimuonVesomThucte = p.DIMUON_VESOM_THUCTE, // Tổng số phút đi muộn về sớm 
                             OtTotalConvert = p.OT_TOTAL_CONVERT_PAY,// Tổng số giờ làm thêm
                             // Số giờ làm thêm tự động tạm tính
                             OtWeekday = p.OT_WEEKDAY, // Số giờ làm thêm thêm ban ngày (ngày thường)
                             OtSunday = p.OT_SUNDAY, // Số giờ làm thêm ban ngày(ngày nghỉ)
                             OtHoliday = p.OT_HOLIDAY, // Số giờ làm thêm ban ngày (ngày Lễ/tết)
                             OtWeeknight = p.OT_WEEKNIGHT,// Số giờ làm thêm ban đêm (ngày thường)
                             OtSundaynight = p.OT_SUNDAYNIGHT, // Số giờ làm thêm ban đêm (ngày nghỉ)
                             OtHolidayNight = p.OT_HOLIDAY_NIGHT, // Số giờ làm thêm ban đêm (ngày Lễ/Tết)
                             IsConfirm = p.IS_CONFIRM ?? false, // Xác nhận
                             CodeColor = p.CODE_COLOR ?? 0,
                             JobOrderNum = (int)(j.ORDERNUM ?? 999),
                         };
            var searchForShiftStart = "";
            var searchForShiftEnd = "";
            var searchForValin1 = "";
            var searchForValin4 = "";
            var skip = request.Pagination!.Skip;
            var take = request.Pagination.Take;
            request.Pagination.Skip = 0;
            request.Pagination.Take = 9999;
            if (request.Search != null)
            {
                request.Search.ForEach(x =>
                {
                    if (x.Field == "shiftStart")
                    {
                        searchForShiftStart = x.SearchFor.ToString().ToLower().Trim();
                        x.SearchFor = "";
                    }
                    if (x.Field == "shiftEnd")
                    {
                        searchForShiftEnd = x.SearchFor.ToString().ToLower().Trim();
                        x.SearchFor = "";
                    }
                    if (x.Field == "valin1")
                    {
                        searchForValin1 = x.SearchFor.ToString().ToLower().Trim();
                        x.SearchFor = "";
                    }
                    if (x.Field == "valin4")
                    {
                        searchForValin4 = x.SearchFor.ToString().ToLower().Trim();
                        x.SearchFor = "";
                    }
                });
            }
            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);

            var resultList = new List<AtTimeTimesheetDailyDTO>();
            foreach (var i in singlePhaseResult.List!)
            {
                resultList.Add(i);
            }

            resultList = resultList.Where(x => (x.ShiftStartString!.Trim().Contains(searchForShiftStart))).ToList();
            resultList = resultList.Where(x => (x.ShiftEndString!.Trim().Contains(searchForShiftEnd))).ToList();
            resultList = resultList.Where(x => (x.Valin1String!.Trim().Contains(searchForValin1))).ToList();
            resultList = resultList.Where(x => (x.Valin4String!.Trim().Contains(searchForValin4))).ToList();
            var result = new
            {
                Count = resultList.Count(),
                List = resultList.Skip(skip).Take(take),
                Page = (skip / take) + 1,
                PageCount = resultList.Count(),
                Skip = skip,
                Take = take,
                MessageCode = CommonMessageCode.QUERY_LIST_SUCCESS,
            };
            return new()
            {
                InnerBody = result,
            };
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

            var joined = await (from l in _dbContext.AtTimeTimesheetDailys.AsNoTracking().Where(l => l.ID == id).DefaultIfEmpty()
                                from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == l.EMPLOYEE_ID).DefaultIfEmpty()
                                from p in _dbContext.HuPositions.AsNoTracking().Where(p => p.ID == l.POSITION_ID).DefaultIfEmpty()
                                from o in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == l.ORG_ID).DefaultIfEmpty()
                                from sb in _dbContext.AtTimeTypes.AsNoTracking().Where(s => s.ID == l.MANUAL_ID).DefaultIfEmpty()
                                select new AtTimeTimesheetDailyDTO
                                {
                                    Id = l.ID,
                                    EmployeeId = e.ID,
                                    EmployeeCode = e.CODE,
                                    EmployeeName = e.Profile!.FULL_NAME,
                                    PositionId = p.ID,
                                    PositionName = p.NAME,
                                    OrgId = o.ID,
                                    OrgName = o.NAME,
                                    Workingday = l.WORKINGDAY,
                                    WorkingdayTo = l.WORKINGDAY,
                                    ManualId = l.MANUAL_ID,
                                    ManualCode = sb.CODE,
                                    Reason = l.REASON,
                                    IsConfirm = l.IS_CONFIRM,
                                    ShiftCode = l.SHIFT_CODE
                                }).FirstOrDefaultAsync();
            if (joined != null)
            {
                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetByImportEdit(long id)
        {

            var joined = await (from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == id).DefaultIfEmpty()
                                from p in _dbContext.HuPositions.AsNoTracking().Where(p => p.ID == e.POSITION_ID).DefaultIfEmpty()
                                from o in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == e.ORG_ID).DefaultIfEmpty()
                                select new AtTimesheetDailyImportDTO
                                {
                                    Id = e.ID,
                                    EmployeeId = e.ID,
                                    EmployeeCode = e.CODE,
                                    EmployeeName = e.Profile!.FULL_NAME,
                                    PositionName = p.NAME,
                                    OrgName = o.NAME
                                }).FirstOrDefaultAsync();
            if (joined != null)
            {
                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> UpdateImportEdit(GenericUnitOfWork _uow, AtTimesheetDailyImportDTO dto, string sid)
        {
            try
            {
                for (int i = 0; i <= (dto.WorkingdayTo - dto.Workingday).Value.Days; i++)
                {
                    DateTime targetDate = dto.Workingday!.Value.AddDays(i);
                    var timeDailyLst = await _dbContext.AtTimeTimesheetDailys
                        .Where(t => t.EMPLOYEE_ID == dto.Id && t.WORKINGDAY!.Value.Date == targetDate.Date)
                        .ToListAsync();

                    foreach (var timeDaily in timeDailyLst)
                    {
                        timeDaily.MANUAL_ID = dto.ManualId;
                        timeDaily.UPDATED_BY = sid;
                        timeDaily.UPDATED_DATE = DateTime.Now;
                        timeDaily.UPDATED_LOG = sid;
                    }


                }

                await _dbContext.SaveChangesAsync();
                return new() { InnerBody = dto, MessageCode = CommonMessageCode.UPDATE_SUCCESS, StatusCode = EnumStatusCode.StatusCode200 };
            }
            catch
            {
                return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.UPDATED_FAILD, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, AtTimeTimesheetDailyDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<AtTimeTimesheetDailyDTO> dtos, string sid)
        {
            var add = new List<AtTimeTimesheetDailyDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, AtTimeTimesheetDailyDTO dto, string sid, bool patchMode = true)
        {
            // check nhan vien thu viec
            var contractType = await (from c in _dbContext.HuContracts.AsNoTracking().Where(c => c.EMPLOYEE_ID == dto.EmployeeId && c.START_DATE <= dto.Workingday && dto.Workingday <= c.EXPIRE_DATE).DefaultIfEmpty()
                                      from t in _dbContext.HuContractTypes.AsNoTracking().Where(t => t.ID == c.CONTRACT_TYPE_ID).DefaultIfEmpty()
                                      from s in _dbContext.SysContractTypes.AsNoTracking().Where(s => s.ID == t.TYPE_ID).DefaultIfEmpty()
                                      orderby c.ID descending
                                      select new
                                      {
                                          Code = s.CODE
                                      }).FirstOrDefaultAsync();

            if (dto.ManualId != null)
            {
                if (dto.ManualId != 0 && dto.ManualId != null)
                {
                    var manual = await _dbContext.AtTimeTypes.Where(t => t.ID == dto.ManualId).FirstOrDefaultAsync();
                    if (contractType != null && contractType!.Code == "HDTV" && (manual!.CODE == "P" || manual!.CODE.Contains("/P") || manual!.CODE.Contains("P/")))
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.EMPLOYEE_HAVE_PROBATIONARY_CONTRACT, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                    // check đăng ký phép không được quá số ngày phép còn lại
                    // lấy tháng, năm dựa theo ngày làm việc
                    var currentMonth = dto.Workingday!.Value.Month;
                    var currentYear = dto.Workingday!.Value.Year;
                    // lấy ra số ngày phép còn lại
                    var res = await _dbContext.AtEntitlements.Where(e => e.EMPLOYEE_ID == dto.EmployeeId && e.YEAR == currentYear && e.MONTH == currentMonth).FirstOrDefaultAsync();
                    double remainingDay = 0;
                    if (res != null)
                    {
                        remainingDay = res.TOTAL_HAVE!.Value;
                        if (remainingDay == 0)
                        {
                            return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.NOT_ENOUGH_DAY_OFF, StatusCode = EnumStatusCode.StatusCode400 };
                        }
                        else
                        {
                            double totalDayRegister = 0;
                            if(manual!.CODE == "P")
                            {
                                totalDayRegister = 1;
                            }
                            else if(manual!.CODE.Contains("/P") || manual!.CODE.Contains("P/"))
                            {
                                totalDayRegister = 0.5;
                            }

                            if(totalDayRegister > remainingDay)
                            {
                                return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.NOT_ENOUGH_DAY_OFF, StatusCode = EnumStatusCode.StatusCode400 };

                            }
                        }
                    }
                    else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.NOT_ENOUGH_DAY_OFF, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                }
            }
            dto.IsEdit = true;
            dto.CodeColor = 1187;
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<AtTimeTimesheetDailyDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetListTimeSheet(AtTimeTimesheetDailyDTO param, AppSettings appSettings)
        {
            try
            {

                string cnnString = appSettings.ConnectionStrings.CoreDb;
                string orgids = "," + string.Join(",", param.ListOrgIds!) + ",";
                DataSet ds = new();
                using SqlConnection cnn = new(cnnString);
                using SqlCommand cmd = new();
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GET_CCT";
                if (param != null)
                {
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_ORG_ID", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = orgids });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_ISDISSOLVE", SqlDbType = SqlDbType.Float, Direction = ParameterDirection.Input, Value = 0 });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_PAGE_INDEX", SqlDbType = SqlDbType.Float, Direction = ParameterDirection.Input, Value = 0 });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_EMPLOYEE_ID", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = param.EmployeeId ?? 0 });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_PAGE_SIZE", SqlDbType = SqlDbType.Float, Direction = ParameterDirection.Input, Value = 0 });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_PERIOD_ID", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = param.PeriodId });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_TERMINATE", SqlDbType = SqlDbType.Float, Direction = ParameterDirection.Input, Value = 0 });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_COLOR", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = "," + param.CodeColorStr + "," });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_LANG", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = "vi-VN" });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_EMPLOYEE_CODE", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = param.EmployeeCodeSearch == null ? "" : param.EmployeeCodeSearch.ToString() });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_EMPLOYEE_NAME", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = param.EmployeeNameSearch == null ? "" : param.EmployeeNameSearch.ToString() });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_DEPARTMENT_NAME", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = param.DepartmentNameSearch == null ? "" : param.DepartmentNameSearch.ToString() });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_POSITION_NAME", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = param.PositionNameSearch == null ? "" : param.PositionNameSearch.ToString() });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_SAL_PERIOD_OBJ", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = param.SalPeriodObjSearch == null ? "" : param.SalPeriodObjSearch.ToString() });


                }

                SqlDataAdapter da = new(cmd);
                await Task.Run(() => da.Fill(ds));

                /*
                var r = await QueryData.ExecutePaging("GET_CCT",
                    new
                    {
                        P_ORG_ID = "", 
                        P_ISDISSOLVE = "",
                        P_PAGE_INDEX = "",
                        P_EMPLOYEE_CODE = "",
                        P_EMPLOYEE_NAME = "",
                        P_ORG_NAME = "",
                        P_TITLE_NAME = "",
                        P_PAGE_SIZE = "",
                        P_PERIOD_ID = "",
                        P_TERMINATE = "",
                        P_COLOR = "",
                        P_LANG = "vi-VN",
                    }, false);
                long totalRecord = r.Data.Count;
                */

                List<AtTimesheetDailyCellDTO> list = ds.Tables[0].ToList<AtTimesheetDailyCellDTO>();
                var type = typeof(AtTimesheetDailyCellDTO);
                var valueProps = type.GetProperties().Where(x => x.Name.StartsWith("D") && !x.Name.EndsWith("_COLOR")).ToList();
                var colorProps = type.GetProperties().Where(x => x.Name.StartsWith("D") && x.Name.EndsWith("_COLOR")).ToList();




                list.ForEach(i =>
                {
                    List<AtTimesheetDailyCellValue> valueList = new();
                    valueProps.ForEach(p =>
                    {
                        var d = int.Parse(p.Name.Split("D")[1]);
                        valueList.Add(new()
                        {
                            Day = d,
                            Value = p.GetValue(i)?.ToString()
                        });
                    });
                    List<AtTimesheetDailyCellColor> colorList = new();
                    colorProps.ForEach(p =>
                    {
                        var d = int.Parse(p.Name.Split("_COLOR")[0].Split("D")[1]);
                        colorList.Add(new()
                        {
                            Day = d,
                            ColorCode = (int?)p.GetValue(i)!
                        });
                    });
                    i.Values = valueList;
                    i.ColorCodes = colorList;
                });

                var result = new
                {
                    List = list,
                    Count = list.Count,
                    MessageCode = CommonMessageCode.QUERY_LIST_SUCCESS,
                };

                return new() { InnerBody = result };
            }
            catch (Exception ex)
            {

                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        public async Task<ResultWithError> Calculate(AtTimeTimesheetDailyDTO request)
        {
            try
            {
                var checkLockOrg = (from p in _dbContext.AtOrgPeriods where request.ListOrgIds!.Contains(p.ORG_ID) && p.STATUSCOLEX == 1 && p.PERIOD_ID == request.PeriodId select p).ToList();
                if (checkLockOrg != null && checkLockOrg.Count() > 0 && checkLockOrg.Count() == request.ListOrgIds!.Count())
                {
                    return new ResultWithError("ORG_LOCKED");
                }
                var x = new
                {
                    P_USER_ID = _dbContext.CurrentUserId,
                    P_PERIOD_ID = request.PeriodId,
                    P_ORG_ID = request.OrgId,
                    P_ISDISSOLVE = -1,
                    P_EMPLOYEE_ID = request.EmployeeId == null ? 0 : request.EmployeeId
                };
                var data = QueryData.ExecuteStoreToTable(Procedures.PKG_ATTENDANCE_CALCULATE_CAL_TIME_TIMESHEET_MACHINE,
                    x, false);
                if (Convert.ToDouble(data.Tables[0].Rows[0][0]) > 0)
                {
                    return new ResultWithError(200, data.Tables[0].Rows[0][0]);
                }
                else
                {

                    return new ResultWithError(400);
                }
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> Confirm(AtTimeTimesheetDailyDTO request)
        {
            try
            {
                var lstConfirm = (from p in _dbContext.AtTimeTimesheetDailys
                                  where request.Ids!.Contains(p.ID)
                                  select p).ToList();
                foreach (var item in lstConfirm)
                {
                    item.IS_CONFIRM = request.IsConfirm;
                    var rs = _dbContext.AtTimeTimesheetDailys.Update(item);
                }
                await _dbContext.SaveChangesAsync();

                return new ResultWithError(request);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
    }
}

