using Microsoft.AspNetCore.Mvc;
using PayrollDAL.Repositories;
using Common.Extensions;
using PayrollDAL.ViewModels;

namespace PayrollAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "003-PAYROLL-REPORT")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class PaReportController : BaseController
    {
        public PaReportController(IPayrollUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }


        [HttpGet]
        public async Task<ActionResult> PA001(ParaInputReport param)
        {
            var r = await _unitOfWork.ReportRepository.PA001(param);
            return Ok(r);
        }

        [HttpPost]
        public async Task<ActionResult> PA002([FromBody] ParaInputReport param)
        {
            try
            {
                var stream = await _unitOfWork.ReportRepository.PA002(param);
                var fileName = "BangLuong.xlsx";
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
        public async Task<ActionResult> ThuNhapBinhQuan([FromBody] SalImpImportParam param)
        {
            try
            {
                var stream = await _unitOfWork.ReportRepository.ThuNhapBinhQuan(param.OrgId);
                var fileName = "ThuNhapBinhQuan.xlsx";
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
        public async Task<ActionResult> CoCauNhanSu([FromBody] CCNSParam param)
        {
            try
            {
                var stream = await _unitOfWork.ReportRepository.CoCauNhanSu(param);
                var fileName = "CoCauNhanSu.xlsx";
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
        public async Task<ActionResult> TongHopQuyLuong([FromBody] CCNSParam param)
        {
            try
            {
                var stream = await _unitOfWork.ReportRepository.TongHopQuyLuong(param);
                var fileName = "TongHopQuyLuong.xlsx";
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
