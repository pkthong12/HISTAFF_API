using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.List
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "002-PROFILE-COMMEND")]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuCommendOldController : BaseController1
    {
        public HuCommendOldController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet]
        public async Task<ActionResult> GetAll(CommendDTO param)
        {
            var r = await _unitOfWork.CommendRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> Get(long Id)
        {
            var r = await _unitOfWork.CommendRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody]CommendInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.CommendRepository.CreateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody]CommendInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.CommendRepository.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Remove([FromBody]long id)
        {
            var r = await _unitOfWork.CommendRepository.RemoveAsync(id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> OpenStatus([FromBody]long id)
        {
            var r = await _unitOfWork.CommendRepository.OpenStatus(id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Approve([FromBody]long id)
        {
            var r = await _unitOfWork.CommendRepository.Approve(id);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> PortalGetAll()
        {
            var r = await _unitOfWork.CommendRepository.PortalGetAll();
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> PortalGetBy(long id)
        {
            var r = await _unitOfWork.CommendRepository.PortalGetBy(id);
            return Ok(r);
        }
    }
}
