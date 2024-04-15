using API.All.DbContexts;
using API.Controllers.HuFamilyEdit;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.HuFamily
{
    [ApiExplorerSettings(GroupName = "093-PROFILE-HU_FAMILY")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class PortalHuFamilyController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IPortalHuFamilyRepository _HuFamilyRepository;
        private readonly IPortalHuFamilyEditRepository _HuFamilyEditRepository;
        private readonly AppSettings _appSettings;

        public PortalHuFamilyController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuFamilyRepository = new PortalHuFamilyRepository(dbContext, _uow);
            _HuFamilyEditRepository = new PortalHuFamilyEditRepository(dbContext, _uow);
         _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _HuFamilyRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuFamilyRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuFamilyRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuFamilyDTO> request)
        {
            var response = await _HuFamilyRepository.SinglePhaseQueryList(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _HuFamilyRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuFamilyRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuFamilyDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuFamilyRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<HuFamilyDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuFamilyRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuFamilyDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuFamilyRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<HuFamilyDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuFamilyRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuFamilyDTO model)
        {
            if (model.Id != null)
            {
                var response = await _HuFamilyRepository.Delete(_uow, (long)model.Id);
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
            var response = await _HuFamilyRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _HuFamilyRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFamilyByEmployee(long employeeId) {
            var response = await _HuFamilyRepository.GetAllFamilyByEmployee(employeeId);
            return Ok(response);
        }
        
        //[HttpPost]
        //public  Task<IActionResult> ApproveHuFamilyEdit(List<long> ids)
        //{
        //    ids.ForEach( id =>
        //    {
        //        var response = _HuFamilyRepository.GetById(id);
        //        if( response.InnerBody == null )
        //        {
        //            throw new Exception("Error");
        //        }
        //        else
        //        {
        //            HuFamilyEditDTO dto = response.inner
        //        }
        //    });
        //}

    }
}

