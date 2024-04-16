using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Common.DataAccess;
using Common.Interfaces;

namespace API.Controllers.PaPayrollsheetTax
{
    public class PaPayrollsheetTaxRepository : IPaPayrollsheetTaxRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<PA_PAYROLLSHEET_TAX, PaPayrollsheetTaxDTO> _genericRepository;
        private readonly GenericReducer<PA_PAYROLLSHEET_TAX, PaPayrollsheetTaxDTO> _genericReducer;
        protected AbsQueryDataTemplate QueryData;

        public PaPayrollsheetTaxRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<PA_PAYROLLSHEET_TAX, PaPayrollsheetTaxDTO>();
            _genericReducer = new();
            QueryData = new SqlQueryDataTemplate(_dbContext);
        }

        public async Task<FormatedResponse> GetDynamicName(PaPayrollsheetTaxDTO param)
        {
            var result = await (from p in _dbContext.PaListSals.AsNoTracking()
                                from t in _dbContext.PaListsalariess.AsNoTracking().Where(t => t.CODE_SAL == p.ID && t.OBJ_SAL_ID == param.ObjSalaryId && t.IS_VISIBLE == true)
                                orderby t.COL_INDEX ascending
                                select new
                                {
                                    Code = p.CODE_LISTSAL,
                                    Name = t.NAME,
                                    DataType = p.DATA_TYPE_ID,
                                }).ToListAsync();
            return new FormatedResponse { InnerBody = result };
        }

        public async Task<FormatedResponse> GetList(PaPayrollsheetTaxDTO param)
        {
            try
            {
                string listOrgIds = string.Join(",", param.OrgIds!.ToArray()).ToString();
                var pageNo = param.PageNo == null ? 1 : param.PageNo;
                var pageSize = param.PageSize == null ? 10 : param.PageSize;
                var skip = pageSize * (pageNo - 1);
                var r = await QueryData.ExecuteList("PKG_PAYROLL_LOAD_PAYROLL_SHEET_TAX",
                new
                {
                    P_ORG_ID = listOrgIds,//param.OrgId,
                    P_YEAR = param.Year,//param.Year,
                    P_PERIOD_ID = param.PeriodId,//,
                    P_SAL_OBJ_ID = param.ObjSalaryId,//param.ObjSalaryId,
                    P_USERNAME = param.EmployeeCal, //param.EmployeeCal,
                    P_TAX_DATE = param.TaxDate.Value.ToString("dd/MM/yyyy"),
                    P_CODE = param.EmployeeCode == null ? "" : param.EmployeeCode,
                    P_NAME = param.EmployeeName == null ? "" : param.EmployeeName,
                    P_ORG_NAME = param.DepartmentName == null ? "" : param.DepartmentName,
                    P_POS_NAME = param.PositionName == null ? "" : param.PositionName,
                    P_JOIN_DATE_SEARCH = param.JoinDateSearch == null ? "" : param.JoinDateSearch,
                    P_FUND_NAME = param.FundName == null ? "" : param.FundName,
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

        public async Task<FormatedResponse> HandleRequest(PaPayrollsheetTaxDTO param)
        {
            try
            {
                string listOrgIds = string.Join(",", param.OrgIds!.ToArray()).ToString();
               
                var checkLockOrg = (from p in _dbContext.AtOrgPeriods where param.OrgIds!.ToList().Contains(p.ORG_ID!.Value) && (p.STATUSPAROX_TAX_MONTH == 1) && p.PERIOD_ID == param.PeriodId select p).ToList(); // lay ra ki cong khoa
                if (checkLockOrg.Count >0)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.PA_PAYROLLSHEET_TAX_HAVE_NOT_LOCK, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }                   
                var r = await QueryData.ExecuteList("PKG_PA_PAYROLL_LOAD_TAX_MONTH",
                    new
                    {
                        P_ORG_ID = listOrgIds,
                        P_YEAR = param.Year.ToString(),
                        P_SAL_OBJ_ID = param.ObjSalaryId.ToString(),
                        P_MONTH = param.Month.ToString(),
                        P_NGAY = param.TaxDate.ToString(),
                        P_USERNAME = param.EmployeeCal
                    }, false);
                return new() { InnerBody = r, MessageCode = CommonMessageCode.CALCULATE_SUCCESS };
                //return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.CALCULATE_FAILED };
            }
            catch (Exception ex)
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
        public async Task<FormatedResponse> GetTaxDate(PaPayrollsheetTaxDTO param)
        {
            var x = await (from s in _dbContext.AtSalaryPeriods.AsNoTracking().Where(s => s.YEAR == param.Year && s.MONTH == param.Month)
                           from p in _dbContext.PaPeriodTaxs.AsNoTracking().Where(p => p.PERIOD_ID == s.ID && p.IS_ACTIVE== true)
                           select new
                           {
                               Id = p.ID,
                               Name = p.TAX_DATE.Value.ToString("dd/MM/yyyy"),
                               Code = p.TAX_DATE.Value.ToString("yyyy-MM-dd"),
                           }).ToListAsync();
            return new FormatedResponse()
            {
                InnerBody = x,
                StatusCode = EnumStatusCode.StatusCode200
            };
        }

        public async Task<FormatedResponse> GetObjSal()
        {//lay doi tuong luong
            var query = await (from o in _dbContext.HuSalaryTypes.AsNoTracking().Where(o => o.IS_ACTIVE == true).DefaultIfEmpty()
                               from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == o.SALARY_TYPE_GROUP).DefaultIfEmpty()
                               where s.CODE == "00021" && (o.CODE == "DTL005" || o.CODE == "ĐTL005")
                               orderby o.NAME
                               select new
                               {
                                   Id = o.ID,
                                   Name = o.NAME,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetMonth(PaPayrollsheetTaxDTO param)
        {
            var x = await (from p in _dbContext.AtSalaryPeriods.AsNoTracking()
                           where p.IS_ACTIVE == true && p.YEAR == param.Year
                           select new
                           {
                               Month = p.MONTH
                           }).Select(x => x.Month).ToListAsync();
            return new FormatedResponse()
            {
                InnerBody = x,
                StatusCode = EnumStatusCode.StatusCode200
            };
        }


        public async Task<GenericPhaseTwoListResponse<PaPayrollsheetTaxDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaPayrollsheetTaxDTO> request)
        {
            var joined = from p in _dbContext.PaPayrollsheetTaxs.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new PaPayrollsheetTaxDTO
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
                var list = new List<PA_PAYROLLSHEET_TAX>
                    {
                        (PA_PAYROLLSHEET_TAX)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new PaPayrollsheetTaxDTO
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, PaPayrollsheetTaxDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<PaPayrollsheetTaxDTO> dtos, string sid)
        {
            var add = new List<PaPayrollsheetTaxDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, PaPayrollsheetTaxDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<PaPayrollsheetTaxDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> ChangeStatusParoxTaxMonth(PaPayrollsheetTaxDTO dto)
        {
            try
            {
                string listOrgIds = string.Join(",", dto.OrgIds!.ToArray()).ToString();
                var r = await QueryData.ExecuteNonQuery("PKG_AT_ORG_PERIOD_CHANGE_STATUSPAROX_TAX_MONTH",
                    new
                    {
                        P_ORGID = listOrgIds,
                        P_STATUS = dto.StatusParoxTaxMonth,
                        P_SALPERIOD = dto.PeriodId,
                        P_UPDATED_DATE = DateTime.UtcNow
                    }, true);
                if (dto.StatusParoxTaxMonth == 1)
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
        public async Task<FormatedResponse> GetPeriodId(long year, long month)
        {
            var r =await _dbContext.AtSalaryPeriods.AsNoTracking().Where(p => p.MONTH == month && p.YEAR == year).FirstOrDefaultAsync();
            if (r != null)
            {
                return new() { InnerBody = r, StatusCode = EnumStatusCode.StatusCode200 };
            }
            return new() { InnerBody = null, MessageCode = CommonMessageCode.UPDATE_SUCCESS };

        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
    }
}

