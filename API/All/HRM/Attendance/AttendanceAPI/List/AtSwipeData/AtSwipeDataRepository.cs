using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.AtSwipeData
{
    public class AtSwipeDataRepository : IAtSwipeDataRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<AT_SWIPE_DATA, AtSwipeDataDTO> _genericRepository;
        private readonly GenericReducer<AT_SWIPE_DATA, AtSwipeDataDTO> _genericReducer;

        public AtSwipeDataRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<AT_SWIPE_DATA, AtSwipeDataDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<AtSwipeDataDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtSwipeDataDTO> request)
        {
            var joined = from p in _dbContext.AtSwipeDatas.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(emp => emp.ITIME_ID == p.ITIME_ID).DefaultIfEmpty()
                         from po in _dbContext.HuPositions.AsNoTracking().Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                         from j in _dbContext.HuJobs.AsNoTracking().Where(x => x.ID == po.JOB_ID).DefaultIfEmpty()
                         from o in _dbContext.HuOrganizations.AsNoTracking().Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                         //from c in _dbContext.AtShifts.AsNoTracking().Where(shif => shif.ID == p.SHIFT_ID).DefaultIfEmpty()
                         from t in _dbContext.AtTerminals.AsNoTracking().Where(ter => ter.ID == p.TERMINAL_ID).DefaultIfEmpty()
                         select new AtSwipeDataDTO
                         {
                             Id = p.ID,
                             EmpId = p.EMP_ID,
                             EmplCode = e.CODE,
                             EmplName = e.Profile!.FULL_NAME,
                             JobOrderNum = (int)(j.ORDERNUM ?? 999),
                             OrgId = e.ORG_ID,
                             PosName = o.NAME,
                             ItimeId = p.ITIME_ID,
                             TerminalCode = t.TERMINAL_CODE,
                             WorkingDay = p.WORKING_DAY,
                             ValTime = p.VALTIME,
                             IsGps = p.IS_GPS,
                             AddressPlace = t.ADDRESS_PLACE,
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
                var list = new List<AT_SWIPE_DATA>
                    {
                        (AT_SWIPE_DATA)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new AtSwipeDataDTO
                              {
                                  Id = l.ID
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, AtSwipeDataDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<AtSwipeDataDTO> dtos, string sid)
        {
            var add = new List<AtSwipeDataDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, AtSwipeDataDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<AtSwipeDataDTO> dtos, string sid, bool patchMode = true)
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

