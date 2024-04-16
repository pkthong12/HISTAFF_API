using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.RcHrYearPlaning
{
    [ApiExplorerSettings(GroupName = "224-RECRUITMENT-RC_HR_YEAR_PLANING")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class RcHrYearPlaningController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IRcHrYearPlaningRepository _RcHrYearPlaningRepository;
        private readonly AppSettings _appSettings;

        public RcHrYearPlaningController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _RcHrYearPlaningRepository = new RcHrYearPlaningRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _RcHrYearPlaningRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _RcHrYearPlaningRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _RcHrYearPlaningRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<RcHrYearPlaningDTO> request)
        {
            var response = await _RcHrYearPlaningRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _RcHrYearPlaningRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _RcHrYearPlaningRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RcHrYearPlaningDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _RcHrYearPlaningRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<RcHrYearPlaningDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _RcHrYearPlaningRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RcHrYearPlaningDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _RcHrYearPlaningRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<RcHrYearPlaningDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _RcHrYearPlaningRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RcHrYearPlaningDTO model)
        {
            if (model.Id != null)
            {
                var response = await _RcHrYearPlaningRepository.Delete(_uow, (long)model.Id);
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
            var response = await _RcHrYearPlaningRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _RcHrYearPlaningRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetYear(long id)
        {
            return Ok(new FormatedResponse()
            {
                InnerBody = new 
                {
                    Name = id,
                    id = id
                }
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPlaning(long? id)
        {
            var response = await _RcHrYearPlaningRepository.GetAllPlaning(id);
            return Ok(response);
        }
    }
}

