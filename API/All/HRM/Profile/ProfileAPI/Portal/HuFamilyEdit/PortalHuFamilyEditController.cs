using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;

namespace API.Controllers.HuFamilyEdit
{
    [ApiExplorerSettings(GroupName = "096-PROFILE-HU_FAMILY_EDIT")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class PortalHuFamilyEditController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IPortalHuFamilyEditRepository _HuFamilyEditRepository;
        private readonly AppSettings _appSettings;

        public PortalHuFamilyEditController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuFamilyEditRepository = new PortalHuFamilyEditRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _HuFamilyEditRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuFamilyEditRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuFamilyEditRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuFamilyEditDTO> request)
        {
            var response = await _HuFamilyEditRepository.SinglePhaseQueryList(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _HuFamilyEditRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuFamilyEditRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuFamilyEditDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            if(model.Id != null)
            {
                model.HuFamilyId = model.Id;
                model.Id = null;
                model.IsSavePortal = true;
                var response = await _HuFamilyEditRepository.Create(_uow, model, sid);
                return Ok(response);
            }
            else
            {
                model.IsSavePortal = true;
                var response = await _HuFamilyEditRepository.Create(_uow, model, sid);
                return Ok(response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveHuFamilyEdit(HuFamilyEditDTO model)
        {
            var sid = Request.Sid(_appSettings);
            bool pathMode = true;
            if (sid == null) return Unauthorized();
            var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                from o in _uow.Context.Set<SYS_OTHER_LIST>().DefaultIfEmpty()
                                where o.CODE == "CD"
                                select new { Id = o.ID }).FirstOrDefault();
            if(model.Id != null && model.IsSavePortal == true)
            {
                var updateResponse = await _HuFamilyEditRepository.Update(_uow, model, sid, pathMode);
                return Ok(updateResponse);
            }
            if(model.Id != null && model.IsSavePortal == false)
            {
                model.IsSavePortal = true;
                var updateResponse = await _HuFamilyEditRepository.Update(_uow, model, sid, pathMode);
                return Ok(updateResponse);
            }
            if(model.Id == null)
            {
                model.IsSavePortal = true;
                var createResponse = await _HuFamilyEditRepository.Create(_uow, model, sid);
                return Ok(createResponse);
            }
            var getData = _uow.Context.Set<HU_FAMILY_EDIT>().Where(x => x.EMPLOYEE_ID == model.EmployeeId && x.IS_SAVE_PORTAL == true);
            if (getData.Any())
            {
                return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.HAD_RECORD_IS_SAVE });
            }
            model.IsSavePortal = true;
            model.HuFamilyId = model.Id;
            model.Id = null;
            var response = await _HuFamilyEditRepository.Create(_uow, model, sid);
            return Ok(response);

        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<HuFamilyEditDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuFamilyEditRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuFamilyEditDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuFamilyEditRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<HuFamilyEditDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuFamilyEditRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuFamilyEditDTO model)
        {
            if (model.Id != null)
            {
                var response = await _HuFamilyEditRepository.Delete(_uow, (long)model.Id);
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
            var response = await _HuFamilyEditRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _HuFamilyEditRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuFamilyEditNotApproved(long employeeId)
        {
            var response = await _HuFamilyEditRepository.GetHuFamilyEditNotApproved(employeeId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuFamilyEditSave(long employeeId)
        {
            var response = await _HuFamilyEditRepository.GetHuFamilyEditSave(employeeId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHuFamilyEditSaveById(long id)
        {
            var response = await _HuFamilyEditRepository.GetHuFamilyEditSaveById(id);
            return Ok(response);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetHuFamilyEditByIdCorrect(long id)
        {
            var response = await _HuFamilyEditRepository.GetHuFamilyEditByIdCorrect(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> InsertHuFamilyEdit(HuFamilyEditDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            bool pathMode = true;
            var getOtherList = (from t in _uow.Context.Set<SYS_OTHER_LIST_TYPE>().Where(x => x.CODE == "STATUS")
                                from o in _uow.Context.Set<SYS_OTHER_LIST>().Where(x => x.CODE == "CD")
                                select new { Id = o.ID }).FirstOrDefault();
            
            if (request.Id != null && request.IsSavePortal == true)
            {
                request.IsSavePortal = false;
                request.IsApprovePortal = false;
                request.StatusId = getOtherList?.Id;
                request.IsSendPortal = true;
                var updateResponse = await _HuFamilyEditRepository.Update(_uow, request, sid, pathMode) ;
                return Ok(updateResponse);
            }
            if(request.Id != null && request.IsSavePortal == false)
            {
                request.IsApprovePortal = false;
                request.StatusId = getOtherList?.Id;
                request.IsSendPortal = true;
                var updateResponse = await _HuFamilyEditRepository.Update(_uow, request, sid, pathMode);
                return Ok(updateResponse);
            }
            var getData = _uow.Context.Set<HU_FAMILY_EDIT>().Where(x => x.EMPLOYEE_ID == request.EmployeeId && x.IS_APPROVE_PORTAL == false && x.IS_SEND_PORTAL == true);
            if(getData.Any())
            {
                return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.HAD_RECORD_IS_APPROVING });
            }
            if(request.Id == 0)
            {
                request.IsApprovePortal = false;
                request.StatusId = getOtherList?.Id;
                request.IsSendPortal = true;
                request.IsSavePortal = false;
                var createResponse = await _HuFamilyEditRepository.Create(_uow, request, sid);
                return Ok(createResponse);
            }

            request.IsSendPortal = true;
            request.IsSavePortal = false;
            request.IsApprovePortal = false;
            request.HuFamilyId = request.Id;
            request.Id = null;
            var response = await _HuFamilyEditRepository.Create(_uow, request, sid);
            return Ok(response);

        }

        [HttpGet]
        public async Task<IActionResult> GetHuFamilyEditRefuse(long employeeId)
        {
            var response = await _HuFamilyEditRepository.GetHuFamilyEditRefuse(employeeId);
            return Ok(response);
        }



    }
}

