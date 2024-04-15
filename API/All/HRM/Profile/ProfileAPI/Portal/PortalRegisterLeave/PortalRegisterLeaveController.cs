using API.All.DbContexts;
using API.All.Services;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.Services.File;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.PortalRegisterLeave
{
    [ApiExplorerSettings(GroupName = "033-PORTAL-AT_REGISTER_LEAVE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class PortalRegisterLeaveController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly PortalRegisterLeaveRepository _portalRegisterLeaveRepository;
        private readonly AppSettings _appSettings;

        public PortalRegisterLeaveController(
            FullDbContext dbContext,
            IOptions<AppSettings> options,
            IWebHostEnvironment env,
            IFileService fileService,
            IEmailService emailService)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _portalRegisterLeaveRepository = new(dbContext, _uow, env, options, fileService, emailService);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> WillLeaveInNextSevenDay()
        {
            var sid = Request.Sid(_appSettings);
            var resposne = await _portalRegisterLeaveRepository.WillLeaveInNextSevenDay(sid);
            return Ok(resposne);
        }

    }
}

