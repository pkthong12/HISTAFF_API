using API.All.DbContexts;
using API.All.HRM.Attendance.AttendanceAPI.Business.AtTimeTimesheetDaily;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using API.DTO.PortalDTO;

namespace API.Controllers.AtTimeTimesheetDaily
{
    [ApiExplorerSettings(GroupName = "003-PORTAL-PORTAL_AT_TIME_TIMESHEET_DAILY")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class PortalAtTimeTimesheetDailyController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IPortalAtTimeTimesheetDailyRepository _AtTimeTimesheetDailyRepository;
        private readonly AppSettings _appSettings;

        public PortalAtTimeTimesheetDailyController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _AtTimeTimesheetDailyRepository = new PortalAtTimeTimesheetDailyRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _AtTimeTimesheetDailyRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _AtTimeTimesheetDailyRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _AtTimeTimesheetDailyRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<AtTimeTimesheetDailyDTO> request)
        {
            var response = await _AtTimeTimesheetDailyRepository.SinglePhaseQueryList(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _AtTimeTimesheetDailyRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _AtTimeTimesheetDailyRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AtTimeTimesheetDailyDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtTimeTimesheetDailyRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<AtTimeTimesheetDailyDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtTimeTimesheetDailyRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AtTimeTimesheetDailyDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtTimeTimesheetDailyRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<AtTimeTimesheetDailyDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtTimeTimesheetDailyRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AtTimeTimesheetDailyDTO model)
        {
            if (model.Id != null)
            {
                var response = await _AtTimeTimesheetDailyRepository.Delete(_uow, (long)model.Id);
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
            var response = await _AtTimeTimesheetDailyRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _AtTimeTimesheetDailyRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetAttendantNoteByMonth(GetAttendantNoteByMonthDTO request)
        {

            //var D1 = new DateTime(request.Year, request.MonthIndex + 1, 1);
            //var D2 = new DateTime(request.Year, request.MonthIndex + 1, request.LastDay);
            if(request.EmployeeId == null)
            {
                return Ok(new FormatedResponse()
                {
                    StatusCode = EnumStatusCode.StatusCode200,
                    InnerBody=  new List<object>(),
                });
            }
            var response = await _AtTimeTimesheetDailyRepository.GetAttendantNoteByMonth(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetAttendatByDay(GetAttendantNoteByMonthDTO request)
        {
            var response = await _AtTimeTimesheetDailyRepository.GetAttendatByDay(request);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetListSymbolType()
        {

            var response = await _AtTimeTimesheetDailyRepository.GetListSymbolType();
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> InsertExplainTime(PortalExplainWorkDTO param)
        {
            var response = await _AtTimeTimesheetDailyRepository.InsertExplainTime(param, _appSettings);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetInfoByMonth(GetAttendantNoteByMonthDTO request)
        {
            var response = await _AtTimeTimesheetDailyRepository.GetInfoByMonth(request);
            return Ok(response);
        }
    }
}

