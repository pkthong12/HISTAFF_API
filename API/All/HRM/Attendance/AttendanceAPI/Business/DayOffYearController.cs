using Microsoft.AspNetCore.Mvc;
using AttendanceDAL.Repositories;
using AttendanceDAL.ViewModels;

namespace AttendanceAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "3-ATTENDANCE-DAY-OFF-YEAR")]
    [ApiController]

    [Route("api/hr/DayOffYear/[action]")]
    public class DayOffYearController : BaseController2
    {
        public DayOffYearController(IAttendanceUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet]
        public async Task<ActionResult> GetAll(DayOffYearDTO param)
        {
            var r = await _unitOfWork.DayOffYearRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetById()
        {
            var r = await _unitOfWork.DayOffYearRepository.GetById();
            return Ok(r);
        }

        [HttpPost]
        public async Task<ActionResult> Update([FromBody] DayOffYearConfigDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.DayOffYearRepository.UpdateAsync(param);
            return ResponseResult(r);
        }

    }
}
