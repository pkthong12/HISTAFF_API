using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using API.Main;

namespace API.All.HRM.Profile.ProfileAPI.InsInformationImport
{
    [ApiExplorerSettings(GroupName = "055-PROFILE-INS_INFORMATION_IMPORT")]
    [Route("api/[controller]/[action]")]
    [HiStaffAuthorize]
    [ApiController]
    public class InsInformationImportController : ControllerBase
    {
        private GenericUnitOfWork _uow;
        private InsInformationImport _InsInformationImport;
        private AppSettings _appSettings;

        public InsInformationImportController(
            IOptions<AppSettings> options,
            FullDbContext fullDbContext
            )
        {
            _uow = new(fullDbContext);
            _InsInformationImport = new InsInformationImport(fullDbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<InsInformationImportDTO> request)
        {
            try
            {

                if (request.Filter?.XlsxSession == null)
                {
                    return Ok(new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.XLSX_IMPORT_NO_SESSION_PROVIDED,
                        StatusCode = EnumStatusCode.StatusCode400
                    });
                }

                var response = await _InsInformationImport.SinglePhaseQueryList(request);

                if (response.ErrorType != EnumErrorType.NONE)
                {
                    // sửa thông báo
                    if (response.MessageCode == "Nullable object must have a value." && response.MessageCode != null)
                    {
                        response.MessageCode = CommonMessageCode.NULLABLE_OBJECT_MUST_HAVE_A_VALUE;
                    }


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

        [HttpPost]
        public async Task<IActionResult> Save(ImportQueryListBaseDTO request)
        {
            var response = await _InsInformationImport.Save(request);
            return Ok(response);
        }
    }
}
