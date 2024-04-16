using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace API.Controllers.TrClass
{
    [ApiExplorerSettings(GroupName = "292-OTHER-TR_CLASS")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class TrClassController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly ITrClassRepository _TrClassRepository;
        private readonly AppSettings _appSettings;

        public TrClassController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _TrClassRepository = new TrClassRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _TrClassRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _TrClassRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _TrClassRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<TrClassDTO> request)
        {
            var response = await _TrClassRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _TrClassRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _TrClassRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TrClassDTO model)
        {
            IFormatProvider culture = new CultureInfo("en-US", true);
            model.TimeFrom = DateTime.ParseExact(model.StartDate!.Value.ToString("dd/MM/yyyy") + " " + model.TimeFromStr, "dd/MM/yyyy HH:mm", culture);
            model.TimeTo = DateTime.ParseExact(model.StartDate!.Value.ToString("dd/MM/yyyy") + " " + model.TimeToStr, "dd/MM/yyyy HH:mm", culture);
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _TrClassRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<TrClassDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _TrClassRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(TrClassDTO model)
        {
            IFormatProvider culture = new CultureInfo("en-US", true);
            model.TimeFrom = DateTime.ParseExact(model.StartDate!.Value.ToString("dd/MM/yyyy") + " " + model.TimeFromStr, "dd/MM/yyyy HH:mm", culture);
            model.TimeTo = DateTime.ParseExact(model.StartDate!.Value.ToString("dd/MM/yyyy") + " " + model.TimeToStr, "dd/MM/yyyy HH:mm", culture);
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _TrClassRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<TrClassDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _TrClassRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(TrClassDTO model)
        {
            if (model.Id != null)
            {
                var response = await _TrClassRepository.Delete(_uow, (long)model.Id);
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
            var response = await _TrClassRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _TrClassRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

    }
}

