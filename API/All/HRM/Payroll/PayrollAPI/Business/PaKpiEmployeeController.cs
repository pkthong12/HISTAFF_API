using Microsoft.AspNetCore.Mvc;
using PayrollDAL.ViewModels;
using PayrollDAL.Repositories;

namespace PayrollAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "003-PAYROLL-KPI-EMPLOYEE")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class PaKpiEmployeeController : BaseController
    {
        public PaKpiEmployeeController(IPayrollUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<ActionResult> GetAll(KpiEmployeeDTO values)
        {
            var r = await _unitOfWork.KpiEmployeeRepository.GetAll(values);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.KpiEmployeeRepository.GetById(Id);
            return ResponseResult(r);
        }




        [HttpPost]
        public async Task<ActionResult> Update([FromBody] KpiEmployeeInputDTO param)
        {

            var r = await _unitOfWork.KpiEmployeeRepository.UpdateAsync(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeStatus([FromBody] List<long> ids)
        {
            var r = await _unitOfWork.KpiEmployeeRepository.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Delete([FromBody] List<long> ids)
        {
            var r = await _unitOfWork.KpiEmployeeRepository.Delete(ids);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> ExportTemplate([FromBody] KpiEmployeeInput param)
        {
            try
            {
                var stream = await _unitOfWork.KpiEmployeeRepository.ExportTemplate(param);
                var fileName = "template.xlsx";
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
        public async Task<ActionResult> ImportFromTemplate([FromForm] KpiEmployeeImport param)
        {
            try
            {
                var res = await _unitOfWork.KpiEmployeeRepository.ImportFromTemplate(param);
                return ResponseResult(res);

            }
            catch (Exception ex)
            {
                return ResponseResult(ex.Message);
            }

        }
        [HttpPost]
        public async Task<ActionResult> CaclKpiSalary([FromBody] KpiEmployeeInput param)
        {
            try
            {
                var res = await _unitOfWork.KpiEmployeeRepository.CaclKpiSalary(param);
                return ResponseResult(res);

            }
            catch (Exception ex)
            {
                return ResponseResult(ex.Message);
            }

        }
        [HttpGet]
        public async Task<ActionResult> CheckKPILock(LockInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.KpiEmployeeRepository.IsLockKPI(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> LockKPI(LockInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.KpiEmployeeRepository.LockKPI(param);
            return Ok(r);
        }

    }
}
