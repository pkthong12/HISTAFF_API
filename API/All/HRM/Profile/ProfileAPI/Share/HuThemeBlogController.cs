using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.Share
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-SHARE-THEME-BLOG")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuThemeBlogController : BaseController1
    {
        public HuThemeBlogController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        [HttpGet]
        public async Task<ActionResult> GetAll(ThemeBlogDTO param)
        {
            var r = await _unitOfWork.ThemeBlogRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            var r = await _unitOfWork.ThemeBlogRepository.GetList();
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.ThemeBlogRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] ThemeBlogInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }

            var r = await _unitOfWork.ThemeBlogRepository.CreateAsync(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] ThemeBlogInputDTO param)
        {

            if (param.Id == null)
            {
                return ResponseResult("ID_NOT_BLANK");
            }

            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }

            var r = await _unitOfWork.ThemeBlogRepository.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> ChangeStatus([FromBody] List<long> ids)
        {
            var r = await _unitOfWork.ThemeBlogRepository.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }
      
    }
}
