using API.All.DbContexts;
using API.DTO;
using API.Main;
using Common.Extensions;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.HuConcurrently
{
    [ApiExplorerSettings(GroupName = "057-PROFILE-HU_CONCURRENTLY")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuConcurrentlyController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IHuConcurrentlyRepository _HuConcurrentlyRepository;
        private readonly AppSettings _appSettings;
        private readonly FullDbContext _fullDbContext;

        public HuConcurrentlyController(
            FullDbContext dbContext,
            IOptions<AppSettings> options,
            FullDbContext fullDbContext)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuConcurrentlyRepository = new HuConcurrentlyRepository(dbContext, _uow);
            _appSettings = options.Value;
            _fullDbContext = fullDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _HuConcurrentlyRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuConcurrentlyRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuConcurrentlyRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuConcurrentlyDTO> request)
        {
          var response = await _HuConcurrentlyRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _HuConcurrentlyRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuConcurrentlyRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuConcurrentlyDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var otherListPending = _fullDbContext.SysOtherLists.Where(x => x.CODE == "CD").FirstOrDefault();
            model.StatusId = otherListPending.ID;
            if (model.EffectiveDate > model.ExpirationDate)
            {
                return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.EFFECTIVEDATE_NOT_BIGGER_THAN_EXPIRATIONDATE, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
            var response = await _HuConcurrentlyRepository.Create(_uow, model, sid);
            
            _fullDbContext.SaveChanges();
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<HuConcurrentlyDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuConcurrentlyRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuConcurrentlyDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var otherListAproved = _fullDbContext.SysOtherLists.Where(x => x.CODE == "CD").FirstOrDefault();
            var otherListPending = _fullDbContext.SysOtherLists.Where(x => x.CODE == "DD").FirstOrDefault();
            if(otherListAproved != null && model.StatusId == otherListAproved.ID)
            {
                return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.NOT_UPDATE_BECAUSE_ROW_APPROVED, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
            if (model.EffectiveDate > model.ExpirationDate)
            {
                return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.EFFECTIVEDATE_NOT_BIGGER_THAN_EXPIRATIONDATE, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
            model.StatusId = otherListPending.ID;
            var response = await _HuConcurrentlyRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<HuConcurrentlyDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuConcurrentlyRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuConcurrentlyDTO model)
        {
            if (model.Id != null)
            {
                var otherListAproved = _fullDbContext.SysOtherLists.Where(x => x.CODE == "CD").FirstOrDefault();
                if (otherListAproved != null && model.StatusId == otherListAproved.ID)
                {
                    return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.NOT_UPDATE_BECAUSE_ROW_APPROVED, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
                }
                var response = await _HuConcurrentlyRepository.Delete(_uow, (long)model.Id);
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

            if (model.Ids != null)
            {
                foreach (var id in model.Ids)
                {
                    var item = await _fullDbContext.HuConcurrentlys.Where(x => x.ID == id).AsNoTracking().SingleOrDefaultAsync();
                    var position = await _fullDbContext.HuPositions.Where(x => x.ID == item!.POSITION_ID).FirstOrDefaultAsync();

                    if (item != null && item.STATUS_ID == OtherConfig.STATUS_APPROVE)
                    {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE });
                    }

                    if (position != null && position.MASTER != null)
                    {
                        position.INTERIM = null;
                        position.MASTER = null;
                    }
                }
                var response = await _HuConcurrentlyRepository.DeleteIds(_uow, model.Ids);
                return Ok(response);
            }
            else
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _HuConcurrentlyRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> QueryListConcurrently(GenericQueryListDTO<HuConcurrentlyDTO> request)
        {
            var response = await _HuConcurrentlyRepository.GetConcurrentEmployee(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> OpenConcurrentlyApprove(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            foreach (var item in model.Ids)
            {
                var r = _fullDbContext.HuConcurrentlys.Where(x => x.ID == item).FirstOrDefault();
                var statusIdCur = r.STATUS_ID;
                r.STATUS_ID = model.ValueToBind == true ? OtherConfig.STATUS_APPROVE : (model.ValueToBind == false ? OtherConfig.STATUS_WAITING : null);
                var position = await _fullDbContext.HuPositions.Where(x => x.ID == r.POSITION_ID).FirstOrDefaultAsync();
                if(r.STATUS_ID == OtherConfig.STATUS_APPROVE)
                {
                    if (position != null && position.MASTER != null)
                    {
                        position.INTERIM = position.MASTER;
                        position.MASTER = r.EMPLOYEE_ID;
                    }
                    else if (position!.MASTER == null)
                    {
                        position.MASTER = r.EMPLOYEE_ID;
                    }
                }
                
                var result = _fullDbContext.HuConcurrentlys.Update(r);
            }
            await _fullDbContext.SaveChangesAsync();
            return Ok(new FormatedResponse() { MessageCode = model.ValueToBind == true ? CommonMessageCode.APPROVE_SUCCESS : (model.ValueToBind == false ? CommonMessageCode.PENDING_APPROVE_SUCCESS : ""), StatusCode = EnumStatusCode.StatusCode200 });
        }

        [HttpGet]
        public async Task<IActionResult> GetPositionPolitical()
        {
            var response = await _HuConcurrentlyRepository.GetPositionPolitical();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeByConcurrentId(long id)
        {
            var response = await _HuConcurrentlyRepository.GetEmployeeByConcurrentId(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllConcurrentByEmployeeCvId(long employeeCvId)
        {
            var response = await _HuConcurrentlyRepository.GetAllConcurrentByEmployeeCvId(employeeCvId);
            return Ok(response);
        }


    }
}

