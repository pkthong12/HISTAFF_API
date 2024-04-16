using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.HuCompetencyDict
{
    public class HuCompetencyDictRepository : IHuCompetencyDictRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_COMPETENCY_DICT, HuCompetencyDictDTO> _genericRepository;
        private readonly GenericReducer<HU_COMPETENCY_DICT, HuCompetencyDictDTO> _genericReducer;

        public HuCompetencyDictRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_COMPETENCY_DICT, HuCompetencyDictDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuCompetencyDictDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCompetencyDictDTO> request)
        {
            var joined = from p in _dbContext.HuCompetencyDicts.AsNoTracking()
                         from g in _dbContext.HuCompetencyGroups.AsNoTracking().Where(g => g.ID == p.COMPETENCY_GROUP_ID).DefaultIfEmpty()
                         from c in _dbContext.HuCompetencys.AsNoTracking().Where(c => c.ID == p.COMPETENCY_ID).DefaultIfEmpty()
                         from a in _dbContext.HuCompetencyAspects.AsNoTracking().Where(a => a.ID == p.COMPETENCY_ASPECT_ID).DefaultIfEmpty()
                         from s in _dbContext.SysOtherLists.AsNoTracking().Where(s=> s.ID == p.LEVEL_NUMBER).DefaultIfEmpty()
                         select new HuCompetencyDictDTO
                         {
                             Id = p.ID,
                             CompetencyGroupId = p.COMPETENCY_GROUP_ID,
                             CompetencyGroupName = g.NAME,
                             CompetencyId = p.COMPETENCY_ID,
                             CompetencyName = c.NAME,
                             CompetencyAspectId = p.COMPETENCY_ASPECT_ID,
                             LevelNumber =  p.LEVEL_NUMBER,
                             LevelNumberName = s.NAME,
                             CompetencyAspectName = a.NAME,
                             Note = c.NOTE,
                             IsActive = p.IS_ACTIVE,
                             Status = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
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
            
                var joined = await (from p in _dbContext.HuCompetencyDicts.AsNoTracking().Where(p => p.ID == id)
                                    from g in _dbContext.HuCompetencyGroups.AsNoTracking().Where(g => g.ID == p.COMPETENCY_GROUP_ID).DefaultIfEmpty()
                                    from c in _dbContext.HuCompetencys.AsNoTracking().Where(c => c.ID == p.COMPETENCY_ID).DefaultIfEmpty()
                                    from a in _dbContext.HuCompetencyAspects.AsNoTracking().Where(a => a.ID == p.COMPETENCY_ASPECT_ID).DefaultIfEmpty()
                                    from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.LEVEL_NUMBER).DefaultIfEmpty()
                                    select new HuCompetencyDictDTO
                                    {
                                        Id = p.ID,
                                        CompetencyGroupId = p.COMPETENCY_GROUP_ID,
                                        CompetencyGroupName = g.NAME,
                                        CompetencyId = p.COMPETENCY_ID,
                                        CompetencyName = c.NAME,
                                        LevelNumber = p.LEVEL_NUMBER,
                                        LevelNumberName = s.NAME,
                                        CompetencyAspectId = p.COMPETENCY_ASPECT_ID,
                                        CompetencyAspectName = a.NAME,
                                        Note = c.NOTE,
                                    }).FirstOrDefaultAsync();
            if(joined != null)
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuCompetencyDictDTO dto, string sid)
        {
            dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuCompetencyDictDTO> dtos, string sid)
        {
            var add = new List<HuCompetencyDictDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuCompetencyDictDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuCompetencyDictDTO> dtos, string sid, bool patchMode = true)
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
    }
}

