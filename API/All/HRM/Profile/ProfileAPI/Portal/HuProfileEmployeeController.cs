using API;
using API.All.DbContexts;
using API.DTO;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-PORTAL-PROFILE-EMPLOYEE")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuProfileEmployeeController : BaseController1
    {
        private readonly GenericUnitOfWork _uow;
        private IGenericRepository<HU_EMPLOYEE, EmployeeDTO> _genericRepository;
        private readonly AppSettings _appSettings;
        private readonly FullDbContext _fullDbContext;

        private readonly GenericReducer<HU_EMPLOYEE, HuEmployeeDTO> _genericReducer;
        public HuProfileEmployeeController(IProfileUnitOfWork unitOfWork, IOptions<AppSettings> options, FullDbContext fullDbContext) : base(unitOfWork)
        {
            _uow = new GenericUnitOfWork(unitOfWork.DataContext);
            _genericRepository = _uow.GenericRepository<HU_EMPLOYEE, EmployeeDTO>();
            _appSettings = options.Value;
            _fullDbContext = fullDbContext;
            _genericReducer = new();
        }

        //[HttpGet]
        //public async Task<ActionResult> GetEmployeeInfo(EmployeeDTO param)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return ResponseValidation();
        //    }
        //    var r = await _unitOfWork.EmployeeRepository.GetEmployeeInfo(param);
        //    return ResponseResult(r);

        //}

        [HttpGet]
        public async Task<IActionResult> GetInfoEmployee(long id)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                var entity = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();
                var employeeCvs = _uow.Context.Set<HU_EMPLOYEE_CV>().AsNoTracking().AsQueryable();
                var positions = _uow.Context.Set<HU_POSITION>().AsNoTracking().AsQueryable();
                var organizations = _uow.Context.Set<HU_ORGANIZATION>().AsNoTracking().AsQueryable();
                var huJobs = _uow.Context.Set<HU_JOB>().AsNoTracking().AsQueryable();
                var joined = (from p in entity
                                        //from e in employees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                                    from cv in employeeCvs.Where(x => x.ID == p.PROFILE_ID).DefaultIfEmpty()
                                    from t in positions.Where(x => x.ID == p.POSITION_ID).DefaultIfEmpty()
                                    from o in organizations.Where(x => x.ID == p.ORG_ID).DefaultIfEmpty()
                                    from j in huJobs.Where(x => x.ID == t.JOB_ID).DefaultIfEmpty()
                                    from pos in positions.Where(x => x.ID == p.POSITION_ID).DefaultIfEmpty()
                                    //from j in huJobs.Where(x => x.ID == pos.JOB_ID).DefaultIfEmpty()
                                    from di in positions.Where(x => x.ID == pos.LM).DefaultIfEmpty()
                                    from p2 in entity.Where(x => x.POSITION_ID == di.ID && x.ORG_ID == di.ORG_ID && x.ID == di.MASTER).DefaultIfEmpty()
                                    from ep2 in employeeCvs.Where(x => x.ID == p2.PROFILE_ID).DefaultIfEmpty()
                                    from pos2 in positions.Where(x => x.ID == p2.POSITION_ID).DefaultIfEmpty()
                                    from st in huJobs.Where(x => x.ID == pos2.JOB_ID).DefaultIfEmpty()
                                    where p.ID == id
                                    select new //HuEmployeeDTO
                                    {
                                        Id = p.ID,
                                        Code = p.CODE,
                                        FullName = p.Profile.FULL_NAME,
                                        Avatar = p.Profile.AVATAR,
                                        DirectManagerId = p.DIRECT_MANAGER_ID,
                                        PositionId = p.POSITION_ID,
                                        PositionName = j.NAME_VN,
                                        Seniority = p.SENIORITY,
                                        PositionMng = (currentDate - p.JOIN_DATE),
                                        ManagerPositionName = st.NAME_VN,
                                        ManagerName = ep2.FULL_NAME,

                                    }).SingleOrDefault();
                var response = new FormatedResponse() { InnerBody = joined };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

    }
}
