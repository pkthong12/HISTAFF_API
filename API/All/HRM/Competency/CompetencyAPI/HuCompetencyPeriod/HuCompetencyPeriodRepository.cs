using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.All.HRM.Competency.CompetencyAPI.HuCompetencyPeroid
{
    public class HuCompetencyPeriodRepository : IHuCompetencyPeriodRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_COMPETENCY_PERIOD, HuCompetencyPeriodDTO> _genericRepository;
        private readonly GenericReducer<HU_COMPETENCY_PERIOD, HuCompetencyPeriodDTO> _genericReducer;

        public HuCompetencyPeriodRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_COMPETENCY_PERIOD, HuCompetencyPeriodDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuCompetencyPeriodDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCompetencyPeriodDTO> request)
        {
            var joined = from p in _dbContext.HuCompetencyPeriods.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                             from s in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.QUARTER_ID).DefaultIfEmpty()
                         select new HuCompetencyPeriodDTO
                         {
                             Id = p.ID,
                             Year = p.YEAR,
                             QuarterId = p.QUARTER_ID,
                             QuarterName = s.NAME,
                             Code = p.CODE,
                             Name = p.NAME,
                             EffectedDate = p.EFFECTED_DATE,
                             ExpriedDate = p.EXPRIED_DATE,
                             Status = p.IS_ACTIVE == true ? "Áp dụng" : "Ngưng áp dụng",
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
            var joined = await (from p in _dbContext.HuCompetencyPeriods.AsNoTracking().Where(x => x.ID == id)
                                from s in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.QUARTER_ID).DefaultIfEmpty()
                                select new HuCompetencyPeriodDTO
                                {
                                    Id = p.ID,
                                    Year = p.YEAR,
                                    QuarterId = p.QUARTER_ID,
                                    QuarterName = s.NAME,
                                    Code = p.CODE,
                                    Name = p.NAME,
                                    EffectedDate = p.EFFECTED_DATE,
                                    ExpriedDate = p.EXPRIED_DATE
                                }).FirstOrDefaultAsync();
            return new FormatedResponse() { InnerBody = joined };
        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuCompetencyPeriodDTO dto, string sid)
        {
            dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuCompetencyPeriodDTO> dtos, string sid)
        {
            var add = new List<HuCompetencyPeriodDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuCompetencyPeriodDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuCompetencyPeriodDTO> dtos, string sid, bool patchMode = true)
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
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }

        public async Task<FormatedResponse> GetPeriodYear()
        {
            var query = await (from p in _dbContext.AtSalaryPeriods.AsNoTracking().Where(x => x.IS_ACTIVE ==  true)
                               select new
                               {
                                   Year = p.YEAR,
                               }).Distinct().ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }

        public async Task<FormatedResponse> GetQuarter()
        {
            var codeQuarter = await (from q in _dbContext.SysOtherListTypes.AsNoTracking().Where(x => x.CODE == "QUARTER") select q.ID).FirstOrDefaultAsync();
            var query = await (from p in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.TYPE_ID == codeQuarter && x.IS_ACTIVE == true)
                               select new
                               {
                                   Id = p.ID,
                                   Name = p.NAME,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }
    }
}

