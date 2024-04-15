using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.InsSpecifiedObjects
{
    public class InsSpecifiedObjectsRepository : IInsSpecifiedObjectsRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<INS_SPECIFIED_OBJECTS, InsSpecifiedObjectsDTO> _genericRepository;
        private readonly GenericReducer<INS_SPECIFIED_OBJECTS, InsSpecifiedObjectsDTO> _genericReducer;

        public InsSpecifiedObjectsRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<INS_SPECIFIED_OBJECTS, InsSpecifiedObjectsDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<InsSpecifiedObjectsDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsSpecifiedObjectsDTO> request)
        {
            var joined = from p in _dbContext.InsSpecifiedObjectss.AsNoTracking()
                         from u in _uow.Context.Set<SYS_USER>().AsNoTracking().Where(u => u.ID == p.UPDATED_BY).DefaultIfEmpty()
                         from c in _uow.Context.Set<SYS_USER>().AsNoTracking().Where(c => c.ID == p.CREATED_BY).DefaultIfEmpty()
                         select new InsSpecifiedObjectsDTO
                         {
                             Id = p.ID,
                             EffectiveDate = p.EFFECTIVE_DATE!,
                             EffectiveDateString = p.EFFECTIVE_DATE!.Value.ToString("dd/MM/yyyy"),
                             ChangeDay = p.CHANGE_DAY,
                             SiHi = p.SI_HI,
                             Ui = p.UI,
                             HiCom = p.HI_COM,
                             SiCom = p.SI_COM,
                             UiCom = p.UI_COM,
                             AiOaiCom = p.AI_OAI_COM,
                             HiEmp = p.HI_EMP,
                             SiEmp = p.SI_EMP,
                             UiEmp = p.UI_EMP,
                             AiOaiEmp = p.AI_OAI_EMP,
                             RetireMale = p.RETIRE_MALE,
                             RetireFemale = p.RETIRE_FEMALE,
                             CreatedDate = p.CREATED_DATE,
                             UpdatedDate = p.UPDATED_DATE == null ? p.CREATED_DATE : p.UPDATED_DATE,
                             CreatedByUsername = c.FULLNAME,
                             UpdatedByUsername = p.UPDATED_BY == null ? c.FULLNAME : u.FULLNAME,
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
                var list = new List<INS_SPECIFIED_OBJECTS>
                    {
                        (INS_SPECIFIED_OBJECTS)response
                    };
                var joined = (from l in list
                              from c in _uow.Context.Set<SYS_USER>().AsNoTracking().ToList().Where(c => c.ID == l.CREATED_BY).DefaultIfEmpty()
                              from u in _uow.Context.Set<SYS_USER>().AsNoTracking().ToList().Where(u => u.ID == l.UPDATED_BY).DefaultIfEmpty()
                              select new InsSpecifiedObjectsDTO
                              {
                                  Id = l.ID,
                                  EffectiveDate = l.EFFECTIVE_DATE,
                                  ChangeDay = l.CHANGE_DAY,
                                  Ui = l.UI,
                                  SiHi = l.SI_HI,
                                  SiCom = l.SI_COM,
                                  SiEmp = l.SI_EMP,
                                  HiCom = l.HI_COM,
                                  HiEmp = l.HI_EMP,
                                  UiCom = l.UI_COM,
                                  UiEmp = l.UI_EMP,
                                  AiOaiCom = l.AI_OAI_COM,
                                  AiOaiEmp = l.AI_OAI_EMP,
                                  RetireMale = l.RETIRE_MALE,
                                  RetireFemale = l.RETIRE_FEMALE,
                                  CreatedDate = l.CREATED_DATE,
                                  CreatedBy = l.CREATED_BY,
                                  UpdatedBy = l.UPDATED_BY,
                                  UpdatedDate = l.UPDATED_DATE == null ? l.CREATED_DATE : l.UPDATED_DATE,
                                  CreatedByUsername = c.FULLNAME,
                                  UpdatedByUsername = l.UPDATED_BY == null ? c.FULLNAME : u.FULLNAME,

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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, InsSpecifiedObjectsDTO dto, string sid)
        {
            var exists = _dbContext.InsSpecifiedObjectss.Where(p => p.EFFECTIVE_DATE!.Value.Date == dto.EffectiveDate!.Value.Date).Count();
            if (exists != 0)
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.DATE_EFFECTIVE_EXISTS, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<InsSpecifiedObjectsDTO> dtos, string sid)
        {
            var add = new List<InsSpecifiedObjectsDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, InsSpecifiedObjectsDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<InsSpecifiedObjectsDTO> dtos, string sid, bool patchMode = true)
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
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

    }
}

