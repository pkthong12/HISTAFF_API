using API.All.DbContexts;
using API.Main;
using API.Socket;
using CORE.DTO;
using CORE.Extension;
using CORE.GenericUOW;
using CORE.SignalHub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using PayrollDAL.ViewModels;

namespace API.All.SYSTEM.CoreAPI.Xlsx
{
    [ApiController]
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "999-SYSTEM-XLSX")]
    [Route("api/[controller]/[action]")]
    public class XlsxController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly FullDbContext _dbContext;
        private readonly GenericUnitOfWork _uow;
        private readonly AppSettings _appSettings;
        private readonly IXlsxRepository _xlsxRepository;

        public XlsxController(IWebHostEnvironment env, FullDbContext dbContext, IOptions<AppSettings> options, IXlsxRepository xlsxRepository)
        {
            _env = env;
            _dbContext = dbContext;
            _uow = new(dbContext);
            _appSettings = options.Value;
            _xlsxRepository = xlsxRepository;
        }

        [HttpPost]
        public async Task<FileStreamResult> ExportCorePageListGridToExcel(ExportCorePageListGridToExcelDTO request)
        {
            var sid = Request.Sid(_appSettings);
            string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.ExcelTemplates);
            var username = Request.Typ(_appSettings) ?? throw new Exception("Unauthorized");
            var response = await _xlsxRepository.ExportCorePageListGridToExcel(request, location, sid, username);

            var contentType = "application/octet-stream";
            var fileName = "CorePageList.xlsx";

            return File(response, contentType, fileName);
        }

        [HttpPost]
        public async Task<FileStreamResult>DownloadTemplate(DownloadTemplateDTO request)
        {
            var sid = Request.Sid(_appSettings);
            string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.ExcelTemplates);
            var response = await _xlsxRepository.DownloadTemplate(request, location, sid);

            var contentType = "application/octet-stream";
            var fileName = request.ExCode;

            return File(response, contentType, fileName);
        }

        [HttpPost]
        public async Task<FileStreamResult> GenerateTemplate(ExObject request)
        {
            var sid = Request.Sid(_appSettings) ?? throw new Exception("Unauthorized");
            var username = Request.Typ(_appSettings) ?? throw new Exception("Unauthorized");

            string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.ExcelTemplates);
            var response = await _xlsxRepository.GenerateTemplate(request, location, sid, username);

            var contentType = "application/octet-stream";
            var fileName = request.ExCode;
            return File(response, contentType, fileName);
        }

        [HttpPost]
        public async Task<FileStreamResult> ImportXlsxToDb(ImportXlsxToDbDTO request)
        {
            var sid = Request.Sid(_appSettings) ?? throw new Exception("Unauthorized");
            var username = Request.Typ(_appSettings) ?? throw new Exception("Unauthorized");

            string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.ExcelTemplates);
            var response = await _xlsxRepository.ImportXlsxToDb(request, location, sid, username);

            var contentType = "application/octet-stream";
            var fileName = $"{request.FileName.Split(".xlsx")[0]}_processed.xlsx";
            return File(response, contentType, fileName);

        }

        [HttpPost]
        public async Task<IActionResult> ImportKpiEmployee(List<KpiEmployeeImport> request)
        {
            // Triển khai sau
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

    }
}
