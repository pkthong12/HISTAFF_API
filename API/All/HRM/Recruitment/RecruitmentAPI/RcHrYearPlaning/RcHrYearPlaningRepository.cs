using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Common.Repositories;
using System.Linq;

namespace API.Controllers.RcHrYearPlaning
{
    public class RcHrYearPlaningRepository : IRcHrYearPlaningRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<RC_HR_YEAR_PLANING, RcHrYearPlaningDTO> _genericRepository;
        private readonly GenericReducer<RC_HR_YEAR_PLANING, RcHrYearPlaningDTO> _genericReducer;
        private IGenericRepository<RC_HR_PLANING_DETAIL, RcHrPlaningDetailDTO> _genericDetailRepository;
        public RcHrYearPlaningRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<RC_HR_YEAR_PLANING, RcHrYearPlaningDTO>();
            _genericDetailRepository = _uow.GenericRepository<RC_HR_PLANING_DETAIL, RcHrPlaningDetailDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<RcHrYearPlaningDTO>> SinglePhaseQueryList(GenericQueryListDTO<RcHrYearPlaningDTO> request)
        {
            var joined = from p in _dbContext.RcHrYearPlanings.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new RcHrYearPlaningDTO
                         {
                             Id = p.ID,
                             OrgId = p.ORG_ID,
                             Year = p.YEAR,
                             EffectDate = p.EFFECT_DATE,
                             ExpireDate = p.EXPIRE_DATE,
                             Version = p.VERSION,
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
                var list = new List<RC_HR_YEAR_PLANING>
                    {
                        (RC_HR_YEAR_PLANING)response
                    };
                var joined = (from p in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new RcHrYearPlaningDTO
                              {
                                  Id = p.ID,
                                  OrgId = p.ORG_ID,
                                  Year = p.YEAR,
                                  EffectDate = p.EFFECT_DATE,
                                  ExpireDate = p.EXPIRE_DATE,
                                  Version = p.VERSION,
                                  Note = p.NOTE
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, RcHrYearPlaningDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            if (response.StatusCode == EnumStatusCode.StatusCode200)
            {
                var joined = (RC_HR_YEAR_PLANING)response.InnerBody!;
                if (dto.CopiedId != null)
                {
                    var query = await (from l in _dbContext.RcHrPlaningDetails.AsNoTracking().Where(p => p.YEAR_PLAN_ID == dto.CopiedId)
                                       select new RcHrPlaningDetailDTO
                                       {
                                           YearPlanId = joined.ID,
                                           OrgId = l.ORG_ID,
                                           //OrgName = o.NAME, please don't open comment
                                           Month1 = l.MONTH_1,
                                           Month2 = l.MONTH_2,
                                           Month3 = l.MONTH_3,
                                           Month4 = l.MONTH_4,
                                           Month5 = l.MONTH_5,
                                           Month6 = l.MONTH_6,
                                           Month7 = l.MONTH_7,
                                           Month8 = l.MONTH_8,
                                           Month9 = l.MONTH_9,
                                           Month10 = l.MONTH_10,
                                           Month11 = l.MONTH_11,
                                           Month12 = l.MONTH_12,
                                       }).ToListAsync();
                    var resposne = await _genericDetailRepository.CreateRange(_uow, query, sid);
                }
            }
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<RcHrYearPlaningDTO> dtos, string sid)
        {
            var add = new List<RcHrYearPlaningDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, RcHrYearPlaningDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            if (response.StatusCode == EnumStatusCode.StatusCode200)
            {
                if (dto.CopiedId != null)
                {
                    //xoa cac ban ghi cu
                    var deteleIds = await _dbContext.RcHrPlaningDetails.AsNoTracking().Where(p => p.YEAR_PLAN_ID == dto.Id).Select(p => p.ID).ToListAsync();
                    await _genericDetailRepository.DeleteIds(_uow, deteleIds);
                    var query = await (from l in _dbContext.RcHrPlaningDetails.AsNoTracking().Where(p => p.YEAR_PLAN_ID == dto.CopiedId)
                                       select new RcHrPlaningDetailDTO
                                       {
                                           YearPlanId = dto.Id,
                                           OrgId = l.ORG_ID,
                                           //OrgName = o.NAME, please don't open comment
                                           Month1 = l.MONTH_1,
                                           Month2 = l.MONTH_2,
                                           Month3 = l.MONTH_3,
                                           Month4 = l.MONTH_4,
                                           Month5 = l.MONTH_5,
                                           Month6 = l.MONTH_6,
                                           Month7 = l.MONTH_7,
                                           Month8 = l.MONTH_8,
                                           Month9 = l.MONTH_9,
                                           Month10 = l.MONTH_10,
                                           Month11 = l.MONTH_11,
                                           Month12 = l.MONTH_12,
                                       }).ToListAsync();
                    //copy ban ghi moi
                    await _genericDetailRepository.CreateRange(_uow, query, sid);
                }
            }
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<RcHrYearPlaningDTO> dtos, string sid, bool patchMode = true)
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
            if (response.StatusCode == EnumStatusCode.StatusCode200)
            {
                var joined = await (_dbContext.RcHrPlaningDetails.AsNoTracking().Where(p => ids.Contains((long)p.YEAR_PLAN_ID!)).Select(p => p.ID)).ToListAsync();
                await _genericDetailRepository.DeleteIds(_uow, joined);
            }
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> GetAllPlaning(long? id)
        {
            var response = await (from p in _dbContext.RcHrYearPlanings.AsNoTracking().Where(p => p.ID != id)
                                  select new
                                  {
                                      Id = p.ID,
                                      EffectDate = p.EFFECT_DATE!.Value.Date.ToString("dd/MM/yyyy"),
                                      Note = p.NOTE,
                                      Year = p.YEAR,
                                      Version = p.VERSION
                                  }).ToListAsync();
            return new FormatedResponse() { InnerBody = response, StatusCode = EnumStatusCode.StatusCode200 };
        }
    }
}

