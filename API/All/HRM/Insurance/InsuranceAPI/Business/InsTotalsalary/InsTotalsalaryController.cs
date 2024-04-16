using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.InsTotalsalary
{
    [ApiExplorerSettings(GroupName = "177-INSURANCE-INS_TOTALSALARY")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class InsTotalsalaryController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IInsTotalsalaryRepository _InsTotalsalaryRepository;
        private readonly AppSettings _appSettings;

        public InsTotalsalaryController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _InsTotalsalaryRepository = new InsTotalsalaryRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _InsTotalsalaryRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _InsTotalsalaryRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _InsTotalsalaryRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<InsTotalsalaryDTO> request)
        {
            var response = await _InsTotalsalaryRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _InsTotalsalaryRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _InsTotalsalaryRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InsTotalsalaryDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsTotalsalaryRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<InsTotalsalaryDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsTotalsalaryRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(InsTotalsalaryDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsTotalsalaryRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<InsTotalsalaryDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsTotalsalaryRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(InsTotalsalaryDTO model)
        {
            if (model.Id != null)
            {
                var response = await _InsTotalsalaryRepository.Delete(_uow, (long)model.Id);
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
            var response = await _InsTotalsalaryRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _InsTotalsalaryRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Calculate(InsTotalsalaryDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsTotalsalaryRepository.Calculate(model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetInforByPeriod(InsTotalsalaryDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsTotalsalaryRepository.GetInforByPeriod(model);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetInforEndPeriod(InsTotalsalaryDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsTotalsalaryRepository.GetInforEndPeriod(model);
            return Ok(response);
        }
    }
}

