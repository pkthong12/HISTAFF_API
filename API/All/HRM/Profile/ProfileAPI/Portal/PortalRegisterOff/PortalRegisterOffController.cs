using API.All.DbContexts;
using API.DTO;
using API.DTO.PortalDTO;
using API.Main;
using API.Socket;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

namespace API.All.HRM.Profile.ProfileAPI.Portal.PortalRegisterOff
{
    [ApiExplorerSettings(GroupName = "001-PORTAL-PORTAL_REGISTER_OFF")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class PortalRegisterOffController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IPortalRegisterOffRepository _PortalRegisterOffRepository;
        private readonly AppSettings _appSettings;

        public PortalRegisterOffController(
            FullDbContext dbContext,
            IOptions<AppSettings> options, IHubContext<SignalHub> hubContext)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _PortalRegisterOffRepository = new PortalRegisterOffRepository(dbContext, _uow, hubContext);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _PortalRegisterOffRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _PortalRegisterOffRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _PortalRegisterOffRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<PortalRegisterOffDTO> request)
        {
            var response = await _PortalRegisterOffRepository.SinglePhaseQueryList(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _PortalRegisterOffRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _PortalRegisterOffRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PortalRegisterOffDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            if(model.TypeCode == "OFF" || model.TypeCode == "OVERTIME")
            {
                model.TimeStart = new DateTime(model.WorkingDay!.Value.Year, model.WorkingDay!.Value.Month, model.WorkingDay!.Value.Day, model.TimeStart!.Value.Hour, model.TimeStart!.Value.Minute, model.TimeStart!.Value.Second);
                model.TimeEnd = new DateTime(model.WorkingDay!.Value.Year, model.WorkingDay!.Value.Month, model.WorkingDay!.Value.Day, model.TimeEnd!.Value.Hour, model.TimeEnd!.Value.Minute, model.TimeEnd!.Value.Second);
            }
            var response = await _PortalRegisterOffRepository.Create(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterOff(DynamicDTO model)
        {
            // check ngày bắt đầu với ngày kết thúc
            if (DateTime.Parse(model["dateStart"].ToString()) > DateTime.Parse(model["dateEnd"].ToString()))
            {
                return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.START_MUST_LESS_THAN_END, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalRegisterOffRepository.RegisterOff(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRange(List<PortalRegisterOffDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalRegisterOffRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PortalRegisterOffDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalRegisterOffRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<PortalRegisterOffDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalRegisterOffRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PortalRegisterOffDTO model)
        {
            if (model.Id != null)
            {
                var response = await _PortalRegisterOffRepository.Delete(_uow, (long)model.Id);
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
            var response = await _PortalRegisterOffRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _PortalRegisterOffRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalOtMonth()
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalRegisterOffRepository.GetTotalOtMonth(sid);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetLeaveDay()
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalRegisterOffRepository.GetLeaveDay(sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterHistory(PortalDateTimeSearch model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalRegisterOffRepository.RegisterHistory(sid, model.DateStart, model.DateEnd);
            return Ok(response);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetRegisterHistoryById(long id)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalRegisterOffRepository.GetRegisterHistoryById(id);
            return Ok(response);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetRegisterById(long id)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalRegisterOffRepository.GetRegisterById(id);
            return Ok(response);
        }
    }
}

