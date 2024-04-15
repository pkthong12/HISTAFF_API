using CORE.DTO;

namespace API.All.HRM.Profile.ProfileAPI.Portal.PortalAtEntitlement
{
    public interface IPortalAtEntitlement
    {
        Task<FormatedResponse> GetEntitlement(string sid);
    }
}
