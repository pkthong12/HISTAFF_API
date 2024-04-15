using API.All.DbContexts;
using API.DTO;
using API.Main;
using API.Socket;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

namespace API.Controllers.HuFamilyEdit
{
    [ApiExplorerSettings(GroupName = "096-PROFILE-HU_FAMILY_EDIT")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class ApproveHuFamilyEditController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IApproveHuFamilyEditRepository _HuFamilyEditRepository;
        private readonly AppSettings _appSettings;

        public ApproveHuFamilyEditController(
            FullDbContext dbContext,
            IOptions<AppSettings> options, IHubContext<SignalHub> hubContext)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuFamilyEditRepository = new ApproveHuFamilyEditRepository(dbContext, _uow, hubContext);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _HuFamilyEditRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuFamilyEditRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuFamilyEditRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuFamilyEditDTO> request)
        {
            var response = await _HuFamilyEditRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _HuFamilyEditRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuFamilyEditRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuFamilyEditDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuFamilyEditRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<HuFamilyEditDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuFamilyEditRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuFamilyEditDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuFamilyEditRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<HuFamilyEditDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuFamilyEditRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuFamilyEditDTO model)
        {
            if (model.Id != null)
            {
                var response = await _HuFamilyEditRepository.Delete(_uow, (long)model.Id);
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
            var response = await _HuFamilyEditRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _HuFamilyEditRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHuFamilyEdit()
        {
            var response = await _HuFamilyEditRepository.GetAllHuFamilyEdit();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveHuFamilyEdit(GenericUnapprovePortalDTO model)
        {
            var response = await _HuFamilyEditRepository.ApproveHuFamilyEdit(model);
            return Ok(response);
        }
    }
}

