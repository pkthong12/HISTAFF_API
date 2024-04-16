using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.RcCandidateCv
{
    [ApiExplorerSettings(GroupName = "218-RECRUITMENT-RC_CANDIDATE_CV")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class RcCandidateCvController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IRcCandidateCvRepository _RcCandidateCvRepository;
        private readonly AppSettings _appSettings;

        public RcCandidateCvController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _RcCandidateCvRepository = new RcCandidateCvRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _RcCandidateCvRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _RcCandidateCvRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _RcCandidateCvRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<RcCandidateCvDTO> request)
        {
            var response = await _RcCandidateCvRepository.SinglePhaseQueryList(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _RcCandidateCvRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _RcCandidateCvRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RcCandidateCvDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _RcCandidateCvRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<RcCandidateCvDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _RcCandidateCvRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RcCandidateCvDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _RcCandidateCvRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<RcCandidateCvDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _RcCandidateCvRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RcCandidateCvDTO model)
        {
            if (model.Id != null)
            {
                var response = await _RcCandidateCvRepository.Delete(_uow, (long)model.Id);
                return Ok(response);
            }
            else
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            var response = await _RcCandidateCvRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _RcCandidateCvRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetListPos()
        {
            var response = await _RcCandidateCvRepository.GetListPos();
            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetCv(long employeeCvId)
        {
            var response = await _RcCandidateCvRepository.GetCv(employeeCvId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCvById(long id)
        {
            var response = await _RcCandidateCvRepository.GetCvById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCv(CandidateEditDTO request)
        {
            var response = await _RcCandidateCvRepository.UpdateCv(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetLevelInfo(long employeeCvId)
        {
            var response = await _RcCandidateCvRepository.GetLevelInfo(employeeCvId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetLevelInfoById(long id)
        {
            var response = await _RcCandidateCvRepository.GetLevelInfoById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLevelInfo(CandidateEditDTO request)
        {
            var response = await _RcCandidateCvRepository.UpdateLevelInfo(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetWish(long employeeCvId)
        {
            var response = await _RcCandidateCvRepository.GetWish(employeeCvId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetWishById(long id)
        {
            var response = await _RcCandidateCvRepository.GetWishById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateWish(CandidateEditDTO request)
        {
            var response = await _RcCandidateCvRepository.UpdateWish(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetInfoOther(long employeeCvId)
        {
            var response = await _RcCandidateCvRepository.GetInfoOther(employeeCvId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetInfoOtherById(long id)
        {
            var response = await _RcCandidateCvRepository.GetInfoOtherById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInfoOther(CandidateEditDTO request)
        {
            var response = await _RcCandidateCvRepository.UpdateInfoOther(request);
            return Ok(response);
        }
    }
}

