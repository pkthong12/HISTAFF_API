using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-SETTING-MAP")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuSettingMapController : BaseController1
    {
        public HuSettingMapController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet]
        public async Task<ActionResult> GetAll(SettingMapDTO param)
        {
            var r = await _unitOfWork.SettingMapRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.SettingMapRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] SettingMapInputDTO param)
        {
            if (param == null)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }

            var r = await _unitOfWork.SettingMapRepository.CreateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody] SettingMapInputDTO param)
        {
            if (param == null)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }

            var r = await _unitOfWork.SettingMapRepository.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> ChangeStatus([FromBody] List<long> ids)
        {
            var r = await _unitOfWork.SettingMapRepository.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            var r = await _unitOfWork.SettingMapRepository.GetList();
            return ResponseResult(r);
        }
        [HttpGet]
        public ActionResult GetIP()
        {
            var r = _unitOfWork.SettingMapRepository.GetIP();
            return ResponseResult(r);
        }

        [HttpGet]
        public ActionResult GetBSSID()
        {
            var r = _unitOfWork.SettingMapRepository.GetBSSID();
            return ResponseResult(r);
        }
    }
}
