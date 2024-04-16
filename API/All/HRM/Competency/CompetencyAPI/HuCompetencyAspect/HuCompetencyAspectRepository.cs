using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Common.Extensions;
using System.Linq;

namespace API.Controllers.HuCompetencyAspect
{
    public class HuCompetencyAspectRepository : IHuCompetencyAspectRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_COMPETENCY_ASPECT, HuCompetencyAspectDTO> _genericRepository;
        private readonly GenericReducer<HU_COMPETENCY_ASPECT, HuCompetencyAspectDTO> _genericReducer;

        public HuCompetencyAspectRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_COMPETENCY_ASPECT, HuCompetencyAspectDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuCompetencyAspectDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCompetencyAspectDTO> request)
        {
            var joined = from p in _dbContext.HuCompetencyAspects.AsNoTracking()
                         from c in _dbContext.HuCompetencys.AsNoTracking().Where(c => c.ID == p.COMPETENCY_ID).DefaultIfEmpty()
                         from g in _dbContext.HuCompetencyGroups.AsNoTracking().Where(g => g.ID == c.COMPETENCY_GROUP_ID).DefaultIfEmpty()
                         select new HuCompetencyAspectDTO
                         {
                             Id = p.ID,
                             Name = p.NAME,
                             Code = p.CODE,
                             CompetencyId = p.COMPETENCY_ID,
                             CompetencyName = c.NAME,
                             CompetencyGroupName = g.NAME,
                             Note = p.NOTE,
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
            
                var joined = await (from p in _dbContext.HuCompetencyAspects.AsNoTracking().Where(p => p.ID == id)
                                    from c in _dbContext.HuCompetencys.AsNoTracking().Where(c => c.ID == p.COMPETENCY_ID).DefaultIfEmpty()
                                    from g in _dbContext.HuCompetencyGroups.AsNoTracking().Where(g => g.ID == c.COMPETENCY_GROUP_ID).DefaultIfEmpty()
                                    select new HuCompetencyAspectDTO
                                    {
                                        Id = p.ID,
                                        Name = p.NAME,
                                        Code = p.CODE,
                                        CompetencyId = p.COMPETENCY_ID,
                                        CompetencyName = c.NAME,
                                        CompetencyGroupName = g.NAME,
                                        Note = p.NOTE,
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuCompetencyAspectDTO dto, string sid)
        {
            //string newCode = "";
            //if (await _dbContext.HuCompetencyAspects.CountAsync() == 0)
            //{
            //    newCode = "KC001";
            //}
            //else
            //{
            //    string lastestData = _dbContext.HuCompetencyAspects.OrderByDescending(t => t.CODE).First().CODE!.ToString();

            //    string newNumber = (Int32.Parse(lastestData.Substring(4)) + 1).ToString();
            //    while (newNumber.Length < 3)
            //    {
            //        newNumber = "0" + newNumber;
            //    }
            //    newCode = lastestData.Substring(0, 4) + newNumber;
            //}
            //dto.Code = newCode;
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuCompetencyAspectDTO> dtos, string sid)
        {
            var add = new List<HuCompetencyAspectDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuCompetencyAspectDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuCompetencyAspectDTO> dtos, string sid, bool patchMode = true)
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
                var data = await (from p in _dbContext.HuCompetencyAspects
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
            if (await _dbContext.HuCompetencyAspects.CountAsync() == 0)
            {
                newCode = "KC001";
            }
            else
            {
                string lastestData = _dbContext.HuCompetencyAspects.OrderByDescending(t => t.CODE).First().CODE!.ToString();
                newCode = lastestData.Substring(0, 2) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
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

