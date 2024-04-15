using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.HuBankBranch
{
    [ApiExplorerSettings(GroupName = "048-PROFILE-HU_BANK_BRANCH")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuBankBranchController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IHuBankBranchRepository _HuBankBranchRepository;
        private readonly AppSettings _appSettings;

        public HuBankBranchController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuBankBranchRepository = new HuBankBranchRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetNewCode()
        {
            var response = await _HuBankBranchRepository.CreateNewCode();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _HuBankBranchRepository.ReadAll();
            return Ok(response);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetBrankByBankId(long? id)
        {
            var response = await _HuBankBranchRepository.GetBrankByBankId(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuBankBranchRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuBankBranchRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuBankBranchDTO> request)
        {
            var response = await _HuBankBranchRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _HuBankBranchRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuBankBranchRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuBankBranchDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuBankBranchRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<HuBankBranchDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuBankBranchRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuBankBranchDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuBankBranchRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<HuBankBranchDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuBankBranchRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuBankBranchDTO model)
        {
            if (model.Id != null)
            {
                var response = await _HuBankBranchRepository.Delete(_uow, (long)model.Id);
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
                var getDataActive = _uow.Context.Set<HU_BANK_BRANCH>().Where(x => x.ID == item && x.IS_ACTIVE == true);
                var getDataUsing = from br in _uow.Context.Set<HU_BANK_BRANCH>()
                                   from e in _uow.Context.Set<HU_EMPLOYEE_CV>().Where(x => x.BANK_BRANCH == br.ID || x.BANK_BRANCH_2 == br.ID)
                                   where br.ID == item
                                   select new { Id = br.ID };
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
                var response = await _HuBankBranchRepository.DeleteIds(_uow, model.Ids);
                return Ok(response);

            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _HuBankBranchRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuBankBranchRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(response);
        }

    }
}

