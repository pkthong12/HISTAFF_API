using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.AtNotification
{
    [ApiExplorerSettings(GroupName = "028-ATTENDANCE-AT_NOTIFICATION")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class AtNotificationController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IAtNotificationRepository _AtNotificationRepository;
        private readonly AppSettings _appSettings;
        private IGenericRepository<AT_NOTIFICATION, AtNotificationDTO> _genericRepository;

        public AtNotificationController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _AtNotificationRepository = new AtNotificationRepository(dbContext, _uow);
            _appSettings = options.Value;
            _genericRepository = _uow.GenericRepository<AT_NOTIFICATION, AtNotificationDTO>();
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _AtNotificationRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _AtNotificationRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _AtNotificationRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetNotify(long employeeId)
        {
            var response = await _AtNotificationRepository.GetNotify(employeeId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<AtNotificationDTO> request)
        {
            var response = await _AtNotificationRepository.SinglePhaseQueryList(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _AtNotificationRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _AtNotificationRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AtNotificationDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtNotificationRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<AtNotificationDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtNotificationRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AtNotificationDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtNotificationRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<AtNotificationDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtNotificationRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AtNotificationDTO model)
        {
            if (model.Id != null)
            {
                var response = await _AtNotificationRepository.Delete(_uow, (long)model.Id);
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
            var response = await _AtNotificationRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _AtNotificationRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
           var sid = Request.Sid(_appSettings);
           if (sid == null) return Unauthorized();
           var response = await _genericRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
           return Ok(new FormatedResponse()
           {
              MessageCode = response.MessageCode,
              InnerBody = response.InnerBody,
              StatusCode = EnumStatusCode.StatusCode200
            });
        }
        
        [HttpGet]
        public async Task<IActionResult> GetCountNotifyUnRead(long employeeId)
        {
            var response = await _AtNotificationRepository.GetCountNotifyUnRead(employeeId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHistoryApprove(long employeeId, long time)
        {
            var response = await _AtNotificationRepository.GetHistoryApprove(employeeId);
            return Ok(response);
        }
    }
}

