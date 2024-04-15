using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.PaPayrollsheetSumSub
{
    [ApiExplorerSettings(GroupName = "149-PAYROLL-PA_PAYROLLSHEET_SUM_SUB")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class PaPayrollsheetSumSubController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IPaPayrollsheetSumSubRepository _PaPayrollsheetSumSubRepository;
        private readonly AppSettings _appSettings;

        public PaPayrollsheetSumSubController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _PaPayrollsheetSumSubRepository = new PaPayrollsheetSumSubRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> GetDynamicName(PaPayrollsheetSumSubDTO param)
        {
            var response = await _PaPayrollsheetSumSubRepository.GetDynamicName(param);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetObjSalPayrollSubGroup()
        {
            var response = await _PaPayrollsheetSumSubRepository.GetObjSalPayrollSubGroup();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetList(PaPayrollsheetSumSubDTO param)
        {
            var response = await _PaPayrollsheetSumSubRepository.GetList(param);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> HandleRequest(PaPayrollsheetSumSubDTO param)
        {
            var response = await _PaPayrollsheetSumSubRepository.HandleRequest(param);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> CheckRequest(long id)
        {
            var response = await _PaPayrollsheetSumSubRepository.CheckRequest(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _PaPayrollsheetSumSubRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _PaPayrollsheetSumSubRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _PaPayrollsheetSumSubRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<PaPayrollsheetSumSubDTO> request)
        {
            var response = await _PaPayrollsheetSumSubRepository.SinglePhaseQueryList(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _PaPayrollsheetSumSubRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _PaPayrollsheetSumSubRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaPayrollsheetSumSubDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPayrollsheetSumSubRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<PaPayrollsheetSumSubDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPayrollsheetSumSubRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PaPayrollsheetSumSubDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPayrollsheetSumSubRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<PaPayrollsheetSumSubDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaPayrollsheetSumSubRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PaPayrollsheetSumSubDTO model)
        {
            if (model.Id != null)
            {
                var response = await _PaPayrollsheetSumSubRepository.Delete(_uow, (long)model.Id);
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
            var response = await _PaPayrollsheetSumSubRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _PaPayrollsheetSumSubRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatusParoxSub(PaPayrollsheetSumSubDTO model)
        {
            var response = await _PaPayrollsheetSumSubRepository.ChangeStatusParoxSub(model);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetPhaseAdvance(PaPayrollsheetSumSubDTO param)
        {
            var response = await _PaPayrollsheetSumSubRepository.GetPhaseAdvance(param);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetPhaseAdvanceById(long id)
        {
            var response = await _PaPayrollsheetSumSubRepository.GetPhaseAdvanceById(id);
            return Ok(response);
        }
    }
}

