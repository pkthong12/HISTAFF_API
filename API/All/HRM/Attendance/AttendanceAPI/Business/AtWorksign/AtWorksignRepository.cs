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
using System.Dynamic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Hangfire.Storage;
using System;
using System.Data;
using ProfileDAL.ViewModels;
using System.Collections.Generic;
using System.Drawing.Printing;

namespace API.Controllers.AtWorksign
{
    public class AtWorksignRepository : IAtWorksignRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<AT_WORKSIGN, AtWorksignDTO> _genericRepository;
        private readonly GenericReducer<AT_WORKSIGN, AtWorksignDTO> _genericReducer;
        protected AbsQueryDataTemplate QueryData;

        public AtWorksignRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<AT_WORKSIGN, AtWorksignDTO>();
            _genericReducer = new();
            QueryData = new SqlQueryDataTemplate(_dbContext);
        }


        public async Task<FormatedResponse> GetShiftDefault(AtWorksignDTO param)
        {
            try
            {
                // kiểm tra phòng ban kỳ công lương bị khóa hay không
                var checkLockOrg = (_dbContext.AtOrgPeriods.Where(p => p.ORG_ID == param.ListOrgIds![0] && p.STATUSCOLEX == 1 && p.PERIOD_ID == param.PeriodId)).Count();
                if (checkLockOrg != 0)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.THE_WORK_PERIOD_HAS_CEASED_TO_APPLY, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
                /*var ds = await QueryData.ExecuteNonQuery("PKG_TIMESHEET_GET_SIGN_DEFAULT",
                            new
                            {
                                P_USERNAME = _dbContext.UserName,
                                P_CREATEDBY = _dbContext.CurrentUserId,
                                P_ORG_ID = param.ListOrgIds[0],
                                P_PERIOD_ID = param.PeriodId,
                                P_ISDISSOLVE = 0
                            }, true);*/

                // kiểm tra phòng ban đã được thiết lập kỳ công lương
                var checkSalaryPeriod = await _dbContext.AtOrgPeriods.Where(o => o.PERIOD_ID == param.PeriodId && o.ORG_ID == param.ListOrgIds![0]).CountAsync();
                if(checkSalaryPeriod == 0)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.ORG_NOT_HAVE_SALARY_PERIOD, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }

                var r = await QueryData.ExecuteNonQuery("PKG_TIMESHEET_GET_SIGN_DEFAULT",
                    new
                    {
                        P_USERNAME = _dbContext.UserName,
                        P_CREATEDBY = _dbContext.CurrentUserId,
                        P_ORG_ID = param.ListOrgIds![0],
                        P_PERIOD_ID = param.PeriodId,
                        P_ISDISSOLVE = 0,
                    }, false);
                return new() { InnerBody = { }, MessageCode = CommonMessageCode.GET_DATA_SUCCESS };
            }
            catch (Exception ex)
            {
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetList(AtWorksignDTO param)
        {
            try
            {

                var pageNo = param.PageNo == null ? 1 : param.PageNo;
                var pageSize = param.PageSize == null ? 10 : param.PageSize;
                var skip = pageSize * (pageNo - 1);
                var r = await QueryData.ExecutePaging("PKG_TIMESHEET_LIST_WORKSIGN",
                    new
                    {
                        P_IS_ADMIN = _dbContext.IsAdmin == true ? 1 : 0,
                        P_ORG_ID = param.ListOrgIds![0],
                        P_PERIOD_ID = param.PeriodId,
                        P_CODE = param.EmployeeCode == null ? "" : param.EmployeeCode,
                        P_NAME = param.EmployeeName == null ? "" : param.EmployeeName,
                        P_ORG_NAME = param.DepartmentName == null ? "" : param.DepartmentName,
                        P_POS_NAME = param.PositionName == null ? "" : param.PositionName,
                        P_TYPE = "",
                        p_page_no = pageNo,
                        p_page_size = pageSize,
                        P_ISDISSOLVE = 1,
                    }, true);
                long totalRecord = r.Data.Count;
                var result = new
                {
                    Count = totalRecord,
                    List = r.Data.Skip(skip.Value).Take(pageSize.Value),
                    Page = pageNo,
                    PageCount = totalRecord > 0 ? (int)Math.Ceiling(totalRecord / (double)pageSize) : 0,
                    Skip = pageSize * (pageNo - 1),
                    Take = pageSize,
                    MessageCode = CommonMessageCode.QUERY_LIST_SUCCESS,
                };

                return new() { InnerBody = result };
                /*
                var list = new List<object>();
                long totalRecord = 0;
                var pageNo = param.PageNo == null ? 1 : param.PageNo;
                var pageSize =  param.PageSize == null ? 10 : param.PageSize;
                foreach (var id in param.ListOrgIds)
                {
                    var r = await QueryData.ExecutePaging("PKG_TIMESHEET_LIST_WORKSIGN",
                    new
                    {
                        P_IS_ADMIN = _dbContext.IsAdmin == true ? 1 : 0,
                        P_ORG_ID = id,
                        P_PERIOD_ID = param.PeriodId,
                        P_CODE = "",
                        P_NAME = "",
                        P_ORG_NAME = "",
                        P_POS_NAME = "",
                        P_TYPE = "",
                        p_page_no = pageNo,
                        p_page_size = pageSize,
                    }, false);
                    if(r.Data.Count() > 0)
                    {
                        var currentPage = pageNo;
                        foreach(var item in r.Data)
                        {
                            list.Add(item);
                            totalRecord += 1;
                        }
                    }
                }
                var skip = pageSize * (pageNo - 1);
                var res = list.Skip(skip.Value).Take(pageSize.Value).ToList();
                var result = new
                {
                    
                    Count = totalRecord,
                    List = list.Skip(skip.Value).Take(pageSize.Value),
                    Page = pageNo,
                    PageCount = totalRecord > 0 ? (int)Math.Ceiling(totalRecord / (double)pageSize) : 0,
                    Skip = pageSize * (pageNo - 1),
                    Take = pageSize,
                    MessageCode = CommonMessageCode.QUERY_LIST_SUCCESS,
                };
                
                return new() { InnerBody = result };
                */
            }
            catch (Exception ex)
            {
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> GetCurrentPeriodSalary()
        {
            var datetimeNow = DateTime.Now;
            var res = await (from e in _dbContext.AtSalaryPeriods
                             where e.START_DATE.Date <= datetimeNow.Date && e.END_DATE.Date >= datetimeNow.Date
                             select new
                             {
                                 Id = e.ID,
                                 StartDate = e.START_DATE,
                                 EndDate = e.END_DATE,
                             }).FirstOrDefaultAsync();
            return new() { InnerBody = res };
        }

        public async Task<GenericPhaseTwoListResponse<AtWorksignDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtWorksignDTO> request)
        {
            var joined = from p in _dbContext.PaPayrollsheetSums.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new AtWorksignDTO
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
                var list = new List<AT_WORKSIGN>
                    {
                        (AT_WORKSIGN)response
                    };
                var joined = (from l in list
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new AtWorksignDTO
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, AtWorksignDTO dto, string sid)
        {
            try
            {
                var response = new FormatedResponse();
                var listData = new List<object>();

                var saturdayShift = _dbContext.AtShifts.Where(a => a.ID == dto.ShiftId).FirstOrDefault()!.SATURDAY;
                var sundayShift = _dbContext.AtShifts.Where(a => a.ID == dto.ShiftId).FirstOrDefault()!.SUNDAY;
                var currentShift = dto.ShiftId;
                if (dto.StartDate > dto.EndDate)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.START_MUST_LESS_THAN_END, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }

                TimeSpan range = dto.EndDate!.Value - dto.StartDate!.Value;

                List<DateTime> days = new List<DateTime>();

                for (int i = 0; i <= range.Days; i++)
                {
                    days.Add((dto.StartDate.Value).AddDays(i));
                }
                if (dto.EmployeeIds != null)
                {
                    _uow.CreateTransaction();
                    foreach (long i in dto.EmployeeIds)
                    {
                        var empOrg = await (from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == i)
                                            from p in _dbContext.HuPositions.AsNoTracking().Where(p => p.ID == e.POSITION_ID)
                                            from o in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == e.ORG_ID)
                                            select new
                                            {
                                                OrgId = o.ID,
                                            }).FirstOrDefaultAsync();
                        var checkLockOrg = (from p in _dbContext.AtOrgPeriods where p.ORG_ID == empOrg!.OrgId && p.STATUSCOLEX == 1 && p.PERIOD_ID == dto.PeriodId select p).ToList();
                        if (checkLockOrg != null && checkLockOrg.Count() > 0)
                        {
                            return new FormatedResponse() { MessageCode = CommonMessageCode.THE_WORK_PERIOD_HAS_CEASED_TO_APPLY, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                        }
                        var orgId = _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == i).First().ORG_ID;
                        var checkSalaryPeriod = await (from sp in _dbContext.AtSalaryPeriods.AsNoTracking().Where(sp => sp.ID == dto.PeriodId)
                                                       from op in _dbContext.AtOrgPeriods.AsNoTracking().Where(op => op.PERIOD_ID == sp.ID && op.ORG_ID == orgId)
                                                       select new
                                                       {
                                                           Id = sp.ID
                                                       }).CountAsync();
                        if (checkSalaryPeriod == 0)
                        {
                            return new FormatedResponse() { MessageCode = CommonMessageCode.EXISTS_ORG_DO_NOT_HAVE_SALARY_PERIOD, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                        }
                    }
                    foreach (long i in dto.EmployeeIds)
                    {
                        var checkWorking = _dbContext.HuEmployees.AsNoTracking().Where(h => h.WORK_STATUS_ID == OtherConfig.EMP_STATUS_TERMINATE && h.ID == i).FirstOrDefault();
                        if (checkWorking != null)
                        {
                            return new FormatedResponse() { MessageCode = CommonMessageCode.EMP_HAVE_NO_WORKING_STATUS, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                        }
                        // check wage
                        var latestWage = await _dbContext.HuWorkings.Where(p => p.EMPLOYEE_ID == i && p.IS_WAGE != 0 && p.STATUS_ID == OtherConfig.STATUS_APPROVE).OrderBy(p => p.EFFECT_DATE).FirstOrDefaultAsync();
                        if (latestWage == null)
                        {
                            return new FormatedResponse() { MessageCode = CommonMessageCode.EMP_NOT_HAVE_SALARY_PROFILE, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                        }
                        else
                        {
                            if ((latestWage.EFFECT_DATE!.Value.Date > dto.StartDate!.Value.Date))
                            {
                                return new FormatedResponse() { MessageCode = CommonMessageCode.SALARY_PROFILE_EXPIRED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                            }
                        }
                        dto.EmployeeId = i;
                        foreach (var dateTime in days)
                        {
                            int checkExists = _dbContext.AtWorksigns.Where(w => w.WORKINGDAY!.Value.Date == dateTime.Date && w.PERIOD_ID == dto.PeriodId && w.EMPLOYEE_ID == i).Count();
                            if (checkExists > 0)
                            {
                                return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DATE_EXISTS };
                            }
                            else
                            {

                                if (dateTime.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    dto.ShiftId = saturdayShift;
                                }
                                else if (dateTime.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    dto.ShiftId = sundayShift;
                                }
                                else
                                {
                                    dto.ShiftId = currentShift;
                                }

                                dto.Workingday = dateTime;
                                response = await _genericRepository.Create(_uow, dto, sid);
                                if (response.MessageCode != CommonMessageCode.CREATE_SUCCESS)
                                {
                                    return response;
                                }
                                listData.Add(response.InnerBody!);
                            }
                        }
                    }
                }
                _uow.Commit();
                return new() { InnerBody = listData, MessageCode = CommonMessageCode.CREATE_SUCCESS };
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<AtWorksignDTO> dtos, string sid)
        {
            var add = new List<AtWorksignDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, AtWorksignDTO dto, string sid, bool patchMode = true)
        {
            var response = new FormatedResponse();
            var listData = new List<object>();

            var saturdayShift = _dbContext.AtShifts.Where(a => a.ID == dto.ShiftId).FirstOrDefault()!.SATURDAY;
            var sundayShift = _dbContext.AtShifts.Where(a => a.ID == dto.ShiftId).FirstOrDefault()!.SUNDAY;
            var currentShift = dto.ShiftId;

            if (dto.StartDate > dto.EndDate)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.START_MUST_LESS_THAN_END, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }

            //check ky cong lock or unlock
            var orgId = await (from e in _dbContext.HuEmployees.AsNoTracking().Where(x => x.ID == dto.EmployeeId) select e.ORG_ID).FirstAsync();
            var atWorksignId = await (from a in _dbContext.AtWorksigns.AsNoTracking().Where(x => x.EMPLOYEE_ID == dto.EmployeeId && x.PERIOD_ID == dto.PeriodId) select a.PERIOD_ID).FirstAsync();
            var statusColex = await (from o in _dbContext.AtOrgPeriods.AsNoTracking().Where(x => x.ORG_ID == orgId && x.PERIOD_ID == atWorksignId) select o.STATUSCOLEX).FirstAsync();
            if(statusColex == 1)
            {
                return new FormatedResponse() 
                { 
                    MessageCode = CommonMessageCode.THE_WORK_PERIOD_HAS_CEASED_TO_APPLY, 
                    ErrorType = EnumErrorType.CATCHABLE, 
                    StatusCode = EnumStatusCode.StatusCode400 
                };
            }

            TimeSpan range = dto.EndDate!.Value - dto.StartDate!.Value;

            List<DateTime> days = new List<DateTime>();

            for (int i = 0; i <= range.Days; i++)
            {
                days.Add((dto.StartDate.Value).AddDays(i));
            }
            foreach (var dateTime in days)
            {
                var data = await _dbContext.AtWorksigns.Where(w => w.WORKINGDAY!.Value.Date == dateTime.Date && w.PERIOD_ID == dto.PeriodId && w.EMPLOYEE_ID == dto.EmployeeId).FirstOrDefaultAsync();
                if(data == null)
                {
                    dto.Id = null;
                    dto.Workingday = dateTime;
                    response = await _genericRepository.Create(_uow, dto, sid);
                    if (response.MessageCode != CommonMessageCode.CREATE_SUCCESS)
                    {
                        return response;
                    }
                    listData.Add(response.InnerBody!);
                }
                else
                {
                    if (dateTime.DayOfWeek == DayOfWeek.Saturday)
                    {
                        dto.ShiftId = saturdayShift;
                    }
                    else if (dateTime.DayOfWeek == DayOfWeek.Sunday)
                    {
                        dto.ShiftId = sundayShift;
                    }
                    else
                    {
                        dto.ShiftId = currentShift;
                    }

                    dto.Id = data.ID;
                    response = await _genericRepository.Update(_uow, dto, sid, patchMode);
                    if (response.MessageCode != CommonMessageCode.UPDATE_SUCCESS)
                    {
                        return response;
                    }
                    listData.Add(response.InnerBody!);
                }
            }
            return new() { InnerBody = listData, MessageCode = CommonMessageCode.UPDATE_SUCCESS };
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<AtWorksignDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> DeleteWorksigns(GenericUnitOfWork _uow, AtWorksignDTO dto)
        {
            var response = new FormatedResponse();
            var listData = new List<object>();

            if (dto.StartDate > dto.EndDate)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.START_MUST_LESS_THAN_END, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }

            TimeSpan range = dto.EndDate!.Value - dto.StartDate!.Value;

            List<DateTime> days = new List<DateTime>();

            for (int i = 0; i <= range.Days; i++)
            {
                days.Add((dto.StartDate.Value).AddDays(i));
            }
            if(dto.EmployeeIds != null)
            {
                foreach (long e in dto.EmployeeIds)
                {
                    foreach (var dateTime in days)
                    {
                        var data = await _dbContext.AtWorksigns.Where(w => w.EMPLOYEE_ID == e && w.WORKINGDAY!.Value.Date == dateTime.Date).FirstOrDefaultAsync();
                        if (data != null)
                        {
                            dto.Id = data.ID;
                            dto.Workingday = dateTime;
                            response = await _genericRepository.Delete(_uow, data.ID);
                            if (response.MessageCode != CommonMessageCode.DELETE_SUCCESS)
                            {
                                return response;
                            }
                            listData.Add(response.InnerBody!);
                        }
                    }
                }
            }
            
            return new() { InnerBody = listData, MessageCode = CommonMessageCode.DELETE_SUCCESS };
        }

        public async Task<FormatedResponse> GetEmployeeInfo(AtWorksignDTO param)
        {
            try
            {
                var result = await (from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == param.EmployeeId).AsQueryable()
                                    from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                                    from p in _dbContext.HuPositions.AsNoTracking().Where(pos => pos.ID == e.POSITION_ID).DefaultIfEmpty()
                                    from d in _dbContext.HuOrganizations.AsNoTracking().Where(o => o.ID == e.ORG_ID).DefaultIfEmpty()
                                    select new
                                    {
                                        Id = e.ID,
                                        Code = e.CODE,
                                        Name = cv.FULL_NAME,
                                        DepartmentName = d.NAME,
                                        PositionName = p.NAME,
                                    }).FirstOrDefaultAsync();
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

    }
}

