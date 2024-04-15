using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Xlsx;
using API.DTO;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.All.HRM.Attendance.AttendanceAPI.List.AtSwipeDataImport
{
    [ApiExplorerSettings(GroupName = "054-ATTENDANCE-AT_SWIPE_DATA_IMPORT")]
    [Route("api/[controller]/[action]")]
    [HiStaffAuthorize]
    [ApiController]
    public class AtSwipeDataImportController : ControllerBase
    {
        private GenericUnitOfWork _uow;
        private AtSwipeDataImport _atSwipeDataImport;
        private AppSettings _appSettings;

        public AtSwipeDataImportController(
            IOptions<AppSettings> options,
            FullDbContext fullDbContext
            )
        {
            _uow = new(fullDbContext);
            _atSwipeDataImport = new AtSwipeDataImport(fullDbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<AtSwipeDataImportDTO> request)
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

                var response = await _atSwipeDataImport.SinglePhaseQueryList(request);

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
            var response = await _atSwipeDataImport.Save(request);
            return Ok(response);
        }
    }
}
