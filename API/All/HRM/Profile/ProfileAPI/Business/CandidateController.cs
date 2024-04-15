using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.List
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "002-PROFILE-CANDIDATE")]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class CandidateController : BaseController1
    {
        private IWebHostEnvironment _hostingEnvironment;
        public CandidateController(IProfileUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment) : base(unitOfWork)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll(CandidateScanCVDTO param)
        {
            var r = await _unitOfWork.CandidateRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> Get(long Id)
        {
            var r = await _unitOfWork.CandidateRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CandidateScanCVInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.CandidateRepository.CreateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody] CandidateScanCVInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.CandidateRepository.UpdateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Remove([FromBody] long id)
        {
            var r = await _unitOfWork.CandidateRepository.RemoveAsync(id);
            return ResponseResult(r);
        }

        [HttpPost]
        public async Task<ActionResult> ReadFileCV([FromForm] CandidateScanCVImportDTO param)
        {
            try
            {
                param.Environment = _hostingEnvironment;
                param.scheme = HttpContext.Request.Scheme;
                param.host = HttpContext.Request.Host.Value;
                var res = await _unitOfWork.CandidateRepository.ReadFileCVAsync(param);
                return ResponseResult(res);

            }
            catch (Exception ex)
            {
                return ResponseResult(ex.Message);
            }

        }



    }
}
