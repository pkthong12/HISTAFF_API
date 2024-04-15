using API.All.DbContexts;
using API.DTO;
using API.DTO.PortalDTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.All.HRM.Profile.ProfileAPI.Portal.PortalApproveHuWorking
{
    [ApiExplorerSettings(GroupName = "093-PROFILE-PORTAL_REQUEST_CHANGE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class PortalApproveWorkingCompanyController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IPortalApproveWorkingCompanyRepository _PortalApproveHuWorkingRepository;
        private readonly AppSettings _appSettings;
        private readonly IWebHostEnvironment _env;

        public PortalApproveWorkingCompanyController(
            FullDbContext dbContext,
            IOptions<AppSettings> options,
            IWebHostEnvironment env)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _PortalApproveHuWorkingRepository = new PortalApproveWorkingCompanyRepository(dbContext, _uow, env, options);
            _appSettings = options.Value;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _PortalApproveHuWorkingRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _PortalApproveHuWorkingRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _PortalApproveHuWorkingRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<PortalRequestChangeDTO> request)
        {
            var response = await _PortalApproveHuWorkingRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse()
            {
                InnerBody = response
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _PortalApproveHuWorkingRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _PortalApproveHuWorkingRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PortalRequestChangeDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalApproveHuWorkingRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<PortalRequestChangeDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalApproveHuWorkingRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PortalRequestChangeDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalApproveHuWorkingRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<PortalRequestChangeDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalApproveHuWorkingRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PortalRequestChangeDTO model)
        {
            if (model.Id != null)
            {
                var response = await _PortalApproveHuWorkingRepository.Delete(_uow, (long)model.Id);
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
            var response = await _PortalApproveHuWorkingRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _PortalApproveHuWorkingRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> GetAllRecord(GenericQueryListDTO<PortalRequestChangeDTO> request)
        {
            var response = await _PortalApproveHuWorkingRepository.GetAllRecord(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }


        // phê duyệt bản ghi
        [HttpPost]
        public async Task<IActionResult> ApproveHuWorking(GenericToggleIsActiveDTO model)
        {
            var response = await _PortalApproveHuWorkingRepository.ApproveHuWorking(model);
            return Ok(response);
        }


        // từ chối phê duyệt bản ghi
        [HttpPost]
        public async Task<IActionResult> UnapproveHuWorking(GenericUnapprovePortalDTO model)
        {
            var response = await _PortalApproveHuWorkingRepository.UnapproveHuWorking(model);
            return Ok(response);
        }
    }
}

