using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.HuContractType
{
    [ApiExplorerSettings(GroupName = "053-PROFILE-HU_CONTRACT_TYPE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuContractTypeController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IHuContractTypeRepository _HuContractTypeRepository;
        private readonly AppSettings _appSettings;

        public HuContractTypeController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuContractTypeRepository = new HuContractTypeRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _HuContractTypeRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuContractTypeRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuContractTypeRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuContractTypeDTO> request)
        {
            var response = await _HuContractTypeRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse()
            {
                InnerBody = response
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _HuContractTypeRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuContractTypeRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuContractTypeDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuContractTypeRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<HuContractTypeDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuContractTypeRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuContractTypeDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuContractTypeRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<HuContractTypeDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuContractTypeRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuContractTypeDTO model)
        {
            if (model.Id != null)
            {
                var response = await _HuContractTypeRepository.Delete(_uow, (long)model.Id);
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
            bool isCheckDataActive = false;
            bool isCheckDataUsing = false;
            model.Ids.ForEach(item =>
            {
                var getDataActive = (from c in _uow.Context.Set<HU_CONTRACT_TYPE>().Where(x => x.ID == item && x.IS_ACTIVE == true)
                                     select new { Id = c.ID }).FirstOrDefault();
                var getDataUsing = (from ct in _uow.Context.Set<HU_CONTRACT_TYPE>()
                                    from c in _uow.Context.Set<HU_CONTRACT>().Where(x => x.CONTRACT_TYPE_ID == ct.ID)
                                    where ct.ID == item
                                    select new { Id = ct.ID }).FirstOrDefault();
                if(getDataActive != null)
                {
                    isCheckDataActive = true;
                    return;
                }
                if(getDataUsing != null)
                {
                    isCheckDataUsing = true;
                    return;
                }

            });
            if(isCheckDataActive == true) 
            {
                return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORDS_HAVE_IS_ACTIVE });
            }
            else if(isCheckDataUsing == true)
            {
                return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORDS_HAVE_USE });
            }
            else
            {
                var response = await _HuContractTypeRepository.DeleteIds(_uow, model.Ids);
                return Ok(response);
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _HuContractTypeRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetListContractTypeSys()
        {
            var response = await _HuContractTypeRepository.GetListContractTypeSys();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetContractTypeSysById(long id)
        {
            var response = await _HuContractTypeRepository.GetContractTypeSysById(id);
            return Ok(response);
        }
         [HttpGet]
        public async Task<IActionResult> GetContractAppendixType()
        {
            var response = await _HuContractTypeRepository.GetContractAppendixType();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuContractTypeRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(response);
        }

        
        [HttpGet]
        public async Task<IActionResult> CheckCodeExists(string code)
        {
            var response = await _HuContractTypeRepository.CheckCodeExists(code);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetContractDashboard()
        {
            var response = await _HuContractTypeRepository.GetContractDashboard();
            return Ok(response);
        }
    }
}

