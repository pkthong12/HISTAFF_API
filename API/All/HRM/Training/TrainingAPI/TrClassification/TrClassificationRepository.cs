using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.TrClassification
{
    public class TrClassificationRepository : ITrClassificationRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<TR_CLASSIFICATION, TrClassificationDTO> _genericRepository;
        private readonly GenericReducer<TR_CLASSIFICATION, TrClassificationDTO> _genericReducer;

        public TrClassificationRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<TR_CLASSIFICATION, TrClassificationDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<TrClassificationDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrClassificationDTO> request)
        {
            var joined = from p in _dbContext.TrClassifications.AsNoTracking()
                         from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.DESC_ID).DefaultIfEmpty()
                         select new TrClassificationDTO
                         {
                             Id = p.ID,
                             Name = p.NAME,
                             DescId = p.DESC_ID,
                             DescName = s.NAME,
                             ScoreFrom = p.SCORE_FROM,
                             ScoreTo = p.SCORE_TO,
                             EffectDate = p.EFFECT_DATE,
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
            
                var joined = await (from p in _dbContext.TrClassifications.AsNoTracking().Where(p => p.ID == id).DefaultIfEmpty()
                                    from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.DESC_ID).DefaultIfEmpty()
                                    select new TrClassificationDTO
                                    {
                                        Id = p.ID,
                                        Name = p.NAME,
                                        DescId = p.DESC_ID,
                                        DescName = s.NAME,
                                        ScoreFrom = p.SCORE_FROM,
                                        ScoreTo = p.SCORE_TO,
                                        EffectDate = p.EFFECT_DATE,
                                        IsActive = p.IS_ACTIVE,
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, TrClassificationDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<TrClassificationDTO> dtos, string sid)
        {
            var add = new List<TrClassificationDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, TrClassificationDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<TrClassificationDTO> dtos, string sid, bool patchMode = true)
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

