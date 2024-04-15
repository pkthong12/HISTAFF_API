using Microsoft.AspNetCore.Mvc;
using PayrollDAL.ViewModels;
using PayrollDAL.Repositories;

namespace PayrollAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "003-PAYROLL-ADVANCE")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class PaAdvanceController : BaseController
    {
        public PaAdvanceController(IPayrollUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<ActionResult> GetAll(AdvanceDTO values)
        {
            var r = await _unitOfWork.AdvanceRepository.GetAll(values);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> Get(int Id)
        {
            var r = await _unitOfWork.AdvanceRepository.GetById(Id);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] AdvanceInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.AdvanceRepository.CreateAsync(param);

            return ResponseResult(r);
        }


        [HttpPost]
        public async Task<ActionResult> Update([FromBody] AdvanceInputDTO param)
        {
            var r = await _unitOfWork.AdvanceRepository.UpdateAsync(param);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> Delete([FromBody] List<long> ids)
        {
            var r = await _unitOfWork.AdvanceRepository.Delete(ids);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> TemplateImport([FromBody] AdvanceTmpParam orgId)
        {
            try
            {
                var stream = await _unitOfWork.AdvanceRepository.TemplateImport(orgId);
                var fileName = "tempAdvance.xlsx";
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
        public async Task<ActionResult> ImportTemplate([FromBody] AdvanceTmpParam param)
        {
            try
            {
                var r = await _unitOfWork.AdvanceRepository.ImportTemplate(param);
                if (r.StatusCode == "200" && r.memoryStream != null)
                {
                    var fileName = "tempAdvanceError.xlsx";
                    return new FileStreamResult(r.memoryStream, "application/octet-stream") { FileDownloadName = fileName };
                }
                else
                {
                    return ImportResult(r);
                }
                
            }
            catch (Exception ex)
            {
                return ResponseResult(ex.Message);
            }
        }

    }
}
