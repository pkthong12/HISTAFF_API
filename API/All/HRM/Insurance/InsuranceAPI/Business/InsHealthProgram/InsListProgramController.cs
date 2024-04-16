//namespace API.All.HRM.Insurance.InsuranceAPI.Business.InsHealthProgram
//{
//    public class InsListProgramController
//    {
//    }
//}

using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.InsListProgram
{
    [ApiExplorerSettings(GroupName = "090-INSURANCE-INS_LIST_PROGRAM")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class InsListProgramController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IInsListProgramRepository _InsListRepository;
        private readonly AppSettings _appSettings;

        public InsListProgramController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _InsListRepository = new InsListProgramRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _InsListRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _InsListRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _InsListRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<InsListProgramDTO> request)
        {
            var response = await _InsListRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _InsListRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _InsListRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InsListProgramDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            model.IsActive = true;
            var response = await _InsListRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<InsListProgramDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsListRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(InsListProgramDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsListRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<InsListProgramDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsListRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(InsListProgramDTO model)
        {
            if (model.Id != null)
            {
                var response = await _InsListRepository.Delete(_uow, (long)model.Id);
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
                var getDataActive = _uow.Context.Set<INS_LIST_PROGRAM>().Where(x => x.ID == item && x.IS_ACTIVE == true).FirstOrDefault();
                //var getDataUsing = (from g in _uow.Context.Set<INS_LIST_PROGRAM>()
                //                    from r in _uow.Context.Set<INS_REGIMES>().Where(x => x.INS_GROUP_ID == g.ID)
                //                    where g.ID == item
                //                    select new { Id = g.ID }).FirstOrDefault();
                if (getDataActive != null)
                {
                    isCheckDataActive = true;
                    return;
                }
                //if (getDataUsing != null)
                //{
                //    isCheckDataUsing = true;
                //    return;
                //}
            });
            if (isCheckDataActive == true)
            {
                return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORDS_HAVE_IS_ACTIVE });
            }
            else if (isCheckDataUsing == true)
            {
                return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORDS_HAVE_USE });
            }
            else
            {
                var response = await _InsListRepository.DeleteIds(_uow, model.Ids);
                return Ok(response);

            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _InsListRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }
        //[HttpGet]
        //public async Task<IActionResult> CreateNewCode()
        //{
        //    var response = await _InsListRepository.CreateNewCode();
        //    return Ok(response);
        //}

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsListRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(response);
        }
    }
}


