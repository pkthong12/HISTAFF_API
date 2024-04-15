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

namespace API.All.HRM.Profile.ProfileAPI.Portal.PortalApproveLeave
{
    [ApiExplorerSettings(GroupName = "001-PORTAL-PORTAL_APPROVE_LEAVE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class PortalApproveLeaveController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IPortalApproveLeaveRepository _PortalApproveLeaveRepository;
        private readonly AppSettings _appSettings;

        public PortalApproveLeaveController(
            FullDbContext dbContext,
            IOptions<AppSettings> options,
            IHubContext<SignalHub> hubContext)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _PortalApproveLeaveRepository = new PortalApproveLeaveRepository(dbContext, _uow, hubContext);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _PortalApproveLeaveRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetById(PortalApproveLeaveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalApproveLeaveRepository.GetById(sid, model);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetByIdVer2(PortalApproveLeaveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalApproveLeaveRepository.GetByIdVer2(sid, model);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetPortalApproveById(long id)
        {
            var response = await _PortalApproveLeaveRepository.GetPortalApproveById(id);
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<PortalApproveLeaveDTO> request)
        {
            var response = await _PortalApproveLeaveRepository.SinglePhaseQueryList(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PortalApproveLeaveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalApproveLeaveRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(PortalApproveLeaveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalApproveLeaveRepository.Approve(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveHistory(PortalDateTimeSearch model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalApproveLeaveRepository.ApproveHistory(sid, model.DateStart, model.DateEnd);
            return Ok(response);
        }

    }
}

