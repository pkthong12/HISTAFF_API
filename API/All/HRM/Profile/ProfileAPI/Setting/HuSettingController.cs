using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-SETTING")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuSettingController : BaseController1
    {
        public HuSettingController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet]
        public async Task<ActionResult> GetUserOrg(string userId)
        {
            var r = await _unitOfWork.UserOrganiRepository.GetUserOrg(userId);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> ListGroupPermission(int Id)
        {
            var r = await _unitOfWork.UserOrganiRepository.ListGroupPermission(Id);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> ListUserPermission(string Id)
        {
            var r = await _unitOfWork.UserOrganiRepository.ListUserPermission(Id);
            return Ok(r);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] UserOrganiDTO param)
        {
            var r = await _unitOfWork.UserOrganiRepository.UpdateAsync(param);
            return Ok(r);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateGroup([FromBody] UserOrganiDTO param)
        {
            var r = await _unitOfWork.UserOrganiRepository.UpdateGroupAsync(param);
            return Ok(r);
        }
    }
}
