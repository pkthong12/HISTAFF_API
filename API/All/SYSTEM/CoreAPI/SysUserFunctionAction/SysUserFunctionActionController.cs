using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.SysUserFunctionAction;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.SysUserFunctionAction
{
    [ApiExplorerSettings(GroupName = "216-SYSTEM-SYS_USER_FUNCTION_ACTION")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class SysUserFunctionActionController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly ISysUserFunctionActionRepository _SysUserFunctionActionRepository;
        private readonly AppSettings _appSettings;

        public SysUserFunctionActionController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _SysUserFunctionActionRepository = new SysUserFunctionActionRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _SysUserFunctionActionRepository.ReadAll();
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _SysUserFunctionActionRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _SysUserFunctionActionRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<SysUserFunctionActionDTO> request)
        {
            var response = await _SysUserFunctionActionRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _SysUserFunctionActionRepository.GetById(id);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _SysUserFunctionActionRepository.GetById(id);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SysUserFunctionActionDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysUserFunctionActionRepository.Create(_uow, model, sid);
            return Ok(new FormatedResponse() { InnerBody = response });
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<SysUserFunctionActionDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysUserFunctionActionRepository.CreateRange(_uow, models, sid);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Update(SysUserFunctionActionDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysUserFunctionActionRepository.Update(_uow, model, sid);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<SysUserFunctionActionDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysUserFunctionActionRepository.UpdateRange(_uow, models, sid);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SysUserFunctionActionDTO model)
        {
            if (model.Id != null)
            {
                var response = await _SysUserFunctionActionRepository.Delete(_uow, (long)model.Id);
                return Ok(new FormatedResponse() { InnerBody = response });
            }
            else
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            var response = await _SysUserFunctionActionRepository.DeleteIds(_uow, model.Ids);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _SysUserFunctionActionRepository.DeleteIds(_uow, model.Ids);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserFunctionActionPermissionRange(UpdateUserFunctionActionPermissionRangeDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysUserFunctionActionRepository.UpdateUserFunctionActionPermissionRange(request, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteByUserId(SysUserFunctionActionDTO dto)
        {
            var response = await _SysUserFunctionActionRepository.DeleteByUserId(dto.UserId!);
            return Ok(response);
        }

    }
}

