using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-SETTING-POSITION-PAPER")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuPositionPaperController : BaseController1
    {
        public HuPositionPaperController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet]
        public async Task<ActionResult> GetAll(PostionPaperDTO param)
        {
            var r = await _unitOfWork.PositionPaperRepository.GetAll(param);
            return Ok(r);
        }
        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] PosPaperInputDTO param)
        {
            var r = await _unitOfWork.PositionPaperRepository.CreateAsync(param);
            return Ok(r);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAsync([FromBody] List<long> ids)
        {
            var r = await _unitOfWork.PositionPaperRepository.DeleteAsync(ids);
            return Ok(r);
        }
    }
}
