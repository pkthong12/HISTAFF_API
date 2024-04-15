using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProfileDAL.ViewModels;

namespace API.Controllers.InsRegimes
{
    [ApiExplorerSettings(GroupName = "101-INSURANCE-INS_REGIMES")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class InsRegimesController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private readonly IInsRegimesRepository _InsRegimesRepository;
        private readonly AppSettings _appSettings;

        public InsRegimesController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _dbContext = dbContext;
            _InsRegimesRepository = new InsRegimesRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _InsRegimesRepository.ReadAll();
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _InsRegimesRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _InsRegimesRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<InsRegimesDTO> request)
        {
            var response = await _InsRegimesRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _InsRegimesRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _InsRegimesRepository.GetById(id);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Create(InsRegimesDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsRegimesRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<InsRegimesDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsRegimesRepository.CreateRange(_uow, models, sid);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Update(InsRegimesDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsRegimesRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<InsRegimesDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _InsRegimesRepository.UpdateRange(_uow, models, sid);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(InsRegimesDTO model)
        {
            if (model.Id != null)
            {
                var response = await _InsRegimesRepository.Delete(_uow, (long)model.Id);
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
            //bool isCheckDataActive = false;
            //bool isCheckDataUsing = false;
            //model.Ids.ForEach(item =>
            //{
            //    var getDataActive = _uow.Context.Set<INS_REGIMES>().Where(x => x.ID == item && x.IS_ACTIVE == true);
            //    var getDataUsing = (from r in _uow.Context.Set<INS_REGIMES>()
            //                        from rm in _uow.Context.Set<INS_REGIMES_MNG>().Where(x => x.ID == item)
            //                        where r.ID == item
            //                        select new { Id = r.ID });
            //    if(getDataActive != null)
            //    {
            //        isCheckDataActive = true;
            //        return;
            //    }
            //    if(getDataUsing != null)
            //    {
            //        isCheckDataUsing = true; 
            //        return;
            //    }
            //});
            //if(isCheckDataActive == true)
            //{
            //    return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORDS_HAVE_IS_ACTIVE });
            //}
            //else if(isCheckDataUsing == true)
            //{
            //    return  Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORDS_HAVE_USE });
            //}
            //else
            //{
                
            //}
            var response = await _InsRegimesRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _InsRegimesRepository.DeleteIds(_uow, model.Ids);
            return Ok(new FormatedResponse() { InnerBody = response });
        }
        [HttpGet]
        public async Task<IActionResult> GetInsGroup()
        {
            var response = await _InsRegimesRepository.GetInsGroup();
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> CreateNewCode()
        {
            var response = await _InsRegimesRepository.CreateNewCode();
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetCalDateType()
        {
            var response = await _InsRegimesRepository.GetCalDateType();
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetCalDateTypeById(long id)
        {
            var response = await _InsRegimesRepository.GetCalDateTypeById(id);
            return Ok(response);
        }
         [HttpPost]
        public async Task<ActionResult> ChangeStatusApprove(InsRegimesDTO request)
        {
            foreach (var item in request.ids!)
            {
                var r = _dbContext.InsRegimess.Where(x => x.ID == item).FirstOrDefault();
                if (r != null)
                {
                    r.IS_ACTIVE = request.ValueToBind;
                    var result = _dbContext.InsRegimess.Update(r);
                }
            }
            await _dbContext.SaveChangesAsync();
            return Ok(new FormatedResponse() { MessageCode = request.ValueToBind == true ? CommonMessageCode.TOGGLE_IS_ACTIVE_SUCCESS : (request.ValueToBind == false ? CommonMessageCode.TOGGLE_IS_INACTIVE_SUCCESS : ""), StatusCode = EnumStatusCode.StatusCode200 });

        }
    }
}

