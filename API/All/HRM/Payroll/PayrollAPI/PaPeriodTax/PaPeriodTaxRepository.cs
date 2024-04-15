using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.PaPeriodTax
{
    public class PaPeriodTaxRepository : IPaPeriodTaxRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<PA_PERIOD_TAX, PaPeriodTaxDTO> _genericRepository;
        private readonly GenericReducer<PA_PERIOD_TAX, PaPeriodTaxDTO> _genericReducer;

        public PaPeriodTaxRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<PA_PERIOD_TAX, PaPeriodTaxDTO>();
            _genericReducer = new();
        }
        
        public async Task<GenericPhaseTwoListResponse<PaPeriodTaxDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaPeriodTaxDTO> request)
        {
            var joined = from p in _dbContext.PaPeriodTaxs.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         //from pr in _dbContext.AtSalaryPeriods.AsNoTracking().Where(x => x.ID == p.PERIOD_ID)
                         from sp in _dbContext.AtSalaryPeriods.AsNoTracking().Where(x => x.ID == p.MONTHLY_TAX_CALCULATION)
                         select new PaPeriodTaxDTO
                         {
                             Id = p.ID,
                             Year = p.YEAR,
                             TaxDate = p.TAX_DATE,
                             MonthlyTaxCalculation = p.MONTHLY_TAX_CALCULATION,
                             TaxMonth = sp.NAME,
                             CalculateTaxFromDate = p.CALCULATE_TAX_FROM_DATE,
                             CalculateTaxToDate = p.CALCULATE_TAX_TO_DATE,
                             Status = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
                             Note = p.NOTE
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
                var list = new List<PA_PERIOD_TAX>
                    {
                        (PA_PERIOD_TAX)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              from pr in _dbContext.AtSalaryPeriods.AsNoTracking().Where(x => x.ID == l.MONTHLY_TAX_CALCULATION).DefaultIfEmpty()
                              select new PaPeriodTaxDTO
                              {
                                  Id = l.ID,
                                  Year = l.YEAR,
                                  //PeriodId = l.PERIOD_ID,
                                  TaxDate = l.TAX_DATE,
                                  MonthlyTaxCalculation = l.MONTHLY_TAX_CALCULATION,
                                  TaxMonth = pr.NAME,
                                  CalculateTaxFromDate = l.CALCULATE_TAX_FROM_DATE,
                                  CalculateTaxToDate = l.CALCULATE_TAX_TO_DATE,
                                  Note = l.NOTE
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, PaPeriodTaxDTO dto, string sid)
        {
            if(dto.MonthlyTaxCalculation != null)
            {
                dto.PeriodId = dto.MonthlyTaxCalculation;
            }
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<PaPeriodTaxDTO> dtos, string sid)
        {
            var add = new List<PaPeriodTaxDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, PaPeriodTaxDTO dto, string sid, bool patchMode = true)
        {
            if (dto.MonthlyTaxCalculation != null)
            {
                dto.PeriodId = dto.MonthlyTaxCalculation;
            }
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<PaPeriodTaxDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetPeriod()
        {
            var query = await (from p in _dbContext.AtSalaryPeriods
                               where p.IS_ACTIVE == true
                               orderby p.ID
                               select new
                               {
                                   Id = p.ID,
                                   Name = p.NAME
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
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }
    }
}

