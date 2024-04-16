using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.SwPushSubscription;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.SwPushSubscription
{
    [ApiExplorerSettings(GroupName = "196-SYSTEM-SW_PUSH_SUBSCRIPTION")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class SwPushSubscriptionController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly ISwPushSubscriptionRepository _swPushSubscriptionRepository;
        private readonly AppSettings _appSettings;

        public SwPushSubscriptionController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _swPushSubscriptionRepository = new SwPushSubscriptionRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> FindSubscription(CheckSubscriptionRequest request)
        {
            var response = await _swPushSubscriptionRepository.FindSubscription(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _swPushSubscriptionRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _swPushSubscriptionRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _swPushSubscriptionRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<SwPushSubscriptionDTO> request)
        {
            var response = await _swPushSubscriptionRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _swPushSubscriptionRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _swPushSubscriptionRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SwPushSubscriptionDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _swPushSubscriptionRepository.Create(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRange(List<SwPushSubscriptionDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _swPushSubscriptionRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SwPushSubscriptionDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _swPushSubscriptionRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<SwPushSubscriptionDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _swPushSubscriptionRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SwPushSubscriptionDTO model)
        {
            if (model.Id != null)
            {
                var response = await _swPushSubscriptionRepository.Delete(_uow, (long)model.Id);
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
            var response = await _swPushSubscriptionRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _swPushSubscriptionRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

    }
}

