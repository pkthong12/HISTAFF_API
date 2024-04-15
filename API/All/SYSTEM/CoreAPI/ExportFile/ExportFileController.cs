using API.All.DbContexts;
using API.Controllers.DemoAttachment;
using API.DTO;
using Azure;
using CORE.DTO;
using CORE.GenericUOW;
using CORE.Services.File;
using CoreAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProfileDAL.ViewModels;

namespace API.All.SYSTEM.CoreAPI.ExportFile
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "002-SYSTEM-EXPORT_FILE")]
    [HiStaffAuthorize]

    public class ExportFileController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IWebHostEnvironment _evn;
        private readonly FullDbContext _fullDbContext;
        private readonly IExportFileRepository _IExportFileRepository;

        public ExportFileController(
            FullDbContext dbContext,
            IWebHostEnvironment env,
            IFileService fileService,
            IExportFileRepository IExportFileRepository

            )
        {
            _evn = env;
            _fullDbContext = dbContext;
            _IExportFileRepository = IExportFileRepository;
        }

        [HttpPost]
        public async Task<IActionResult> ExportExel(ExportExelRequest request)
        {
            var response = await _IExportFileRepository.ExportExel(request);
            return File(response.Bytes!, response.ContentType!, response.FileName);
        }
    }
}
