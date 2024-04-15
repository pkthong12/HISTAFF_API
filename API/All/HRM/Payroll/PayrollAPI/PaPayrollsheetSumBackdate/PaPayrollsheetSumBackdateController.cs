using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.PaPayrollsheetSumBackdate
{
    [ApiExplorerSettings(GroupName = "156-PAYROLL-PA_PAYROLLSHEET_SUM_BACKDATE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class PaPayrollsheetSumBackdateController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IPaPayrollsheetSumBackdateRepository _PaPayrollsheetSumBackdateRepository;
        private readonly AppSettings _appSettings;

        public PaPayrollsheetSumBackdateController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _PaPayrollsheetSumBackdateRepository = new PaPayrollsheetSumBackdateRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> GetListSalaryInYear(PaPayrollsheetSumBackdateDTO param)
        {
            var response = await _PaPayrollsheetSumBackdateRepository.GetListSalaryInYear(param);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetObjSalPayrollBackdateGroup()
        {
            var response = await _PaPayrollsheetSumBackdateRepository.GetObjSalPayrollBackdateGroup();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetNextPeriod(long periodId)
        {
            var response = await _PaPayrollsheetSumBackdateRepository.GetNextPeriod(periodId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetDynamicName(PaPayrollsheetSumBackdateDTO param)
        {
            var response = await _PaPayrollsheetSumBackdateRepository.GetDynamicName(param);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetList(PaPayrollsheetSumBackdateDTO param)
        {
            var response = await _PaPayrollsheetSumBackdateRepository.GetList(param);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> HandleRequest(PaPayrollsheetSumBackdateDTO param)
        {
            var response = await _PaPayrollsheetSumBackdateRepository.HandleRequest(param);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> CheckRequest(long id)
        {
            var response = await _PaPayrollsheetSumBackdateRepository.CheckRequest(id);
            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _PaPayrollsheetSumBackdateRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _PaPayrollsheetSumBackdateRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _PaPayrollsheetSumBackdateRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<PaPayrollsheetSumBackdateDTO> request)
        {
            var response = await _PaPayrollsheetSumBackdateRepository.SinglePhaseQueryList(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _PaPayrollsheetSumBackdateRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _PaPayrollsheetSumBackdateRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaPayrollsheetSumBackdateDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPayrollsheetSumBackdateRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<PaPayrollsheetSumBackdateDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPayrollsheetSumBackdateRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PaPayrollsheetSumBackdateDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPayrollsheetSumBackdateRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<PaPayrollsheetSumBackdateDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPayrollsheetSumBackdateRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PaPayrollsheetSumBackdateDTO model)
        {
            if (model.Id != null)
            {
                var response = await _PaPayrollsheetSumBackdateRepository.Delete(_uow, (long)model.Id);
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
            var response = await _PaPayrollsheetSumBackdateRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _PaPayrollsheetSumBackdateRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatusParoxBackdate(PaPayrollsheetSumBackdateDTO model)
        {
            var response = await _PaPayrollsheetSumBackdateRepository.ChangeStatusParoxBackdate(model);
            return Ok(response);
        }

    }
}

