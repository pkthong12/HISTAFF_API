using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.Share
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-SHARE-POSITION-SYS")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuPositionSysController : BaseController1
    {
        public HuPositionSysController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet]
        public async Task<ActionResult> GetAll(PositionSysViewDTO param)
        {
            var r = await _unitOfWork.PositionSysRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.PositionSysRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] PositionSysInputDTO param)
        {
            if (param == null)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }
            if (param.GroupId == null)
            {
                return ResponseResult("GROUP_NOT_BLANK");
            }
            if (string.IsNullOrWhiteSpace(param.Code))
            {
                return ResponseResult("CODE_NOT_BLANK");
            }
            if (string.IsNullOrWhiteSpace(param.Name))
            {
                return ResponseResult("NAME_NOT_BLANK");
            }

            var r = await _unitOfWork.PositionSysRepository.CreateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody] PositionSysInputDTO param)
        {
            if (param == null)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }
            if (param.Id == null)
            {
                return ResponseResult("ID_NOT_BLANK");
            }
            if (param.GroupId == null)
            {
                return ResponseResult("GROUP_NOT_BLANK");
            }
            if (string.IsNullOrWhiteSpace(param.Code))
            {
                return ResponseResult("CODE_NOT_BLANK");
            }
            if (string.IsNullOrWhiteSpace(param.Name))
            {
                return ResponseResult("NAME_NOT_BLANK");
            }

            var r = await _unitOfWork.PositionSysRepository.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> ChangeStatus([FromBody] List<long> ids)
        {
            var r = await _unitOfWork.PositionSysRepository.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetList(int groupId)
        {
            var r = await _unitOfWork.PositionSysRepository.GetList(groupId);
            return ResponseResult(r);
        }

    }
}
