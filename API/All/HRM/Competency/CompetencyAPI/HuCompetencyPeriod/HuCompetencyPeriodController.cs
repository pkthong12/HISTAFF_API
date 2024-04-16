using API.All.DbContexts;
using API.All.HRM.Competency.CompetencyAPI.HuCompetencyPeroid;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.All.HRM.Competency.CompetencyAPI.HuCompetencyPeroid
{
    [ApiExplorerSettings(GroupName = "104-PROFILE-HU_COMPETENCY_PERIOD")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuCompetencyPeriodController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IHuCompetencyPeriodRepository _HuCompetencyPeriodRepository;
        private readonly AppSettings _appSettings;

        public HuCompetencyPeriodController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuCompetencyPeriodRepository = new HuCompetencyPeriodRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _HuCompetencyPeriodRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuCompetencyPeriodRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuCompetencyPeriodRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuCompetencyPeriodDTO> request)
        {
            var response = await _HuCompetencyPeriodRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _HuCompetencyPeriodRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuCompetencyPeriodRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuCompetencyPeriodDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuCompetencyPeriodRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<HuCompetencyPeriodDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuCompetencyPeriodRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuCompetencyPeriodDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuCompetencyPeriodRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<HuCompetencyPeriodDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuCompetencyPeriodRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuCompetencyPeriodDTO model)
        {
            if (model.Id != null)
            {
                var response = await _HuCompetencyPeriodRepository.Delete(_uow, (long)model.Id);
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
            var response = await _HuCompetencyPeriodRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _HuCompetencyPeriodRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetQuarter()
        {
            var response = await _HuCompetencyPeriodRepository.GetQuarter();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetPeriodYear()
        {
            var response = await _HuCompetencyPeriodRepository.GetPeriodYear();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuCompetencyPeriodRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(response);
        }
    }
}

