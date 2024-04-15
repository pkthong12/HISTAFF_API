using API;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.Services.File;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace CoreAPI.File
{
    [ApiExplorerSettings(GroupName = "999-SYSTEM-FILE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class FileController : ControllerBase
    {

        private readonly IWebHostEnvironment _env;
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;

        public FileController(IWebHostEnvironment environment, IOptions<AppSettings> options, IFileService fileService)
        {
            _env = environment;
            _appSettings = options.Value;
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(UploadRequest request)
        {
            string location = Path.Combine(_env.ContentRootPath, _appSettings.SharedFolders.Images);
            var response = await _fileService.UploadFile(request, location, Request.Sid(_appSettings));
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAttachment(DeleteFileRequest request)
        {
            string targetFolder = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Attachments);
            string serverFilePath = targetFolder + request.FileName;

            var response = await _fileService.DeleteAttachment(serverFilePath);
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> GetImageAsBase64Url(FileUrlDTO request)
        {
            using var httClient = new HttpClient();
            httClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Headers["Authorization"].Single()?.Split(" ")[1]);
            var imageBytes = await httClient.GetByteArrayAsync(request.Url);
            return Ok(new FormatedResponse()
            {
                InnerBody = new FileBase64Response() { Base64 = Convert.ToBase64String(imageBytes) }
            });
        }

        [HttpGet]
        public async Task<IActionResult> LogFileList()
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();

            string targetFolder = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Logs);
            IEnumerable<string> files = await Task.Run(() => Directory.EnumerateFiles(targetFolder));
            List<string> shortPathList = new();
            string pathToAdd = "";

            foreach (string f in files)
            {
                pathToAdd = Path.GetFileName(f);
                shortPathList.Add(pathToAdd);
            }

            return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode200, InnerBody = shortPathList });

        }

        [HttpPost]
        public async Task<FileStreamResult?> FileDownload(DownloadFileRequest request)
        {

            var sid = Request.Sid(_appSettings);
            if (sid == null) throw new UnauthorizedAccessException();

            string? jwtToken = Request.Headers.Authorization.ToString().Split("Bearer ")[1];
            var handler = new JwtSecurityTokenHandler();
            if (jwtToken != null)
            {
                IdentityModelEventSource.ShowPII = true;
                var token = handler.ReadJwtToken(jwtToken);
                var jti = token.Claims.First(claim => claim.Type == "IsAdmin").Value;
                if (jti != "True") throw new UnauthorizedAccessException();
                var staticLocation = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root);
                var fileStream = await _fileService.DownloadFile(staticLocation, request);
                fileStream!.Position = 0;
                var contentType = "application/octet-stream";
                return File(fileStream!, contentType, request.FileName);
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

    }
}
