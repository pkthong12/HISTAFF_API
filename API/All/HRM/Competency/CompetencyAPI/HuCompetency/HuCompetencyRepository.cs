using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using Common.Extensions;

namespace API.Controllers.HuCompetency
{
    public class HuCompetencyRepository : IHuCompetencyRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_COMPETENCY, HuCompetencyDTO> _genericRepository;
        private readonly GenericReducer<HU_COMPETENCY, HuCompetencyDTO> _genericReducer;

        public HuCompetencyRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_COMPETENCY, HuCompetencyDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuCompetencyDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCompetencyDTO> request)
        {
            var joined = from p in _dbContext.HuCompetencys.AsNoTracking()
                         from g in _dbContext.HuCompetencyGroups.AsNoTracking().Where(g => g.ID == p.COMPETENCY_GROUP_ID).DefaultIfEmpty()
                         select new HuCompetencyDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Name = p.NAME,
                             CompetencyGroupId = p.COMPETENCY_GROUP_ID,
                             CompetencyGroupName = g.NAME,
                             Note = p.NAME,
                             EffectDate = p.EFFECT_DATE,
                             ExpireDate = p.EXPIRE_DATE,
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
            
                var joined = await (from p in _dbContext.HuCompetencys.AsNoTracking().Where(p => p.ID == id)
                                    from g in _dbContext.HuCompetencyGroups.AsNoTracking().Where(g => g.ID == p.COMPETENCY_GROUP_ID).DefaultIfEmpty()
                                    select new HuCompetencyDTO
                                    {
                                        Id = p.ID,
                                        Code = p.CODE,
                                        Name = p.NAME,
                                        CompetencyGroupId = p.COMPETENCY_GROUP_ID,
                                        CompetencyGroupName = g.NAME,
                                        Note = p.NAME,
                                        EffectDate = p.EFFECT_DATE,
                                        ExpireDate = p.EXPIRE_DATE,
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuCompetencyDTO dto, string sid)
        {
            dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuCompetencyDTO> dtos, string sid)
        {
            var add = new List<HuCompetencyDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuCompetencyDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuCompetencyDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetList()
        {
            try
            {
                var data = await (from p in _dbContext.HuCompetencys
                                  where p.IS_ACTIVE == true
                                  orderby p.NAME
                                  select new
                                  {
                                      Id = p.ID,
                                      Name = "[" + p.CODE + "] " + p.NAME
                                  }).ToListAsync();
                return new FormatedResponse() { InnerBody = data };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public async Task<FormatedResponse> CreateNewCode()
        {
            string newCode = "";
            if (await _dbContext.HuCompetencys.CountAsync() == 0)
            {
                newCode = "NL001";
            }
            else
            {
                string lastestData = _dbContext.HuCompetencys.OrderByDescending(t => t.CODE).First().CODE!.ToString();

                string newNumber = (Int32.Parse(lastestData.Substring(4)) + 1).ToString();
                while (newNumber.Length < 3)
                {
                    newNumber = "0" + newNumber;
                }
                newCode = lastestData.Substring(0, 4) + newNumber;
            }
            return new FormatedResponse() { InnerBody = new { Code = newCode } };
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }
    }
}

