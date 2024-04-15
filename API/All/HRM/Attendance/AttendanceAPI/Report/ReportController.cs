using Microsoft.AspNetCore.Mvc;
using AttendanceDAL.Repositories;
using Common.Extensions;

namespace AttendanceAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "3-ATTENDANCE-REPORT")]
    [ApiController]

    [Route("api/attendance/report/[action]")]
    public class ReportController : BaseController2
    {
        public ReportController(IAttendanceUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }


        [HttpPost]
        public async Task<ActionResult> AT002([FromBody] ParaInputReport param)
        {
            try
            {
                var stream = await _unitOfWork.ReportRepository.AT002(param);
                var fileName = "KeHoachCong.xlsx";
                if (stream.StatusCode == "200")
                {
                    return new FileStreamResult(stream.memoryStream, "application/octet-stream") { FileDownloadName = fileName };
                }
                return ResponseResult(stream);
            }
            catch (Exception ex)
            {
                return ResponseResult(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AT003([FromBody] ParaInputReport param)
        {
            try
            {
                var stream = await _unitOfWork.ReportRepository.AT003(param);
                var fileName = "BangCong.xlsx";
                if (stream.StatusCode == "200")
                {
                    return new FileStreamResult(stream.memoryStream, "application/octet-stream") { FileDownloadName = fileName };
                }
                return ResponseResult(stream);
            }
            catch (Exception ex)
            {
                return ResponseResult(ex.Message);
            }
        }
    }
}
