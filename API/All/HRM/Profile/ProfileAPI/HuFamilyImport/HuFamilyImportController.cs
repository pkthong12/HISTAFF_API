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

namespace API.All.HRM.Profile.ProfileAPI.HuFamilyImport
{
    [ApiExplorerSettings(GroupName = "056-PROFILE-HU_FAMILY_IMPORT")]
    [Route("api/[controller]/[action]")]
    [HiStaffAuthorize]
    [ApiController]
    public class HuFamilyImportController : ControllerBase
    {
        private GenericUnitOfWork _uow;
        private HuFamilyImport _HuFamilyImport;
        private AppSettings _appSettings;

        public HuFamilyImportController(
            IOptions<AppSettings> options,
            FullDbContext fullDbContext
            )
        {
            _uow = new(fullDbContext);
            _HuFamilyImport = new HuFamilyImport(fullDbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuFamilyImportDTO> request)
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

                var response = await _HuFamilyImport.SinglePhaseQueryList(request);

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
            var response = await _HuFamilyImport.Save(request);
            return Ok(response);
        }
    }
}
