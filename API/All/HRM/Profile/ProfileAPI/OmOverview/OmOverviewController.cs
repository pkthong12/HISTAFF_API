using CORE.DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.All.HRM.Profile.ProfileAPI.OmOverview
{
    [ApiExplorerSettings(GroupName = "050-OM-OM_OVERVIEW")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class OmOverviewController : ControllerBase
    {
        private readonly IOmOverviewService _omOverviewService;

        public OmOverviewController(IOmOverviewService omOverviewService)
        {
            _omOverviewService = omOverviewService;
        }

        [HttpPost]
        public async Task<IActionResult> OrganizationOverview(OmOverviewRequest request)
        {
            var response = await _omOverviewService.OrganizationOverview(request);
            return Ok(response);
        }
    }
}
