using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.All.HRM.Profile.ProfileAPI.HuAllowanceEmpImport
{
    [ApiExplorerSettings(GroupName = "073-PROFILE-HU_ALLOWANCE_EMP_IMPORT")]
    [Route("api/[controller]/[action]")]
    [HiStaffAuthorize]
    [ApiController]
    public class HuAllowanceEmpImportController : ControllerBase
    {
        private GenericUnitOfWork _uow;
        private HuAllowanceEmpImportRepository _huAllowanceEmpImportRepository;
        private AppSettings _appSettings;

        public HuAllowanceEmpImportController(
            IOptions<AppSettings> options,
            FullDbContext fullDbContext
            )
        {
            _uow = new(fullDbContext);
            _huAllowanceEmpImportRepository = new HuAllowanceEmpImportRepository(fullDbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuAllowanceEmpImportDTO> request)
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

                var response = await _huAllowanceEmpImportRepository.SinglePhaseQueryList(request);

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
            var response = await _huAllowanceEmpImportRepository.Save(request);
            return Ok(response);
        }
    }
}
