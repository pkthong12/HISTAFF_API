using API.Main;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.All.HRM.Profile.ProfileAPI.Portal.PortalAtEntitlement
{
    [ApiController]
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "010-PORTAL-PORTAL_AT_ENTITLEMENT")]
    [Route("api/[controller]/[action]")]
    public class PortalAtEntitlementController : ControllerBase
    {

        private readonly IPortalAtEntitlement _portalAtEntitlement;
        private readonly AppSettings _appSettings;

        public PortalAtEntitlementController(IPortalAtEntitlement portalAtEntitlement, IOptions<AppSettings> options)
        {
            _portalAtEntitlement = portalAtEntitlement;
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetEntitlement()
        {
            string sid = Request.Sid(_appSettings);
            var response = await _portalAtEntitlement.GetEntitlement(sid);
            return Ok(response);
        }
    }
}
