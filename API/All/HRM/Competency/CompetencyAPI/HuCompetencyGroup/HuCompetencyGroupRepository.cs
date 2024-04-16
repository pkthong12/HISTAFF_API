using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Common.Extensions;

namespace API.Controllers.HuCompetencyGroup
{
    public class HuCompetencyGroupRepository : IHuCompetencyGroupRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_COMPETENCY_GROUP, HuCompetencyGroupDTO> _genericRepository;
        private readonly GenericReducer<HU_COMPETENCY_GROUP, HuCompetencyGroupDTO> _genericReducer;

        public HuCompetencyGroupRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_COMPETENCY_GROUP, HuCompetencyGroupDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuCompetencyGroupDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCompetencyGroupDTO> request)
        {
            var joined = from p in _dbContext.HuCompetencyGroups.AsNoTracking()
                         select new HuCompetencyGroupDTO
                         {
                             Id = p.ID,
                             Name = p.NAME,
                             Code = p.CODE,
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
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<HU_COMPETENCY_GROUP>
                    {
                        (HU_COMPETENCY_GROUP)response
                    };
                var joined = (from l in list
                              select new HuCompetencyGroupDTO
                              {
                                  Id = l.ID,
                                  Name = l.NAME,
                                  Code = l.CODE,
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuCompetencyGroupDTO dto, string sid)
        {
            dto.IsActive = true;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuCompetencyGroupDTO> dtos, string sid)
        {
            var add = new List<HuCompetencyGroupDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuCompetencyGroupDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuCompetencyGroupDTO> dtos, string sid, bool patchMode = true)
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
                var data = await (from p in _dbContext.HuCompetencyGroups
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
            if (await _dbContext.HuCompetencyGroups.CountAsync() == 0)
            {
                newCode = "NNL001";
            }
            else
            {
                string lastestData = _dbContext.HuCompetencyGroups.OrderByDescending(t => t.CODE).First().CODE!.ToString();

                string newNumber = (Int32.Parse(lastestData.Substring(5)) + 1).ToString();
                while (newNumber.Length < 4)
                {
                    newNumber = "0" + newNumber;
                }
                newCode = lastestData.Substring(0, 5) + newNumber;
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

