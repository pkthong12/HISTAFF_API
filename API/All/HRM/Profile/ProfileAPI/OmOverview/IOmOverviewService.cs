using CORE.DTO;

namespace API.All.HRM.Profile.ProfileAPI.OmOverview
{
    public interface IOmOverviewService
    {
        Task<FormatedResponse> OrganizationOverview(OmOverviewRequest request);
    }
}
