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

namespace API.Controllers.HuEmployeeCvEdit
{
    [ApiExplorerSettings(GroupName = "093-PROFILE-HU_EMPLOYEE_CV_EDIT")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class ApproveHuEmployeeCvEditController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IApproveHuEmployeeCvEditRepository _HuEmployeeCvEditRepository;
        private readonly AppSettings _appSettings;
        private IGenericRepository<HU_EMPLOYEE_CV_EDIT, HuEmployeeCvEditDTO> _genericRepository;

        public ApproveHuEmployeeCvEditController(
            FullDbContext dbContext,
            IOptions<AppSettings> options, IHubContext<SignalHub> _hubContext
            )
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuEmployeeCvEditRepository = new ApproveHuEmployeeCvEditRepository(dbContext, _uow, _hubContext);
            _appSettings = options.Value;
            _genericRepository = _uow.GenericRepository<HU_EMPLOYEE_CV_EDIT, HuEmployeeCvEditDTO>();
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _HuEmployeeCvEditRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuEmployeeCvEditRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuEmployeeCvEditRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuEmployeeCvEditDTO> request)
        {
            var response = await _HuEmployeeCvEditRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response});
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _HuEmployeeCvEditRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuEmployeeCvEditRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuEmployeeCvEditDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvEditRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<HuEmployeeCvEditDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvEditRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuEmployeeCvEditDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvEditRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<HuEmployeeCvEditDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuEmployeeCvEditRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuEmployeeCvEditDTO model)
        {
            if (model.Id != null)
            {
                var response = await _HuEmployeeCvEditRepository.Delete(_uow, (long)model.Id);
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
            var response = await _HuEmployeeCvEditRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _HuEmployeeCvEditRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
           var sid = Request.Sid(_appSettings);
           if (sid == null) return Unauthorized();
           var response = await _genericRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
           return Ok(new FormatedResponse()
           {
              MessageCode = response.MessageCode,
              InnerBody = response.InnerBody,
              StatusCode = EnumStatusCode.StatusCode200
            });
        }

        [HttpPost]
        public async Task<IActionResult> QueryListCvEdit(GenericQueryListDTO<HuEmployeeCvEditDTO> request)
        {
            var response = await _HuEmployeeCvEditRepository.QueryListCvEdit(request);
            return Ok(new FormatedResponse() { InnerBody = response});
        }

        [HttpPost]
        public async Task<IActionResult> QueryListContactEdit(GenericQueryListDTO<HuEmployeeCvEditDTO> request)
        {
            var response = await _HuEmployeeCvEditRepository.QueryListContactEdit(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> QueryListAdditionalInfoEdit(GenericQueryListDTO<HuEmployeeCvEditDTO> request)
        {
            var response = await _HuEmployeeCvEditRepository.QueryListAdditionalInfoEdit(request);
            return Ok(new FormatedResponse() { InnerBody = response});
        }

        [HttpPost]
        public async Task<IActionResult> QueryListBankInfoEdit(GenericQueryListDTO<HuEmployeeCvEditDTO> request)
        {
            var response = await _HuEmployeeCvEditRepository.QueryListBankInfoEdit(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> QueryListEducationEdit(GenericQueryListDTO<HuEmployeeCvEditDTO> request)
        {
            var response = await _HuEmployeeCvEditRepository.QueryListEducationEdit(request);
            return Ok(new FormatedResponse() { InnerBody = response});
        }

        [HttpPost]
        public async Task<IActionResult> ApproveCvEdit(GenericUnapprovePortalDTO model)
        {
            var response = await _HuEmployeeCvEditRepository.ApproveCvEdit(model);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveContactEdit(GenericUnapprovePortalDTO model)
        {
            var response = await _HuEmployeeCvEditRepository.ApproveContactEdit(model);
            return Ok(response); 
        }

        [HttpPost]
        public async Task<IActionResult> ApproveAdditionalEdit(GenericUnapprovePortalDTO model)
        {
            var response = await _HuEmployeeCvEditRepository.ApproveAdditionalEdit(model);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveBankInfoEdit(GenericUnapprovePortalDTO model)
        {
            var sid = Request.Sid(_appSettings);
            var response = await _HuEmployeeCvEditRepository.ApproveBankInfoEdit(model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveEducationEdit(GenericUnapprovePortalDTO model)
        {
            var response = await _HuEmployeeCvEditRepository.ApproveEducationEdit(model);
            return Ok(response);
        }
    }
}

