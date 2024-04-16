using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Microsoft.EntityFrameworkCore;
using Azure;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Common.Extensions;
using Common.Interfaces;
using Common.DataAccess;
using System.Reflection.Emit;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProfileDAL.ViewModels;
using System;
using CORE.Services.File;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Data.Entity.SqlServer;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Text.RegularExpressions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Linq.Dynamic.Core;
using API.Entities;

namespace API.Controllers.AtDecleareSeniority
{
    public class AtRegisterOverTimeRepository : IAtRegisterOverTimeRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<AT_OVERTIME, AtOvertimeDTO> _genericRepository;
        private readonly GenericReducer<AT_OVERTIME, AtOvertimeDTO> _genericReducer;
        private readonly IWebHostEnvironment _env;
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;

        public AtRegisterOverTimeRepository(FullDbContext context, GenericUnitOfWork uow, IWebHostEnvironment env,
            IOptions<AppSettings> options,
            IFileService fileService)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<AT_OVERTIME, AtOvertimeDTO>();
            _genericReducer = new();
            _env = env;
            _appSettings = options.Value;
            _fileService = fileService;
        }
        #region SinglePhaseQueryList old
        //public async Task<FormatedResponse> SinglePhaseQueryList(GenericQueryListDTO<AtOvertimeDTO> request)
        //{
        //    var joined = from p in _dbContext.AtOvertimes.AsNoTracking()
        //                 from e in _dbContext.HuEmployees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
        //                 from o in _dbContext.HuOrganizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
        //                 from ecv in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
        //                 from po in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
        //                 select new AtOvertimeDTO
        //                 {
        //                     Id = p.ID,
        //                     EmployeeId = p.EMPLOYEE_ID,
        //                     EmployeeCode = e.CODE,
        //                     EmployeeName = ecv.FULL_NAME,
        //                     OrgId = e.ORG_ID,
        //                     OrgName = o.NAME,
        //                     PositionName = po.NAME,
        //                     EndDate = p.END_DATE,
        //                     StartDate = p.START_DATE,
        //                     IsActive = p.IS_ACTIVE,
        //                     Reason = p.REASON,
        //                     TimeEnd = p.TIME_END,
        //                     TimeStart = p.TIME_START,
        //                     TimeEndStr = p.TIME_END == null?"": p.TIME_END.Value.ToString("HH:mm"),
        //                     TimeStartStr = p.TIME_START == null ? "" : p.TIME_START.Value.ToString("HH:mm"),
        //                     FileName = p.FILE_NAME
        //                 };

        //    var searchForHoursStart = "";
        //    var searchForHoursStop = "";
        //    var skip = request.Pagination.Skip;
        //    var take = request.Pagination.Take;
        //    request.Pagination.Skip = 0;
        //    request.Pagination.Take = 9999;
        //    if (request.Search != null)
        //    {
        //        request.Search.ForEach(x =>
        //        {
        //            if (x.Field == "timeStart")
        //            {
        //                searchForHoursStart = x.SearchFor.ToString().ToLower().Trim();
        //                x.SearchFor = "";
        //            }
        //            if (x.Field == "timeEnd")
        //            {
        //                searchForHoursStop = x.SearchFor.ToString().ToLower().Trim();
        //                x.SearchFor = "";
        //            }
        //        });
        //    }

        //    var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
        //    var resultList = new List<AtOvertimeDTO>();

        //    foreach (var i in singlePhaseResult!.List)
        //    {
        //        resultList.Add(i);
        //    }

        //    resultList = resultList.Where(x => (x.TimeStartStr!.Trim().Contains(searchForHoursStart))).ToList();
        //    resultList = resultList.Where(x => (x.TimeEndStr!.Trim().Contains(searchForHoursStop))).ToList();

        //    var result = new
        //    {
        //        Count = resultList.Count(),
        //        List = resultList.Skip(skip).Take(take),
        //        Page = (skip / take) + 1,
        //        PageCount = resultList.Count(),
        //        Skip = skip,
        //        Take = take,
        //        MessageCode = CommonMessageCode.QUERY_LIST_SUCCESS,
        //    };
        //    return new()
        //    {
        //        InnerBody = result,
        //    };
        //}
        #endregion
        public async Task<GenericPhaseTwoListResponse<AtOvertimeDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtOvertimeDTO> request)
        {
            var period = new AT_SALARY_PERIOD();
            var i = 0; var index = 0;

            if (request.InOperators != null)
            {
                request.InOperators.ForEach(x =>
                {
                    if (x.Field == "periodId")
                    {
                        period = _dbContext.AtSalaryPeriods.AsNoTracking().Where(p => x.Values[0] == null ? false : p.ID == (long)x.Values[0]).FirstOrDefault();
                        x.Values = new List<long?>();
                        index = i;
                    }
                    i++;
                });
                request.InOperators.RemoveAt(index);
            }
            var joined = from p in _dbContext.AtOvertimes.AsNoTracking().Where(p => period.ID == 0 ? true : (
                                                              (p.START_DATE!.Value.Date >= period.START_DATE.Date && p.START_DATE.Value.Date <= period.END_DATE.Date) ||
                                                              (p.END_DATE!.Value.Date >= period.START_DATE.Date && p.END_DATE.Value.Date <= period.END_DATE.Date) ||
                                                              (p.START_DATE.Value.Date <= period.START_DATE.Date && p.END_DATE.Value.Date >= period.END_DATE.Date))).DefaultIfEmpty()
                         from e in _dbContext.HuEmployees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                         from ecv in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                         from po in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                         from j in _dbContext.HuJobs.AsNoTracking().Where(x => x.ID == po.JOB_ID).DefaultIfEmpty()
                         orderby j.ORDERNUM
                         select new AtOvertimeDTO
                         {
                             Id = p.ID,
                             EmployeeId = p.EMPLOYEE_ID,
                             EmployeeCode = e.CODE,
                             JobOrderNum = (int)(j.ORDERNUM ?? 999),
                             EmployeeName = ecv.FULL_NAME,
                             OrgId = e.ORG_ID,
                             OrgName = o.NAME,
                             PositionName = po.NAME,
                             EndDate = p.END_DATE,
                             StartDate = p.START_DATE,
                             IsActive = p.IS_ACTIVE,
                             Reason = p.REASON,
                             TimeEnd = p.TIME_END,
                             TimeStart = p.TIME_START,
                             TimeEndStr = p.TIME_END == null ? "" : p.TIME_END.Value.ToString("HH:mm"),
                             TimeStartStr = p.TIME_START == null ? "" : p.TIME_START.Value.ToString("HH:mm"),
                             FileName = p.FILE_NAME
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
                var list = new List<AT_OVERTIME>
                    {
                        (AT_OVERTIME)response
                    };
                var joined = (from p in list
                              select new AtOvertimeDTO
                              {
                                  Id = p.ID,
                                  EmployeeId = p.EMPLOYEE_ID,
                                  EndDate = p.END_DATE,
                                  StartDate = p.START_DATE,
                                  IsActive = p.IS_ACTIVE,
                                  Reason = p.REASON,
                                  TimeEnd = p.TIME_END,
                                  TimeStart = p.TIME_START,
                                  FileName = p.FILE_NAME,
                                  TimeEndStr = p.TIME_END.Value.ToString("HH:mm"),
                                  TimeStartStr = p.TIME_START.Value.ToString("HH:mm")
                              }).FirstOrDefault();


                if (joined != null)
                {
                    if (res.StatusCode == EnumStatusCode.StatusCode200)
                    {
                        var child = (from e in _dbContext.HuEmployees.Where(x => x.ID == joined.EmployeeId).DefaultIfEmpty()
                                     from o in _dbContext.HuOrganizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                     from ecv in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                                     from po in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                     select new
                                     {
                                         EmployeeCode = e.CODE,
                                         EmployeeName = ecv.FULL_NAME,
                                         OrgId = e.ORG_ID,
                                         OrgName = o.NAME,
                                         PositionName = po.NAME
                                     }).FirstOrDefault();
                        joined.EmployeeCode = child?.EmployeeCode;
                        joined.EmployeeName = child?.EmployeeName;
                        joined.OrgId = child?.OrgId;
                        joined.OrgName = child?.OrgName;
                        joined.PositionName = child?.PositionName;
                        return new FormatedResponse() { InnerBody = joined };
                    }
                    else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.GET_LIST_BY_KEY_RESPOSNE_IS_NOT_OF_STATUS_CODE_200, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                }
                else
                {
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.JOINED_QUERY_AFTER_GET_BY_ID_WAS_NULL, StatusCode = EnumStatusCode.StatusCode400 };
                }
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, AtOvertimeDTO dto, string sid)
        {

            _uow.CreateTransaction();
            List<UploadFileResponse> uploadFiles = new();
            var atShift = _uow.Context.Set<AT_SHIFT>().AsNoTracking().AsQueryable();//ca lam viec
            var atShiftSort = _uow.Context.Set<AT_WORKSIGN>().AsNoTracking().AsQueryable();//xep ca
            var huEmployeeCv = _uow.Context.Set<HU_EMPLOYEE_CV>().AsNoTracking().AsQueryable();//nv inf
            var huEmployee = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking();//nv

            // First of all we need to upload all the attachments
            if (dto.FirstAttachmentBuffer != null)
            {
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                {
                    ClientFileName = dto.FirstAttachmentBuffer.ClientFileName,
                    ClientFileType = dto.FirstAttachmentBuffer.ClientFileType,
                    ClientFileData = dto.FirstAttachmentBuffer.ClientFileData
                }, location, sid);

                // Assign saved paths
                var property = typeof(AtOvertimeDTO).GetProperty("FileName");

                if (property != null)
                {
                    property?.SetValue(dto, uploadFileResponse.SavedAs);
                    uploadFiles.Add(uploadFileResponse);
                }
                else
                {
                    return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment" };
                }

            }
            var response = await _genericRepository.Create(_uow, dto, sid);
            _uow.Commit();
            return response;
        }

        public async Task<FormatedResponse> CreateAsync(GenericUnitOfWork _uow, AtOvertimeDTO dto, string sid)
        {
            try
            {
                _uow.CreateTransaction();
                List<UploadFileResponse> uploadFiles = new();

                var atShift = _uow.Context.Set<AT_SHIFT>().AsNoTracking().AsQueryable();//ca lam viec

                var lstHoliday = _uow.Context.Set<AT_HOLIDAY>().AsNoTracking().AsQueryable();//ca lam viec
                var atWorksign = _uow.Context.Set<AT_WORKSIGN>().AsNoTracking().AsQueryable();//xep ca
                var atOverTime = _uow.Context.Set<AT_OVERTIME>().AsNoTracking();
                var atSalPeriod = _uow.Context.Set<AT_SALARY_PERIOD>().AsNoTracking();
                var atOrgPeriod = _uow.Context.Set<AT_ORG_PERIOD>().AsNoTracking();//check ky cong
                var totalTime = _uow.Context.Set<AT_SALARY_PERIOD>().AsNoTracking();
                var hourToltal = _uow.Context.Set<AT_OTHER_LIST>().AsNoTracking();
                var huemp = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking();

                var hourM = (from p in hourToltal.OrderByDescending(x => x.ID).Where(x => x.IS_ACTIVE == true) select p.MAX_WORKING_MONTH).First();//tong gio lam them thang
                var hourY = (from p in hourToltal.OrderByDescending(x => x.ID).Where(x => x.IS_ACTIVE == true) select p.MAX_WORKING_YEAR).First();//tong gio lam them nam

                //check nv da duoc xep ca lam viec chua
                if (dto.EmployeeIds != null)
                {
                    foreach (var item in dto.EmployeeIds)
                    {
                        var a = _dbContext.AtWorksigns.AsNoTracking().Where(x => x.EMPLOYEE_ID == item).ToList();//lay nv dc xep ca
                        if (a.Count == 0)
                        {
                            return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.EMPLOYEES_HAVE_NOT_BEEN_SCHEDULED_FOR_SHIFTS, StatusCode = EnumStatusCode.StatusCode400 };//tra mess co nv chua duoc xep ca
                        }
                        var dateT = (from t in atWorksign.Where(x => x.EMPLOYEE_ID == item) orderby t.WORKINGDAY descending select t.WORKINGDAY).ToList().First();//lay den ngay
                        var dateF = (from w in atWorksign.Where(x => x.EMPLOYEE_ID == item) orderby w.WORKINGDAY descending select w.WORKINGDAY).ToList().Last();//lay tu ngay

                        if (dto.StartDate < dateF)
                        {
                            return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "START_DATE_MUST_BE_GREATER_THAN_THE_WORK_SHIFT_ASSIGNMENT_DATE", StatusCode = EnumStatusCode.StatusCode400 };//tu ngay chua dc xep ca
                        }
                        if (dto.EndDate > dateT)
                        {
                            return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "END_DATE_MUST_BE_EARLIER_THAN_THE_WORK_SHIFT_ASSIGNMENT_DATE", StatusCode = EnumStatusCode.StatusCode400 };//den ngay chua dc xep ca
                        }

                        //check ky cong ngung ap dung
                        long? checkperiod = 0;
                        checkperiod = a.First().PERIOD_ID;
                        var o = (from t in huemp.Where(x => x.ID == item) orderby t.ID descending select t.ORG_ID).ToList().FirstOrDefault();
                        var period = await atOrgPeriod.Where(x => x.PERIOD_ID == checkperiod && x.STATUSCOLEX == 1 && x.ORG_ID == o).AnyAsync();
                        if (period)
                        {
                            return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.THE_WORK_PERIOD_HAS_CEASED_TO_APPLY, StatusCode = EnumStatusCode.StatusCode400 };//ky cong da ngung ap dung
                        }

                        //check nv co trung ca lam viec

                        long? shiftID = 0;
                        a.ForEach(p =>
                        {
                            if (p.WORKINGDAY!.Value.ToShortDateString() == dto.StartDate!.Value.ToShortDateString())
                            {
                                shiftID = p.SHIFT_ID;
                            }
                        });

                        var shift = atShift.AsNoTracking().Where(p => p.ID == shiftID).Any();// Check ton tai ca
                        if (!shift)
                        {
                            return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "SHIFT_NOT_EXIST", StatusCode = EnumStatusCode.StatusCode400 };//den ngay chua dc xep ca
                        }

                        var shiftTimeStop = (from s in atShift.AsNoTracking().Where(p => p.ID == shiftID)
                                             select s.HOURS_STOP).First();// lay dc gio ket thuc ca lm vc

                        TimeSpan timeStart = new TimeSpan(dto.TimeStart.Value.Hour, dto.TimeStart.Value.Minute, dto.TimeStart.Value.Second);
                        TimeSpan timeStop = new TimeSpan(shiftTimeStop.Hour, shiftTimeStop.Minute, shiftTimeStop.Second);


                        var lstHld = lstHoliday.AsNoTracking().Where(x => x.IS_ACTIVE == true && ((dto.StartDate.Value.Date >= x.START_DAYOFF.Date && dto.StartDate.Value.Date <= x.END_DAYOFF.Date) || (dto.EndDate.Value.Date >= x.START_DAYOFF.Date && dto.EndDate.Value.Date <= x.END_DAYOFF.Date))).ToList();


                            var shiftE = atShift.AsNoTracking().Where(p => p.ID == shiftID).FirstOrDefault();

                            if (shiftE.CODE != "T7" && shiftE.CODE != "CN" && shiftE.CODE != "OFF")
                            {
                                if (shiftTimeStop != null)
                                {
                                    if (timeStart < timeStop && lstHld.Count==0)
                                    {
                                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "NO_OVERTIME_REGISTRATION_DURING_THE_WORKING_SHIFT_OVERLAP", StatusCode = EnumStatusCode.StatusCode400 };//gio lam them khong duoc trong ca lam viec
                                    }
                                }
                            }
                        


                        //check dang ky nhieu ngay
                        var dateStart1 = DateTime.Parse(dto.StartDate.Value.ToShortDateString());
                        var dateEnd1 = DateTime.Parse(dto.EndDate.Value.ToShortDateString());

                        TimeSpan range = dateEnd1 - dateStart1;

                        List<DateTime> days = new List<DateTime>();

                        for (int i = 0; i <= range.Days; i++)
                        {
                            days.Add((dateStart1).AddDays(i));
                        }
                        foreach (var datetime in days)
                        {
                            var dateStartOneDay = new DateTime(datetime.Year, datetime.Month, datetime.Day, dto.TimeStart.Value.Hour, dto.TimeStart.Value.Minute, dto.TimeStart.Value.Second);
                            var dateEndOneDay = new DateTime(datetime.Year, datetime.Month, datetime.Day, dto.TimeEnd.Value.Hour, dto.TimeEnd.Value.Minute, dto.TimeEnd.Value.Second);
                            if (dto.TimeStart > dto.TimeEnd)
                            {
                                dateEndOneDay.AddDays(1);
                            }

                            long? shiftIdOneDay = 0;
                            a.ForEach(p =>
                            {
                                if (p.WORKINGDAY!.Value.ToShortDateString() == dateEndOneDay.ToShortDateString())
                                {
                                    shiftIdOneDay = p.SHIFT_ID;
                                }
                            });

                            var shiftTimeStopOneDay = (from s in atShift.AsNoTracking().Where(p => p.ID == shiftIdOneDay)
                                                       select s).First();
                            TimeSpan timeStartOneDay = new TimeSpan(shiftTimeStopOneDay.HOURS_START.Hour, shiftTimeStopOneDay.HOURS_START.Minute, shiftTimeStopOneDay.HOURS_START.Second);
                            TimeSpan timeStopOneDay = new TimeSpan(shiftTimeStopOneDay.HOURS_STOP.Hour, shiftTimeStopOneDay.HOURS_STOP.Minute, shiftTimeStopOneDay.HOURS_STOP.Second);
                            //check gio ket thuc trung ca moi
                            var timeStopCheck = new TimeSpan(dateEndOneDay.Hour, dateEndOneDay.Minute, dateEndOneDay.Second);
                            if (shiftTimeStopOneDay.CODE != "T7" && shiftTimeStopOneDay.CODE != "CN" && shiftTimeStopOneDay.CODE != "OFF")
                            {
                                if (timeStartOneDay <= timeStopCheck && timeStopCheck <= timeStopOneDay && lstHld.Count == 0)
                                {
                                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "NO_OVERTIME_REGISTRATION_DURING_THE_WORKING_SHIFT_OVERLAP", StatusCode = EnumStatusCode.StatusCode400 };//gio lam them khong duoc trong ca lam viec
                                }
                            }

                        }

                        //check tong gio lam them cua nv trong thang
                        var h1 = _dbContext.AtOvertimes.AsNoTracking().Where(x => x.EMPLOYEE_ID == item).ToList();
                        int ms = dto.StartDate!.Value.Month;
                        int me = dto.EndDate!.Value.Month;
                        int ys = dto.StartDate!.Value.Year;
                        int ye = dto.EndDate!.Value.Year;
                        float toltalH = 0;
                        float toltalHY = 0;
                        foreach (var hour in h1)//so gio lam them da dang ky
                        {
                            //lay ban ghi cung thang, nam
                            int m1 = hour.START_DATE!.Value.Month;
                            int y1 = hour.START_DATE!.Value.Year;
                            int m2 = hour.END_DATE!.Value.Month;
                            int y2 = hour.END_DATE!.Value.Year;
                            if (ms == m1 && me == m2 && ys == y1 && ye == y2)
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
                                float numDays = (float)day.TotalDays + 1;
                                //if (numDays == 0) numDays = 1;
                                toltalH += (float)(toltalHour * numDays);
                            }
                            if (ys == y1 && ye == y2)
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
                                float numDays = (float)day.TotalDays + 1;
                                //if (numDays == 0) numDays = 1;
                                toltalHY += (float)(toltalHourY * numDays);
                            }
                        }
                        //so gio lam them dky
                        DateTime sDateCr = dto.StartDate.Value.Date;
                        DateTime eDateCr = dto.EndDate.Value.Date;
                        DateTime sh = dto.TimeStart.Value; TimeSpan snumH = new TimeSpan(sh.Hour, sh.Minute, sh.Second);
                        DateTime eh = dto.TimeEnd.Value; TimeSpan enumH = new TimeSpan(eh.Hour, eh.Minute, eh.Second);
                        int sc = sDateCr.DayOfYear; TimeSpan numH = (enumH - snumH);
                        int ec = eDateCr.DayOfYear;
                        float numDayDky = (ec - sc) + 1; float numHourDky = (float)numH.TotalHours;
                        toltalH += (numHourDky * numDayDky);
                        toltalHY += (numHourDky * numDayDky);
                        if (toltalH > hourM)
                        {
                            return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "OVERTIME_HOURS_EXCEEDED_THIS_MONTH", StatusCode = EnumStatusCode.StatusCode400 };//qua so gio lam them trong thang
                        }
                        if (toltalHY > hourY)
                        {
                            return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "OVERTIME_HOURS_EXCEEDED_THIS_YEAR", StatusCode = EnumStatusCode.StatusCode400 };//qua so gio lam them trong nam
                        }
                    }
                }

                // First of all we need to upload all the attachments
                if (dto.FirstAttachmentBuffer != null)
                {
                    string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                    var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                    {
                        ClientFileName = dto.FirstAttachmentBuffer.ClientFileName,
                        ClientFileType = dto.FirstAttachmentBuffer.ClientFileType,
                        ClientFileData = dto.FirstAttachmentBuffer.ClientFileData
                    }, location, sid);

                    // Assign saved paths
                    var property = typeof(AtOvertimeDTO).GetProperty("FileName");

                    if (property != null)
                    {
                        property?.SetValue(dto, uploadFileResponse.SavedAs);
                        uploadFiles.Add(uploadFileResponse);
                    }
                    else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment" };
                    }

                }
                foreach (var item in dto.EmployeeIds)
                {
                    ////check dang ky nhieu ngay
                    //var dateStart2 = DateTime.Parse(dto.StartDate!.Value.ToShortDateString());
                    //var dateEnd2 = DateTime.Parse(dto.EndDate!.Value.ToShortDateString());

                    //TimeSpan range = dateEnd2 - dateStart2;

                    //List<DateTime> days = new List<DateTime>();

                    //for (int i = 0; i <= range.Days; i++)
                    //{
                    //    days.Add((dateStart2).AddDays(i));
                    //}
                    //foreach (var x in days)
                    //{
                    //    var s = DateTime.Parse(x.Year + "-" + x.Month + "-" + x.Day + " " + dto.TimeStart!.Value.Hour + ":" + dto.TimeStart.Value.Minute + ":" + dto.TimeStart.Value.Second);
                    //    var e = DateTime.Parse(x.Year + "-" + x.Month + "-" + x.Day + " " + dto.TimeEnd!.Value.Hour + ":" + dto.TimeEnd.Value.Minute + ":" + dto.TimeEnd.Value.Second);
                    //    //if (dto.TimeEnd < dto.TimeStart)
                    //    //{
                    //    //    e.AddDays(1);
                    //    //}
                    //    var data = new AT_OVERTIME();
                    //    data.TIME_START = s;
                    //    data.TIME_END = (dto.TimeEnd < dto.TimeStart)? e.AddDays(1):e;
                    //    data.STATUS_ID = dto.StatusID;
                    //    data.IS_ACTIVE = dto.IsActive;
                    //    data.NOTE = dto.Note;
                    //    data.START_DATE = (DateTime)x;
                    //    data.END_DATE = (dto.TimeEnd < dto.TimeStart) ? (DateTime)x.AddDays(1) : (DateTime)x;
                    //    data.REASON = dto.Reason;
                    //    data.FILE_NAME = dto.FileName;
                    //    data.EMPLOYEE_ID = item;
                    //    data.CREATED_BY = sid;
                    //    data.CREATED_DATE = DateTime.Now;
                    //    data.UPDATED_BY = sid;
                    //    data.UPDATED_DATE = DateTime.Now;
                    //    await _dbContext.AtOvertimes.AddAsync(data);

                    //}
                    var data = new AT_OVERTIME();
                    data.TIME_START = dto.TimeStart;
                    data.TIME_END = dto.TimeEnd;
                    data.STATUS_ID = dto.StatusID;
                    data.IS_ACTIVE = dto.IsActive;
                    data.NOTE = dto.Note;
                    data.START_DATE = (DateTime)dto.StartDate!;
                    data.END_DATE = (DateTime)dto.EndDate!;
                    data.REASON = dto.Reason;
                    data.FILE_NAME = dto.FileName;
                    data.EMPLOYEE_ID = item;
                    data.CREATED_BY = sid;
                    data.CREATED_DATE = DateTime.Now;
                    data.UPDATED_BY = sid;
                    data.UPDATED_DATE = DateTime.Now;
                    await _dbContext.AtOvertimes.AddAsync(data);

                }
                await _dbContext.SaveChangesAsync();
                _uow.Commit();
                return new()
                {
                    MessageCode = CommonMessageCode.CREATE_SUCCESS,
                    InnerBody = dto,
                    StatusCode = EnumStatusCode.StatusCode200
                };
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };

            }


        }


        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<AtOvertimeDTO> dtos, string sid)
        {
            var add = new List<AtOvertimeDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, AtOvertimeDTO dto, string sid, bool patchMode = true)
        {
            _uow.CreateTransaction();
            List<UploadFileResponse> uploadFiles = new();

            // Check đóng/mở kỳ công 
            foreach (var empID in dto.EmployeeIds!)
            {
                // get month + year datestart, dateend
                int monthStartDate = dto.StartDate!.Value.Month;
                int monthEndDate = dto.EndDate!.Value.Month;
                int yearStartDate = dto.StartDate!.Value.Year;
                int yearEndDate = dto.EndDate!.Value.Year;

                if (yearStartDate == yearEndDate)
                {
                    for (int i = monthStartDate; i <= monthEndDate; i++)
                    {
                        var salPeriod = await _dbContext.AtSalaryPeriods.Where(s => s.MONTH == i && s.YEAR == yearStartDate).FirstOrDefaultAsync();
                        var org = await _dbContext.HuEmployees.Where(e => e.ID == empID).FirstOrDefaultAsync();
                        var checkLockOrgDateStart = (_dbContext.AtOrgPeriods.Where(p => p.ORG_ID == org!.ORG_ID && p.STATUSCOLEX == 1 && p.PERIOD_ID == salPeriod!.ID)).Count();
                        if (checkLockOrgDateStart != 0)
                        {
                            return new FormatedResponse() { MessageCode = CommonMessageCode.THE_WORK_PERIOD_HAS_CEASED_TO_APPLY, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                        }
                    }
                }
            }

            var atShift = _uow.Context.Set<AT_SHIFT>().AsNoTracking().AsQueryable();//ca lam viec
            var atWorksign = _uow.Context.Set<AT_WORKSIGN>().AsNoTracking().AsQueryable();//xep ca
            var atOverTime = _uow.Context.Set<AT_OVERTIME>().AsNoTracking();
            var atSalPeriod = _uow.Context.Set<AT_SALARY_PERIOD>().AsNoTracking();
            var totalTime = _uow.Context.Set<AT_SALARY_PERIOD>().AsNoTracking();
            var hourToltal = _uow.Context.Set<AT_OTHER_LIST>().AsNoTracking();

            var hourM = (from p in hourToltal.OrderByDescending(x => x.ID).Where(x => x.IS_ACTIVE == true) select p.MAX_WORKING_MONTH).First();//tong gio lam them thang

            //check nv da duoc xep ca lam viec chua
            if (dto.EmployeeIds != null)
            {
                foreach (var item in dto.EmployeeIds)
                {
                    var a = _dbContext.AtWorksigns.AsNoTracking().Where(x => x.EMPLOYEE_ID == item).ToList();//lay nv dc xep ca
                    if (a.Count == 0)
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.EMPLOYEES_HAVE_NOT_BEEN_SCHEDULED_FOR_SHIFTS, StatusCode = EnumStatusCode.StatusCode400 };//tra mess co nv chua duoc xep ca
                    }
                    var dateT = (from t in atWorksign.Where(x => x.EMPLOYEE_ID == item) orderby t.WORKINGDAY descending select t.WORKINGDAY).ToList().First();//lay den ngay
                    var dateF = (from w in atWorksign.Where(x => x.EMPLOYEE_ID == item) orderby w.WORKINGDAY descending select w.WORKINGDAY).ToList().Last();//lay tu ngay

                    if (dto.StartDate < dateF)
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "START_DATE_MUST_BE_GREATER_THAN_THE_WORK_SHIFT_ASSIGNMENT_DATE", StatusCode = EnumStatusCode.StatusCode400 };//tu ngay chua dc xep ca
                    }
                    else if (dto.EndDate > dateT)
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "END_DATE_MUST_BE_EARLIER_THAN_THE_WORK_SHIFT_ASSIGNMENT_DATE", StatusCode = EnumStatusCode.StatusCode400 };//den ngay chua dc xep ca
                    }

                    //check ky cong ngung ap dung
                    long? checkperiod = 0;
                    a.ForEach(x => checkperiod = x.PERIOD_ID);
                    var period = await atSalPeriod.Where(x => x.ID == checkperiod && x.IS_ACTIVE != true).AnyAsync();
                    if (period)
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "THE_WORK_PERIOD_HAS_CEASED_TO_APPLY", StatusCode = EnumStatusCode.StatusCode400 };//ky cong da ngung ap dung
                    }

                    ////check nv co trung ca lam viec

                    long? shiftID = 0;
                    a.ForEach(p =>
                    {
                        if (p.WORKINGDAY!.Value.ToShortDateString() == dto.StartDate!.Value.ToShortDateString())
                        {
                            shiftID = p.SHIFT_ID;
                        }
                    });

                    var shift = atShift.AsNoTracking().Where(p => p.ID == shiftID).Any();// Check ton tai ca
                    if (!shift)
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "SHIFT_NOT_EXIST", StatusCode = EnumStatusCode.StatusCode400 };//den ngay chua dc xep ca
                    }

                    var shiftTimeStop = (from s in atShift.AsNoTracking().Where(p => p.ID == shiftID)
                                         select s.HOURS_STOP).First();// lay dc gio ket thuc ca lm vc

                    //var c = (from p in atWorking.Where(x => x.EMPLOYEE_ID == item)
                    //         from s in atShift.Where(x => x.ID == p.SHIFT_ID)
                    //         select s.TIME_STOP).First();//lay dc gio ket thuc ca lm vc

                    //DateTime dateTime = dto.TimeStart;
                    TimeSpan timeStart = new TimeSpan(dto.TimeStart.Value.Hour, dto.TimeStart.Value.Minute, dto.TimeStart.Value.Second);
                    TimeSpan timeStop = new TimeSpan(shiftTimeStop.Hour, shiftTimeStop.Minute, shiftTimeStop.Second);
                    var shiftE = atShift.AsNoTracking().Where(p => p.ID == shiftID).FirstOrDefault();
                    if (shiftE.CODE != "T7" && shiftE.CODE != "CN" && shiftE.CODE != "OFF")
                    {
                        if (shiftTimeStop != null)
                        {
                            if (timeStart < timeStop)
                            {
                                return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "NO_OVERTIME_REGISTRATION_DURING_THE_WORKING_SHIFT_OVERLAP", StatusCode = EnumStatusCode.StatusCode400 };//gio lam them khong duoc trong ca lam viec
                            }
                        }
                    }

                    //check dang ky nhieu ngay
                    var dateStart1 = DateTime.Parse(dto.StartDate.Value.ToShortDateString());
                    var dateEnd1 = DateTime.Parse(dto.EndDate.Value.ToShortDateString());

                    TimeSpan range = dateEnd1 - dateStart1;

                    List<DateTime> days = new List<DateTime>();

                    for (int i = 0; i <= range.Days; i++)
                    {
                        days.Add((dateStart1).AddDays(i));
                    }
                    foreach (var datetime in days)
                    {
                        var dateStartOneDay = new DateTime(datetime.Year, datetime.Month, datetime.Day, dto.TimeStart.Value.Hour, dto.TimeStart.Value.Minute, dto.TimeStart.Value.Second);
                        var dateEndOneDay = new DateTime(datetime.Year, datetime.Month, datetime.Day, dto.TimeEnd.Value.Hour, dto.TimeEnd.Value.Minute, dto.TimeEnd.Value.Second);
                        if (dto.TimeStart > dto.TimeEnd)
                        {
                            dateEndOneDay.AddDays(1);
                        }

                        long? shiftIdOneDay = 0;
                        a.ForEach(p =>
                        {
                            if (p.WORKINGDAY!.Value.ToShortDateString() == dateEndOneDay.ToShortDateString())
                            {
                                shiftIdOneDay = p.SHIFT_ID;
                            }
                        });

                        var shiftTimeStopOneDay = (from s in atShift.AsNoTracking().Where(p => p.ID == shiftIdOneDay)
                                                   select s).First();
                        TimeSpan timeStartOneDay = new TimeSpan(shiftTimeStopOneDay.HOURS_START.Hour, shiftTimeStopOneDay.HOURS_START.Minute, shiftTimeStopOneDay.HOURS_START.Second);
                        TimeSpan timeStopOneDay = new TimeSpan(shiftTimeStopOneDay.HOURS_STOP.Hour, shiftTimeStopOneDay.HOURS_STOP.Minute, shiftTimeStopOneDay.HOURS_STOP.Second);
                        //check gio ket thuc trung ca moi
                        var timeStopCheck = new TimeSpan(dateEndOneDay.Hour, dateEndOneDay.Minute, dateEndOneDay.Second);
                        if (shiftTimeStopOneDay.CODE != "T7" && shiftTimeStopOneDay.CODE != "CN" && shiftTimeStopOneDay.CODE != "OFF")
                        {
                            if (timeStartOneDay <= timeStopCheck && timeStopCheck <= timeStopOneDay)
                            {
                                return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "NO_OVERTIME_REGISTRATION_DURING_THE_WORKING_SHIFT_OVERLAP", StatusCode = EnumStatusCode.StatusCode400 };//gio lam them khong duoc trong ca lam viec
                            }
                        }

                    }

                    //check tong gio lam them cua nv trong thang
                    //var h1 = _dbContext.AtOvertimes.AsNoTracking().Where(x => x.EMPLOYEE_ID == item && x.PERIOD_ID == dto.PeriodId).ToList();
                    var h1 = _dbContext.AtOvertimes.AsNoTracking().Where(x => x.EMPLOYEE_ID == item).ToList();
                    int toltalH = 0;
                    foreach (var hour in h1)
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
                        toltalH += (int)(toltalHour * numDays);
                    }
                    if (toltalH > hourM)
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "OVERTIME_HOURS_EXCEEDED_THIS_MONTH", StatusCode = EnumStatusCode.StatusCode400 };//qua so gio lam them trong thang
                    }
                }
            }

            // First of all we need to upload all the attachments
            if (dto.FirstAttachmentBuffer != null)
            {
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                {
                    ClientFileName = dto.FirstAttachmentBuffer.ClientFileName,
                    ClientFileType = dto.FirstAttachmentBuffer.ClientFileType,
                    ClientFileData = dto.FirstAttachmentBuffer.ClientFileData
                }, location, sid);

                // Assign saved paths
                var property = typeof(AtOvertimeDTO).GetProperty("FileName");

                if (property != null)
                {
                    property?.SetValue(dto, uploadFileResponse.SavedAs);
                    uploadFiles.Add(uploadFileResponse);
                }
                else
                {
                    return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment" };
                }

            }
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            _uow.Commit();
            return response;
        }


        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<AtOvertimeDTO> dtos, string sid, bool patchMode = true)
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

    }
}

