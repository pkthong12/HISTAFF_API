using Microsoft.AspNetCore.Mvc;
using ProfileDAL.ViewModels;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using AttendanceDAL.ViewModels;
using API.All.DbContexts;
using API.Main;
using API;
using Microsoft.Extensions.Options;
using API.DTO;

namespace ProfileAPI.List
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "002-PROFILE-WORKING-BEFORE")]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class HuWorkingBeforeController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly GenericReducer<HU_WORKING_BEFORE, HuWorkingBeforeDTO> genericReducer;
        private IGenericRepository<HU_WORKING_BEFORE, HuWorkingBeforeDTO> _genericRepository;
        private AppSettings _appSettings;
        public HuWorkingBeforeController(CoreDbContext coreDbContext, IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(coreDbContext);
            genericReducer = new();
            _genericRepository = _uow.GenericRepository<HU_WORKING_BEFORE, HuWorkingBeforeDTO>();
            _appSettings = options.Value;
        }
        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuWorkingBeforeDTO> request)
        {

            try
            {
                var entity = _uow.Context.Set<HU_WORKING_BEFORE>().AsNoTracking().AsQueryable();
                var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
                var otherLists = _uow.Context.Set<SYS_OTHER_LIST>().AsNoTracking().AsQueryable();
                var job = _uow.Context.Set<HU_JOB>().AsNoTracking().AsQueryable();
                var joined = from p in entity
                             from e in employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                             from t in positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                             from o in organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                             from j in job.Where(x => x.ID == t.JOB_ID).DefaultIfEmpty()
                             orderby p.CREATED_DATE descending
                             select new HuWorkingBeforeDTO
                             {
                                 Id = p.ID,
                                 EmployeeName = e.Profile.FULL_NAME,
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
                                 CreatedBy = p.CREATED_BY,
                                 UpdatedBy = p.UPDATED_BY,
                                 CreatedDate = p.CREATED_DATE,
                                 UpdatedDate = p.UPDATED_DATE,
                                 JobOrderNum = (int)(j.ORDERNUM ?? 99)
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
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            try
            {
                if (model.Ids != null)
                {
                    var response = await _genericRepository.DeleteIds(_uow, model.Ids);
                    return Ok(response);
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

        [HttpPost]
        public async Task<IActionResult> Delete(HuWorkingBeforeDTO model)
        {
            try
            {
                if (model.Id != null)
                {
                    var response = await _genericRepository.Delete(_uow, (long)model.Id);
                    return Ok(response);
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

        [HttpGet]
        public async Task<IActionResult> GetWorkingBeforeByEmployee(long id)
        {
            try
            {
                var entity = _uow.Context.Set<HU_WORKING_BEFORE>().AsNoTracking().AsQueryable();
                var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
                var joined = await (from p in entity
                                    from e in employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                                    from t in positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                    from o in organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                    where p.EMPLOYEE_ID == id
                                    orderby p.FROM_DATE ascending
                                    select new HuWorkingBeforeDTO()
                                    {
                                        Id = p.ID,
                                        EmployeeId = p.EMPLOYEE_ID,
                                        EmployeeName = e.Profile.FULL_NAME,
                                        EmployeeCode = e.CODE,
                                        PositionName = t.NAME,
                                        OrgName = o.NAME,
                                        FromDate = p.FROM_DATE,
                                        EndDate = p.END_DATE,
                                        MainDuty = p.MAIN_DUTY,
                                        CompanyName = p.COMPANY_NAME,
                                        Seniority = p.SENIORITY,
                                        TitleName = p.TITLE_NAME,
                                        TerReason = p.TER_REASON,
                                        CreatedBy = p.CREATED_BY,
                                        UpdatedBy = p.UPDATED_BY,
                                        CreatedDate = p.CREATED_DATE,
                                        UpdatedDate = p.UPDATED_DATE
                                    }).ToListAsync();

                foreach (var x in joined)
                {
                    var totalDay = x.EndDate.Value.Subtract(x.FromDate.Value).TotalDays;

                    if (Math.Floor(totalDay) > 365)
                    {
                        // kiểm tra số dư
                        int surplus = (int)totalDay % 365;

                        if (surplus == 0)
                        {
                            int seniority = (int)totalDay / 365;

                            x.Seniority = seniority.ToString() + " năm";
                        }
                        else
                        {
                            double quantityYear = Math.Floor(totalDay / 365);

                            double daysLeft = totalDay - (quantityYear * 365);

                            double quantityMonth = Math.Round(daysLeft / 30, 2);

                            x.Seniority = $"{quantityYear} năm {quantityMonth} tháng";
                        }
                    }
                    else if (Math.Floor(totalDay) < 365)
                    {
                        // không làm gì
                    }
                    else
                    {
                        x.Seniority = "1 năm";
                    }
                }

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
                var entity = _uow.Context.Set<HU_WORKING_BEFORE>().AsNoTracking().AsQueryable();
                var employees = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
                var joined = await (from p in entity
                                    from e in employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                                    from t in positions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
                                    from o in organizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
                                    where p.ID == id
                                    select new 
                                    {
                                        Id = p.ID,
                                        EmployeeId = p.EMPLOYEE_ID,
                                        EmployeeName = e.Profile.FULL_NAME,
                                        EmployeeCode = e.CODE,
                                        PositionName = t.NAME,
                                        OrgName = o.NAME,
                                        FromDate = p.FROM_DATE,
                                        EndDate = p.END_DATE,
                                        MainDuty = p.MAIN_DUTY,
                                        CompanyName = p.COMPANY_NAME,
                                        TitleName = p.TITLE_NAME,
                                        TerReason = p.TER_REASON,
                                        CreatedBy = p.CREATED_BY,
                                        UpdatedBy = p.UPDATED_BY,
                                        CreatedDate = p.CREATED_DATE,
                                        UpdatedDate = p.UPDATED_DATE
                                    }).FirstOrDefaultAsync(); ;
                var response = new FormatedResponse() { InnerBody = joined };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(HuWorkingBeforeDTO model)
        {
            try
            {
                if (model.FromDate > model.EndDate)
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_FROM_DATE_MUST_LESS_THAN_EQUAL_TO_DATE" });
                }
                var contractEntity = _uow.Context.Set<HU_CONTRACT>().AsNoTracking().AsQueryable();
                var contract = (from p in contractEntity where p.EMPLOYEE_ID == model.EmployeeId && p.START_DATE <= model.EndDate && p.STATUS_ID == 994 select p).Any();
                if (contract)
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode519, MessageCode = "UI_FORM_CONTROL_ERROR_WORKING_BEFORE_END_DATE_VIOLATION" });
                }

                var fromDate = model.FromDate.Value;
                var endDate = model.EndDate.Value;
                var seniority = Math.Round((endDate.Subtract(fromDate).TotalDays / 365.25) * 12, 2) + " tháng";
                model.Seniority = seniority;

                var sid = Request.Sid(_appSettings);
                HU_WORKING_BEFORE entity = new();
                var response = await _genericRepository.Create(_uow, model, sid ?? "");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(HuWorkingBeforeDTO model)
        {
            try
            {
                if (model.FromDate > model.EndDate)
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = "UI_FORM_CONTROL_ERROR_FROM_DATE_MUST_LESS_THAN_EQUAL_TO_DATE" });
                }
                var contractEntity = _uow.Context.Set<HU_CONTRACT>().AsNoTracking().AsQueryable();
                var contract = (from p in contractEntity where p.EMPLOYEE_ID == model.EmployeeId && p.START_DATE <= model.EndDate && p.STATUS_ID == 994 select p).Any();
                if (contract)
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode519, MessageCode = "UI_FORM_CONTROL_ERROR_WORKING_BEFORE_END_DATE_VIOLATION" });
                }

                var fromDate = model.FromDate.Value;
                var endDate = model.EndDate.Value;
                var seniority = Math.Round((endDate.Subtract(fromDate).TotalDays / 365.25) * 12, 2) + " tháng";
                model.Seniority = seniority;
                var response = await _genericRepository.Update(_uow, model, Request.Sid(_appSettings));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }
    }
}
