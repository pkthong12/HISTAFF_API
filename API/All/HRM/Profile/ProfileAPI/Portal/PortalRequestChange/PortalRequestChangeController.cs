using API.All.DbContexts;
using API.DTO;
using API.DTO.PortalDTO;
using API.Entities.PORTAL;
using API.Main;
using API.Socket;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.Services.File;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using ProfileDAL.ViewModels;

namespace API.Controllers.PortalRequestChange
{
    [ApiExplorerSettings(GroupName = "003-PORTAL-PORTAL_REQUEST_CHANGE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class PortalRequestChangeController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IPortalRequestChangeRepository _PortalRequestChangeRepository;
        private readonly AppSettings _appSettings;
        private readonly IWebHostEnvironment _env;
        private readonly IFileService _fileService;

        //private readonly GenericReducer<HU_WORKING_BEFORE, HuWorkingBeforeDTO> genericReducer;
        private readonly GenericReducer<PORTAL_REQUEST_CHANGE, PortalRequestChangeDTO> genericReducer;

        public PortalRequestChangeController(
            FullDbContext dbContext,
            IFileService fileService,
             IWebHostEnvironment env,
            IOptions<AppSettings> options,
            IHubContext<SignalHub> hubContext
            )
        {
            _uow = new GenericUnitOfWork(dbContext);
            _PortalRequestChangeRepository = new PortalRequestChangeRepository(dbContext, _uow, env, hubContext, fileService, options);
            genericReducer = new();
            _appSettings = options.Value;
            _fileService = fileService;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _PortalRequestChangeRepository.ReadAll();
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<PortalRequestChangeDTO> request)
        {
            var response = await _PortalRequestChangeRepository.SinglePhaseQueryList(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PortalRequestChangeDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalRequestChangeRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(PortalRequestChangeDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalRequestChangeRepository.Approve(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SendRequest(PortalRequestChangeDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalRequestChangeRepository.SendRequest(model, sid);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetSalAllowanceProcessByEmp(long id)
        {
            var response = await _PortalRequestChangeRepository.GetSalAllowanceProcessByEmp(id);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetConcurrentlyByEmpId(long id)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PortalRequestChangeRepository.GetConcurrentlyByEmpId(id);
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> QueryListApproveWorkingBefore(GenericQueryListDTO<PortalRequestChangeDTO> request)
        {

            try
            {
                var get_id_wait_approve = (from item_sub in _uow.Context.Set<SYS_OTHER_LIST_TYPE>()
                                           .Where(x => x.CODE == "STATUS")

                                           from item_main in _uow.Context.Set<SYS_OTHER_LIST>()
                                           .Where(x => x.TYPE_ID == item_sub.ID && x.CODE == "CD")

                                           select item_main.ID).First();

                var code = "00042";
                var entity = _uow.Context.Set<HU_WORKING_BEFORE>().AsNoTracking().AsQueryable();
                var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var job = _uow.Context.Set<HU_JOB>().AsNoTracking().AsQueryable();
                var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
                var portalWorking = _uow.Context.Set<PORTAL_REQUEST_CHANGE>().AsNoTracking().AsQueryable();
                var isApprove = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var joined = from pw in portalWorking.Where(x => x.SYS_OTHER_CODE == code && x.IS_APPROVE == get_id_wait_approve)
                             from p in entity.Where(x => x.ID == pw.ID_CHANGE).DefaultIfEmpty()
                             from e in employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                             from t in positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                             from j in job.Where(x => x.ID == t.JOB_ID).DefaultIfEmpty()
                             from o in organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                             from l in isApprove.Where(x => x.ID == pw.IS_APPROVE).DefaultIfEmpty()
                             orderby p.CREATED_DATE descending
                             select new PortalRequestChangeDTO
                             {
                                 Id = pw.ID,
                                 EmployeeName = e.Profile.FULL_NAME,
                                 JobOrderNum = (int)(j.ORDERNUM ?? 999),
                                 EmployeeCode = e.CODE,
                                 EmployeeStatus = e.WORK_STATUS_ID,
                                 PositionName = t.NAME,
                                 OrgId = e.ORG_ID,
                                 OrgName = o.NAME,
                                 FromDate = p.FROM_DATE,
                                 EndDate = p.END_DATE,
                                 MainDuty = p.MAIN_DUTY,
                                 CompanyName = p.COMPANY_NAME,
                                 TitleName = p.TITLE_NAME,
                                 TerReason = p.TER_REASON,
                                 Seniority = p.SENIORITY,
                                 CreatedBy = pw.CREATED_BY,
                                 UpdatedBy = pw.UPDATED_BY,
                                 CreatedDate = pw.CREATED_DATE,
                                 UpdatedDate = pw.UPDATED_DATE,
                                 ReasonChange = pw.REASON_CHANGE,
                                 IsApproveName = l.NAME,
                                 IsApprove = pw.IS_APPROVE,
                             };
                var response = await genericReducer.SinglePhaseReduce(joined, request);
                if (response.ErrorType != EnumErrorType.NONE)
                {
                    return Ok(new FormatedResponse()
                    {
                        ErrorType = response.ErrorType,
                        MessageCode = response.MessageCode ?? CommonMessageCode.NOT_ALL_CASES_CATCHED,
                        StatusCode = EnumStatusCode.StatusCode400,
                    });
                }
                else
                {
                    return Ok(new FormatedResponse()
                    {
                        InnerBody = response
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> ApproveWorkingBefore(GenericToggleIsActiveDTO model)
        {
            var response = await _PortalRequestChangeRepository.ApproveWorkingBeforeIds(model.Ids);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetSalInsuByEmployeeId(long employeeId)
        {
            var response = await _PortalRequestChangeRepository.GetSalInsuByEmployeeId(employeeId);
            return  Ok(response);
        }
    }
}

