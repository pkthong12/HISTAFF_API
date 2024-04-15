using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.List
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "002-PROFILE-REPORT")]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuReportController : BaseController1
    {
        public HuReportController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet]
        public async Task<ActionResult> GetAll(ReportInputDTO param)
        {
            var r = await _unitOfWork.ReportRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetTreeView()
        {
            var r = await _unitOfWork.ReportRepository.GetList();
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetListReport()
        {
            var r = await _unitOfWork.ReportRepository.GetListReport();
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.ReportRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] ReportInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }


            var r = await _unitOfWork.ReportRepository.CreateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody] ReportInputDTO param)
        {
            if (param == null)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }


            var r = await _unitOfWork.ReportRepository.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            var r = await _unitOfWork.ReportRepository.GetList();
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> Delete(long id)
        {
            if (id == 0)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }
            var r = await _unitOfWork.ReportRepository.Delete(id);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetOrgPermission()
        {
            var r = await _unitOfWork.UserOrganiRepository.GetOrgPermission();
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> ReportIns(ReportInsInputDTO param)
        {
            var r = await _unitOfWork.ReportRepository.ReportIns(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> ReportEmployee(ReportEmployeeDTO param)
        {
            var r = await _unitOfWork.ReportRepository.ReportEmployee(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> ReportInsByOrg(ReportInsByOrgDTO param)
        {
            var r = await _unitOfWork.ReportRepository.ReportInsByOrg(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> ReportChartProfile(ReportParam param)
        {
            var r = await _unitOfWork.ReportRepository.REPORT_HU001(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> ReportsProfile(ReportParam param)
        {
            var r = await _unitOfWork.ReportRepository.REPORT_HU009(param);
            return Ok(r);
        }

    }
}
