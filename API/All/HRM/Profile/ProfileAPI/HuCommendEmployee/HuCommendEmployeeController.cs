using API.All.DbContexts;
using API.Controllers.HuCommend;
using API.DTO;
using API.Main;
using Common.Extensions;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.HuCommendEmployee
{
    [ApiExplorerSettings(GroupName = "059-PROFILE-HU_COMMEND_EMPLOYEE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuCommendEmployeeController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IHuCommendEmployeeRepository _HuCommendEmployeeRepository;
        private readonly AppSettings _appSettings;
        private CoreDbContext _coreDbContext;
        private readonly IHuCommendRepository _huCommendRepository;

        public HuCommendEmployeeController(
            FullDbContext dbContext, CoreDbContext coreDbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuCommendEmployeeRepository = new HuCommendEmployeeRepository(dbContext, _uow);
            _huCommendRepository = new HuCommendRepository(dbContext, _uow);
            _appSettings = options.Value;
            _coreDbContext = coreDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _HuCommendEmployeeRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuCommendEmployeeRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuCommendEmployeeRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuCommendEmployeeDTO> request)
        {
            var response = await _HuCommendEmployeeRepository.SinglePhaseQueryList(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _HuCommendEmployeeRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuCommendEmployeeRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuCommendEmployeeDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuCommendEmployeeRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<HuCommendEmployeeDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuCommendEmployeeRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuCommendEmployeeDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuCommendEmployeeRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<HuCommendEmployeeDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuCommendEmployeeRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuCommendEmployeeDTO model)
        {
            if (model.Id != null)
            {
                var response = await _HuCommendEmployeeRepository.Delete(_uow, (long)model.Id);
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

            bool isCheckStatus = false;
            model.Ids.ForEach(item =>
                {
                    var otherList = _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.CODE == "DD").FirstOrDefault();
                    var getHuCommend = _uow.Context.Set<HU_COMMEND_EMPLOYEE>().Where(x => x.ID == item).FirstOrDefault();
                    if (otherList.ID == getHuCommend.STATUS_ID)
                    {
                        isCheckStatus = true;
                        return;
                    }

                });
            if (isCheckStatus == true)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.APPROVED_RECORDS_CAN_NOT_BE_DELETED });
            }
            else
            {
                var response = await _HuCommendEmployeeRepository.DeleteIds(_uow, model.Ids);
                return Ok(response);
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _HuCommendEmployeeRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveCommend(GenericToggleIsActiveDTO model)
        {
            var response = await _HuCommendEmployeeRepository.ApproveCommend(model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> OpenApproveCommend(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            bool pathMode = true;
            List<HuCommendDTO> dto = new();
            List<HuCommendEmployeeDTO> dtos = new();
            if (model.ValueToBind == true)
            {
                model.Ids.ForEach(item =>
                {
                    var getCommendEmp = _uow.Context.Set<HU_COMMEND_EMPLOYEE>().Where(x => x.ID == item).FirstOrDefault();
                    var getCommendEmpList = _uow.Context.Set<HU_COMMEND_EMPLOYEE>().Where(x => x.ID == item).ToList();
                    var getCommend = _uow.Context.Set<HU_COMMEND>().Where(x => x.ID == getCommendEmp!.COMMEND_ID).FirstOrDefault();
                    var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                        from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.TYPE_ID == t.ID)
                                        where o.CODE == "DD"
                                        select new { Id = o.ID }).FirstOrDefault();
                    if (getCommend?.ID != null)
                    {
                        dto.Add(new()
                        {
                            Id = getCommend.ID,
                            StatusPaymentId = getOtherList!.Id
                        });
                    }
                    if (getCommendEmpList != null)
                    {
                        foreach (var items in getCommendEmpList)
                        {
                            dtos.Add(new()
                            {
                                Id = items.ID,
                                StatusId = getOtherList!.Id
                            });
                        }
                    }
                });
                await _HuCommendEmployeeRepository.UpdateRange(_uow, dtos, sid, pathMode);
                await _huCommendRepository.UpdateRange(_uow, dto, sid, pathMode);
                return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode200, InnerBody = dto, MessageCode = CommonMessageCode.APPROVED_SUCCESS });

            }
            else
            {
                model.Ids.ForEach(item =>
                {
                    var getCommendEmp = _uow.Context.Set<HU_COMMEND_EMPLOYEE>().Where(x => x.ID == item).FirstOrDefault();
                    var getCommendEmpList = _uow.Context.Set<HU_COMMEND_EMPLOYEE>().Where(x => x.ID == item).ToList();
                    var getCommend = _uow.Context.Set<HU_COMMEND>().Where(x => x.ID == getCommendEmp.COMMEND_ID).FirstOrDefault();
                    var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                        from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.TYPE_ID == t.ID)
                                        where o.CODE == "CD"
                                        select new { Id = o.ID }).FirstOrDefault();
                    if (getCommend?.ID != null)
                    {
                        dto.Add(new()
                        {
                            Id = getCommend.ID,
                            StatusPaymentId = getOtherList!.Id
                        });
                    }
                    if (getCommendEmpList != null)
                    {
                        foreach (var items in getCommendEmpList)
                        {
                            dtos.Add(new()
                            {
                                Id = items.ID,
                                StatusId = getOtherList!.Id
                            });
                        }
                    }
                });
                await _HuCommendEmployeeRepository.UpdateRange(_uow, dtos, sid, pathMode);
                await _huCommendRepository.UpdateRange(_uow, dto, sid, pathMode);
                return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode200, InnerBody = dto, MessageCode = CommonMessageCode.UNAPPROVED_SUCCESS });
            }

            //{
            //    var r = _coreDbContext.HuCommendEmployees.Where(x => x.ID == item).FirstOrDefault();
            //    var statusIdCur = r.STATUS_ID;
            //    r.STATUS_ID = model.ValueToBind == true ? OtherConfig.STATUS_APPROVE : (model.ValueToBind == false ? OtherConfig.STATUS_WAITING : null);
            //    var result = _coreDbContext.HuCommendEmployees.Update(r);
            //}
            //await _coreDbContext.SaveChangesAsync();
            //return Ok(new FormatedResponse() { MessageCode = model.ValueToBind == true ? CommonMessageCode.APPROVE_SUCCESS : (model.ValueToBind == false ? CommonMessageCode.PENDING_APPROVE_SUCCESS : ""), StatusCode = EnumStatusCode.StatusCode200 });
        }

    }
}

