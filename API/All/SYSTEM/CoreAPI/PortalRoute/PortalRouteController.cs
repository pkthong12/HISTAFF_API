using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.CustomAttributes;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.PortalRoute
{
    [ApiExplorerSettings(GroupName = "192-SYSTEM-PORTAL_ROUTE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class PortalRouteController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IPortalRouteRepository _PortalRouteRepository;
        private readonly AppSettings _appSettings;

        public PortalRouteController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _PortalRouteRepository = new PortalRouteRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _PortalRouteRepository.ReadAll();
            return Ok(response);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ReadAllMini()
        {
            var response = await _PortalRouteRepository.ReadAllMini();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _PortalRouteRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _PortalRouteRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<PortalRouteDTO> request)
        {
            var response = await _PortalRouteRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _PortalRouteRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _PortalRouteRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PortalRouteDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalRouteRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<PortalRouteDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalRouteRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PortalRouteDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalRouteRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<PortalRouteDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalRouteRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PortalRouteDTO model)
        {
            if (model.Id != null)
            {
                var response = await _PortalRouteRepository.Delete(_uow, (long)model.Id);
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
            var response = await _PortalRouteRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _PortalRouteRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

    }
}

