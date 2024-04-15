using API.All.DbContexts;
using API.All.HRM.DynamicReport.HuDynamicReport;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.HuDynamicReport
{
    [ApiExplorerSettings(GroupName = "103-PROFILE-HU_DYNAMIC_REPORT")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuDynamicReportController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IHuDynamicReportRepository _HuDynamicReportRepository;
        private readonly AppSettings _appSettings;
        private readonly IWebHostEnvironment env;
        private IGenericRepository<HU_DYNAMIC_REPORT, HuDynamicReportDTO> _genericRepository;

        public HuDynamicReportController(
            FullDbContext dbContext,
            IOptions<AppSettings> options, IWebHostEnvironment _env)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _HuDynamicReportRepository = new HuDynamicReportRepository(dbContext, _uow, _env, options);
            _appSettings = options.Value;
            _genericRepository = _uow.GenericRepository<HU_DYNAMIC_REPORT, HuDynamicReportDTO>();
        }

        [HttpGet]
        public async Task<IActionResult> ReadAllByViewName(string? viewName)
        {
            var response = await _HuDynamicReportRepository.ReadAllByViewName(viewName);
            
            return Ok(response);
        }
        
        [HttpPost]
        public async Task<IActionResult> GetListByConditionToExport(DynamicReportDTO? request1)
        {
            var response = await _HuDynamicReportRepository.GetListByConditionToExport(request1);
            var contentType = "application/octet-stream";
            var fileName = "";
            return File(response, contentType, fileName);
        }

        //[HttpPost]
        //public async Task<IActionResult> ExportExelDynamicReport(DynamicReportDTO? request)
        //{
        //    var response = await _HuDynamicReportRepository.ExportExelDynamicReport(request);
        //    return File(response.Bytes!, response.ContentType!, response.FileName);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetAllByFid(long? fid)
        //{
        //    var response = await _HuDynamicReportRepository.GetAllByFid(fid);

        //    return Ok(response);
        //}


        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _HuDynamicReportRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _HuDynamicReportRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuDynamicReportDTO> request)
        {
            var response = await _HuDynamicReportRepository.SinglePhaseQueryList(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _HuDynamicReportRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _HuDynamicReportRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuDynamicReportDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuDynamicReportRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<HuDynamicReportDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuDynamicReportRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuDynamicReportDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuDynamicReportRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<HuDynamicReportDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _HuDynamicReportRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HuDynamicReportDTO model)
        {
            if (model.Id != null)
            {
                var response = await _HuDynamicReportRepository.Delete(_uow, (long)model.Id);
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
            var response = await _HuDynamicReportRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _HuDynamicReportRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
           var sid = Request.Sid(_appSettings);
           if (sid == null) return Unauthorized();
           var response = await _genericRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
           return Ok(new FormatedResponse()
           {
              MessageCode = response.MessageCode,
              InnerBody = response.InnerBody,
              StatusCode = EnumStatusCode.StatusCode200
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetViewList()
        {
            var response = await _HuDynamicReportRepository.GetViewList();
            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetColumnList(GetColumnListRequest request)
        {
            var response = await _HuDynamicReportRepository.GetColumnList(request);
            return Ok(response);
        }

    }
}

