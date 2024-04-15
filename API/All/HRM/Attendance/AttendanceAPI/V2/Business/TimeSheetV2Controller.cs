using Microsoft.AspNetCore.Mvc;
using AttendanceDAL.Repositories;
using AttendanceDAL.ViewModels;

namespace AttendanceAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "3-ATTENDANCE-V2")]
    [ApiController]

    [Route("api/v2/TimeSheetDaily/[action]")]
    public class TimeSheetV2Controller : BaseController2
    {
        public TimeSheetV2Controller(IAttendanceUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        //[HttpGet]
        //public async Task<ActionResult> GetAll(TimeSheetDailyDTO param)
        //{
        //    var r = await _unitOfWork.TimeSheetDailyRepository.V2GetAll(param);
        //    return Ok(r);
        //}

    }
}
