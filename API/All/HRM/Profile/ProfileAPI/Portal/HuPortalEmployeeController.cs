using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-PORTAL-EMPLOYEE")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuPortalEmployeeController : BaseController1
    {
        public HuPortalEmployeeController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
       

        [HttpPost]
        public async Task<ActionResult> EditInfomation([FromBody] EmployeeEditInput param)
        {
            var r = await _unitOfWork.EmployeeRepository.PortalEditInfomation(param);
            return Ok(r);
        }

        [HttpPost]
        public async Task<ActionResult> AddSituation([FromBody] SituationEditDTO param)
        {
            var r = await _unitOfWork.EmployeeRepository.PortalAddSituation(param);
            return Ok(r);
        }

    }
}
