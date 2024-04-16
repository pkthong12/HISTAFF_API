using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.TrPrepare
{
    public class TrPrepareRepository : ITrPrepareRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<TR_PREPARE, TrPrepareDTO> _genericRepository;
        private readonly GenericReducer<TR_PREPARE, TrPrepareDTO> _genericReducer;

        public TrPrepareRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<TR_PREPARE, TrPrepareDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<TrPrepareDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrPrepareDTO> request)
        {
            var joined = from p in _dbContext.TrPrepares.AsNoTracking()
                         from t in _dbContext.TrPrograms.AsNoTracking().Where(t => t.ID == p.TR_PROGRAM_ID).DefaultIfEmpty()
                         from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.TR_LIST_PREPARE_ID).DefaultIfEmpty()
                         select new TrPrepareDTO
                         {
                             Id = p.ID,
                             TrProgramId = p.TR_PROGRAM_ID,
                             TrProgramName = t.TR_PROGRAM_CODE,
                             TrListPrepareId = p.TR_LIST_PREPARE_ID,
                             TrListPrepareName = s.NAME,
                             EmployeeId = e.ID,
                             EmployeeCode = e.CODE,
                             EmployeeName = e.Profile!.FULL_NAME,
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
            
            var joined = await (from p in _dbContext.TrPrepares.AsNoTracking().Where(p => p.ID == id)
                                from t in _dbContext.TrPrograms.AsNoTracking().Where(t => t.ID == p.TR_PROGRAM_ID).DefaultIfEmpty()
                                from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                                from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.TR_LIST_PREPARE_ID).DefaultIfEmpty()
                                    // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                                select new TrPrepareDTO
                                {
                                    Id = p.ID,
                                    TrProgramId = p.TR_PROGRAM_ID,
                                    TrProgramName = t.NAME,
                                    TrListPrepareId = p.TR_LIST_PREPARE_ID,
                                    TrListPrepareName = s.NAME,
                                    EmployeeId = e.ID,
                                    EmployeeCode = e.CODE,
                                    EmployeeName = e.Profile!.FULL_NAME,
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, TrPrepareDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<TrPrepareDTO> dtos, string sid)
        {
            var add = new List<TrPrepareDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, TrPrepareDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<TrPrepareDTO> dtos, string sid, bool patchMode = true)
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

        public Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            throw new NotImplementedException();
        }
    }
}

