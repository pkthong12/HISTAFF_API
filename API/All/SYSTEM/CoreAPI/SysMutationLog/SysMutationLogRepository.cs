using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;

namespace API.Controllers.SysMutationLog
{
    public class SysMutationLogRepository : ISysMutationLogRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<SYS_MUTATION_LOG, SysMutationLogDTO> _genericRepository;
        private readonly GenericReducer<SYS_MUTATION_LOG, SysMutationLogDTO> _genericReducer;

        public SysMutationLogRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<SYS_MUTATION_LOG, SysMutationLogDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<SysMutationLogDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysMutationLogDTO> request)
        {
            var joined = from p in _dbContext.SysMutationLogs.AsNoTracking()
                         select new SysMutationLogDTO
                         {
                             Id = p.ID,
                             SysFunctionCode = p.SYS_FUNCTION_CODE,
                             SysActionCode = p.SYS_ACTION_CODE,
                             Before = p.BEFORE,
                             After = p.AFTER,
                             Username = p.USERNAME,
                             CreatedDate = p.CREATED_DATE
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
            var response = await _genericRepository.ReadAllByKey(key, value);
            return response;
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, string value)
        {
            var response = await _genericRepository.ReadAllByKey(key, value);
            return response;
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<SYS_MUTATION_LOG>
                    {
                        (SYS_MUTATION_LOG)response
                    };
                var joined = (from l in list
                              select new SysMutationLogDTO
                              {
                                  Id = l.ID,
                                  SysFunctionCode = l.SYS_FUNCTION_CODE,
                                  SysActionCode = l.SYS_ACTION_CODE,
                                  Username = l.USERNAME,
                                  Before = l.BEFORE + (l.BEFORE1 ?? "") + (l.BEFORE2 ?? "") + (l.BEFORE3 ?? ""),
                                  After = l.AFTER + (l.AFTER1 ?? "") + (l.AFTER2 ?? "") + (l.AFTER3 ?? ""),
                                  CreatedDate = l.CREATED_DATE
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, SysMutationLogDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<SysMutationLogDTO> dtos, string sid)
        {
            var add = new List<SysMutationLogDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, SysMutationLogDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<SysMutationLogDTO> dtos, string sid, bool patchMode = true)
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

