using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Common.Interfaces;
using Common.DataAccess;

namespace API.Controllers.PaPayrollsheetSumSub
{
    public class PaPayrollsheetSumSubRepository : IPaPayrollsheetSumSubRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<PA_PAYROLLSHEET_SUM_SUB, PaPayrollsheetSumSubDTO> _genericRepository;
        private readonly GenericReducer<PA_PAYROLLSHEET_SUM_SUB, PaPayrollsheetSumSubDTO> _genericReducer;
        protected AbsQueryDataTemplate QueryData;

        public PaPayrollsheetSumSubRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<PA_PAYROLLSHEET_SUM_SUB, PaPayrollsheetSumSubDTO>();
            _genericReducer = new();
            QueryData = new SqlQueryDataTemplate(_dbContext);
        }

        public async Task<FormatedResponse> GetDynamicName(PaPayrollsheetSumSubDTO param)
        {
            var result = await (from p in _dbContext.PaListSals.AsNoTracking().Where(p=> p.CODE_LISTSAL != "CL7")
                                from t in _dbContext.PaListsalariess.AsNoTracking().Where(t => t.CODE_SAL == p.ID && t.OBJ_SAL_ID == param.ObjSalaryId && t.IS_VISIBLE == true)
                                orderby t.COL_INDEX, t.NAME
                                select new
                                {
                                    Code = p.CODE_LISTSAL,
                                    Name = t.NAME,
                                }).ToListAsync();
            return new FormatedResponse { InnerBody = result };
        }

        public async Task<FormatedResponse> GetList(PaPayrollsheetSumSubDTO param)
         {
            try
            {
                string listOrgIds = string.Join(",", param.OrgIds!.ToArray()).ToString();
                var pageNo = param.PageNo == null ? 1 : param.PageNo;
                var pageSize = param.PageSize == null ? 10 : param.PageSize;
                var skip = pageSize * (pageNo - 1);
                var r = await QueryData.ExecuteList("PKG_PAYROLL_LOAD_PAYROLL_SHEET_SUM_SUB",
                new
                {
                    P_ORG_ID = listOrgIds, //1530,
                    P_YEAR = param.Year, //2023,
                    P_PERIOD_ID = param.PhaseAdvanceId, //2024,
                    P_SAL_OBJ_ID = param.ObjSalaryId, //1382,
                    P_FUND_ID = param.PayrollFund == null ? "" : param.PayrollFund.ToString(),
                    P_CODE = param.EmployeeCode == null ? "" : param.EmployeeCode,
                    //P_PAY_DATE = param.PayDate == null ? "" : param.PayDate!.Value.ToString("dd/MM/yyyy"),
                    P_NAME = param.EmployeeName == null ? "" : param.EmployeeName,
                    P_ORG_NAME = param.DepartmentName == null ? "" : param.DepartmentName,
                    P_POS_NAME = param.PositionName == null ? "" : param.PositionName,
                    P_JOIN_DATE_SEARCH = param.JoinDateSearch == null ? "" : param.JoinDateSearch,
                    //P_PAY_DATE_SEARCH = param.PayDateSearch == null ? "" : param.PayDateSearch,
                    P_FUND_NAME = param.FundName == null ? "" : param.FundName,
                    P_USERNAME = param.EmployeeCal,
                    
                }, false);
                long totalRecord = r.Count;
                var result = new
                {
                    Count = totalRecord,
                    List = r.Skip(skip.Value).Take(pageSize.Value),
                    Page = pageNo,
                    PageCount = totalRecord > 0 ? (int)Math.Ceiling(totalRecord / (double)pageSize) : 0,
                    Skip = pageSize * (pageNo - 1),
                    Take = pageSize,
                    MessageCode = CommonMessageCode.QUERY_LIST_SUCCESS,
                };

                return new() { InnerBody = result };
            }
            catch (Exception ex)
            {
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> HandleRequest(PaPayrollsheetSumSubDTO param)
        {
            
            try
            {
                string listOrgIds = string.Join(",", param.OrgIds!.ToArray()).ToString();
                // check dong bang luong
                var checkLockOrg = (from p in _dbContext.AtOrgPeriods where param.OrgIds!.ToList().Contains(p.ORG_ID!.Value) && p.STATUSPAROX_SUB == 1 && p.PERIOD_ID == param.PhaseAdvanceId select p).Any();     // // lay ra ki cong khoa
                if (checkLockOrg)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.PHASE_ADVANCE_PERIOD_IS_LOCKED, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
                var r = await QueryData.ExecuteList("PKG_PA_PAYROLL_LOAD_DATA_BS",
                    new
                    {
                        P_ORG_ID = listOrgIds,
                        P_YEAR = param.Year,
                        P_PERIOD_ID = param.PhaseAdvanceId,
                        P_SAL_OBJ_ID = param.ObjSalaryId,
                        P_SAL_FUND = param.PayrollFund,
                        P_USERNAME = param.EmployeeCal,
                        P_NGAYCHI = param.PayDate
                    }, false);
                return new() { InnerBody = r[0], MessageCode = CommonMessageCode.CALCULATE_SUCCESS };
                //return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.CALCULATE_FAILED };
            }
            catch(Exception ex)
            {
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> CheckRequest(long id)
        {
            var x = await _dbContext.AdRequests.AsNoTracking().SingleAsync(x => x.REQUEST_ID == id);

            if (x != null)
            {
                return new() { InnerBody = x.PHASE_CODE };

            }
            else
            {
                return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, StatusCode = EnumStatusCode.StatusCode400 };
            }

        }

        public async Task<GenericPhaseTwoListResponse<PaPayrollsheetSumSubDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaPayrollsheetSumSubDTO> request)
        {
            var joined = from p in _dbContext.PaPayrollsheetSumSubs.AsNoTracking()
                         // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new PaPayrollsheetSumSubDTO
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
                var list = new List<PA_PAYROLLSHEET_SUM_SUB>
                    {
                        (PA_PAYROLLSHEET_SUM_SUB)response
                    };
                var joined = (from l in list
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new PaPayrollsheetSumSubDTO
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, PaPayrollsheetSumSubDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<PaPayrollsheetSumSubDTO> dtos, string sid)
        {
            var add = new List<PaPayrollsheetSumSubDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, PaPayrollsheetSumSubDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<PaPayrollsheetSumSubDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> ChangeStatusParoxSub(PaPayrollsheetSumSubDTO dto)
        {
            try
            {
                string listOrgIds = string.Join(",", dto.OrgIds!.ToArray()).ToString();
                //if(dto.StatusParoxSub == 0)
                //{
                //    // check mo bang luong backdate
                //    var checkLockOrg = (from p in _dbContext.AtOrgPeriods where dto.OrgIds!.ToList().Contains(p.ORG_ID!.Value) && p.STATUSPAROX_BACKDATE != 1 && p.PERIOD_ID == dto.PeriodId select p).ToList();     // lay ra ki cong mo khoa
                //    if (checkLockOrg != null && checkLockOrg.Count() != dto.OrgIds!.Count())
                //    {
                //        return new FormatedResponse() { MessageCode = CommonMessageCode.THE_WORK_PERIOD_HAS_CEASED_TO_APPLY, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                //    }
                //}
                var r = await QueryData.ExecuteNonQuery("PKG_AT_ORG_PERIOD_CHANGE_STATUSPAROX_SUB",
                        new
                        {
                            P_ORGID = listOrgIds,
                            P_STATUS = dto.StatusParoxSub,
                            P_SALPERIOD = dto.PeriodId,
                            P_UPDATED_DATE = DateTime.UtcNow
                        }, true);
                if(dto.StatusParoxSub == 1)
                {
                    return new() { InnerBody = r, MessageCode = CommonMessageCode.LOCK_SUCCESS };
                }
                else
                {
                    return new() { InnerBody = r, MessageCode = CommonMessageCode.UNLOCK_SUCCESS };
                }
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

        public async Task<FormatedResponse> GetPhaseAdvance(PaPayrollsheetSumSubDTO param)
        {
            var queryable = await (from p in _dbContext.PaPhaseAdvances.AsNoTracking().Where(p => p.IS_ACTIVE == true && p.PERIOD_ID == param.PeriodId)
                                   from s in _dbContext.AtSalaryPeriods.AsNoTracking().Where(s => s.ID == p.PERIOD_ID).DefaultIfEmpty()
                                   orderby s.MONTH ascending
                                   select new
                                   {
                                       Id = p.ID,
                                       SalaryName = s.NAME,
                                       Month = s.MONTH,
                                       Name = p.NAME_VN + "[" + p.PHASE_DAY!.Value.ToString("dd/MM/yyyy") + "]",
                                       Day = p.PHASE_DAY
                                   }).ToListAsync();
            return new() { InnerBody = queryable };
        }
        public async Task<FormatedResponse> GetPhaseAdvanceById(long id)
        {
            var queryable = await (from p in _dbContext.PaPhaseAdvances.AsNoTracking().Where(p => p.ID == id)
                                   select new
                                   {
                                       Id = p.ID,
                                       Day = p.PHASE_DAY
                                   }).FirstOrDefaultAsync();
            return new() { InnerBody = queryable };
        }

        //lay doi tuong luong bo sung
        public async Task<FormatedResponse> GetObjSalPayrollSubGroup()
        {
            var query = await (from o in _dbContext.HuSalaryTypes.AsNoTracking().Where(o => o.IS_ACTIVE == true).DefaultIfEmpty()
                               from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == o.SALARY_TYPE_GROUP).DefaultIfEmpty()
                               where s.CODE == "00020"
                               orderby o.NAME
                               select new
                               {
                                   Id = o.ID,
                                   Name = o.NAME,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }
    }
}

