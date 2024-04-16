using API.All.DbContexts;
using API.DTO;
using API.Main;
using AttendanceDAL.ViewModels;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.TrCourse
{
    [ApiExplorerSettings(GroupName = "160-TRAINING-TR_COURSE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class TrCourseController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly ITrCourseRepository _TrCourseRepository;
        private readonly AppSettings _appSettings;

        public TrCourseController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _TrCourseRepository = new TrCourseRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _TrCourseRepository.ReadAll();
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _TrCourseRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _TrCourseRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<TrCourseDTO> request)
        {
            var response = await _TrCourseRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _TrCourseRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _TrCourseRepository.GetById(id);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Create(TrCourseDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _TrCourseRepository.Create(_uow, model, sid);
            return Ok(response );
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<TrCourseDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _TrCourseRepository.CreateRange(_uow, models, sid);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Update(TrCourseDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _TrCourseRepository.Update(_uow, model, sid);
            return Ok( response );
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<TrCourseDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _TrCourseRepository.UpdateRange(_uow, models, sid);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(TrCourseDTO model)
        {
            if (model.Id != null)
            {
                var response = await _TrCourseRepository.Delete(_uow, (long)model.Id);
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
            var response = await _TrCourseRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _TrCourseRepository.DeleteIds(_uow, model.Ids);
            return Ok(new FormatedResponse() { InnerBody = response });
        }
        [HttpGet]
        public async Task<IActionResult> CreateNewCode()
        {
            var response = await _TrCourseRepository.CreateNewCode();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _TrCourseRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult> GetListCourse()
        {
            var response = await _TrCourseRepository.GetListCourse();
            return Ok(response);
            
        }
    }
}

