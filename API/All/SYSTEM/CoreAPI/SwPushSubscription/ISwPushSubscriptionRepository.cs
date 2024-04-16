using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.SYSTEM.CoreAPI.SwPushSubscription
{
    public interface ISwPushSubscriptionRepository : IGenericRepository<SW_PUSH_SUBSCRIPTION, SwPushSubscriptionDTO>
    {
        Task<GenericPhaseTwoListResponse<SwPushSubscriptionDTO>> SinglePhaseQueryList(GenericQueryListDTO<SwPushSubscriptionDTO> request);
        Task<FormatedResponse> FindSubscription(CheckSubscriptionRequest request);
    }
}

