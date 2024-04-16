using API.DTO;
using Microsoft.AspNetCore.Mvc;
using CORE.GenericUOW;
using API.All.DbContexts;
using Microsoft.Extensions.Options;
using API;
using CORE.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.Main;
using CORE.Services.File;
using ProfileDAL.ViewModels;
using InsuranceDAL.Repositories;
using Common.Extensions;

namespace InsuranceAPI.Business
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "004-INSURANCE-INS-ARISING")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class InsArisingController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly GenericReducer<INS_ARISING, InsArisingDTO> genericReducer;
        private IGenericRepository<INS_ARISING, InsArisingDTO> _genericRepository;
        private IInsArisingRepository _insArisingRepository;
        private readonly CoreDbContext _coreDbContext;
        private AppSettings _appSettings;

        public InsArisingController(CoreDbContext coreDbContext, IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(coreDbContext);
            genericReducer = new();
            _coreDbContext = coreDbContext;
            _genericRepository = _uow.GenericRepository<INS_ARISING, InsArisingDTO>();
            _insArisingRepository = new InsArisingRepository(coreDbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<InsArisingDTO> request)
        {
            try
            {
                var entity = _uow.Context.Set<INS_ARISING>().AsNoTracking().AsQueryable();
                var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var job = _uow.Context.Set<HU_JOB>().AsNoTracking().AsQueryable();
                var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
                var companies = _uow.Context.Set<HU_COMPANY>().AsNoTracking().AsQueryable();
                var otherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var information = _uow.Context.Set<INS_INFORMATION>().AsNoTracking().AsQueryable();
                var working = _uow.Context.Set<HU_WORKING>().AsNoTracking().AsQueryable();
                var region = _uow.Context.Set<INS_REGION>().AsNoTracking().AsQueryable();
                var specified = _uow.Context.Set<INS_SPECIFIED_OBJECTS>().AsNoTracking().AsQueryable();

                var joined = from p in entity
                             from e in employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                             from t in positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                             from j in job.Where(x => x.ID == t.JOB_ID).DefaultIfEmpty()
                             from o in organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                             from c in companies.Where(x => x.ID == o.COMPANY_ID).DefaultIfEmpty()
                             from i in otherLists.Where(x => x.ID == p.INS_ORG_ID).DefaultIfEmpty()
                             from inf in information.Where(x => x.ID == p.INS_INFORMATION_ID).DefaultIfEmpty()
                             from spec in specified.Where(x => x.ID == p.INS_SPECIFIED_ID).DefaultIfEmpty()
                             from r in region.Where(x => x.AREA_ID == c.REGION_ID && (x.EFFECT_DATE <= p.EFFECT_DATE || (x.EFFECT_DATE <= p.EFFECT_DATE && p.EFFECT_DATE <= x.EXPRIVED_DATE))).OrderByDescending(x => x.EFFECT_DATE).Take(1).DefaultIfEmpty()
                             where p.STATUS == 0
                             //orderby p.CREATED_DATE ascending
                             select new InsArisingDTO
                             {
                                 Id = p.ID,
                                 EmployeeName = e.Profile.FULL_NAME,
                                 EmployeeCode = e.CODE,
                                 JobOrderNum = (int)(j.ORDERNUM ?? 999),
                                 PositionName = t.NAME,
                                 OrgId = e.ORG_ID,
                                 OrgName = o.NAME,
                                 EffectDate = p.EFFECT_DATE,
                                 DeclaredDate = p.DECLARED_DATE,

                                 NewSal = (p.NEW_SAL == null ? 0 : (float)p.NEW_SAL!) >= (spec.SI_HI == null ? 0 : (float)spec.SI_HI) ? (spec.SI_HI == null ? 0 : (float)spec.SI_HI) : (p.NEW_SAL == null ? 0 : (float)p.NEW_SAL!),
                                 OldSal = (p.OLD_SAL == null ? 0 : (float)p.OLD_SAL!) >= (spec.SI_HI == null ? 0 : (float)spec.SI_HI) ? (spec.SI_HI == null ? 0 : (float)spec.SI_HI) : (p.OLD_SAL == null ? 0 : (float)p.OLD_SAL!),
                                 InsOrgId = i.ID == null ? 0 : i.ID,
                                 InsOrgName = i.NAME,
                                 InsGroupType = p.INS_GROUP_TYPE,
                                 InsGroupTypeName = p.INS_GROUP_TYPE == 1 ? "Tăng" : p.INS_GROUP_TYPE == 2 ? "Giảm" : "Điều chỉnh",
                                 Hi = p.HI,
                                 Ai = p.AI,
                                 Si = p.SI,
                                 Ui = p.UI,
                                 CreatedBy = p.CREATED_BY,
                                 UpdatedBy = p.UPDATED_BY,
                                 CreatedDate = p.CREATED_DATE,
                                 UpdatedDate = p.UPDATED_DATE,
                                 InsNo = e.Profile!.INSURENCE_NUMBER,

                                 NewInsSal = (p.NEW_SAL == null ? 0 : (float)p.NEW_SAL!) < (r.CEILING_UI == null ? 0 : (float)r.CEILING_UI) ? (p.NEW_SAL == null ? 0 : (float)p.NEW_SAL!) : (r.CEILING_UI == null ? 0 : (float)r.CEILING_UI),
                                 //NewInsSal = (p.NEW_SAL == null ? 0 : (float)p.NEW_SAL) > (spec.UI == null ? 0 : (float)spec.UI) ? (spec.UI == null ? 0 : (float)spec.UI) : (p.NEW_SAL == null ? 0 : (float)p.NEW_SAL),
                                 OldInsSal = (p.OLD_SAL == null ? 0 : (float)p.OLD_SAL!) < (r.CEILING_UI == null ? 0 : (float)r.CEILING_UI) ? (p.OLD_SAL == null ? 0 : (float)p.OLD_SAL!) : (r.CEILING_UI == null ? 0 : (float)r.CEILING_UI),
                                 //OldInsSal = (p.OLD_SAL == null ? 0 : (float)p.OLD_SAL) > (spec.UI == null ? 0 : (float)spec.UI) ? (spec.UI == null ? 0 : (float)spec.UI) : (p.OLD_SAL == null ? 0 : (float)p.OLD_SAL),
                                 Reasons = p.REASONS
                             };

                //NewInsSal = w.SAL_INSU > r.MONEY * 20 ?(float) r.MONEY * 20 : (float)w.SAL_INSU,
                // OldInsSal = w2.SAL_INSU > r.MONEY * 20 ? (float)r.MONEY * 20 : (float)w2.SAL_INSU,
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

        [HttpGet]
        public async Task<IActionResult> GetInsTypeList()
        {
            try
            {
                var entity = _uow.Context.Set<INS_TYPE>().AsNoTracking().AsQueryable();
                var joined = await (from p in entity
                                    orderby p.NAME
                                    select new InsTypeDTO
                                    {
                                        Id = p.ID,
                                        Name = p.NAME,
                                        CreatedBy = p.CREATED_BY,
                                        UpdatedBy = p.UPDATED_BY,
                                        CreatedDate = p.CREATED_DATE,
                                        UpdatedDate = p.UPDATED_DATE
                                    }).ToListAsync();
                var response = new FormatedResponse() { InnerBody = joined };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetInsTypeById(long id)
        {
            try
            {
                var entity = _uow.Context.Set<INS_TYPE>().AsNoTracking().AsQueryable();
                var joined = await (from p in entity
                                    where p.ID == id && p.IS_ACTIVE == true
                                    orderby p.NAME
                                    select new
                                    {
                                        Id = p.ID,
                                        Name = p.NAME,
                                        CreatedBy = p.CREATED_BY,
                                        UpdatedBy = p.UPDATED_BY,
                                        CreatedDate = p.CREATED_DATE,
                                        UpdatedDate = p.UPDATED_DATE
                                    }).FirstOrDefaultAsync();
                var response = new FormatedResponse() { InnerBody = joined };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var entity = _uow.Context.Set<INS_ARISING>().AsNoTracking().AsQueryable();
                var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
                var companies = _uow.Context.Set<HU_COMPANY>().AsNoTracking().AsQueryable();
                var otherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var joined = (from p in entity
                              from e in employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                              from t in positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                              from o in organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                              from c in companies.Where(x => x.ID == o.COMPANY_ID).DefaultIfEmpty()
                              from i in otherLists.Where(x => x.ID == c.INS_UNIT).DefaultIfEmpty()
                              where p.ID == id
                              orderby p.CREATED_DATE descending
                              select new InsArisingDTO
                              {
                                  Id = p.ID,
                                  EmployeeName = e.Profile.FULL_NAME,
                                  EmployeeCode = e.CODE,
                                  PositionName = t.NAME,
                                  OrgId = e.ORG_ID,
                                  OrgName = o.NAME,
                                  InsOrgId = i.ID,
                                  InsOrgName = i.NAME,
                                  InsGroupType = p.INS_GROUP_TYPE,
                                  InsGroupTypeName = p.INS_GROUP_TYPE == 1 ? "Tăng" : p.INS_GROUP_TYPE == 2 ? "Giảm" : "Điều chỉnh",
                                  Hi = p.HI,
                                  Ai = p.AI,
                                  Si = p.SI,
                                  Ui = p.UI,
                                  CreatedBy = p.CREATED_BY,
                                  UpdatedBy = p.UPDATED_BY,
                                  CreatedDate = p.CREATED_DATE,
                                  UpdatedDate = p.UPDATED_DATE
                              }).FirstOrDefault();
                var response = new FormatedResponse() { InnerBody = joined };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(InsArisingDTO model)
        {
            var sid = Request.Sid(_appSettings);
            try
            {
                var r = await _insArisingRepository.Create(_uow, model, sid);
                return Ok(r);

            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update(InsArisingDTO model)
        {
            var sid = Request.Sid(_appSettings);
            try
            {
                var r = await _insArisingRepository.Create(_uow, model, sid);
                return Ok(r);

            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            try
            {
                if (model.Ids != null)
                {
                    foreach (var id in model.Ids)
                    {
                        var entity = (from p in _coreDbContext.Arisings where p.ID == id select p).FirstOrDefault();
                        _coreDbContext.RemoveRange(entity);
                    }
                    await _coreDbContext.SaveChangesAsync();
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.DELETE_SUCCESS });
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
                }
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
    }
}
