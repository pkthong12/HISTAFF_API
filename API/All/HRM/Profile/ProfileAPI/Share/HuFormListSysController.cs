using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.Share
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-SHARE-FORM-LIST-SYS")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuFormListSysController : BaseController1
    {
        public HuFormListSysController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet]
        public async Task<ActionResult> GetById(long id)
        {
            var r = await _unitOfWork.FormListSysRepository.GetById(id);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            var r = await _unitOfWork.FormListSysRepository.GetList();
            return Ok(r);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] FormListSysDTO Param)
        {
            var r = await _unitOfWork.FormListSysRepository.UpdateAsync(Param);
            return ResponseResult(r);
        }
    }
}
