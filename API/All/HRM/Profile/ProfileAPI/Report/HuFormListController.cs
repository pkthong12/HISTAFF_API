using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-REPORT-FORM-LIST")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuFormListController : BaseController1
    {
        public HuFormListController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet]
        public async Task<ActionResult> GetById(long id)
        {
            var r = await _unitOfWork.FormListRepository.GetById(id);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetTreeView()
        {
            var r = await _unitOfWork.FormListRepository.GetTreeView();
            return Ok(r);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] FormListDTO Param)
        {
            var r = await _unitOfWork.FormListRepository.UpdateAsync(Param);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetListRemind()
        {
            var r = await _unitOfWork.FormListRepository.GetListRemind();
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetRemind()
        {
            var r = await _unitOfWork.FormListRepository.GetRemind();
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> PrintForm(int id, int typeId)
        {
            var r = await _unitOfWork.FormListRepository.PrintForm(id, typeId);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> PrintFormProfile(int id)
        {
            var r = await _unitOfWork.FormListRepository.PrintFormProfile(id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> PrintFormSalary([FromBody] PayrollInputDTO param)
        {
            var r = await _unitOfWork.FormListRepository.PrintFormSalary(param);

            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> PrintFormAttendance([FromBody] PayrollInputDTO param)
        {
            var r = await _unitOfWork.FormListRepository.PrintFormAttendance(param);

            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateRemind([FromBody] List<SettingRemindDTO> param)
        {
            var r = await _unitOfWork.FormListRepository.UpdateRemind(param);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetDashboard()
        {
            var r = await _unitOfWork.FormListRepository.GetDashboard();
            return Ok(r);
        }

    }
}
