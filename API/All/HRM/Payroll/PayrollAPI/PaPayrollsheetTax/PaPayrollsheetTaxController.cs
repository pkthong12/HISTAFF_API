using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.PaPayrollsheetTax
{
    [ApiExplorerSettings(GroupName = "150-PAYROLL-PA_PAYROLLSHEET_TAX")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class PaPayrollsheetTaxController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IPaPayrollsheetTaxRepository _PaPayrollsheetTaxRepository;
        private readonly AppSettings _appSettings;

        public PaPayrollsheetTaxController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _PaPayrollsheetTaxRepository = new PaPayrollsheetTaxRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _PaPayrollsheetTaxRepository.ReadAll();
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _PaPayrollsheetTaxRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _PaPayrollsheetTaxRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }
        [HttpPost]
        public async Task<IActionResult> GetDynamicName(PaPayrollsheetTaxDTO param)
        {
            var response = await _PaPayrollsheetTaxRepository.GetDynamicName(param);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetList(PaPayrollsheetTaxDTO param)
        {
            var response = await _PaPayrollsheetTaxRepository.GetList(param);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> HandleRequest(PaPayrollsheetTaxDTO param)
        {
            var response = await _PaPayrollsheetTaxRepository.HandleRequest(param);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> CheckRequest(long id)
        {
            var response = await _PaPayrollsheetTaxRepository.CheckRequest(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<PaPayrollsheetTaxDTO> request)
        {
            var response = await _PaPayrollsheetTaxRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _PaPayrollsheetTaxRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _PaPayrollsheetTaxRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaPayrollsheetTaxDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPayrollsheetTaxRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<PaPayrollsheetTaxDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPayrollsheetTaxRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PaPayrollsheetTaxDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPayrollsheetTaxRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<PaPayrollsheetTaxDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPayrollsheetTaxRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PaPayrollsheetTaxDTO model)
        {
            if (model.Id != null)
            {
                var response = await _PaPayrollsheetTaxRepository.Delete(_uow, (long)model.Id);
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
            var response = await _PaPayrollsheetTaxRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _PaPayrollsheetTaxRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatusParoxTaxMonth(PaPayrollsheetTaxDTO model)
        {
            var response = await _PaPayrollsheetTaxRepository.ChangeStatusParoxTaxMonth(model);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetTaxDate(PaPayrollsheetTaxDTO param)
        {
            var response = await _PaPayrollsheetTaxRepository.GetTaxDate(param);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetMonth(PaPayrollsheetTaxDTO param)
        {
            var response = await _PaPayrollsheetTaxRepository.GetMonth(param);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetPeriodId(long year, long month)
        {
            var response = await _PaPayrollsheetTaxRepository.GetPeriodId(year, month);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetObjSal()
        {
            var response = await _PaPayrollsheetTaxRepository.GetObjSal();
            return Ok(response);
        }
    }
}

