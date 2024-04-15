using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.PaPayrollTaxYear
{
    [ApiExplorerSettings(GroupName = "157-PAYROLL-PA_PAYROLL_TAX_YEAR")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class PaPayrollTaxYearController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IPaPayrollTaxYearRepository _PaPayrollTaxYearRepository;
        private readonly AppSettings _appSettings;

        public PaPayrollTaxYearController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _PaPayrollTaxYearRepository = new PaPayrollTaxYearRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> GetDynamicName(PaPayrollTaxYearDTO param)
        {
            var response = await _PaPayrollTaxYearRepository.GetDynamicName(param);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetList(PaPayrollTaxYearDTO param)
        {
            var response = await _PaPayrollTaxYearRepository.GetList(param);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> HandleRequest(PaPayrollTaxYearDTO param)
        {
            var response = await _PaPayrollTaxYearRepository.HandleRequest(param);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetObjSalTaxGroup()
        {
            var response = await _PaPayrollTaxYearRepository.GetObjSalTaxGroup();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> CheckRequest(long id)
        {
            var response = await _PaPayrollTaxYearRepository.CheckRequest(id);
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> ChangeStatusParoxTaxYear(PaPayrollTaxYearDTO model)
        {
            var response = await _PaPayrollTaxYearRepository.ChangeStatusParoxTaxYear(model);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _PaPayrollTaxYearRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _PaPayrollTaxYearRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _PaPayrollTaxYearRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<PaPayrollTaxYearDTO> request)
        {
            var response = await _PaPayrollTaxYearRepository.SinglePhaseQueryList(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _PaPayrollTaxYearRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _PaPayrollTaxYearRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaPayrollTaxYearDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPayrollTaxYearRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<PaPayrollTaxYearDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPayrollTaxYearRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PaPayrollTaxYearDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPayrollTaxYearRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<PaPayrollTaxYearDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPayrollTaxYearRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PaPayrollTaxYearDTO model)
        {
            if (model.Id != null)
            {
                var response = await _PaPayrollTaxYearRepository.Delete(_uow, (long)model.Id);
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
            var response = await _PaPayrollTaxYearRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _PaPayrollTaxYearRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

    }
}

