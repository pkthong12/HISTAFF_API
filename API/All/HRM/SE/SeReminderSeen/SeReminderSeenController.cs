using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.SeReminderSeen
{
    [ApiExplorerSettings(GroupName = "232-OTHER-SE_REMINDER_SEEN")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class SeReminderSeenController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly ISeReminderSeenRepository _SeReminderSeenRepository;
        private readonly AppSettings _appSettings;
        private IGenericRepository<SE_REMINDER_SEEN, SeReminderSeenDTO> _genericRepository;

        public SeReminderSeenController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _SeReminderSeenRepository = new SeReminderSeenRepository(dbContext, _uow);
            _appSettings = options.Value;
            _genericRepository = _uow.GenericRepository<SE_REMINDER_SEEN, SeReminderSeenDTO>();
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _genericRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _SeReminderSeenRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _SeReminderSeenRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<SeReminderSeenDTO> request)
        {
            var response = await _SeReminderSeenRepository.SinglePhaseQueryList(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _SeReminderSeenRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _SeReminderSeenRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SeReminderSeenDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SeReminderSeenRepository.Create(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> InsertReminderSeen(SeReminderSeenDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SeReminderSeenRepository.InsertReminderSeen(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRange(List<SeReminderSeenDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SeReminderSeenRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SeReminderSeenDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SeReminderSeenRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<SeReminderSeenDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SeReminderSeenRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SeReminderSeenDTO model)
        {
            if (model.Id != null)
            {
                var response = await _SeReminderSeenRepository.Delete(_uow, (long)model.Id);
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
            var response = await _SeReminderSeenRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _SeReminderSeenRepository.DeleteIds(_uow, model.Ids);
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

    }
}

