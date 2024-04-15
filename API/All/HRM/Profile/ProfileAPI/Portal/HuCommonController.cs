using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using Common.Extensions;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-PORTAL-COMMON")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuCommonController : BaseController1
    {
        public HuCommonController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        [HttpGet]
        public async Task<ActionResult> GetInfo()
        {
            var r = await _unitOfWork.EmployeeRepository.GetInfo();
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<ActionResult> InsuranceGetAll()
        {
            var r = await _unitOfWork.InsChangeRepository.PortalGetAll();
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> PortalWatched([FromBody]long id)
        {
            try
            {
                var r = await _unitOfWork.BlogInternalRepository.PortalWatched(id);
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public async Task<ActionResult> ConvertSolar2Lunar(string date)
        {
            CommonFunction cs = new CommonFunction();
            var r = cs.convertSolar2Lunar(date,7);
            return ResponseResult(r);
        }
    }
}
