//using Microsoft.AspNetCore.Mvc;
//using ProfileDAL.Repositories;
//using ProfileDAL.ViewModels;

//namespace ProfileAPI.List
//{
//    [HiStaffAuthorize]
//    [ApiExplorerSettings(GroupName = "002-PROFILE-INS-CHANGE")]
//    [ApiController]
//    [Route("api/[controller]/[action]")]
//    public class InsChangesController : BaseController1
//    {
//        public InsChangesController(IProfileUnitOfWork unitOfWork) : base(unitOfWork)
//        {

//        }
//        [HttpGet]
//        public async Task<ActionResult> GetAll(InsChangeDTO param)
//        {
//            var r = await _unitOfWork.InsChangeRepository.GetAll(param);
//            return Ok(r);
//        }
//        [HttpGet]
//        public async Task<ActionResult> Get(long Id)
//        {
//            var r = await _unitOfWork.InsChangeRepository.GetById(Id);
//            return ResponseResult(r);
//        }
//        [HttpGet]
//        public async Task<ActionResult> GetListType()
//        {
//            var r = await _unitOfWork.InsuranceTypeRepository.GetList();
//            return ResponseResult(r);
//        }
//        [HttpPost]
//        public async Task<ActionResult> Add([FromBody] InsChangeInputDTO param)
//        {
//            if (!ModelState.IsValid)
//            {
//                return ResponseValidation();
//            }
//            var r = await _unitOfWork.InsChangeRepository.CreateAsync(param);
//            return ResponseResult(r);
//        }
//        [HttpPost]
//        public async Task<ActionResult> Update([FromBody] InsChangeInputDTO param)
//        {
//            if (!ModelState.IsValid)
//            {
//                return ResponseValidation();
//            }
//            var r = await _unitOfWork.InsChangeRepository.UpdateAsync(param);
//            return ResponseResult(r);
//        }
//        [HttpPost]
//        public async Task<ActionResult> Remove([FromBody] List<long> id)
//        {
//            var r = await _unitOfWork.InsChangeRepository.RemoveAsync(id);
//            return ResponseResult(r);
//        }
//        [HttpPost]
//        public async Task<ActionResult> TemplateImport([FromBody] int orgId)
//        {
//            try
//            {
//                var stream = await _unitOfWork.InsChangeRepository.TemplateImport(orgId);
//                var fileName = "TempInsurance.xlsx";
//                if (stream.StatusCode == "200")
//                {
//                    return new FileStreamResult(stream.memoryStream, "application/octet-stream") { FileDownloadName = fileName };
//                }
//                return ResponseResult(stream);
//            }
//            catch (Exception ex)
//            {
//                return ResponseResult(ex.Message);
//            }
//        }

//        [HttpPost]
//        public async Task<ActionResult> ImportTemplate([FromBody] ImportInsParam param)
//        {
//            try
//            {
//                var r = await _unitOfWork.InsChangeRepository.ImportTemplate(param);
//                if (r.memoryStream != null)
//                {
//                    var fileName = "TempInsuranceError.xlsx";
//                    return new FileStreamResult(r.memoryStream, "application/octet-stream") { FileDownloadName = fileName };
//                }
//                return ImportResult(r);
//            }
//            catch (Exception ex)
//            {
//                return ResponseResult(ex.Message);
//            }
//        }

//    }
//}
