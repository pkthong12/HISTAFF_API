using Microsoft.AspNetCore.Mvc;
using PayrollDAL.ViewModels;
using PayrollDAL.Repositories;

namespace PayrollAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "003-PAYROLL-SALARY-IMPORT")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class PaSalaryImportController : BaseController
    {
        public PaSalaryImportController(IPayrollUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<ActionResult> GetAll(SalImportDTO values)
        {
            var r = await _unitOfWork.SalaryImportRepository.GetAll(values);
            return Ok(r);
        }


        [HttpPost]
        public async Task<ActionResult> ExportTemplate([FromBody] SalImpSearchParam param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            try
            {
                var stream = await _unitOfWork.SalaryImportRepository.ExportTemplate(param);
                var fileName = "TempImpSal.xlsx";
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
        public async Task<ActionResult> ImportTemplate([FromBody] SalImpImportParam param)
        {
            var r = await _unitOfWork.SalaryImportRepository.ImportTemplate(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> Delete([FromBody] SalImportDelParam param)
        {
            var r = await _unitOfWork.SalaryImportRepository.Delete(param);
            return ResponseResult(r);
        }
    }
}
