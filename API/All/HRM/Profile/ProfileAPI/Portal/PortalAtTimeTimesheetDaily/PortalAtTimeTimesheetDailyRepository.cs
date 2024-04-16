using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using API.All.HRM.Attendance.AttendanceAPI.Business.AtTimeTimesheetDaily;
using System.Data;
using System.Data.SqlClient;
using API.DTO.PortalDTO;

namespace API.Controllers.AtTimeTimesheetDaily
{
    public class PortalAtTimeTimesheetDailyRepository : IPortalAtTimeTimesheetDailyRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<AT_TIME_TIMESHEET_DAILY, AtTimeTimesheetDailyDTO> _genericRepository;
        private readonly GenericReducer<AT_TIME_TIMESHEET_DAILY, AtTimeTimesheetDailyDTO> _genericReducer;

        public PortalAtTimeTimesheetDailyRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<AT_TIME_TIMESHEET_DAILY, AtTimeTimesheetDailyDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<AtTimeTimesheetDailyDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtTimeTimesheetDailyDTO> request)
        {
            var joined = from p in _dbContext.AtTimeTimesheetDailys.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new AtTimeTimesheetDailyDTO
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
                var list = new List<AT_TIME_TIMESHEET_DAILY>
                    {
                        (AT_TIME_TIMESHEET_DAILY)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new AtTimeTimesheetDailyDTO
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
        public async Task<FormatedResponse> GetAttendantNoteByMonth(GetAttendantNoteByMonthDTO request)
        {
            var D1 = new DateTime(request.Year!.Value, request.MonthIndex!.Value + 1, 1);
            var D2 = new DateTime(request.Year!.Value, request.MonthIndex!.Value + 1, request.LastDay!.Value);
            var query = await (from a in _dbContext.AtTimeTimesheetDailys.Where(
                                a => a.WORKINGDAY >= D1 && a.WORKINGDAY <= D2 && a.EMPLOYEE_ID == request.EmployeeId)
                               from s in _dbContext.AtTimeTypes.AsNoTracking().Where(x => x.ID == a.MANUAL_ID).DefaultIfEmpty()
                               select new
                               {
                                   Id = a.ID,
                                   ManualId = s.CODE != "P" && s.CODE != "L" && s.CODE != null ? 343 : a.MANUAL_ID,
                                   ManualCode = (s.CODE == "L") ? "L" :
                                                 (s.CODE == "P" ? "P" :
                                                 (a.LATE != 0 || a.COMEBACKOUT != 0) ? "T" :
                                                 (s.CODE == "X" ? "X" :
                                                 (s.CODE == null || a.VALIN1 == null || a.VALIN4 == null ? "T" : "P"))),
                                   Workingday = a.WORKINGDAY,
                               }
                ).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }
        public async Task<FormatedResponse> GetAttendatByDay(GetAttendantNoteByMonthDTO request)
        {
            DateTime day = new DateTime(request.Year!.Value, request.MonthIndex!.Value + 1, 1);
            DateTime currentDay = day.AddDays(request.Date!.Value - 1);
            var late = await (from a in _dbContext.AtTimeTimesheetDailys.Where(a => a.WORKINGDAY == currentDay && a.EMPLOYEE_ID == request.EmployeeId)
                              from s in _dbContext.AtTimeTypes.AsNoTracking().Where(x => x.ID == a.MANUAL_ID).DefaultIfEmpty()
                              select new
                              {
                                  Id = a.ID,
                                  Workingday = a.WORKINGDAY,
                                  Valin1 = a.VALIN1,
                                  Valin4 = a.VALIN4,
                                  Code = s.CODE ?? "--",
                                  Name = (s.NAME != null) ? ((s.CODE == "P") ? s.NAME : ((a.COMEBACKOUT != 0 || a.LATE != 0) ? "Đi muộn/về sớm" : s.NAME)) : ((a.VALIN1 == null || a.VALIN4 == null) ? CommonMessageCode.MISSING_ATTENDANCE_DATA : s.NAME),
                              }).ToListAsync();

            var minu = _dbContext.AtTimeTimesheetDailys.AsNoTracking().Where(a => a.WORKINGDAY == currentDay && a.EMPLOYEE_ID == request.EmployeeId).FirstOrDefault();
            if (minu != null)
            {
                return new FormatedResponse()
                {
                    InnerBody = new
                    {
                        Info = late,
                        Late = minu.LATE ?? 0,
                        ComebackOut = minu.COMEBACKOUT ?? 0,
                        TotalOt = minu.OT_TOTAL_CONVERT_PAY ?? 0,
                    }
                };
            }
            return new FormatedResponse()
            {
                InnerBody = new
                {
                    Info = new { },
                    Late = 0,
                    ComebackOut = 0
                }
            };
        }

        public async Task<FormatedResponse> GetListSymbolType()
        {
            var result = await (from at in _dbContext.AtTimeTypes.AsNoTracking().AsQueryable()
                                where at.IS_ACTIVE == true
                                select new
                                {
                                    Id = at.ID,
                                    Code = at.CODE,
                                    Name = at.NAME,
                                }).ToListAsync();
            return new FormatedResponse() { InnerBody = result };
        }
        public async Task<FormatedResponse> InsertExplainTime(PortalExplainWorkDTO param, AppSettings appSettings)
        {
            try
            {
                string reggroupId = Guid.NewGuid().ToString();
                string cnnString = appSettings.ConnectionStrings.CoreDb;
                DataSet ds = new();
                using SqlConnection cnn = new(cnnString);
                using SqlCommand cmd = new();
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PKG_AT_PROCESS_PRI_PROCESS_APP";
                if (param != null)
                {
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_EMPLOYEE_ID", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = param.EmployeeId });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_PROCESS_TYPE", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = param.TypeCode });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_ID_REGGROUP", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = reggroupId });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_SENDER", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input, Value = param.EmployeeId });
                }
                SqlDataAdapter da = new(cmd);
                await Task.Run(() => da.Fill(ds));

                var result = new
                {
                    List = ds.Tables[0],
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
        public async Task<FormatedResponse> GetInfoByMonth(GetAttendantNoteByMonthDTO request)
        {
            try
            {
                var D1 = new DateTime(request.Year!.Value, request.MonthIndex!.Value + 1, 1);
                var D2 = new DateTime(request.Year!.Value, request.MonthIndex!.Value + 1, request.LastDay!.Value);
                var period = _dbContext.AtSalaryPeriods.AsNoTracking().Where(p => (p.MONTH == request.MonthIndex + 1) && (p.YEAR == request.Year)).FirstOrDefault();

                var standard = (from e in _dbContext.HuEmployees.AsNoTracking().Where(p => p.ID == request.EmployeeId).DefaultIfEmpty()
                                from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(p => p.ID == e.PROFILE_ID).DefaultIfEmpty()
                                from a in _dbContext.AtPeriodStandards.AsNoTracking().Where(p => p.OBJECT_ID == cv.EMPLOYEE_OBJECT_ID && p.PERIOD_ID == period.ID).DefaultIfEmpty()
                                select a.PERIOD_STANDARD).FirstOrDefault();
                var orther = _dbContext.AtTimesheetMonthlys.AsNoTracking().Where(p => p.PERIOD_ID == period.ID && p.EMPLOYEE_ID == request.EmployeeId).OrderByDescending(p => p.ID).ToList();

                if (orther != null)
                {
                    return new FormatedResponse()
                    {
                        InnerBody = new
                        {
                            Standard = standard ?? 0,
                            RealAt = ((decimal?)orther.Select(x => x.TOTAL_WORKING_XJ).Sum()) ?? 0,
                            PaidAt = orther.Select(x => x.WORKING_X).Sum() ?? 0,
                            PaidLeave = orther.Select(x => x.WORKING_H).Sum() ?? 0,
                            NotPaidLeave = orther.Select(x => x.WORKING_RO).Sum() ?? 0,
                            TardinessEarly = (orther.Select(x => x.TOTAL_COMEBACKOUT).Sum() ?? 0) + (orther.Select(x => x.TOTAL_LATE).Sum() ?? 0),
                        }
                    };


                }
                return new FormatedResponse()
                {
                    InnerBody = new
                    {
                        Standard = 0,
                        RealAt = 0,
                        PaidAt = 0,
                        PaidLeave = 0,
                        NotPaidLeave = 0,
                        TardinessEarly = 0,
                    }
                };
            }
            catch (Exception ex)
            {
                return new FormatedResponse()
                {
                    InnerBody = new
                    {
                        Standard = 0,
                        RealAt = 0,
                        PaidAt = 0,
                        PaidLeave = 0,
                        NotPaidLeave = 0,
                        TardinessEarly = 0,
                    }
                };
            }
        }
    }
}

