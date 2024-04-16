using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;

namespace API.All.SYSTEM.CoreAPI.SwPushSubscription
{
    public class SwPushSubscriptionRepository : ISwPushSubscriptionRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<SW_PUSH_SUBSCRIPTION, SwPushSubscriptionDTO> _genericRepository;
        private readonly GenericReducer<SW_PUSH_SUBSCRIPTION, SwPushSubscriptionDTO> _genericReducer;

        public SwPushSubscriptionRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<SW_PUSH_SUBSCRIPTION, SwPushSubscriptionDTO>();
            _genericReducer = new();
        }

        public async Task<FormatedResponse> FindSubscription(CheckSubscriptionRequest request)
        {
            var check = await _dbContext.SwPushSubscriptions.SingleOrDefaultAsync(x => x.USER_ID == request.UserId && x.ENDPOINT == request.Endpoint);
            if (check != null)
            {
                return new() { StatusCode = EnumStatusCode.StatusCode200, InnerBody = true };
            } else
            {
                return new() { StatusCode = EnumStatusCode.StatusCode200, InnerBody = false };
            }
        }

        public async Task<GenericPhaseTwoListResponse<SwPushSubscriptionDTO>> SinglePhaseQueryList(GenericQueryListDTO<SwPushSubscriptionDTO> request)
        {
            var joined = from p in _dbContext.SwPushSubscriptions.AsNoTracking()
                         select new SwPushSubscriptionDTO
                         {
                             Id = p.ID,
                             UserId = p.USER_ID,
                             Endpoint = p.ENDPOINT,
                             Subscription = p.SUBSCRIPTION,
                             CreatedDate = p.CREATED_DATE,
                             CreatedBy = p.CREATED_BY,
                             UpdatedDate = p.UPDATED_DATE,
                             UpdatedBy = p.UPDATED_BY
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            var response = await _genericRepository.ReadAll();
            return response;
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
                var list = new List<SW_PUSH_SUBSCRIPTION>
                    {
                        (SW_PUSH_SUBSCRIPTION)response
                    };
                var joined = (from p in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new SwPushSubscriptionDTO
                              {
                                  Id = p.ID,
                                  UserId = p.USER_ID,
                                  Endpoint = p.ENDPOINT,
                                  Subscription = p.SUBSCRIPTION,
                                  CreatedDate = p.CREATED_DATE,
                                  CreatedBy = p.CREATED_BY,
                                  UpdatedDate = p.UPDATED_DATE,
                                  UpdatedBy = p.UPDATED_BY
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, SwPushSubscriptionDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

                public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<SwPushSubscriptionDTO> dtos, string sid)
        {
            var add = new List<SwPushSubscriptionDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, SwPushSubscriptionDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<SwPushSubscriptionDTO> dtos, string sid, bool patchMode = true)
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

