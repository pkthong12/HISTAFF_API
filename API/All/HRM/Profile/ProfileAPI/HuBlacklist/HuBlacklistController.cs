using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System.Linq.Dynamic.Core;

namespace API.All.HRM.Profile.ProfileAPI.HuBlacklist
{
    [ApiExplorerSettings(GroupName = "567-PROFILE-HU_BLACKLIST")]

    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]

    public class HuBlacklistController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly HuBlacklistRepository _HuBlacklistRepository;
        private readonly AppSettings _appSettings;

        public HuBlacklistController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuBlacklistRepository = new HuBlacklistRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _HuBlacklistRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuBlacklistRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuBlacklistRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuBlacklistDTO> request)
        {
            try
            {
                var response = await _HuBlacklistRepository.SinglePhaseQueryList(request);

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
            var response = await _HuBlacklistRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuBlacklistRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HuBlacklistDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();

            var response = await _HuBlacklistRepository.Create(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRange(List<HuBlacklistDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuBlacklistRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuBlacklistDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();

            var response = await _HuBlacklistRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<HuBlacklistDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuBlacklistRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuBlacklistDTO model)
        {
            if (model.Id != null)
            {
                var response = await _HuBlacklistRepository.Delete(_uow, (long)model.Id);
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
            var response = await _HuBlacklistRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _HuBlacklistRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }
    }
}