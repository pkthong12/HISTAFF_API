using API.Entities;
using CORE.DTO;
using CORE.GenericUOW;
using Microsoft.AspNetCore.Mvc;
using API.All.SYSTEM.CoreDAL.System.Language.Models;
using CORE.Enum;
using CORE.StaticConstant;
using API.Main;
using Microsoft.Extensions.Options;
using API.DTO;
using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.SysConfigurationCommon;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;

namespace API.All.SYSTEM.CoreAPI.SysConfigurationCommon
{
    [ApiExplorerSettings(GroupName = "003-SYSTEM-SYS_CONFIGURATION_COMMON")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]

    public class SysConfigurationCommonController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly SysConfigurationCommonRepository _SysConfigurationCommonRepository;
        private readonly AppSettings _appSettings;

        public SysConfigurationCommonController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _SysConfigurationCommonRepository = new SysConfigurationCommonRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _SysConfigurationCommonRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _SysConfigurationCommonRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _SysConfigurationCommonRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<SysConfigurationCommonDTO> request)
        {
            try
            {
                var response = await _SysConfigurationCommonRepository.SinglePhaseQueryList(request);

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
            var response = await _SysConfigurationCommonRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _SysConfigurationCommonRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SysConfigurationCommonDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();


            var response = await _SysConfigurationCommonRepository.Create(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRange(List<SysConfigurationCommonDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysConfigurationCommonRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SysConfigurationCommonDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();


            var response = await _SysConfigurationCommonRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<SysConfigurationCommonDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysConfigurationCommonRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SysConfigurationCommonDTO model)
        {
            if (model.Id != null)
            {
                var response = await _SysConfigurationCommonRepository.Delete(_uow, (long)model.Id);
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
            var response = await _SysConfigurationCommonRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _SysConfigurationCommonRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }
    }
}