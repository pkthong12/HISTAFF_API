using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using API.Entities.PORTAL;
using API.DTO.PortalDTO;
using Common.Interfaces;
using Common.DataAccess;
using Common.Extensions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using API.Entities;
using AttendanceDAL.ViewModels;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Extensions.Logging.Abstractions;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.SignalR;
using API.Socket;

namespace API.All.HRM.Profile.ProfileAPI.Portal.PortalRegisterOff
{
    public class PortalRegisterOffRepository : IPortalRegisterOffRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<PORTAL_REGISTER_OFF, PortalRegisterOffDTO> _genericRepository;
        private readonly GenericReducer<PORTAL_REGISTER_OFF, PortalRegisterOffDTO> _genericReducer;
        protected AbsQueryDataTemplate QueryData;
        IHubContext<SignalHub> _hubContext;

        public PortalRegisterOffRepository(FullDbContext context, GenericUnitOfWork uow, IHubContext<SignalHub> hubContext)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<PORTAL_REGISTER_OFF, PortalRegisterOffDTO>();
            _genericReducer = new();
            QueryData = new SqlQueryDataTemplate(_dbContext);
            _hubContext = hubContext;
        }

        public async Task<GenericPhaseTwoListResponse<PortalRegisterOffDTO>> SinglePhaseQueryList(GenericQueryListDTO<PortalRegisterOffDTO> request)
        {
            var joined = from p in _dbContext.PortalRegisterOffs.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new PortalRegisterOffDTO
                         {
                             Id = p.ID
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
                var list = new List<PORTAL_REGISTER_OFF>
                    {
                        (PORTAL_REGISTER_OFF)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new PortalRegisterOffDTO
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

        public async Task<FormatedResponse> RegisterOff(GenericUnitOfWork _uow, DynamicDTO model, string sid)
        {
            try
            {
                // check nhan vien thu viec
                var contractType = await (from c in _dbContext.HuContracts.AsNoTracking().Where(c => c.EMPLOYEE_ID == (long)model["employeeId"] && c.START_DATE <= DateTime.Parse(model["dateStart"].ToString()!) && DateTime.Parse(model["dateEnd"].ToString()!) <= c.EXPIRE_DATE).DefaultIfEmpty()
                                          from t in _dbContext.HuContractTypes.AsNoTracking().Where(t => t.ID == c.CONTRACT_TYPE_ID).DefaultIfEmpty()
                                          from s in _dbContext.SysContractTypes.AsNoTracking().Where(s => s.ID == t.TYPE_ID).DefaultIfEmpty()
                                          orderby c.ID descending
                                          select new
                                          {
                                              Code = s.CODE
                                          }).FirstOrDefaultAsync();
                if (contractType != null && contractType!.Code == "HDTV")
                {
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.EMPLOYEE_HAVE_PROBATIONARY_CONTRACT, StatusCode = EnumStatusCode.StatusCode400 };
                }

                /* ============ check ki cong dong mo ============ */
                // get month + year datestart, dateend
                int monthStartDate = DateTime.Parse(model["dateStart"].ToString()!).Month;
                int monthEndDate = DateTime.Parse(model["dateEnd"].ToString()!).Month;
                int yearStartDate = DateTime.Parse(model["dateStart"].ToString()!).Year;
                int yearEndDate = DateTime.Parse(model["dateEnd"].ToString()!).Year;

                if (yearStartDate == yearEndDate)
                {
                    for (int i = monthStartDate; i <= monthEndDate; i++)
                    {
                        var salPeriod = await _dbContext.AtSalaryPeriods.Where(s => s.MONTH == i && s.YEAR == yearStartDate).FirstOrDefaultAsync();
                        var org = await _dbContext.HuEmployees.Where(e => e.ID == (long)model["employeeId"]).FirstOrDefaultAsync();
                        var checkLockOrgDateStart = (_dbContext.AtOrgPeriods.Where(p => p.ORG_ID == org!.ORG_ID && p.STATUSCOLEX == 1 && p.PERIOD_ID == salPeriod!.ID)).Count();
                        if (checkLockOrgDateStart != 0)
                        {
                            return new FormatedResponse() { MessageCode = CommonMessageCode.THE_WORK_PERIOD_HAS_CEASED_TO_APPLY, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                        }
                    }
                }

                // check đăng ký phép không được quá số ngày phép còn lại
                // lấy ra số ngày phép còn lại
                DateTime now = DateTime.Now;
                var currentMonth = now.Month;
                var currentYear = now.Year;
                var res = await _dbContext.AtEntitlements.Where(e => e.EMPLOYEE_ID == (long)model["employeeId"] && e.YEAR == currentYear && e.MONTH == currentMonth).FirstOrDefaultAsync();
                double remainingDay = 0;
                if (res != null)
                {
                    remainingDay = res.TOTAL_HAVE ?? 0;// day ve 0 neu null
                }

                //int range = DateTime.Parse(model["dateEnd"].ToString()!).Day - DateTime.Parse(model["dateStart"].ToString()!).Day;
                // check nhan vien da dc xep ca hay chua 
                DateTime startDate = DateTime.Parse(model["dateStart"].ToString()!).Date;
                DateTime endDate = DateTime.Parse(model["dateEnd"].ToString()!).Date;
                TimeSpan range = endDate.Subtract(startDate);

                // tổng số ngày đăng ký 
                double totalDayRegister = 0;
                // trường hợp không khai báo từng 
                if ((bool)model["isEachDay"] == false)
                {
                    // lấy ra loại đăng ký
                    var registerCode = await _dbContext.AtTimeTypes.Where(t => t.ID == (long?)model["timeTypeId"]).FirstOrDefaultAsync();
                    if (registerCode!.CODE == "P")
                    {
                        totalDayRegister += (1 * ((int)range.TotalDays + 1));
                    }
                    else if (registerCode!.CODE.Contains("/P") || registerCode!.CODE.Contains("P/"))
                    {
                        totalDayRegister += (0.5 * ((int)range.TotalDays + 1));
                    }
                }
                // khai báo từng ngày
                else
                {
                    for (int i = 0; i <= (int)range.TotalDays; i++)
                    {
                        var manualId = _dbContext.AtTimeTypes.Where(x => x.CODE == model["shiftCode" + (i + 1)].ToString()).FirstOrDefault();
                        if(model["shiftCode" + (i + 1)].ToString()!.Contains("/P")  || model["shiftCode" + (i + 1)].ToString()!.Contains("P/"))
                        {
                            totalDayRegister += 0.5;
                        }
                        else if(model["shiftCode" + (i + 1)].ToString()  == "P")
                        {
                            totalDayRegister += 1;
                        }

                    }
                }

                
                if (totalDayRegister > remainingDay)
                {
                    return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.NOT_ENOUGH_DAY_OFF };
                }



                // check nhan vien da dc xep ca hay chua 
                var checkExisistShift = await _dbContext.AtWorksigns.Where(w => w.EMPLOYEE_ID == (long)model["employeeId"] && DateTime.Parse(model["dateStart"].ToString()!).Date <= w.WORKINGDAY!.Value.Date && w.WORKINGDAY!.Value.Date <= DateTime.Parse(model["dateEnd"].ToString()!).Date).CountAsync();
                if (checkExisistShift != range.TotalDays + 1)
                {
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.EMPLOYEE_NOT_HAVE_SHIFT, StatusCode = EnumStatusCode.StatusCode400 };
                }

                // thêm vào bảng chính 
                string reggroupId = Guid.NewGuid().ToString();

                var data = new PORTAL_REGISTER_OFF();
                data.TYPE_CODE = (string?)model["typeCode"];
                data.ID_REGGROUP = reggroupId;
                data.EMPLOYEE_ID = (long)model["employeeId"];
                data.DATE_START = DateTime.Parse(model["dateStart"].ToString());
                data.DATE_END = DateTime.Parse(model["dateEnd"].ToString());
                data.TIME_TYPE_ID = (long?)model["timeTypeId"];
                data.RECEIVE_WORKER_ID = (long?)(model["receiveWorkerId"] == "" ? null : model["receiveWorkerId"]);
                data.NOTE = (string?)model["note"];
                data.IS_EACH_DAY = (bool)model["isEachDay"];
                data.CREATED_BY = sid;
                data.CREATED_DATE = DateTime.Now;
                data.UPDATED_BY = sid;
                data.UPDATED_DATE = DateTime.Now;
                data.TOTAL_DAY = ((int)range.TotalDays + 1);

                // check trùng khoảng thời gian với những bản ghi đã được đăng kí 
                var listCheck = _dbContext.PortalRegisterOffs.Where(x => x.EMPLOYEE_ID == data.EMPLOYEE_ID && x.TYPE_CODE == "OFF").ToList();
                bool isValid = true;
                foreach (var itemCheck in listCheck)
                {
                    if (data.DATE_END.Value.Date < itemCheck.DATE_START!.Value.Date)
                    {
                        isValid = true;
                    }
                    else if (data.DATE_START.Value.Date > itemCheck.DATE_END!.Value.Date)
                    {
                        isValid = true;
                    }
                    else
                    {
                        isValid = false; break;
                    }
                }
                if (isValid == false)
                {
                    return new FormatedResponse()
                    {
                        MessageCode = CommonMessageCode.EMPLOYEE_HAVE_EXIST_TIME_REGISTER,
                        InnerBody = model,
                        StatusCode = EnumStatusCode.StatusCode500
                    };
                }

                // check trùng khoảng thời gian với những bản ghi đã được phê duyệt
                List<DateTime> days = new List<DateTime>();
                for (int i = 0; i <= (int)range.TotalDays; i++)
                {
                    days.Add((startDate).AddDays(i));
                }
                foreach (var dateTime in days)
                {
                    int checkExists = _dbContext.AtRegisterLeaves.Where(w => w.DATE_START!.Value.Date <= dateTime.Date && dateTime.Date <= w.DATE_END!.Value.Date && w.EMPLOYEE_ID == (long)model["employeeId"]).Count();
                    if (checkExists > 0)
                    {
                        return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DATE_HAS_APPROVED };
                    }
                }

                /*var checkValidate = await QueryData.ExecuteList("CHECK_VALIDATE_REGISTER_LEAVE",
                 new
                 {
                     P_EMPLOYEEID = data.EMPLOYEE_ID,
                     P_LEAVEFROM = data.DATE_START.Value.ToString("dd/MM/yyyy"),
                     P_LEAVETO = data.DATE_END.Value.ToString("dd/MM/yyyy"),
                     P_MANUALID = data.TIME_TYPE_ID,
                     P_LEAVEID = 0,
                     P_LEAVE_TYPE = 0,
                     P_DAYNUM = data.TOTAL_DAY,
                 }, false);
                if (checkValidate != null)
                {
                    var ListValidates = checkValidate.Select(c => (string?)((dynamic)c).MESSAGECODE).FirstOrDefault();
                    if (ListValidates != null && ListValidates != "")
                    {
                        return new FormatedResponse()
                        {
                            MessageCode = ListValidates,
                            InnerBody = model,
                            StatusCode = EnumStatusCode.StatusCode500
                        };
                    }
                }*/
                await _dbContext.PortalRegisterOffs.AddAsync(data);
                await _dbContext.SaveChangesAsync();

                // thêm vào bảng chi tiết
                // lấy ra thông tin đơn
                var obj = _dbContext.PortalRegisterOffs.Where(x => x.EMPLOYEE_ID == data.EMPLOYEE_ID && x.ID_REGGROUP == data.ID_REGGROUP).FirstOrDefault();

                if (obj != null)
                {

                    try
                    {
                        // khai báo từng ngày = true
                        if (obj.IS_EACH_DAY == true)
                        {
                            for (int i = 0; i <= (obj.DATE_END - obj.DATE_START).Value.Days; i++)
                            {

                                if (model["dType" + (i + 1)] == null)
                                {
                                    _dbContext.PortalRegisterOffs.Remove(obj);
                                    await _dbContext.SaveChangesAsync();
                                    return new FormatedResponse() { MessageCode = CommonMessageCode.REGISTER_LEAVE_DETAIL_NULL, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                                }
                                else
                                {

                                    var manualId = _dbContext.AtTimeTypes.Where(x => x.CODE == model["shiftCode" + (i + 1)].ToString()).FirstOrDefault();
                                    if (manualId != null)
                                    {
                                        var dataDetail = new PORTAL_REGISTER_OFF_DETAIL();
                                        dataDetail.REGISTER_ID = obj.ID;
                                        dataDetail.ID_REGGROUP = obj.ID_REGGROUP;
                                        dataDetail.LEAVE_DATE = obj.DATE_START.Value.AddDays(i);
                                        dataDetail.MANUAL_ID = manualId.ID;
                                        dataDetail.NUMBER_DAY = decimal.Parse(model["day" + (i + 1)].ToString());
                                        dataDetail.TYPE_OFF = ((long?)model["dType" + (i + 1)]);
                                        dataDetail.SHIFT_ID = (long?)model["shiftId" + (i + 1)];
                                        await _dbContext.PortalRegisterOffDetails.AddAsync(dataDetail);
                                    }
                                    else
                                    {
                                        _dbContext.PortalRegisterOffs.Remove(obj);
                                        await _dbContext.SaveChangesAsync();
                                        return new FormatedResponse() { MessageCode = CommonMessageCode.REGISTER_LEAVE_DETAIL_TIMETYPE_NULL, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                                    }

                                }

                            }
                        }
                        // khai báo từng ngày = false
                        else
                        {
                            for (int i = 0; i <= (obj.DATE_END - obj.DATE_START).Value.Days; i++)
                            {
                                var dataDetail = new PORTAL_REGISTER_OFF_DETAIL();
                                dataDetail.REGISTER_ID = obj.ID;
                                dataDetail.ID_REGGROUP = obj.ID_REGGROUP;
                                dataDetail.LEAVE_DATE = obj.DATE_START.Value.AddDays(i);
                                dataDetail.NUMBER_DAY = 1;
                                dataDetail.MANUAL_ID = obj.TYPE_ID;

                                await _dbContext.PortalRegisterOffDetails.AddAsync(dataDetail);
                            }
                        }

                        var objStatus = new
                        {
                            P_EMPLOYEE_ID = obj.EMPLOYEE_ID,
                            P_PROCESS_TYPE = obj.TYPE_CODE,
                            P_ID_REGGROUP = obj.ID_REGGROUP,
                            P_SENDER = obj.EMPLOYEE_ID
                        };
                        try
                        {
                            var dataStatus = QueryData.ExecuteStoreToTable(Procedures.PKG_AT_PROCESS_PRI_PROCESS_APP, objStatus, false);

                            var a = obj.ID;
                            var test = dataStatus.Tables[0].Rows[0]["RESULT"].ToString();
                            if (test == "3")
                            {
                                var portalRegisterOffId = obj.ID;
                                var registerOff = await _dbContext.PortalRegisterOffs.FindAsync(portalRegisterOffId);
                                _dbContext.PortalRegisterOffs.Remove(registerOff!);
                                await _dbContext.SaveChangesAsync();
                                return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.EMP_NOT_HAVE_APPROVE_POS, StatusCode = EnumStatusCode.StatusCode400 };//chưa có người người phê duyệt
                            }
                            else if (test == "2")
                            {
                                var portalRegisterOffId = obj.ID;
                                var registerOff = await _dbContext.PortalRegisterOffs.FindAsync(portalRegisterOffId);
                                _dbContext.PortalRegisterOffs.Remove(registerOff!);
                                await _dbContext.SaveChangesAsync();
                                return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.NOT_HAVE_PROCESS_APPROVE, StatusCode = EnumStatusCode.StatusCode400 };//chưa có thiết lập phê duyệt
                            }
                            else
                            {
                                var employeeIds = test.Split(",");
                                if (employeeIds?.Count() > 0)
                                    for (var i = 0; i < employeeIds.Count(); i++)
                                    {
                                        var user = await _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == Int64.Parse(employeeIds[i])).FirstOrDefaultAsync();
                                        var username = user?.USERNAME;
                                        if (!string.IsNullOrEmpty(username))
                                        {
                                            await _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                                            {
                                                SignalType = "APPROVE_NOTIFICATION",
                                                Message = "Bạn có thông báo mới trên Portal"/*message*/,
                                                Data = new
                                                {

                                                }
                                            });
                                        }

                                    }
                            }

                            await _dbContext.SaveChangesAsync();
                        }
                        catch(Exception ex)
                        {
                            var detail = (from p in _dbContext.PortalRegisterOffDetails.AsNoTracking().Where(p => p.REGISTER_ID == obj.ID)
                                         select p);
                            var noti = (from p in _dbContext.AtNotifications.AsNoTracking().Where(p => p.REF_ID == obj.ID)
                                         select p);
                            _dbContext.PortalRegisterOffs.Remove(obj);
                            _dbContext.PortalRegisterOffDetails.RemoveRange(detail);
                            _dbContext.AtNotifications.RemoveRange(noti);
                            await _dbContext.SaveChangesAsync();
                            return new FormatedResponse() { MessageCode = "NOT_EXISTS_SUITABLE_APPROVE_PROCCESS", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        _dbContext.PortalRegisterOffs.Remove(obj);
                        await _dbContext.SaveChangesAsync();
                        return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                    }


                }

                return new() { InnerBody = model, MessageCode = CommonMessageCode.CREATE_SUCCESS };

            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };

            }
        }


        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, PortalRegisterOffDTO dto, string sid)
        {
            //if (dto.TypeCode == "OFF")
            //{
            //    // Check đóng/mở kỳ công 
            //    // get month + year datestart, dateend
            //    int monthStartDate = dto.DateStart!.Value.Month;
            //    int monthEndDate = dto.DateEnd!.Value.Month;
            //    int yearStartDate = dto.DateStart!.Value.Year;
            //    int yearEndDate = dto.DateEnd!.Value.Year;

            //    if (yearStartDate == yearEndDate)
            //    {
            //        for (int i = monthStartDate; i <= monthEndDate; i++)
            //        {
            //            var salPeriod = await _dbContext.AtSalaryPeriods.Where(s => s.MONTH == i && s.YEAR == yearStartDate).FirstOrDefaultAsync();
            //            var org = await _dbContext.HuEmployees.Where(e => e.ID == dto.EmployeeId).FirstOrDefaultAsync();
            //            var checkLockOrgDateStart = (_dbContext.AtOrgPeriods.Where(p => p.ORG_ID == org!.ORG_ID && p.STATUSCOLEX == 1 && p.PERIOD_ID == salPeriod!.ID)).Count();
            //            if (checkLockOrgDateStart != 0)
            //            {
            //                return new FormatedResponse() { MessageCode = CommonMessageCode.THE_WORK_PERIOD_HAS_CEASED_TO_APPLY, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            //            }
            //        }
            //    }
            //}
            // Check đóng/mở kỳ công 
            // get month + year datestart, dateend
            int monthStartDate = dto.DateStart!.Value.Month;
            int monthEndDate = dto.DateEnd!.Value.Month;
            int yearStartDate = dto.DateStart!.Value.Year;
            int yearEndDate = dto.DateEnd!.Value.Year;

            if (yearStartDate == yearEndDate)
            {
                for (int i = monthStartDate; i <= monthEndDate; i++)
                {
                    var salPeriod = await _dbContext.AtSalaryPeriods.Where(s => s.MONTH == i && s.YEAR == yearStartDate).FirstOrDefaultAsync();
                    var org = await _dbContext.HuEmployees.Where(e => e.ID == dto.EmployeeId).FirstOrDefaultAsync();
                    var checkLockOrgDateStart = (_dbContext.AtOrgPeriods.Where(p => p.ORG_ID == org!.ORG_ID && p.STATUSCOLEX == 1 && p.PERIOD_ID == salPeriod!.ID)).Count();
                    if (checkLockOrgDateStart != 0)
                    {
                        return new FormatedResponse() { MessageCode = CommonMessageCode.THE_WORK_PERIOD_HAS_CEASED_TO_APPLY, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                }
            }

            string reggroupId = Guid.NewGuid().ToString();
            if (dto.TypeCode == "OFF")
            {
                // check nhan vien da dc xep ca hay chua 
                var checkExisistShift = await _dbContext.AtWorksigns.Where(w => w.EMPLOYEE_ID == dto.EmployeeId && w.WORKINGDAY!.Value.Date == dto.DateStart!.Value.Date).FirstOrDefaultAsync();
                if (checkExisistShift == null || checkExisistShift.SHIFT_ID == null)
                {
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.EMPLOYEE_NOT_HAVE_SHIFT, StatusCode = EnumStatusCode.StatusCode400 };
                }

                // check trùng khoảng thời gian với những bản ghi đã được phê duyệt
                TimeSpan rangeDayOff = dto.DateEnd!.Value - dto.DateStart!.Value;
                List<DateTime> days = new List<DateTime>();
                for (int i = 0; i <= rangeDayOff.Days; i++)
                {
                    days.Add((dto.DateStart.Value).AddDays(i));
                }
                foreach (var dateTime in days)
                {
                    int checkExists = _dbContext.AtRegisterLeaves.Where(w => w.DATE_START!.Value.Date <= dateTime.Date && dateTime.Date <= w.DATE_END!.Value.Date && w.EMPLOYEE_ID == dto.EmployeeId).Count();
                    var checkRegisterOffEmp = _dbContext.PortalRegisterOffs.AsNoTracking().Where(p => p.EMPLOYEE_ID == dto.EmployeeId && p.TYPE_CODE == "OFF" && p.DATE_START!.Value.Date <= dateTime.Date && dateTime.Date <= p.DATE_END!.Value.Date && (p.STATUS_ID == 1 || p.STATUS_ID == null)).Any();
                    var dayOfWeek = dateTime.DayOfWeek;
                    var checkHoliday = _dbContext.AtHolidays.AsNoTracking().Where(p => p.START_DAYOFF.Date <= dateTime.Date && dateTime.Date <= p.END_DAYOFF.Date && p.IS_ACTIVE == true).Any();

                    if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday || checkHoliday)
                    {
                        // T7, CN, NGAY LE
                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.REGISTRATION_TIME_EXISTS, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                    if (checkRegisterOffEmp)
                    {
                        // da dang ki nhung chua duoc phe duyet
                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.REGISTRATION_TIME_EXISTS, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                    if (checkExists > 0)
                    {
                        // da duoc phe duyet
                        return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DATE_HAS_APPROVED };
                    }
                }

                // check đăng ký phép không được quá số ngày phép còn lại
                // lấy ra số ngày phép còn lại
                DateTime now = DateTime.Now;
                var currentMonth = now.Month;
                var currentYear = now.Year;
                var res = await _dbContext.AtEntitlements.Where(e => e.EMPLOYEE_ID == dto.EmployeeId && e.YEAR == currentYear && e.MONTH == currentMonth).FirstOrDefaultAsync();
                double remainingDay = 0;
                if (res != null)
                {
                    remainingDay = res.TOTAL_HAVE!.Value;
                }

                int range = dto.DateEnd!.Value.Day - dto.DateStart!.Value.Day;
                // lấy ra loại đăng ký
                var registerCode = await _dbContext.AtTimeTypes.Where(t => t.ID == dto.TimeTypeId).FirstOrDefaultAsync();
                if (range + 1 > remainingDay && (registerCode!.CODE == "P" || registerCode!.CODE.Contains("/P") || registerCode!.CODE.Contains("P/")))
                {
                    return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.NOT_ENOUGH_DAY_OFF };
                }

                dto.TotalDay = range + 1;
            }
            if (dto.TypeCode == "OVERTIME")
            {
                dto.DateStart = dto.WorkingDay!.Value;
                dto.DateEnd = dto.WorkingDay!.Value;
                if (dto.TimeStart > dto.TimeEnd && (dto.IsWorkingOvernight == false || dto.IsWorkingOvernight == null))
                {
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.TIMESTART_MUST_LESS_THAN_TIMEEND, StatusCode = EnumStatusCode.StatusCode400 };
                }
                if (dto.TimeStart > dto.TimeEnd && dto.IsWorkingOvernight == true)
                {
                    dto.TimeEnd = dto.TimeEnd!.Value.AddDays(1);
                    dto.DateEnd = dto.WorkingDay!.Value.AddDays(1);
                }
                TimeSpan diffOfDates = dto.TimeEnd!.Value.Subtract(dto.TimeStart!.Value);

                //// check dki duoi 120p
                //double totalMinutes = diffOfDates.TotalMinutes;
                //if (totalMinutes < 120)
                //{
                //    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.NOT_ENOUGH_TIME_MINIMUM, StatusCode = EnumStatusCode.StatusCode400 };
                //}
                //else
                //{
                //    dto.TotalOt = (decimal)totalMinutes;
                //}
                dto.TotalOt = (decimal)diffOfDates.TotalMinutes;
                // Check trung gio lam 
                var atWorking = _uow.Context.Set<AT_WORKSIGN>().AsNoTracking().AsQueryable();   // xep ca
                var atShift = _uow.Context.Set<AT_SHIFT>().AsNoTracking().AsQueryable();        // ca lam viec

                //check nv co trung ca lam viec -fail
                //var ws = await atWorking.AsNoTracking().Where(x => x.EMPLOYEE_ID == dto.EmployeeId && x.WORKINGDAY!.Value.Date == dto.WorkingDay!.Value.Date).ToListAsync();
                var dayOfWeek = dto.WorkingDay!.Value.DayOfWeek;
                var checkHoliday = _dbContext.AtHolidays.AsNoTracking().Where(p => p.START_DAYOFF.Date <= dto.WorkingDay.Value.Date && dto.WorkingDay.Value.Date <= p.END_DAYOFF.Date && p.IS_ACTIVE == true).Any();

                if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday || checkHoliday)
                {
                    // T7, CN, NGAY LE
                }
                else
                {
                    var c = await (from p in atWorking.AsNoTracking().Where(x => x.EMPLOYEE_ID == dto.EmployeeId && x.WORKINGDAY!.Value.Date == dto.WorkingDay!.Value.Date).DefaultIfEmpty()
                                   from s in atShift.AsNoTracking().Where(x => x.ID == p.SHIFT_ID).DefaultIfEmpty()
                                   select s.HOURS_STOP).FirstOrDefaultAsync();           // lay dc gio ket thuc ca lm vc
                    if (c != null)
                    {
                        //TimeSpan timeShfitStop = TimeSpan.Parse(c);     // h ket thuc ca
                        TimeSpan timeShiftStop = new TimeSpan(c.Hour, c.Minute, c.Second);// h ket thuc ca
                        DateTime dateTime = dto.TimeStart!.Value;       // h bat dau dki
                        TimeSpan timeRegisterStart = new TimeSpan(dateTime.Hour, dateTime.Minute, dateTime.Second);
                        if (timeRegisterStart <= timeShiftStop)
                        {
                            return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.NO_OVERTIME_REGISTRATION_DURING_THE_WORKING_SHIFT_OVERLAP, StatusCode = EnumStatusCode.StatusCode400 };           //gio lam them khong duoc trong ca lam viec
                        }
                    }
                }


                // check trung gio dang ky
                var checkTime = (_dbContext.PortalRegisterOffs.AsNoTracking()
                                    .Where(p => p.EMPLOYEE_ID == dto.EmployeeId && p.TYPE_CODE == "OVERTIME"
                                            && ((p.DATE_START!.Value.Date == dto.WorkingDay!.Value.Date || p.DATE_END!.Value.Date == dto.WorkingDay!.Value.Date)
                                                || (p.DATE_START!.Value.Date <= dto.WorkingDay!.Value.Date && dto.WorkingDay!.Value.Date <= p.DATE_END!.Value.Date))
                                            && (p.TIME_START <= dto.TimeStart && dto.TimeStart <= p.TIME_END || p.TIME_START <= dto.TimeEnd && dto.TimeEnd <= p.TIME_END)
                                            && (p.STATUS_ID == 1 || p.STATUS_ID == null))).Count();

                var checkRegisterInWebApp = (_dbContext.AtOvertimes.AsNoTracking().Where(p => (p.EMPLOYEE_ID == dto.EmployeeId) &&
                                                                                              (p.START_DATE!.Value.Date <= dto.WorkingDay!.Value.Date && dto.WorkingDay!.Value.Date <= p.END_DATE!.Value.Date) &&
                                                                                              (p.TIME_START <= dto.TimeStart && dto.TimeStart <= p.TIME_END || p.TIME_START <= dto.TimeEnd && dto.TimeEnd <= p.TIME_END))
                                            ).Count();
                if (checkTime != 0 || checkRegisterInWebApp != 0)
                {
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.REGISTRATION_TIME_EXISTS, StatusCode = EnumStatusCode.StatusCode400 };
                }

                // check nhan vien da dc xep ca hay chua 
                var checkExisistShift = await _dbContext.AtWorksigns.Where(w => w.EMPLOYEE_ID == dto.EmployeeId && w.WORKINGDAY!.Value.Date == dto.WorkingDay!.Value.Date).FirstOrDefaultAsync();
                if (checkExisistShift == null || checkExisistShift.SHIFT_ID == null)
                {
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.EMPLOYEE_NOT_HAVE_SHIFT, StatusCode = EnumStatusCode.StatusCode400 };
                }

                // check ky cong dong/mo
                var salaryPeriod = await _dbContext.AtSalaryPeriods.Where(s => s.START_DATE <= dto.WorkingDay && dto.WorkingDay <= s.END_DATE).FirstOrDefaultAsync();

                var employee = await _dbContext.HuEmployees.Where(e => e.ID == dto.EmployeeId).FirstOrDefaultAsync();
                var period = await _dbContext.AtOrgPeriods.Where(x => x.PERIOD_ID == salaryPeriod!.ID && x.STATUSCOLEX == 1 && x.ORG_ID == employee!.ORG_ID).CountAsync();
                if (period != 0)
                {
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "THE_WORK_PERIOD_HAS_CEASED_TO_APPLY", StatusCode = EnumStatusCode.StatusCode400 };//ky cong da ngung ap dung
                }

                // Nhân viên không được đăng ký quá số giờ làm thêm theo tháng
                var hourToltal = _uow.Context.Set<AT_OTHER_LIST>().AsNoTracking();
                var hourM = (from p in hourToltal.OrderByDescending(x => x.ID).Where(x => x.IS_ACTIVE == true) select p.MAX_WORKING_MONTH).First();//tong gio lam them thang
                var hourY = (from p in hourToltal.OrderByDescending(x => x.ID).Where(x => x.IS_ACTIVE == true) select p.MAX_WORKING_YEAR).First();//tong gio lam them nam
                var h1 = _dbContext.AtOvertimes.AsNoTracking().Where(x => x.EMPLOYEE_ID == dto.EmployeeId).ToList();
                int ms = dto.WorkingDay!.Value.Month;
                int ys = dto.WorkingDay!.Value.Year;
                float toltalH = 0;
                float toltalHY = 0;
                foreach (var hour in h1)//so gio lam them da dang ky
                {
                    //lay ban ghi cung thang, nam
                    int m1 = hour.START_DATE!.Value.Month;
                    int y1 = hour.START_DATE!.Value.Year;
                    int m2 = hour.END_DATE!.Value.Month;
                    int y2 = hour.END_DATE!.Value.Year;
                    if (ms == m1 && ys == y1)
                    {
                        //so gio lam them 1 ca/day
                        DateTime timeS = hour.TIME_START.Value;
                        DateTime timeE = hour.TIME_END.Value;
                        TimeSpan numTimeS = new TimeSpan(timeS.Hour, timeS.Minute, timeS.Second);
                        TimeSpan numTimeE = new TimeSpan(timeE.Hour, timeE.Minute, timeE.Second);
                        TimeSpan numHour = (numTimeE - numTimeS);
                        float toltalHour = (float)numHour.TotalHours;

                        //so gio lam them thang
                        DateTime dateE = hour.END_DATE!.Value.Date;
                        DateTime dateS = hour.START_DATE!.Value.Date;
                        TimeSpan day = (dateE - dateS);
                        float numDays = (float)day.TotalDays;
                        if (numDays == 0) numDays = 1;
                        toltalH += (float)(toltalHour * numDays);
                    }
                    if (ys == y1)
                    {
                        //so gio lam them 1 ca/day
                        DateTime timeS = hour.TIME_START.Value;
                        DateTime timeE = hour.TIME_END.Value;
                        TimeSpan numTimeS = new TimeSpan(timeS.Hour, timeS.Minute, timeS.Second);
                        TimeSpan numTimeE = new TimeSpan(timeE.Hour, timeE.Minute, timeE.Second);
                        TimeSpan numHour = (numTimeE - numTimeS);
                        float toltalHourY = (float)numHour.TotalHours;

                        //so gio lam them thang
                        DateTime dateE = hour.END_DATE!.Value.Date;
                        DateTime dateS = hour.START_DATE!.Value.Date;
                        TimeSpan day = (dateE - dateS);
                        float numDays = (float)day.TotalDays;
                        if (numDays == 0) numDays = 1;
                        toltalHY += (float)(toltalHourY * numDays);
                    }
                }

                //so gio lam them dky
                DateTime sDateCr = dto.WorkingDay.Value.Date;
                DateTime eDateCr = dto.WorkingDay.Value.Date;
                DateTime sh = dto.TimeStart.Value; TimeSpan snumH = new TimeSpan(sh.Hour, sh.Minute, sh.Second);
                DateTime eh = dto.TimeEnd.Value; TimeSpan enumH = new TimeSpan(eh.Hour, eh.Minute, eh.Second);
                int sc = sDateCr.DayOfYear; TimeSpan numH = (enumH - snumH);
                int ec = eDateCr.DayOfYear;
                float numDayDky = (ec - sc) + 1; float numHourDky = (float)numH.TotalHours;
                toltalH += (numHourDky * numDayDky);
                if (toltalH > hourM)
                {
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.OVERTIME_HOURS_EXCEEDED_THIS_MONTH, StatusCode = EnumStatusCode.StatusCode400 };//qua so gio lam them trong thang
                }
                if (toltalHY > hourY)
                {
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.OVERTIME_HOURS_EXCEEDED_THIS_YEAR, StatusCode = EnumStatusCode.StatusCode400 };//qua so gio lam them trong nam
                }
            }
            if (dto.TypeCode == "EXPLAINWORK")
            {
                dto.WorkingDay = dto.WorkingDay!.Value.AddDays(1);
                var checkExplan = _dbContext.AtTimeExplanations.AsNoTracking().Where(p => p.EXPLANATION_DAY!.Value.Date == dto.WorkingDay.Value.Date && dto.EmployeeId == p.EMPLOYEE_ID).Any();
                var checkRegis = _dbContext.PortalRegisterOffs.AsNoTracking().Where(p => p.TYPE_CODE == "EXPLAINWORK" && p.WORKING_DAY!.Value.DayOfYear == dto.WorkingDay.Value.DayOfYear && dto.EmployeeId == p.EMPLOYEE_ID).Any();

                var salaryPeriod = await _dbContext.AtSalaryPeriods.Where(s => s.START_DATE <= dto.WorkingDay && s.END_DATE <= dto.WorkingDay).FirstOrDefaultAsync();

                var org = await _dbContext.HuEmployees.Where(e => e.ID == dto.EmployeeId).FirstOrDefaultAsync();
                var period = await _dbContext.AtOrgPeriods.Where(x => x.PERIOD_ID == salaryPeriod!.ID && x.STATUSCOLEX == 1 && x.ORG_ID == org!.ID).AnyAsync();
                if (period)
                {
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "THE_WORK_PERIOD_HAS_CEASED_TO_APPLY", StatusCode = EnumStatusCode.StatusCode400 };//ky cong da ngung ap dung
                }

                if (checkExplan || checkRegis)
                {
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.WORKING_DAY_IS_EXIST_TIME_EXPLAINWORK, StatusCode = EnumStatusCode.StatusCode400 };//ngay lam viec da dc giai trinh
                }
            }
            dto.IdReggroup = reggroupId;
            var response = await _genericRepository.Create(_uow, dto, sid); // INSERT TO REGISTER OFF
            // IINSERT TO 
            if (response.StatusCode == EnumStatusCode.StatusCode200)
            {
                var obj = new
                {
                    P_EMPLOYEE_ID = response.InnerBody!.GetType().GetProperty("EMPLOYEE_ID")!.GetValue(response.InnerBody, null),
                    P_PROCESS_TYPE = response.InnerBody!.GetType().GetProperty("TYPE_CODE")!.GetValue(response.InnerBody, null),
                    P_ID_REGGROUP = reggroupId,
                    P_SENDER = response.InnerBody!.GetType().GetProperty("EMPLOYEE_ID")!.GetValue(response.InnerBody, null)
                };
                try
                {
                    var data = QueryData.ExecuteStoreToTable(Procedures.PKG_AT_PROCESS_PRI_PROCESS_APP, obj, false);

                    var a = response.InnerBody!.GetType().GetProperty("ID")!.GetValue(response.InnerBody, null);
                    var test = data.Tables[0].Rows[0]["RESULT"].ToString();
                    if (test == "3")
                    {
                        var portalRegisterOffId = response.InnerBody!.GetType().GetProperty("ID")!.GetValue(response.InnerBody, null);
                        var registerOff = await _dbContext.PortalRegisterOffs.FindAsync(portalRegisterOffId);
                        _dbContext.PortalRegisterOffs.Remove(registerOff!);
                        await _dbContext.SaveChangesAsync();
                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.EMP_NOT_HAVE_APPROVE_POS, StatusCode = EnumStatusCode.StatusCode400 };//chưa có người người phê duyệt
                    }
                    else if (test == "2")
                    {
                        var portalRegisterOffId = response.InnerBody!.GetType().GetProperty("ID")!.GetValue(response.InnerBody, null);
                        var registerOff = await _dbContext.PortalRegisterOffs.FindAsync(portalRegisterOffId);
                        _dbContext.PortalRegisterOffs.Remove(registerOff!);
                        await _dbContext.SaveChangesAsync();
                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.NOT_HAVE_PROCESS_APPROVE, StatusCode = EnumStatusCode.StatusCode400 };//chưa có thiết lập phê duyệt
                    }
                    else
                    {
                        var employeeIds = test.Split(",");
                        if (employeeIds?.Count() > 0)
                            for (var i = 0; i < employeeIds.Count(); i++)
                            {
                                var user = await _dbContext.SysUsers.Where(x => x.EMPLOYEE_ID == Int64.Parse(employeeIds[i])).FirstOrDefaultAsync();
                                var username = user?.USERNAME;
                                if (!string.IsNullOrEmpty(username))
                                {
                                    await _hubContext.Clients.User(username).SendAsync("ReceiveMessage", new
                                    {
                                        SignalType = "APPROVE_NOTIFICATION",
                                        Message = "Bạn có thông báo mới trên Portal"/*message*/,
                                        Data = new
                                        {

                                        }
                                    });
                                }

                            }
                    }
                }
                catch (Exception ex)
                {
                    var portalRegisterOffId = response.InnerBody!.GetType().GetProperty("ID")!.GetValue(response.InnerBody, null);

                    var registerOff = await _dbContext.PortalRegisterOffs.FindAsync(portalRegisterOffId);

                    var detail = (from p in _dbContext.PortalRegisterOffDetails.AsNoTracking().Where(p => p.REGISTER_ID == registerOff!.ID)
                                  select p);
                    var noti = (from p in _dbContext.AtNotifications.AsNoTracking().Where(p => p.REF_ID == registerOff!.ID)
                                select p);
                    _dbContext.PortalRegisterOffs.Remove(registerOff!);

                    _dbContext.AtNotifications.RemoveRange(noti);
                    if (detail != null)
                    {
                        _dbContext.PortalRegisterOffDetails.RemoveRange(detail);
                    }
                    _dbContext.SaveChanges();
                    return new()
                    {
                        InnerBody = null,
                        MessageCode = "NOT_EXISTS_SUITABLE_APPROVE_PROCCESS",
                        ErrorType = EnumErrorType.CATCHABLE,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }
            }
            return new()
            {
                InnerBody = response.InnerBody,
                MessageCode = dto.TypeCode == "EXPLAINWORK" ? "EXPLAINWORK_SUCCESS" : "REGISTER_SUCCESS",
            };
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<PortalRegisterOffDTO> dtos, string sid)
        {
            var add = new List<PortalRegisterOffDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, PortalRegisterOffDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<PortalRegisterOffDTO> dtos, string sid, bool patchMode = true)
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

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> GetTotalOtMonth(string id)
        {
            DateTime now = DateTime.Now;
            var currentSalaryPeriod = await _dbContext.AtSalaryPeriods.Where(s => s.START_DATE <= now && now <= s.END_DATE).FirstOrDefaultAsync();
            var empId = (from p in _dbContext.SysUsers where p.ID == id select p.EMPLOYEE_ID).FirstOrDefault();
            var listOt = await _dbContext.AtTimesheetMonthlys.Where(p => p.EMPLOYEE_ID == empId && currentSalaryPeriod!.ID == p.PERIOD_ID).ToListAsync();

            double totalHour = 0;
            foreach (var item in listOt)
            {
                totalHour += (item.TOTAL_OT_WEEKDAY!.Value + item.TOTAL_OT_SUNDAY!.Value + item.TOTAL_OT_HOLIDAY!.Value + item.TOTAL_OT_WEEKNIGHT!.Value + item.TOTAL_OT_SUNDAYNIGTH!.Value + item.TOTAL_OT_HOLIDAY_NIGTH!.Value);
            }
            string totaOtMonth = (TimeSpan.FromMinutes((double)totalHour * 60).ToString(@"hh\:mm"));
            return new FormatedResponse() { InnerBody = new { TotalOtMonth = totaOtMonth } };
        }

        public async Task<FormatedResponse> GetLeaveDay(string id)
        {
            DateTime now = DateTime.Now;
            var currentMonth = now.Month;
            var currentYear = now.Year;
            var empId = (from p in _dbContext.SysUsers where p.ID == id select p.EMPLOYEE_ID).FirstOrDefault();
            var res = await _dbContext.AtEntitlements.Where(e => e.EMPLOYEE_ID == empId && e.YEAR == currentYear && e.MONTH == currentMonth).FirstOrDefaultAsync();
            double remainingDay = 0;
            double used = 0;
            if (res != null)
            {
                remainingDay = res.TOTAL_HAVE ?? 0;// day ve 0 neu null
                used = res.QP_YEARX_USED ?? 0;// day ve 0 neu null
            }
            return new FormatedResponse() { InnerBody = new { RemainingDay = remainingDay, Used = used } };
        }

        public async Task<FormatedResponse> RegisterHistory(string id, DateTime fromDate, DateTime toDate)
        {
            var empId = (from p in _dbContext.SysUsers where p.ID == id select p.EMPLOYEE_ID).FirstOrDefault();

            /*var listRegister = await (from p in _dbContext.PortalRegisterOffs.AsNoTracking()
                                      join pas in _dbContext.ProcessApproveStatuses.AsNoTracking() on p.ID_REGGROUP equals pas.ID_REGGROUP
                                      join t in _dbContext.AtTimeTypes.AsNoTracking() on p.TIME_TYPE_ID equals t.ID
                                      where p.EMPLOYEE_ID == empId
                                      orderby pas.CREATED_DATE*/

            var listRegister = await (from p in _dbContext.PortalRegisterOffs.AsNoTracking().Where(p => p.EMPLOYEE_ID == empId)
                                      from pas in _dbContext.ProcessApproveStatuses.AsNoTracking().Where(pas => p.ID_REGGROUP == pas.ID_REGGROUP)
                                      from t in _dbContext.AtTimeTypes.AsNoTracking().Where(t => p.TIME_TYPE_ID == t.ID).DefaultIfEmpty()
                                      from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == pas.EMPLOYEE_APPROVED).DefaultIfEmpty()
                                      from pos in _dbContext.HuPositions.AsNoTracking().Where(pos => pos.ID == e.POSITION_ID).DefaultIfEmpty()
                                      where p.CREATED_DATE >= fromDate && p.CREATED_DATE <= toDate
                                      orderby pas.ID descending
                                      select new
                                      {
                                          Id = pas.ID,
                                          IdReggroup = p.ID_REGGROUP,
                                          TypeId = p.TYPE_ID,
                                          TimeTypeId = p.TIME_TYPE_ID,
                                          TimeTypeName = t.NAME,
                                          TimeEarly = p.TIME_EARLY,
                                          TimeLate = p.TIME_LATE,
                                          TypeCode = p.TYPE_CODE,
                                          IsOff = p.TYPE_CODE == "OFF" ? true : false,
                                          IsOvertime = p.TYPE_CODE == "OVERTIME" ? true : false,
                                          IsExplainWork = p.TYPE_CODE == "EXPLAINWORK" ? true : false,
                                          Name = p.TYPE_CODE == "OFF" ? CommonMessageCode.REGISTER_OFF : (p.TYPE_CODE == "OVERTIME" ? CommonMessageCode.REGISTER_OVERTIME : CommonMessageCode.REGISTER_EXPLAIN_WORK),
                                          WorkingDay = p.WORKING_DAY,
                                          TotalOt = p.TOTAL_OT,
                                          TimeStart = p.TIME_START == null ? p.TIME_START : p.TIME_START!.Value,//.AddHours(7)
                                          TimeEnd = p.TIME_END == null ? p.TIME_END : p.TIME_END!.Value,//.AddHours(7)
                                          TotalDay = p.TOTAL_DAY,
                                          DateStart = p.DATE_START == null ? p.DATE_START : p.DATE_START!.Value,//.AddHours(7)
                                          DateEnd = p.DATE_END == null ? p.DATE_END : p.DATE_END!.Value,//.AddHours(7)
                                          AppStatus = pas.APP_STATUS,
                                          AppLevel = pas.APP_LEVEL,
                                          Reason = p.NOTE,
                                          ApproveStatus = pas.APP_STATUS == 0 ? CommonMessageCode.WAITING_APPROVED : (pas.APP_STATUS == 1 ? CommonMessageCode.APPROVED : CommonMessageCode.REJECTED),
                                          ApproveId = pas.EMPLOYEE_APPROVED,
                                          ApproveName = pas.EMPLOYEE_APPROVED == null ? "" : e.Profile!.FULL_NAME,
                                          ApprovePos = pos.NAME,
                                          ApproveNote = pas.APP_NOTE,
                                          ApproveDate = pas.APP_DATE,
                                          CreateDate = p.CREATED_DATE,
                                          IsEachDay = p.IS_EACH_DAY,
                                          ListDetail = (from d in _dbContext.PortalRegisterOffDetails.Where(d => d.REGISTER_ID == p.ID).DefaultIfEmpty()
                                                        from t in _dbContext.AtTimeTypes.Where(t => t.ID == d.MANUAL_ID).DefaultIfEmpty()
                                                        from s in _dbContext.AtShifts.Where(s => s.ID == d.SHIFT_ID).DefaultIfEmpty()
                                                        from to in _dbContext.SysOtherLists.Where(x => x.ID == d.TYPE_OFF).DefaultIfEmpty()
                                                        select new
                                                        {
                                                            Date = d.LEAVE_DATE,
                                                            DType = to.NAME,
                                                            Number = d.NUMBER_DAY,
                                                            ShiftName = s.NAME,
                                                            ShiftCode = t.CODE,
                                                        }).ToList()
                                      }).ToListAsync();
            return new FormatedResponse() { InnerBody = listRegister };
        }

        public async Task<FormatedResponse> GetRegisterHistoryById(long id)
        {
            var listRegister = await (from p in _dbContext.PortalRegisterOffs.AsNoTracking()
                                      from y in _dbContext.HuEmployees.AsNoTracking().Where(y => y.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                                      from pas in _dbContext.ProcessApproveStatuses.AsNoTracking().Where(pas => p.ID_REGGROUP == pas.ID_REGGROUP)
                                      from t in _dbContext.AtTimeTypes.AsNoTracking().Where(t => p.TIME_TYPE_ID == t.ID).DefaultIfEmpty()
                                      from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == pas.EMPLOYEE_APPROVED).DefaultIfEmpty()
                                      from pos in _dbContext.HuPositions.AsNoTracking().Where(pos => pos.ID == e.POSITION_ID).DefaultIfEmpty()
                                      where p.ID == id
                                      orderby pas.ID descending
                                      select new
                                      {
                                          Id = pas.ID,
                                          IdReggroup = p.ID_REGGROUP,
                                          TypeId = p.TYPE_ID,
                                          TimeTypeId = p.TIME_TYPE_ID,
                                          TimeTypeName = t.NAME,
                                          TimeEarly = p.TIME_EARLY,
                                          TimeLate = p.TIME_LATE,
                                          TypeCode = p.TYPE_CODE,
                                          IsOff = p.TYPE_CODE == "OFF" ? true : false,
                                          IsOvertime = p.TYPE_CODE == "OVERTIME" ? true : false,
                                          IsExplainWork = p.TYPE_CODE == "EXPLAINWORK" ? true : false,
                                          Name = p.TYPE_CODE == "OFF" ? CommonMessageCode.REGISTER_OFF : (p.TYPE_CODE == "OVERTIME" ? CommonMessageCode.REGISTER_OVERTIME : CommonMessageCode.REGISTER_EXPLAIN_WORK),
                                          WorkingDay = p.WORKING_DAY,
                                          TotalOt = p.TOTAL_OT,
                                          TimeStart = p.TIME_START == null ? p.TIME_START : p.TIME_START!.Value,//.AddHours(7)
                                          TimeEnd = p.TIME_END == null ? p.TIME_END : p.TIME_END!.Value,//.AddHours(7)
                                          TotalDay = p.TOTAL_DAY,
                                          DateStart = p.DATE_START == null ? p.DATE_START : p.DATE_START!.Value,//.AddHours(7)
                                          DateEnd = p.DATE_END == null ? p.DATE_END : p.DATE_END!.Value,//.AddHours(7)
                                          AppStatus = pas.APP_STATUS,
                                          AppLevel = pas.APP_LEVEL,
                                          Reason = p.NOTE,
                                          ApproveStatus = pas.APP_STATUS == 0 ? CommonMessageCode.WAITING_APPROVED : (pas.APP_STATUS == 1 ? CommonMessageCode.APPROVED : CommonMessageCode.REJECTED),
                                          ApproveId = pas.EMPLOYEE_APPROVED,
                                          ApproveName = pas.EMPLOYEE_APPROVED == null ? "" : e.Profile!.FULL_NAME,
                                          ApprovePos = pos.NAME,
                                          ApproveNote = pas.APP_NOTE,
                                          ApproveDate = pas.APP_DATE,
                                          CreateDate = p.CREATED_DATE,
                                          IsEachDay = p.IS_EACH_DAY,
                                          ListDetail = (from d in _dbContext.PortalRegisterOffDetails.Where(d => d.REGISTER_ID == p.ID).DefaultIfEmpty()
                                                        from t in _dbContext.AtTimeTypes.Where(t => t.ID == d.MANUAL_ID).DefaultIfEmpty()
                                                        from s in _dbContext.AtShifts.Where(s => s.ID == d.SHIFT_ID).DefaultIfEmpty()
                                                        from to in _dbContext.SysOtherLists.Where(x => x.ID == d.TYPE_OFF).DefaultIfEmpty()
                                                        select new
                                                        {
                                                            Date = d.LEAVE_DATE,
                                                            DType = to.NAME,
                                                            Number = d.NUMBER_DAY,
                                                            ShiftName = s.NAME,
                                                            ShiftCode = t.CODE,
                                                        }).ToList()
                                      }).ToListAsync();

            return new FormatedResponse() { InnerBody = listRegister };
        }


        public async Task<FormatedResponse> GetRegisterById(long id)
        {
            var query = (from p in _dbContext.PortalRegisterOffs.AsNoTracking().Where(x => x.ID == id).DefaultIfEmpty()
                         from y in _dbContext.HuEmployees.AsNoTracking().Where(y => y.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from pas in _dbContext.ProcessApproveStatuses.AsNoTracking().Where(pas => p.ID_REGGROUP == pas.ID_REGGROUP)
                         from t in _dbContext.AtTimeTypes.AsNoTracking().Where(t => p.TIME_TYPE_ID == t.ID).DefaultIfEmpty()
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == pas.EMPLOYEE_APPROVED).DefaultIfEmpty()
                         from pos in _dbContext.HuPositions.AsNoTracking().Where(pos => pos.ID == e.POSITION_ID).DefaultIfEmpty()
                         where pas.EMPLOYEE_APPROVED != null
                         orderby pas.APP_LEVEL descending
                         select new
                         {
                             Id = p.ID,
                             IdReggroup = p.ID_REGGROUP,
                             TypeId = p.TYPE_ID,
                             TimeTypeId = p.TIME_TYPE_ID,
                             //TimeTypeName = t.NAME,
                             TimeEarly = p.TIME_EARLY,
                             TimeLate = p.TIME_LATE,
                             TypeCode = p.TYPE_CODE,
                             IsOff = p.TYPE_CODE == "OFF" ? true : false,
                             IsOvertime = p.TYPE_CODE == "OVERTIME" ? true : false,
                             IsExplainWork = p.TYPE_CODE == "EXPLAINWORK" ? true : false,
                             Name = p.TYPE_CODE == "OFF" ? CommonMessageCode.REGISTER_OFF : (p.TYPE_CODE == "OVERTIME" ? CommonMessageCode.REGISTER_OVERTIME : CommonMessageCode.REGISTER_EXPLAIN_WORK),
                             WorkingDay = p.WORKING_DAY,
                             TotalOt = p.TOTAL_OT,
                             TimeStart = p.TIME_START == null ? p.TIME_START : p.TIME_START!.Value,//.AddHours(7)
                             TimeEnd = p.TIME_END == null ? p.TIME_END : p.TIME_END!.Value,//.AddHours(7)
                             TotalDay = p.TOTAL_DAY,
                             DateStart = p.DATE_START == null ? p.DATE_START : p.DATE_START!.Value,//.AddHours(7)
                             DateEnd = p.DATE_END == null ? p.DATE_END : p.DATE_END!.Value,//.AddHours(7)
                             AppStatus = pas.APP_STATUS,
                             AppLevel = pas.APP_LEVEL,
                             Reason = p.NOTE,
                             ApproveStatus = pas.APP_STATUS == 0 ? CommonMessageCode.WAITING_APPROVED : (pas.APP_STATUS == 1 ? CommonMessageCode.APPROVED : CommonMessageCode.REJECTED),
                             ApproveId = pas.EMPLOYEE_APPROVED,
                             ApproveName = pas.EMPLOYEE_APPROVED == null ? "" : e.Profile!.FULL_NAME,
                             ApprovePos = pos.NAME,
                             ApproveNote = pas.APP_NOTE,
                             ApproveDate = pas.APP_DATE,
                             CreateDate = p.CREATED_DATE,
                             IsEachDay = p.IS_EACH_DAY,
                             ListDetail = (from d in _dbContext.PortalRegisterOffDetails.Where(d => d.REGISTER_ID == p.ID).DefaultIfEmpty()
                                           from t in _dbContext.AtTimeTypes.Where(t => t.ID == d.MANUAL_ID).DefaultIfEmpty()
                                           from s in _dbContext.AtShifts.Where(s => s.ID == d.SHIFT_ID).DefaultIfEmpty()
                                           from to in _dbContext.SysOtherLists.Where(x => x.ID == d.TYPE_OFF).DefaultIfEmpty()
                                           select new
                                           {
                                               Date = d.LEAVE_DATE,
                                               DType = to.NAME,
                                               Number = d.NUMBER_DAY,
                                               ShiftName = s.NAME,
                                               ShiftCode = t.CODE,
                                           }).ToList()
                         }).Take(1);

            return new FormatedResponse() { InnerBody = query };
        }
    }
}

