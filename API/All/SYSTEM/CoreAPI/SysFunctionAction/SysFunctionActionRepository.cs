using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using API.All.SYSTEM.CoreAPI.SysFunctionAction;

namespace API.Controllers.SysFunctionAction
{
    public class SysFunctionActionRepository : ISysFunctionActionRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<SYS_FUNCTION_ACTION, SysFunctionActionDTO> _genericRepository;
        private readonly GenericReducer<SYS_FUNCTION_ACTION, SysFunctionActionDTO> _genericReducer;

        public SysFunctionActionRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<SYS_FUNCTION_ACTION, SysFunctionActionDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<SysFunctionActionDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysFunctionActionDTO> request)
        {
            var joined = from p in _dbContext.SysFunctionActions.AsNoTracking()
                         from t in _dbContext.SysFunctions.AsNoTracking().DefaultIfEmpty()
                         from q in _dbContext.SysActions.AsNoTracking().DefaultIfEmpty()
                         where t.ID == p.FUNCTION_ID && q.ID == p.ACTION_ID && t.ROOT_ONLY != true
                         // JOIN OTHER ENTITIES BASED ON THE BUSINESS

                         select new SysFunctionActionDTO
                         {
                             Id = p.ID,
                             FunctionId = p.FUNCTION_ID,
                             ActionId = p.ACTION_ID,
                             ActionCode = q.CODE,
                             FunctionCode = t.CODE
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
                return new FormatedResponse() { InnerBody = response };
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, SysFunctionActionDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<SysFunctionActionDTO> dtos, string sid)
        {
            var add = new List<SysFunctionActionDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, SysFunctionActionDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<SysFunctionActionDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> UpdateSysFunctionActionRange(UpdateSysFunctionActionDTO request, string sid)
        {

            try
            {
                _uow.CreateTransaction();
                var now = DateTime.UtcNow;

                // remove old range
                var list = _dbContext.SysFunctionActions.Where(x => x.FUNCTION_ID == request.FunctionId);
                _dbContext.SysFunctionActions.RemoveRange(list);

                // prepare Actions
                List<SYS_ACTION> actions = new();
                request.Codes.ForEach(code =>
                {
                    var action = _dbContext.SysActions.Where(x => x.CODE == code).FirstOrDefault();
                    if (action != null)
                    {
                        actions.Add(action);
                    } else
                    {
                        SYS_ACTION newAction = new()
                        {
                            CODE = code,
                            CREATED_DATE = now,
                            CREATED_BY = sid,
                            UPDATED_DATE = now,
                            UPDATED_BY = sid
                        };
                        _dbContext.SysActions.Add(newAction);
                        _dbContext.SaveChanges();
                        actions.Add(newAction);
                    }
                });

                List<SYS_FUNCTION_ACTION> newList = new();
                actions.ForEach(action =>
                {
                    newList.Add(new()
                    {
                        FUNCTION_ID = request.FunctionId,
                        ACTION_ID = action.ID,
                        CREATED_DATE = now,
                        CREATED_BY = sid,
                        UPDATED_DATE = now,
                        UPDATED_BY = sid
                    });
                });

                await _dbContext.SysFunctionActions.AddRangeAsync(newList);
                await _dbContext.SaveChangesAsync();

                _uow.Commit();

                return new()
                {
                    InnerBody = newList
                };

            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return new()
                {
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    MessageCode = ex.Message,
                    StatusCode = EnumStatusCode.StatusCode500
                };
            }
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

    }
}

