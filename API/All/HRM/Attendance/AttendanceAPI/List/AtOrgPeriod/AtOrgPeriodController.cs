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

namespace API.Controllers.AtOrgPeriod
{
    [ApiExplorerSettings(GroupName = "011-ATTENDANCE-AT_ORG_PERIOD")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class AtOrgPeriodController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IAtOrgPeriodRepository _AtOrgPeriodRepository;
        private readonly AppSettings _appSettings;

        public AtOrgPeriodController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _AtOrgPeriodRepository = new AtOrgPeriodRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _AtOrgPeriodRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _AtOrgPeriodRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _AtOrgPeriodRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<AtOrgPeriodDTO> request)
        {
            var response = await _AtOrgPeriodRepository.SinglePhaseQueryList(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _AtOrgPeriodRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _AtOrgPeriodRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AtOrgPeriodDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtOrgPeriodRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<AtOrgPeriodDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtOrgPeriodRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AtOrgPeriodDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtOrgPeriodRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<AtOrgPeriodDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtOrgPeriodRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AtOrgPeriodDTO model)
        {
            if (model.Id != null)
            {
                var response = await _AtOrgPeriodRepository.Delete(_uow, (long)model.Id);
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
            var response = await _AtOrgPeriodRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _AtOrgPeriodRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> ReadAllOrgByPeriodID(AtOrgPeriodDTO periodId)
        {
            var response = await _AtOrgPeriodRepository.ReadAllOrgByPeriodId(periodId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetStatusByOrgAndPeriod(AtOrgPeriodDTO periodId)
        {
            var response = await _AtOrgPeriodRepository.GetStatusByOrgAndPeriod(periodId);
            return Ok(response);
        }

    }
}

