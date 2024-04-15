using CORE.DTO;

namespace API.Controllers.PortalRegisterLeave
{
    public interface IPortalRegisterLeaveRepository
    {
        Task<FormatedResponse> WillLeaveInNextSevenDay(string sid);
    }
}

