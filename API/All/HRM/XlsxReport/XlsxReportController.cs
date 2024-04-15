using API.All.DbContexts;
using API.All.HRM.XlsxReport;
using API.All.SYSTEM.CoreAPI.Report;
using API.Controllers.SysFunction;
using API.Main;
using CORE.GenericUOW;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.EmployeeReport
{
    [ApiExplorerSettings(GroupName = "039-REPORT-HU_EMPLOYEE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class XlsxReportController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IXlsxReportRepository _XlsxReportRepository;
        private readonly AppSettings _appSettings;
        private readonly IHostEnvironment _env;

        public XlsxReportController(
            FullDbContext dbContext,
            IOptions<AppSettings> options,
            IHostEnvironment env,
            IXlsxReportRepository xlsxReportRepository
            )
        {
            _uow = new GenericUnitOfWork(dbContext);

            _XlsxReportRepository = xlsxReportRepository;
            _appSettings = options.Value;
            _env = env;
        }

        [HttpPost]
        public async Task<FileStreamResult> GetReport(ReportDTO request)
        {

            var sid = Request.Sid(_appSettings) ?? throw new Exception("Unauthorized");
            var username = Request.Typ(_appSettings) ?? throw new Exception("Unauthorized");

            string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.ExcelReports);
            var response = await _XlsxReportRepository.GetReport(request, location, sid, username);
            var contentType = "application/octet-stream";
            var fileName = request.ErCode;

            return File(response, contentType, fileName);

        }

        [HttpGet]
        public async Task<IActionResult> GetListReport()
        {
            var response = await _XlsxReportRepository.GetListReport();
            return Ok(response);
        }
    }
}
