using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.All.HRM.Profile.ProfileAPI.HuEmployeeImport
{
    [ApiExplorerSettings(GroupName = "054-PROFILE-HU_EMPLOYEE_IMPORT")]
    [Route("api/[controller]/[action]")]
    [HiStaffAuthorize]
    [ApiController]
    public class HuEmployeeImportController : ControllerBase
    {
        private GenericUnitOfWork _uow;
        private HuEmployeeImport _huEmployeeImport;
        private AppSettings _appSettings;

        public HuEmployeeImportController(
            IOptions<AppSettings> options,
            FullDbContext fullDbContext
            )
        {
            _uow = new(fullDbContext);
            _huEmployeeImport = new HuEmployeeImport(fullDbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuEmployeeImportDTO> request)
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

                var response = await _huEmployeeImport.SinglePhaseQueryList(request);

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

        [HttpPost]
        public async Task<IActionResult> Save(ImportQueryListBaseDTO request)
        {
            var response = await _huEmployeeImport.Save(request);
            return Ok(response);
        }
    }
}
