using API.All.DbContexts;
using API.DTO;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.Services.File;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.HuCommend
{
    [ApiExplorerSettings(GroupName = "048-SYSTEM-SYS_MAIL_TEMPLATE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class SysMailTemplateController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly ISysMailTemplateRepository _sysMailTemplateRepository;
        private readonly AppSettings _appSettings;
        private readonly IFileService _fileService;
        private readonly FullDbContext _fullDbContext;
        private readonly IWebHostEnvironment _env;

        public SysMailTemplateController(
            FullDbContext fullDbContext,
            FullDbContext dbContext, IFileService fileService,
            IWebHostEnvironment env,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _sysMailTemplateRepository = new SysMailTemplateRepository(dbContext, _uow);
            _appSettings = options.Value;
            _fileService = fileService;
            _env = env;
            _fullDbContext = fullDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _sysMailTemplateRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _sysMailTemplateRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _sysMailTemplateRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<SysMailTemplateDTO> request)
        {
            var response = await _sysMailTemplateRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response});
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _sysMailTemplateRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _sysMailTemplateRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SysMailTemplateDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _sysMailTemplateRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<SysMailTemplateDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _sysMailTemplateRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SysMailTemplateDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _sysMailTemplateRepository.Update(_uow, request, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<SysMailTemplateDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _sysMailTemplateRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SysMailTemplateDTO model)
        {
            if (model.Id != null)
            {
                var response = await _sysMailTemplateRepository.Delete(_uow, (long)model.Id);
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
            try
            {
                
                var response = await _sysMailTemplateRepository.DeleteIds(_uow, model.Ids);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _sysMailTemplateRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetListCommendByEmployee(long employeeId)
        {
            var entity = _uow.Context.Set<HU_COMMEND>().AsNoTracking();
            var employee = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
            var commendEmployee = _uow.Context.Set<HU_COMMEND_EMPLOYEE>().AsNoTracking().AsQueryable();
            var organization = _uow.Context.Set<HU_ORG_LEVEL>().AsNoTracking().AsQueryable();
            var otherList = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
            var orgLevel = _uow.Context.Set<HU_ORG_LEVEL>().AsNoTracking().AsQueryable();
            var list_title_commend = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();

            var result = await(from c in entity
                               from ce in commendEmployee.Where(x => x.COMMEND_ID == c.ID).DefaultIfEmpty() 
                               from r in otherList.Where(x => x.ID == c.REWARD_ID).DefaultIfEmpty()
                               from f in otherList.Where(x => x.ID == c.STATUS_PAYMENT_ID).DefaultIfEmpty()
                               from e in employee.Where(x => x.ID == ce.EMPLOYEE_ID).DefaultIfEmpty()
                               from o in organization.Where(x => x.ID == c.ORG_LEVEL_ID).DefaultIfEmpty()
                               from rLevel in orgLevel.Where(x => x.ID == c.REWARD_LEVEL_ID).DefaultIfEmpty()
                               from reference_1 in list_title_commend.Where(x => x.ID == c.AWARD_TITLE_ID).DefaultIfEmpty()
                               where e.ID == employeeId && f.CODE == "DD"
                               select new HuCommendDTO()
                               {
                                   Id = c.ID,
                                   EmployeeId = employeeId,
                                   No = c.NO,
                                   SignDate = c.SIGN_DATE,
                                   CommendType = c.COMMEND_TYPE,
                                   Reason = c.REASON,
                                   Money = c.MONEY,
                                   EffectDate = c.EFFECT_DATE,
                                   RewardName = r.NAME,
                                   OrgLevelId = c.ORG_LEVEL_ID,
                                   OrgLevelName = o.NAME,
                                   Year = c.YEAR,
                                   PaymentNo = c.PAYMENT_NO,
                                   SignPaymentDate = c.SIGN_PAYMENT_DATE,
                                   RewardLevelName = rLevel.NAME,
                                   Note = c.NOTE,
                                   StatusId = c.STATUS_ID,
                                   StatusPaymentName = f.NAME,
                                   PaymentContent = c.PAYMENT_CONTENT,
                                   AwardTitleName = reference_1.NAME
                               }).ToListAsync();
            return Ok(new FormatedResponse() { InnerBody = result });
        }
        [HttpGet]
        public async Task<IActionResult> GetStatusList()
        {
            var result = await (from o in _fullDbContext.SysOtherLists.AsNoTracking().AsQueryable()
                                from t in _fullDbContext.SysOtherListTypes.AsNoTracking().Where(x => x.ID == o.TYPE_ID)
                                where t.CODE == "STATUS"
                                select new
                                {
                                    Id = o.ID,
                                    Name = o.NAME,
                                }).ToListAsync();
            return Ok(new FormatedResponse() { InnerBody = result });
        }
        }
}

