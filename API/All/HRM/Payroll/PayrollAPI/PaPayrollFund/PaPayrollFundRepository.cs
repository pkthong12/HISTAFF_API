using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.PaPayrollFund
{
    public class PaPayrollFundRepository : IPaPayrollFundRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<PA_PAYROLL_FUND, PaPayrollFundDTO> _genericRepository;
        private readonly GenericReducer<PA_PAYROLL_FUND, PaPayrollFundDTO> _genericReducer;

        public PaPayrollFundRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<PA_PAYROLL_FUND, PaPayrollFundDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<PaPayrollFundDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaPayrollFundDTO> request)
        {
            var joined = from p in _dbContext.PaPayrollFunds.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from sp in _dbContext.AtSalaryPeriods.AsNoTracking().Where(sp => sp.ID == p.SALARY_PERIOD_ID).DefaultIfEmpty()
                         from c in _dbContext.HuCompanys.AsNoTracking().Where(c => c.ID == p.COMPANY_ID).DefaultIfEmpty()
                         from s in _dbContext.PaListFundSources.AsNoTracking().Where(s => s.ID == p.LIST_FUND_SOURCE_ID).DefaultIfEmpty()
                         from f in _dbContext.PaListfunds.AsNoTracking().Where(f => f.ID == p.LIST_FUND_ID).DefaultIfEmpty()
                         select new PaPayrollFundDTO
                         {
                             Id = p.ID,
                             Year = p.YEAR,
                             CompanyName = c.NAME_VN,
                             ListFundSourceName = s.NAME,
                             Amount = p.AMOUNT,
                             ApprovalDate = p.APPROVAL_DATE,
                             Note = p.NOTE,
                             SalaryPeriodId = p.SALARY_PERIOD_ID,
                             Month = sp.MONTH,
                             ListFundSourceId = p.LIST_FUND_SOURCE_ID,
                             ListFundId = p.LIST_FUND_ID,
                             CompanyId = p.COMPANY_ID,
                             ListFundName = f.LISTFUND_NAME,
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
            var joined = (from l in _dbContext.PaPayrollFunds.AsNoTracking().Where(l => l.ID == id)
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                          from sp in _dbContext.AtSalaryPeriods.AsNoTracking().Where(sp => sp.ID == l.SALARY_PERIOD_ID).DefaultIfEmpty()
                          from p in _dbContext.HuCompanys.AsNoTracking().Where(p => p.ID == l.COMPANY_ID).DefaultIfEmpty()
                          from s in _dbContext.PaListFundSources.AsNoTracking().Where(s => s.ID == l.LIST_FUND_SOURCE_ID).DefaultIfEmpty()
                          from f in _dbContext.PaListfunds.AsNoTracking().Where(f => f.ID == l.LIST_FUND_ID).DefaultIfEmpty()
                          from c in _dbContext.SysUsers.AsNoTracking().Where(c => l.CREATED_BY == null ? false : c.ID == l.CREATED_BY).DefaultIfEmpty()
                          from u in _dbContext.SysUsers.AsNoTracking().Where(u => l.UPDATED_BY == null ? false : u.ID == l.UPDATED_BY).DefaultIfEmpty()
                          select new PaPayrollFundDTO
                          {
                              Id = l.ID,
                              Year = l.YEAR,
                              CompanyName = p.NAME_VN,
                              ListFundSourceName = s.NAME,
                              Amount = l.AMOUNT,
                              ApprovalDate = l.APPROVAL_DATE,
                              Note = l.NOTE,
                              ListFundSourceId = l.LIST_FUND_SOURCE_ID,
                              ListFundId = l.LIST_FUND_ID,
                              SalaryPeriodId = l.SALARY_PERIOD_ID,
                              Month = sp.MONTH,
                              CompanyId = l.COMPANY_ID,
                              ListFundName = f.LISTFUND_NAME,
                              CreatedByUsername = c.USERNAME,
                              CreatedDate = l.CREATED_DATE,
                              UpdatedByUsername = u.USERNAME,
                              UpdatedDate = l.UPDATED_DATE,
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

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, PaPayrollFundDTO dto, string sid)
        {
            try
            {
                var response = await _genericRepository.Create(_uow, dto, sid);
                return response;
            }
            catch (Exception ex)
            {
                return new FormatedResponse()
                {
                    MessageCode = ex.ToString(),
                    StatusCode = EnumStatusCode.StatusCode500
                };
            }
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<PaPayrollFundDTO> dtos, string sid)
        {
            var add = new List<PaPayrollFundDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, PaPayrollFundDTO dto, string sid, bool patchMode = true)
        {
            
            try
            {
                var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
                return response;
            }
            catch (Exception ex)
            {
                return new FormatedResponse()
                {
                    MessageCode = ex.ToString(),
                    StatusCode = EnumStatusCode.StatusCode500
                };
            }
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<PaPayrollFundDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(_uow, dtos, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, long id)
        {
            try
            {
                var response = await _genericRepository.Delete(_uow, id);
                return response;
            }
            catch (Exception ex)
            {
                return new FormatedResponse()
                {
                    MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE,
                    StatusCode = EnumStatusCode.StatusCode400
                };
            }
        }

        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<long> ids)
        {
            try
            {
                var response = await _genericRepository.DeleteIds(_uow, ids);
                return response;
            }
            catch (Exception ex)
            {
                return new FormatedResponse()
                {
                    MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE,
                    StatusCode = EnumStatusCode.StatusCode400
                };
            }
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
        public async Task<FormatedResponse> GetCompany()
        {
            var query = await (from o in _dbContext.HuCompanys
                               where o.IS_ACTIVE == true
                               orderby o.CODE
                               select new
                               {
                                   Id = o.ID,
                                   Name = o.NAME_VN,
                                   Code = o.CODE,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }
        public async Task<FormatedResponse> GetListFundSource(long id)
        {
            var query = await (from o in _dbContext.PaListFundSources
                               where o.COMPANY_ID == id
                               orderby o.ID
                               select new
                               {
                                   Id = o.ID,
                                   Name = o.NAME,
                                   Code = o.CODE,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }
        public async Task<FormatedResponse> GetListFund(long id)
        {
            var query = await (from o in _dbContext.PaListfunds
                               where o.COMPANY_ID == id
                               orderby o.ID
                               select new
                               {
                                   Id = o.ID,
                                   Name = o.LISTFUND_NAME,
                                   Code = o.LISTFUND_CODE,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }
        public async Task<FormatedResponse> GetMonth(long year)
        {
            var query = await (from o in _dbContext.AtSalaryPeriods.AsNoTracking().Where(o => o.YEAR == year && o.MONTH != null)
                               select new
                               {
                                   Id = o.ID,
                                   Month = o.MONTH,
                                   Year = o.YEAR,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
    }
}

