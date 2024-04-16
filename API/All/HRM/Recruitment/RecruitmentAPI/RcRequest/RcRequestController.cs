using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.Services.File;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using ProfileDAL.ViewModels;
using System.Linq.Dynamic.Core;

namespace API.All.HRM.Recruitment.RecruitmentAPI.RcRequest
{
    [ApiExplorerSettings(GroupName = "568-RECRUITMENT-RC_REQUEST")]

    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]

    public class RcRequestController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly RcRequestRepository _RcRequestRepository;
        private readonly AppSettings _appSettings;
        private readonly IWebHostEnvironment _env;
        private readonly IFileService _fileService;

        public RcRequestController(
            FullDbContext dbContext,
            IOptions<AppSettings> options,
            IWebHostEnvironment env,
            IFileService fileService
        )
        {
            _uow = new GenericUnitOfWork(dbContext);
            _RcRequestRepository = new RcRequestRepository(dbContext, _uow);
            _appSettings = options.Value;
            _env = env;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _RcRequestRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _RcRequestRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _RcRequestRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<RcRequestDTO> request)
        {
            try
            {
                var response = await _RcRequestRepository.SinglePhaseQueryList(request);

                if (response.ErrorType != EnumErrorType.NONE)
                {
                    return Ok(new FormatedResponse()
                    {
                        ErrorType = response.ErrorType,
                        MessageCode = response.MessageCode ?? CommonMessageCode.NOT_ALL_CASES_CATCHED,
                        StatusCode = EnumStatusCode.StatusCode400,
                    });
                }
                else
                {
                    return Ok(new FormatedResponse()
                    {
                        InnerBody = response
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _RcRequestRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _RcRequestRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RcRequestDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();

            if (model.ListRadio == 1)
            {
                model.IsInBoundary = true;
                model.IsOutBoundary = false;
            }
            else if (model.ListRadio == 2)
            {
                model.IsInBoundary = false;
                model.IsOutBoundary = true;
            }

            if (model.AttachmentBuffer != null)
            {
                List<UploadFileResponse> uploadFiles = new();
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                {
                    ClientFileName = model.AttachmentBuffer.ClientFileName,
                    ClientFileType = model.AttachmentBuffer.ClientFileType,
                    ClientFileData = model.AttachmentBuffer.ClientFileData
                }, location, sid);

                // Assign saved paths
                var property = typeof(RcRequestDTO).GetProperty("NameOfFile");

                if (property != null)
                {
                    property?.SetValue(model, uploadFileResponse.SavedAs);
                    uploadFiles.Add(uploadFileResponse);
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": Attachment" });
                }

            }

            var response = await _RcRequestRepository.Create(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRange(List<RcRequestDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _RcRequestRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RcRequestDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();

            if (model.ListRadio == 1)
            {
                model.IsInBoundary = true;
                model.IsOutBoundary = false;
            }
            else if (model.ListRadio == 2)
            {
                model.IsInBoundary = false;
                model.IsOutBoundary = true;
            }

            if (model.AttachmentBuffer != null)
            {
                List<UploadFileResponse> uploadFiles = new();
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
                var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
                {
                    ClientFileName = model.AttachmentBuffer.ClientFileName,
                    ClientFileType = model.AttachmentBuffer.ClientFileType,
                    ClientFileData = model.AttachmentBuffer.ClientFileData
                }, location, sid);

                // Assign saved paths
                var property = typeof(RcRequestDTO).GetProperty("NameOfFile");

                if (property != null)
                {
                    property?.SetValue(model, uploadFileResponse.SavedAs);
                    uploadFiles.Add(uploadFileResponse);
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": Attachment" });
                }

            }

            var response = await _RcRequestRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<RcRequestDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _RcRequestRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RcRequestDTO model)
        {
            if (model.Id != null)
            {
                var response = await _RcRequestRepository.Delete(_uow, (long)model.Id);
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
            var response = await _RcRequestRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _RcRequestRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetDropDownRecruitmentForm(string code)
        {
            var response = await _RcRequestRepository.GetDropDownRecruitmentForm(code);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkingAddressAccordingToCompany(long orgId)
        {
            var response = await _RcRequestRepository.GetWorkingAddressAccordingToCompany(orgId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetDropDownRecruitmentReason(string code)
        {
            var response = await _RcRequestRepository.GetDropDownRecruitmentReason(code);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> ReadWorkAddress(long id)
        {
            var response = await _RcRequestRepository.ReadWorkAddress(id);
            return Ok(response);
        }
    }
}