using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.HuComClassification
{
    [ApiExplorerSettings(GroupName = "093-PROFILE-HU_COM_CLASSIFICATION")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuComClassificationController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IHuComClassificationRepository _HuComClassificationRepository;
        private readonly AppSettings _appSettings;

        public HuComClassificationController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuComClassificationRepository = new HuComClassificationRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _HuComClassificationRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuComClassificationRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuComClassificationRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuComClassificationDTO> request)
        {
            var response = await _HuComClassificationRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _HuComClassificationRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuComClassificationRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuComClassificationDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuComClassificationRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<HuComClassificationDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuComClassificationRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuComClassificationDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuComClassificationRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<HuComClassificationDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuComClassificationRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuComClassificationDTO model)
        {
            if (model.Id != null)
            {
                var response = await _HuComClassificationRepository.Delete(_uow, (long)model.Id);
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
            var response = await _HuComClassificationRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _HuComClassificationRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

    }
}

