using API.All.DbContexts;
using API.Controllers.HuConcurrently;
using API.Controllers.HuContract;
using API.Controllers.HuOrganization;
using API.Controllers.SysUser;
using Common.Middleware;
using CORE.CustomAttributes;
using CORE.GenericUOW;
using CORE.Services.File;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ProfileDAL.Repositories;

namespace API.All.Services
{
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "998-SYSTEM-HANGFIRE")]
    public class HangfireController : ControllerBase
    {
        private IEmailService _emailService;
        private IWorkingRepository _workingRepository;
        private IBackgroundService _backgroundService;
        private ISchedureService _schedureService;
        private IBackgroundJobClient _backgroundJobClient;
        private IRecurringJobManager _recurringJobManager;
        private HuConcurrentlyRepository _huConcurrentlyRepository;
        private TerminateRepository _terminateRepository;
        private HuOrganizationRepository _huOrganizationRepository;
        private ContractRepository _contractRepository;
        //private IHuOrganizationRepository _huOrganizationRepository;

        public HangfireController(FullDbContext fullDbContext, ProfileDbContext profileDbContext, IEmailService emailService, IWorkingRepository workingRepository, ISchedureService schedureService, IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager, IBackgroundService backgroundService,
            IWebHostEnvironment env,
            IOptions<AppSettings> options,
            IFileService fileService)
        {
            var uow = new GenericUnitOfWork(fullDbContext);
            _emailService = emailService;
            _workingRepository = workingRepository;
            _schedureService = schedureService;
            _backgroundService = backgroundService;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
            _huConcurrentlyRepository = new HuConcurrentlyRepository(fullDbContext, uow);
            _terminateRepository = new TerminateRepository(profileDbContext);
            _huOrganizationRepository = new HuOrganizationRepository(fullDbContext, env, options, fileService, emailService);
            _contractRepository = new ContractRepository(profileDbContext);
            //_huOrganizationRepository = huOrganizationRepository;
        }

        [HttpGet]
        public ActionResult FireAndForgetJob()
        {
            _backgroundJobClient.Enqueue(() => _emailService.SendEmail("Fire-and-Forget SendTesting Email"));
            return Ok();
        }

        [HttpGet]
        public ActionResult DelayedJob()
        {
            _backgroundJobClient.Schedule(() => _emailService.SendEmail("Delayed SendTesting Email"), TimeSpan.FromSeconds(30));
            return Ok();
        }

        [HttpGet]
        public ActionResult ReccuringJob()
        {
            _recurringJobManager.AddOrUpdate("5AM_JOB", () => _emailService.SendEmail("Recurring SendTesting Email At 05:00AM every morning"), "0 22 * * *");
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult InsertArisingJob()
        {
            _recurringJobManager.AddOrUpdate("INSERT_INS_ARISING_5AM", () => _backgroundService.InsertArising(), "0 22/1 * * *");
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ApproveWorkingJob()
        {
            _recurringJobManager.AddOrUpdate("APPROVE_WORKING_3AM", () => _backgroundService.ApproveWorking(), "00 20 * * *");
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ScanApproveWorkingsJob()
        {
            _recurringJobManager.AddOrUpdate("Điều chuyển vị trí nhân viên", () => _workingRepository.ScanApproveWorkings(), "0 22 * * *");
            return Ok();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ScanApproveTerminatesJob()
        {
            //_backgroundJobClient.Enqueue(() =>  _terminateRepository.ScanApproveTerminate());
            _recurringJobManager.AddOrUpdate("Nhân viên nghỉ việc", () => _terminateRepository.ScanApproveTerminate(), "0 22 * * *");
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ApproveTerminateJob()
        {
            _recurringJobManager.AddOrUpdate("APPROVE_TERMINATE_3AM", () => _backgroundService.ApproveTerminate(), "00 20 * * *");
            return Ok();
        }

        [HttpGet]
        public ActionResult ContinuationJob()
        {
            var jobId = _backgroundJobClient.Schedule(() => _emailService.SendEmail("Continuation SendTesing Email 1"), TimeSpan.FromSeconds(45));
            _backgroundJobClient.ContinueJobWith(jobId, () => Console.WriteLine("Continuation SendTesing Email 2 - Email Reminder"));
            return Ok();
        }

        [HttpGet]
        public IActionResult AddTestingRecordAt5AmEveryMorning()
        {
            _recurringJobManager.AddOrUpdate("AutoRecord5AM", () => _schedureService.AddTestingRecordAt5AmEveryMorning(), "0 22 * * *");
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult DatLichNhiemVu5Am()
        {
            _recurringJobManager.AddOrUpdate("QUET_HO_SO_5AM", () => _schedureService.TestNhanh(), "0 22 * * *");
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult DeveloperRooting()
        {
            _recurringJobManager.AddOrUpdate("DeveloperRooting", () => _schedureService.DeveloperRooting(), Cron.Hourly);
            return Ok();
        }

        [HttpGet]
        public IActionResult TranferPosition()
        {
            _recurringJobManager.AddOrUpdate("Update concurrent position in HuPosition", () => _huConcurrentlyRepository.TranferPosition(), "0 22 * * *");
            return Ok();
        }

        [HttpGet]
        public IActionResult ChangePositionPoliticalByDate()
        {
            _recurringJobManager.AddOrUpdate("Update position political in HuEmployeeCv", () => _huConcurrentlyRepository.ChangePositionPoliticalByDate(), "0 22 * * *");
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ScanDissolveOrgJob()
        {
            //_backgroundJobClient.Enqueue(() => _huOrganizationRepository.ScanDissolveOrg());
            _recurringJobManager.AddOrUpdate("Phòng ban giải thể", () => _huOrganizationRepository.ScanDissolveOrg(), "0 22 * * *");
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ChangeIsActivePosition()
        {
            _backgroundJobClient.Enqueue(() => _schedureService.ChangeIsActivePosition());
            //_recurringJobManager.AddOrUpdate("ChangeIsActivePosition", () => _schedureService.ChangeIsActivePosition(), "0 22 * * *");
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ScanUpdateStatusEmpDetail()
        {
            //_backgroundJobClient.Enqueue(() => _contractRepository.ScanUpdateStatusEmpDetail());
            _recurringJobManager.AddOrUpdate("Update status employee detail.", () => _contractRepository.ScanUpdateStatusEmpDetail(), "0 22 * * *");
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult UpdateStatusEmpDetail()
        {
            //_backgroundJobClient.Enqueue(() => _backgroundService.UpdateStatusEmpDetail());
            _recurringJobManager.AddOrUpdate("UPDATE_STATUS_EMP_DETAIL", () => _backgroundService.UpdateStatusEmpDetail(), "00 20 * * *");
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult TheSystemChecksTheTerminate()
        {
            _recurringJobManager.AddOrUpdate("KIEM_TRA_NGHI_VIEC", () => _schedureService.TheSystemChecksTheTerminate(), "0 22 * * *");
            return Ok();
        }
    }
}