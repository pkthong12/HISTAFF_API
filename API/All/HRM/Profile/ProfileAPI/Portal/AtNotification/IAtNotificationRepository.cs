using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.AtNotification
{
    public interface IAtNotificationRepository: IGenericRepository<AT_NOTIFICATION, AtNotificationDTO>
    {
       Task<GenericPhaseTwoListResponse<AtNotificationDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtNotificationDTO> request);
       Task<FormatedResponse> GetNotify(long employeeId);
       Task<FormatedResponse> GetCountNotifyUnRead(long? employeeId);
       Task<FormatedResponse> GetHistoryApprove(long employeeId);
    }
}

