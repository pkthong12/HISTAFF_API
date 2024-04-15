using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProfileAPI;
using ProfileDAL.Repositories;

namespace API.Controllers.HuWelfareAuto
{
    [ApiExplorerSettings(GroupName = "087-PROFILE-HU_WELFARE_AUTO")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuWelfareAutoController : BaseController1
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IHuWelfareAutoRepository _HuWelfareAutoRepository;
        private readonly AppSettings _appSettings;

        public HuWelfareAutoController(IProfileUnitOfWork unitOfWork, FullDbContext dbContext, IOptions<AppSettings> options) : base(unitOfWork)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuWelfareAutoRepository = new HuWelfareAutoRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _HuWelfareAutoRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuWelfareAutoRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuWelfareAutoRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuWelfareAutoDTO> request)
        {
            var response = await _HuWelfareAutoRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _HuWelfareAutoRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuWelfareAutoRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuWelfareAutoDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuWelfareAutoRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<HuWelfareAutoDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuWelfareAutoRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuWelfareAutoDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuWelfareAutoRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<HuWelfareAutoDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuWelfareAutoRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuWelfareAutoDTO model)
        {
            if (model.Id != null)
            {
                var response = await _HuWelfareAutoRepository.Delete(_uow, (long)model.Id);
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
            var response = await _HuWelfareAutoRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _HuWelfareAutoRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPeriodYear()
        {
            var response = await _HuWelfareAutoRepository.GetAllPeriodYear();
            return Ok(response);
        }
        [HttpGet]
        // CHuyển thành POST quan DTO
        public async Task<IActionResult> Calculate(long? orgId, long? welfareId, long? periodId, string? calculateDate)
        {
            var r = await _HuWelfareAutoRepository.Calculate(orgId, welfareId, periodId, calculateDate.Replace("/", "_"));
            return Ok(new FormatedResponse() { InnerBody = r.Data });
        }
    }
}

