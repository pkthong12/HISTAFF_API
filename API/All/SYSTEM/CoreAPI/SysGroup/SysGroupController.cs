using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.SysGroup
{
    [ApiExplorerSettings(GroupName = "196-SYSTEM-SYS_GROUP")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class SysGroupController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly ISysGroupRepository _SysGroupRepository;
        private readonly AppSettings _appSettings;

        public SysGroupController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _SysGroupRepository = new SysGroupRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _SysGroupRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _SysGroupRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _SysGroupRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<SysGroupDTO> request)
        {
            var response = await _SysGroupRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _SysGroupRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _SysGroupRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SysGroupDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysGroupRepository.Create(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Clone(SysGroupDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysGroupRepository.Clone(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRange(List<SysGroupDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysGroupRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SysGroupDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysGroupRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<SysGroupDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _SysGroupRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SysGroupDTO model)
        {
            if (model.Id != null)
            {
                var response = await _SysGroupRepository.Delete(_uow, (long)model.Id);
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
            var response = await _SysGroupRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _SysGroupRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> QueryGroupOrgPermissionList(long objectId)
        {
            var response = await _SysGroupRepository.QueryOrgPermissionList(objectId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> QueryFunctionActionPermissionList(long objectId)
        {
            var response = await _SysGroupRepository.QueryFunctionActionPermissionList(objectId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAllFunctionActionPermissionByGroupId(SysGroupDTO model)
        {
            var response = await _SysGroupRepository.DeleteAllFunctionActionPermissionByGroupId((long)model.GroupId!);
            return Ok(response);
        }

    }
}

