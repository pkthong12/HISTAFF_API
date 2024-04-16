using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.HuAssetMng
{
    [ApiExplorerSettings(GroupName = "087-PROFILE-HU_ASSET_MNG")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuAssetMngController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IHuAssetMngRepository _HuAssetMngRepository;
        private readonly AppSettings _appSettings;

        public HuAssetMngController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuAssetMngRepository = new HuAssetMngRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _HuAssetMngRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuAssetMngRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuAssetMngRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuAssetMngDTO> request)
        {
            var response = await _HuAssetMngRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response});
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _HuAssetMngRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuAssetMngRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuAssetMngDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuAssetMngRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<HuAssetMngDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuAssetMngRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuAssetMngDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuAssetMngRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<HuAssetMngDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuAssetMngRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuAssetMngDTO model)
        {
            if (model.Id != null)
            {
                var response = await _HuAssetMngRepository.Delete(_uow, (long)model.Id);
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
            var response = await _HuAssetMngRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _HuAssetMngRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuAssetMngRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetAsset()
        {
            var response = await _HuAssetMngRepository.GetAsset();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetStatusAsset()
        {
            var response = await _HuAssetMngRepository.GetStatusAsset();
            return Ok(response);
        }
    }
}

