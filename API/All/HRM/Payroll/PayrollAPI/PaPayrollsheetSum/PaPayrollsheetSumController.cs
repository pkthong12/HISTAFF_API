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


namespace API.Controllers.PaPayrollsheetSum
{
    [ApiExplorerSettings(GroupName = "094-PAYROLL-PA_PAYROLLSHEET_SUM")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class PaPayrollsheetSumController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IPaPayrollsheetSumRepository _PaPayrollsheetSumRepository;
        private readonly AppSettings _appSettings;
        IHubContext<SignalHub> _hubcontext;

        public PaPayrollsheetSumController(
            FullDbContext dbContext,
            IOptions<AppSettings> options,
            IHubContext<SignalHub> hubcontext)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _PaPayrollsheetSumRepository = new PaPayrollsheetSumRepository(dbContext, _uow);
            _appSettings = options.Value;
            _hubcontext = hubcontext;
        }

        [HttpPost]
        public async Task<IActionResult> ComparePayrollFund(PaPayrollsheetSumDTO dto)
        {
            var response = await _PaPayrollsheetSumRepository.ComparePayrollFund(dto);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetPayrollByEmployee(long id, long salaryPeriodId)
        {
            var response = await _PaPayrollsheetSumRepository.GetPayrollByEmployee(id, salaryPeriodId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetDynamicName(PaPayrollsheetSumDTO param)
        {
            var response = await _PaPayrollsheetSumRepository.GetDynamicName(param);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetList(PaPayrollsheetSumDTO param)
        {
            var response = await _PaPayrollsheetSumRepository.GetList(param);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> HandleRequest(PaPayrollsheetSumDTO param)
        {
            var response = await _PaPayrollsheetSumRepository.HandleRequest(param);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> CheckRequest(long id)
        {
            var response = await _PaPayrollsheetSumRepository.CheckRequest(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _PaPayrollsheetSumRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _PaPayrollsheetSumRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _PaPayrollsheetSumRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<PaPayrollsheetSumDTO> request)
        {
            var response = await _PaPayrollsheetSumRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _PaPayrollsheetSumRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _PaPayrollsheetSumRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaPayrollsheetSumDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPayrollsheetSumRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<PaPayrollsheetSumDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPayrollsheetSumRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PaPayrollsheetSumDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPayrollsheetSumRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<PaPayrollsheetSumDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPayrollsheetSumRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PaPayrollsheetSumDTO model)
        {
            if (model.Id != null)
            {
                var response = await _PaPayrollsheetSumRepository.Delete(_uow, (long)model.Id);
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
            var response = await _PaPayrollsheetSumRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _PaPayrollsheetSumRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatusParox(PaPayrollsheetSumDTO model)
        {
            var response = await _PaPayrollsheetSumRepository.ChangeStatusParox(model);
            return Ok(response);
        }

    }
}
