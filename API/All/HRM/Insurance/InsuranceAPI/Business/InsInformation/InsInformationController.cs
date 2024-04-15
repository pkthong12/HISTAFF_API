using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.InsInformation
{
    [ApiExplorerSettings(GroupName = "098-INSURANCE-INS_INFORMATION")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class InsInformationController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IInsInformationRepository _InsInformationRepository;
        private readonly AppSettings _appSettings;

        public InsInformationController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _InsInformationRepository = new InsInformationRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _InsInformationRepository.ReadAll();
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _InsInformationRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _InsInformationRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<InsInformationDTO> request)
        {
            var response = await _InsInformationRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _InsInformationRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _InsInformationRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InsInformationDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsInformationRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<InsInformationDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsInformationRepository.CreateRange(_uow, models, sid);
            return Ok(response );
        }

        [HttpPost]
        public async Task<IActionResult> Update(InsInformationDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsInformationRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<InsInformationDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsInformationRepository.UpdateRange(_uow, models, sid);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(InsInformationDTO model)
        {
            if (model.Id != null)
            {
                var response = await _InsInformationRepository.Delete(_uow, (long)model.Id);
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
            var response = await _InsInformationRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _InsInformationRepository.DeleteIds(_uow, model.Ids);
            return Ok(new FormatedResponse() { InnerBody = response });
        }
        [HttpGet]
        public async Task<IActionResult> GetBhxhStatus()
        {
            var response = await _InsInformationRepository.GetBhxhStatus();
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetBhYtStatus()
        {
            var response = await _InsInformationRepository.GetBhYtStatus();
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetInsWhereHealth()
        {
            var response = await _InsInformationRepository.GetInsWhereHealth();
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetBhxhStatusById(long id)
        {
            var response = await _InsInformationRepository.GetBhxhStatusById(id);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetBhYtStatusById(long id)
        {
            var response = await _InsInformationRepository.GetBhYtStatusById(id);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetInsWhereHealthById(long id)
        {
            var response = await _InsInformationRepository.GetInsWhereHealthById(id);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetInforById(long id)
        {
            var response = await _InsInformationRepository.GetInforById(id);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetLstInsCheck(long id)
        {
            var response = await _InsInformationRepository.GetLstInsCheck(id);
            return Ok(response);
        }
    }
}

