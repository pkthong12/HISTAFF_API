using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.InsRegimesMng
{
    [ApiExplorerSettings(GroupName = "102-INSURANCE-INS_REGIMES_MNG")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class InsRegimesMngController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IInsRegimesMngRepository _InsRegimesMngRepository;
        private readonly AppSettings _appSettings;

        public InsRegimesMngController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _InsRegimesMngRepository = new InsRegimesMngRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _InsRegimesMngRepository.ReadAll();
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _InsRegimesMngRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _InsRegimesMngRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<InsRegimesMngDTO> request)
        {
            var response = await _InsRegimesMngRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _InsRegimesMngRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _InsRegimesMngRepository.GetById(id);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Create(InsRegimesMngDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            //if (model.)
            //{

            //}
                var response = await _InsRegimesMngRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<InsRegimesMngDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsRegimesMngRepository.CreateRange(_uow, models, sid);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Update(InsRegimesMngDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsRegimesMngRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<InsRegimesMngDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsRegimesMngRepository.UpdateRange(_uow, models, sid);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(InsRegimesMngDTO model)
        {
            if (model.Id != null)
            {
                var response = await _InsRegimesMngRepository.Delete(_uow, (long)model.Id);
                return Ok(new FormatedResponse() { InnerBody = response });
            }
            else
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            var response = await _InsRegimesMngRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _InsRegimesMngRepository.DeleteIds(_uow, model.Ids);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetRegimesByGroupId(long id)
        {
            var response = await _InsRegimesMngRepository.GetRegimesByGroupId(id);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetRegimes()
        {
            var response = await _InsRegimesMngRepository.GetRegimes();
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetInforByEmployeeId(long id)
        {
            var response = await _InsRegimesMngRepository.GetInforByEmployeeId(id);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> SpsTienCheDo(InsRegimesMngDTO dto)
        {
            var response = await _InsRegimesMngRepository.SpsTienCheDo(dto);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllGroup()
        {
            var response = await _InsRegimesMngRepository.GetAllGroup();
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetAccumulateDay(InsRegimesMngDTO dto)
        {
            var response = await _InsRegimesMngRepository.GetAccumulateDay(dto);
            return Ok(response);
        }
         [HttpGet]
        public async Task<IActionResult> SpsTienBHTH(long? empId, string? date)
        {
            var response = await _InsRegimesMngRepository.SpsTienBH6TH(empId, date.Replace("/", "_"));
            return Ok(response);
        }
    }
}

