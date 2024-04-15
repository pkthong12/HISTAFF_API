using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-PORTAL_NOTIFY")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuNotifyController : BaseController1
    {
        public HuNotifyController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }



        /// <summary>
        /// Portal Get slider for Home 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> PortalHomeCount()
        {
            var r = await _unitOfWork.BlogInternalRepository.PortalHomeNotify();
            return ResponseResult(r);
        }

        /// <summary>
        /// Portal Get slider for Home 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> PortalApproveCount()
        {
            try
            {
                var r = await _unitOfWork.BlogInternalRepository.PortalApproveNotify();
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
