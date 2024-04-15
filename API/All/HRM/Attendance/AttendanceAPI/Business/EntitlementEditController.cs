using Microsoft.AspNetCore.Mvc;
using AttendanceDAL.Repositories;
using AttendanceDAL.ViewModels;
using Common.Extensions;

namespace AttendanceAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "3-ATTENDANCE-ENTITLEMENT-EDIT")]
    [ApiController]

    [Route("api/hr/entitlement/[action]")]
    public class EntitlementEditController : BaseController2
    {
        public EntitlementEditController(IAttendanceUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<ActionResult> GetAll(EntitlementEditDTO param)
        {
            var r = await _unitOfWork.EntitlementEditRepository.GetAll(param);
            return Ok(r);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] EntitlementEditInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.EntitlementEditRepository.CreateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody] EntitlementEditInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.EntitlementEditRepository.UpdateAsync(param);
            return ResponseResult(r);
        }
      
        [HttpPost]
        public async Task<ActionResult> Delete([FromBody] List<long> param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.EntitlementEditRepository.Delete(param);
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<ActionResult> Get(int id)
        {
            var r = await _unitOfWork.EntitlementEditRepository.GetBy(id);
            return Ok(r);
        }

        [HttpPost]
        public async Task<ActionResult> ExportTemplate([FromBody] ParaOrg param)
        {
            try
            {
                var stream = await _unitOfWork.EntitlementEditRepository.ExportTemplate(param);
                var fileName = "ImportPhepNam.xlsx";
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
        public async Task<ActionResult> ImportTemplate([FromBody]  EntitlementEditParam param )
        {
            try
            {
                var r = await _unitOfWork.EntitlementEditRepository.ImportTemplate(param);
                if (r.memoryStream != null)
                {
                    var fileName = "ImportPhepNamError.xlsx";
                    return new FileStreamResult(r.memoryStream, "application/octet-stream") { FileDownloadName = fileName };
                }
                return ImportResult(r);
            }
            catch (Exception ex)
            {
                return ResponseResult(ex.Message);
            }
        }
    }
}
