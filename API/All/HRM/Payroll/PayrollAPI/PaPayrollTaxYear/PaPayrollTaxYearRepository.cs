using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Common.Interfaces;
using Common.DataAccess;

namespace API.Controllers.PaPayrollTaxYear
{
    public class PaPayrollTaxYearRepository : IPaPayrollTaxYearRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<PA_PAYROLL_TAX_YEAR, PaPayrollTaxYearDTO> _genericRepository;
        private readonly GenericReducer<PA_PAYROLL_TAX_YEAR, PaPayrollTaxYearDTO> _genericReducer;
        protected AbsQueryDataTemplate QueryData;

        public PaPayrollTaxYearRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<PA_PAYROLL_TAX_YEAR, PaPayrollTaxYearDTO>();
            _genericReducer = new();
            QueryData = new SqlQueryDataTemplate(_dbContext);
        }

        public async Task<FormatedResponse> GetObjSalTaxGroup()
        {//lay doi tuong luong
            var query = await (from o in _dbContext.HuSalaryTypes.AsNoTracking().Where(o => o.IS_ACTIVE == true).DefaultIfEmpty()
                               from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == o.SALARY_TYPE_GROUP).DefaultIfEmpty()
                               where s.CODE == "00021" && (o.CODE == "ĐTL006" || o.CODE == "DTL006")
                               orderby o.NAME
                               select new
                               {
                                   Id = o.ID,
                                   Name = o.NAME,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetDynamicName(PaPayrollTaxYearDTO param)
        {
            var result = await (from p in _dbContext.PaListSals.AsNoTracking()
                                from t in _dbContext.PaListsalariess.AsNoTracking().Where(t => t.CODE_SAL == p.ID && t.OBJ_SAL_ID == param.ObjSalaryId && t.IS_VISIBLE == true)
                                orderby t.COL_INDEX
                                select new
                                {
                                    Code = p.CODE_LISTSAL,
                                    Name = t.NAME,
                                }).ToListAsync();
            return new FormatedResponse { InnerBody = result };
        }

        public async Task<FormatedResponse> GetList(PaPayrollTaxYearDTO param)
        {
            try
            {
                string listOrgIds = string.Join(",", param.OrgIds!.ToArray()).ToString();
                var pageNo = param.PageNo == null ? 1 : param.PageNo;
                var pageSize = param.PageSize == null ? 50 : param.PageSize;
                var skip = pageSize * (pageNo - 1);
                var r = await QueryData.ExecuteList("PKG_PAYROLL_LOAD_PAYROLL_SHEET_TAX_YEAR",
                new
                {
                    P_ORG_ID = listOrgIds,
                    P_YEAR = param.Year,
                    P_SAL_OBJ_ID = param.ObjSalaryId,
                    P_DATE_START = param.DateStart,
                    P_DATE_END = param.DateEnd,
                    P_CODE = param.EmployeeCode == null ? "" : param.EmployeeCode,
                    P_NAME = param.EmployeeName == null ? "" : param.EmployeeName,
                    P_ORG_NAME = param.DepartmentName == null ? "" : param.DepartmentName,
                    P_POS_NAME = param.PositionName == null ? "" : param.PositionName,
                    P_TAX_CODE = param.TaxCodeSearch == null ? "" : param.TaxCodeSearch,
                    P_ID_NO = param.IdNoSearch == null ? "" : param.IdNoSearch,
                }, false);
                long totalRecord = r.Count;

                r = r.Where(x => ((string)((IDictionary<string, object>)x)["CODE"]).ToUpper().Contains(param.EmployeeCode?.ToUpper() ?? "")).ToList();
                r = r.Where(x => ((string)((IDictionary<string, object>)x)["FULL_NAME"]).ToUpper().Contains(param.EmployeeName?.ToUpper() ?? "")).ToList();
                r = r.Where(x => ((string)((IDictionary<string, object>)x)["ORG_NAME"]).ToUpper().Contains(param.DepartmentName?.ToUpper() ?? "")).ToList();
                r = r.Where(x => ((string)((IDictionary<string, object>)x)["POS_NAME"]).ToUpper().Contains(param.PositionName?.ToUpper() ?? "")).ToList();
                r = r.Where(x => ((string)((IDictionary<string, object>)x)["ID_NO"]).ToUpper().Contains(param.IdNoSearch?.ToUpper() ?? "")).ToList();


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

        public async Task<FormatedResponse> HandleRequest(PaPayrollTaxYearDTO param)
        {
            try
            {
                string listOrgIds = string.Join(",", param.OrgIds!.ToArray()).ToString();

                var lst = (from p in _dbContext.PaPayrollTaxYears
                           where p.IS_LOCK == 1 && param.OrgIds.Contains(p.ORG_ID) && p.YEAR == param.Year
                           select p).ToList().Count();

                if (lst > 0)
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.PA_PAYROLLSHEET_TAXYEAR_HAVE_NOT_LOCK, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };

                }


                var r = await QueryData.ExecuteList("PKG_PA_PAYROLL_LOAD_TAX_YEAR",
                    new
                    {
                        P_ORG_ID = listOrgIds,
                        P_YEAR = param.Year.ToString(),
                        P_SAL_OBJ_ID = param.ObjSalaryId.ToString(),
                        P_STARTDATE = param.DateStart.ToString(),
                        P_ENDDATE = param.DateEnd.ToString(),
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

        public async Task<GenericPhaseTwoListResponse<PaPayrollTaxYearDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaPayrollTaxYearDTO> request)
        {
            var joined = from p in _dbContext.PaPayrollTaxYears.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new PaPayrollTaxYearDTO
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
                var list = new List<PA_PAYROLL_TAX_YEAR>
                    {
                        (PA_PAYROLL_TAX_YEAR)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new PaPayrollTaxYearDTO
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, PaPayrollTaxYearDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<PaPayrollTaxYearDTO> dtos, string sid)
        {
            var add = new List<PaPayrollTaxYearDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, PaPayrollTaxYearDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<PaPayrollTaxYearDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> ChangeStatusParoxTaxYear(PaPayrollTaxYearDTO dto)
        {
            try
            {

                var lst = await (from p in _dbContext.PaPayrollTaxYears
                                 where dto.OrgIds.Contains(p.ORG_ID) && p.YEAR == dto.Year
                                 select p).ToListAsync();


                foreach (var item in lst)
                {
                    item.IS_LOCK = dto.Lock;
                    item.UPDATED_DATE = DateTime.Now;
                    item.UPDATED_BY = "HUNGNX";
                }

                _dbContext.SaveChanges();
                if (dto.Lock == 1)
                {
                    return new() { InnerBody = lst, MessageCode = CommonMessageCode.LOCK_SUCCESS };
                }
                else
                {
                    return new() { InnerBody = lst, MessageCode = CommonMessageCode.UNLOCK_SUCCESS };
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

    }
}

