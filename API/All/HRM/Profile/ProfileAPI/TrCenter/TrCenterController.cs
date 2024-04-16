using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.Services.File;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.TrCenter
{
    [ApiExplorerSettings(GroupName = "159-TRAINING-TR_CENTER")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class TrCenterController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly ITrCenterRepository _TrCenterRepository;
        private readonly AppSettings _appSettings;

        public TrCenterController(
            FullDbContext dbContext,
            IOptions<AppSettings> options,
            IWebHostEnvironment env,
            IFileService fileService)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _TrCenterRepository = new TrCenterRepository(dbContext, _uow, env, options, fileService);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _TrCenterRepository.ReadAll();
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _TrCenterRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _TrCenterRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<TrCenterDTO> request)
        {
            var response = await _TrCenterRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _TrCenterRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _TrCenterRepository.GetById(id);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Create(TrCenterDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _TrCenterRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<TrCenterDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _TrCenterRepository.CreateRange(_uow, models, sid);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Update(TrCenterDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _TrCenterRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<TrCenterDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _TrCenterRepository.UpdateRange(_uow, models, sid);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(TrCenterDTO model)
        {
            if (model.Id != null)
            {
                var response = await _TrCenterRepository.Delete(_uow, (long)model.Id);
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
            var response = await _TrCenterRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _TrCenterRepository.DeleteIds(_uow, model.Ids);
            return Ok(new FormatedResponse() { InnerBody = response });
        }
        [HttpGet]
        public async Task<IActionResult> CreateNewCode()
        {
            var response = await _TrCenterRepository.CreateNewCode();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _TrCenterRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(response);
        }
    }
}

