using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Authorization.SysFunction;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.SysFunction
{
    [ApiExplorerSettings(GroupName = "183-SYSTEM-SYS_FUNCTION")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class SysFunctionController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly ISysFunctionRepository _SysFunctionRepository;
        private readonly AppSettings _appSettings;

        public SysFunctionController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _SysFunctionRepository = new SysFunctionRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _SysFunctionRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _SysFunctionRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _SysFunctionRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<SysFunctionDTO> request)
        {
            var response = await _SysFunctionRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> FunctionPermissionList(GenericQueryListDTO<SysFunctionDTO> request)
        {
            var response = await _SysFunctionRepository.FunctionPermissionList(request);
            if (response.ErrorType == EnumErrorType.NONE)
            {
                return Ok(new FormatedResponse() { InnerBody = response });
            } else
            {
                return Ok(new FormatedResponse() { 
                    ErrorType = response.ErrorType,
                    MessageCode = response.MessageCode ?? "",
                    StatusCode = EnumStatusCode.StatusCode400
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _SysFunctionRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _SysFunctionRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SysFunctionDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysFunctionRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<SysFunctionDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysFunctionRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SysFunctionDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysFunctionRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<SysFunctionDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysFunctionRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SysFunctionDTO model)
        {
            if (model.Id != null)
            {
                var response = await _SysFunctionRepository.Delete(_uow, (long)model.Id);
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
            var response = await _SysFunctionRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _SysFunctionRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFunctionThenUpdateFunctionIdForMenu(CreateUpdateFunctionThenAsignFunctionIdToMenuDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysFunctionRepository.CreateFunctionThenUpdateFunctionIdForMenu(request, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFunctionThenUpdateFunctionIdForMenu(CreateUpdateFunctionThenAsignFunctionIdToMenuDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysFunctionRepository.UpdateFunctionThenUpdateFunctionIdForMenu(request, sid);
            return Ok(response);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ReadAllWithAllActions()
        {
            var response = await _SysFunctionRepository.ReadAllWithAllActions();
            return Ok(response);
        }

    }
}

