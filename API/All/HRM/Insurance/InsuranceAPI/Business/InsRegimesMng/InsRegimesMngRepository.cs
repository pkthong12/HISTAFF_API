using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.SqlServer;
using Common.Interfaces;
using Common.DataAccess;


namespace API.Controllers.InsRegimesMng
{
    public class InsRegimesMngRepository : IInsRegimesMngRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<INS_REGIMES_MNG, InsRegimesMngDTO> _genericRepository;
        private readonly GenericReducer<INS_REGIMES_MNG, InsRegimesMngDTO> _genericReducer;
        protected AbsQueryDataTemplate QueryData;
        public InsRegimesMngRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<INS_REGIMES_MNG, InsRegimesMngDTO>();
            _genericReducer = new();
            QueryData = new SqlQueryDataTemplate(_dbContext);
        }

        public async Task<GenericPhaseTwoListResponse<InsRegimesMngDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsRegimesMngDTO> request)
        {
            var x = _dbContext.InsRegimesMngs.AsNoTracking().ToList();
            var joined = from p in _dbContext.InsRegimesMngs.AsNoTracking()
                         from r in _dbContext.InsRegimess.AsNoTracking().Where(r => r.ID == p.REGIME_ID).DefaultIfEmpty()
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(emp => emp.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                         from t in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                         from j in _dbContext.HuJobs.AsNoTracking().Where(x => x.ID == t.JOB_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                         from cmp in _dbContext.HuCompanys.AsNoTracking().Where(cmp => cmp.ID == o.COMPANY_ID).DefaultIfEmpty()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         orderby j.ORDERNUM
                         select new InsRegimesMngDTO
                         {
                             Id = p.ID,
                             EmployeeCode = e.CODE,
                             EmployeeId = p.EMPLOYEE_ID,
                             JobOrderNum = (int)(j.ORDERNUM ?? 999),
                             EmployeeName = cv.FULL_NAME,
                             PositionName = t.NAME,
                             OrgName = o.NAME,
                             OrgId = e.ORG_ID,
                             BirthDate = cv.BIRTH_DATE,
                             BirthPlace = cv.BIRTH_PLACE,
                             RegimeId = p.REGIME_ID,
                             RegimeName = r.NAME,
                             FromDate = p.FROM_DATE,
                             ToDate = p.TO_DATE,
                             StartDate = p.START_DATE,
                             EndDate = p.END_DATE,
                             DayCalculator = p.DAY_CALCULATOR,
                             AccumulateDay = p.ACCUMULATE_DAY,
                             ChildrenNo = p.CHILDREN_NO,
                             AverageSalSixMonth = p.AVERAGE_SAL_SIX_MONTH,
                             BhxhSalary = p.BHXH_SALARY,
                             RegimeSalary = p.REGIME_SALARY,
                             SubsidyAmountChange = p.SUBSIDY_AMOUNT_CHANGE,
                             SubsidyMoneyAdvance = p.SUBSIDY_MONEY_ADVANCE,
                             DeclareDate = p.DECLARE_DATE,
                             DateCalculator = p.DATE_CALCULATOR,
                             InsPayAmount = p.INS_PAY_AMOUNT,
                             ApprovDayNum = p.APPROV_DAY_NUM,
                             PayApproveDate = p.PAY_APPROVE_DATE,
                             CreatedBy = p.CREATED_BY,
                             Status = p.STATUS,
                             IsActive = p.IS_ACTIVE,
                             Note = p.NOTE,
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
            try
            {
                var joined = (from p in _dbContext.InsRegimesMngs.AsNoTracking().Where(p => p.ID == id)
                              from i in _dbContext.InsInformations.AsNoTracking().Where(i => i.EMPLOYEE_ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                              from r in _dbContext.InsRegimess.AsNoTracking().Where(r => r.ID == p.REGIME_ID).DefaultIfEmpty()
                              from e in _dbContext.HuEmployees.AsNoTracking().Where(emp => emp.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                              from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(cv => cv.ID == e.PROFILE_ID).DefaultIfEmpty()
                              from t in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                              from o in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                              from cmp in _dbContext.HuCompanys.AsNoTracking().Where(cmp => cmp.ID == o.COMPANY_ID).DefaultIfEmpty()
                              from c in _dbContext.SysUsers.AsNoTracking().Where(c => p.CREATED_BY == null ? false : c.ID == p.CREATED_BY).DefaultIfEmpty()
                              from u in _dbContext.SysUsers.AsNoTracking().Where(u => p.UPDATED_BY == null ? false : u.ID == p.UPDATED_BY).DefaultIfEmpty()
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new InsRegimesMngDTO
                              {
                                  Id = p.ID,
                                  EmployeeCode = e.CODE,
                                  EmployeeId = p.EMPLOYEE_ID,
                                  EmployeeName = cv.FULL_NAME,
                                  PositionName = t.NAME,
                                  OrgName = o.NAME,
                                  BirthDate = cv.BIRTH_DATE,
                                  BirthPlace = cv.BIRTH_PLACE,
                                  InsGroupId = r.INS_GROUP_ID,
                                  BhxhNo = i.BHXH_NO,
                                  RegimeId = p.REGIME_ID,
                                  RegimeName = r.NAME,
                                  FromDate = p.FROM_DATE,
                                  ToDate = p.TO_DATE,
                                  StartDate = p.START_DATE,
                                  EndDate = p.END_DATE,
                                  DayCalculator = p.DAY_CALCULATOR,
                                  AccumulateDay = p.ACCUMULATE_DAY,
                                  ChildrenNo = p.CHILDREN_NO,
                                  AverageSalSixMonth = p.AVERAGE_SAL_SIX_MONTH,
                                  BhxhSalary = p.BHXH_SALARY,
                                  RegimeSalary = p.REGIME_SALARY,
                                  SubsidyAmountChange = p.SUBSIDY_AMOUNT_CHANGE,
                                  SubsidyMoneyAdvance = p.SUBSIDY_MONEY_ADVANCE,
                                  DeclareDate = p.DECLARE_DATE,
                                  DateCalculator = p.DATE_CALCULATOR,
                                  InsPayAmount = p.INS_PAY_AMOUNT,
                                  ApprovDayNum = p.APPROV_DAY_NUM,
                                  PayApproveDate = p.PAY_APPROVE_DATE,
                                  CreatedBy = p.CREATED_BY,
                                  Status = p.STATUS,
                                  IsActive = p.IS_ACTIVE,
                                  Note = p.NOTE,
                                  CreatedLog = p.CREATED_LOG,
                                  CreatedByUsername = c.USERNAME,
                                  CreatedDate = p.CREATED_DATE,
                                  UpdatedBy = p.UPDATED_BY,
                                  UpdatedByUsername = u.USERNAME,
                                  UpdatedDate = p.UPDATED_DATE,
                              }).FirstOrDefault();

                if (joined != null)
                {
                    return new FormatedResponse() { InnerBody = joined };
                }
                else
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.NULLABLE_DATE_TIME_EXTENSION_METHOD_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }

        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, InsRegimesMngDTO dto, string sid)
        {

            var x = _dbContext.InsInformations.AsNoTracking().Where(p => p.EMPLOYEE_ID == dto.EmployeeId).FirstOrDefault();
            if (x == null)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.EMPLOYEE_NOT_EXIST_RECORD_INSU_INFO, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            else
            {
                var c = _dbContext.InsRegimess.AsNoTracking().Where(p => p.ID == dto.RegimeId).FirstOrDefault();
                if(c != null)
                {
                    if(c.TOTAL_DAY < dto.AccumulateDay)
                    {
                        return new FormatedResponse() { MessageCode = CommonMessageCode.ACCUMULATE_DAY_MORE_THAN_TOTAL_DAY, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                }
                if (x.BHYT_EFFECT_DATE > dto.StartDate)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.FROM_DATE_MORE_THAN_EXPIRE_DATE_INS_REGIMES_MNG, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<InsRegimesMngDTO> dtos, string sid)
        {
            var add = new List<InsRegimesMngDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, InsRegimesMngDTO dto, string sid, bool patchMode = true)
        {
            var x = _dbContext.InsInformations.AsNoTracking().Where(p => p.EMPLOYEE_ID == dto.EmployeeId).FirstOrDefault();
            if (x == null)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.EMPLOYEE_NOT_EXIST_RECORD_INSU_INFO, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            else
            {
                var c = _dbContext.InsRegimess.AsNoTracking().Where(p => p.ID == dto.RegimeId).FirstOrDefault();
                if (c != null)
                {
                    if (c.TOTAL_DAY < dto.AccumulateDay)
                    {
                        return new FormatedResponse() { MessageCode = CommonMessageCode.ACCUMULATE_DAY_MORE_THAN_TOTAL_DAY, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                }
                if (x.BHYT_EFFECT_DATE > dto.StartDate)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.FROM_DATE_MORE_THAN_EXPIRE_DATE_INS_REGIMES_MNG, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<InsRegimesMngDTO> dtos, string sid, bool patchMode = true)
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
        public async Task<FormatedResponse> GetRegimesByGroupId(long id)
        {
            var list = await (from o in _dbContext.InsRegimess.AsNoTracking().Where(o => o.IS_ACTIVE == true && o.INS_GROUP_ID == id)
                              from s in _dbContext.SysOtherLists.AsNoTracking().Where(p => p.ID == o.CAL_DATE_TYPE).DefaultIfEmpty()
                              orderby o.ID
                              select new
                              {
                                  Id = o.ID,
                                  Name = o.NAME ?? string.Empty,
                                  Code = s.CODE,
                              }).ToListAsync();
            return new FormatedResponse() { InnerBody = list };
        }

        public async Task<FormatedResponse> GetRegimes()
        {
            var list = await (from o in _dbContext.InsRegimess.AsNoTracking().Where(o => o.IS_ACTIVE == true)
                              orderby o.ID
                              select new
                              {
                                  Id = o.ID,
                                  Name = o.NAME ?? string.Empty
                              }).ToListAsync();
            return new FormatedResponse() { InnerBody = list };
        }
        public async Task<FormatedResponse> GetInforByEmployeeId(long id)
        {
            try
            {
                var joined = await (from c in _dbContext.HuEmployees.AsNoTracking().Where(c => c.ID == id)
                                    from e in _dbContext.HuEmployeeCvs.AsNoTracking().Where(e => e.ID == c.PROFILE_ID).DefaultIfEmpty()
                                        // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                                    select new
                                    {
                                        Id = e.ID,
                                        BhxhNo = e.INSURENCE_NUMBER ?? string.Empty
                                    }).SingleOrDefaultAsync();
                if (joined != null)
                {
                    return new FormatedResponse() { InnerBody = joined, StatusCode = EnumStatusCode.StatusCode200 };
                }
                else
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.NULLABLE_DATE_TIME_EXTENSION_METHOD_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public async Task<FormatedResponse> SpsTienCheDo(InsRegimesMngDTO dto)
        {
            var r = await QueryData.ExecuteList("PKG_SPS_TIEN_CHEDO",
                    new
                    {
                        P_EMPLOYEEID = dto.EmployeeId,
                        P_INSENTILEDKEY = 1,
                        P_NUMOFF = 1,
                        P_ATHOME = 1,
                        P_SALARY_ADJACENT = 1,
                        P_FROMDATE = dto.FromDate,
                        P_SOCON = dto.ChildrenNo
                    }, false);
            return new FormatedResponse() { InnerBody = r[0], StatusCode = EnumStatusCode.StatusCode200 };
        }
        public async Task<FormatedResponse> GetAllGroup()
        {
            var list = await (from o in _dbContext.InsGroups.AsNoTracking().Where(o => o.IS_ACTIVE == true)
                              orderby o.ID
                              select new
                              {
                                  Id = o.ID,
                                  Name = o.NAME ?? string.Empty,
                                  Code = o.CODE
                              }).ToListAsync();
            return new FormatedResponse() { InnerBody = list };
        }
        public async Task<FormatedResponse> GetAccumulateDay(InsRegimesMngDTO dto)
        {
            var calldatetype = (from p in _dbContext.InsRegimess
                                from s in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.CAL_DATE_TYPE)
                                where (s.CODE == "00051" && p.ID == dto.RegimeId)
                                select p).FirstOrDefault();
            if (calldatetype == null)
            {
                var body = new
                {
                    Count = 0,
                    Shifts = ""
                };
                return new FormatedResponse() { InnerBody = body };
            }
            var listShift = await (from w in _dbContext.AtTimeTimesheetDailys.AsNoTracking().Where(p => p.EMPLOYEE_ID == dto.EmployeeId && dto.StartDate.Value.Date <= p.WORKINGDAY.Value.Date && p.WORKINGDAY.Value.Date <= dto.EndDate.Value.Date)
                                   from s in _dbContext.AtShifts.AsNoTracking().Where(p => p.ID == w.SHIFT_ID)
                                   where s.CODE == "OFF" || s.CODE == "L" || s.CODE == "T7" || s.CODE == "CN"
                                   select w.ID).ToListAsync();
            var response = new
            {
                Count = listShift == null ? 0 : listShift.Count,
                Shifts = listShift
            };
            return new FormatedResponse() { InnerBody = response };
        }
        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> SpsTienBH6TH(long? empId, string? date)
        {
            var r = await QueryData.ExecuteList("PKG_INS_BUSINESS_TIENBH6TH",
                    new
                    {
                        P_EMPLOYEEID = empId,
                        P_FROMDATE = (Convert.ToDateTime(date.Replace("_", "/")).Date).AddMonths(-1)
                    }, false);
            return new FormatedResponse() { InnerBody = r[0], StatusCode = EnumStatusCode.StatusCode200 };
        }
    }

}

