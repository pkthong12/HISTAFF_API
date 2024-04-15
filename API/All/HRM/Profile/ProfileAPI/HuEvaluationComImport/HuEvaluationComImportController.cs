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

namespace API.Controllers.HuEvaluate
{
    [ApiExplorerSettings(GroupName = "072-PROFILE-HU_EVALUATE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuEvaluationComImportController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IHuEvaluationComImportRepository _HuEvaluatinComImportRepository;
        private readonly AppSettings _appSettings;

        public HuEvaluationComImportController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuEvaluatinComImportRepository = new HuEvaluationComImportRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuEvaluationComImportDTO> request)
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

                var response = await _HuEvaluatinComImportRepository.SinglePhaseQueryList(request);

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
            var response = await _HuEvaluatinComImportRepository.Save(request);
            return Ok(response);
        }

    }
}

