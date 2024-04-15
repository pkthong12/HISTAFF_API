using API.All.DbContexts;
using API.DTO;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using ProfileDAL.Repositories;
using ProfileDAL.ViewModels;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "002-PROFILE-ALLOWANCE-EMP")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class HuAllowanceEmpController : BaseController1
    {
        private readonly ProfileDbContext _profileDbContext;
        private readonly GenericUnitOfWork _uow;
        private IGenericRepository<HU_ALLOWANCE_EMP, AllowanceEmpDTO> _genericRepository;
        public HuAllowanceEmpController(IProfileUnitOfWork unitOfWork,ProfileDbContext context) : base(unitOfWork)
        {
             _profileDbContext = context;
            _uow = new GenericUnitOfWork(context);
            _genericRepository = _uow.GenericRepository<HU_ALLOWANCE_EMP, AllowanceEmpDTO>();
        }
        [HttpGet]
        public async Task<ActionResult> GetAll(AllowanceEmpDTO param)
        {
            var r = await _unitOfWork.AllowanceEmpRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetById(int Id)
        {
            var r = await _unitOfWork.AllowanceEmpRepository.GetById(Id);
            return Ok(new FormatedResponse() { InnerBody = r.InnerBody });
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody]AllowanceEmpInputDTO param)
        {
            if (param.DateEnd < param.DateStart)
            {
                return Ok(new FormatedResponse() {MessageCode ="LESS_THAN_OR_EQUAL",
                                                    InnerBody = param,
                                                    StatusCode = EnumStatusCode.StatusCode400 }); 
            }
            if (param.EmployeeIds!.Length <=0)
            {
                return Ok(new FormatedResponse() {MessageCode ="EMPLOYEE_IS_NOTNULL",
                                                    InnerBody = param,
                                                    StatusCode = EnumStatusCode.StatusCode400 }); 
            }
            var r = await _unitOfWork.AllowanceEmpRepository.CreateAsync(param);
            return Ok(r);
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody]AllowanceEmpInputDTO param)
        {
            if (param == null)
            {
                return ResponseResult("PARAM_NOT_BLANK");
            }
            if (param.Id == null)
            {
                return ResponseResult("ID_NOT_BLANK");
            }
           if (param.DateEnd < param.DateStart)
            {
                return Ok(new FormatedResponse() {MessageCode ="LESS_THAN_OR_EQUAL",
                                                    InnerBody = param,
                                                    StatusCode = EnumStatusCode.StatusCode400 }); 
            }
            var r = await _unitOfWork.AllowanceEmpRepository.UpdateAsync(param);
            return Ok(r);
        }
        [HttpPost]
        public async Task<ActionResult> ChangeStatus([FromBody]List<long> ids)
        {
            var r = await _unitOfWork.AllowanceEmpRepository.ChangeStatusAsync(ids);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Remove([FromBody]List<long> ids)
        {
            var r = await _unitOfWork.AllowanceEmpRepository.RemoteAsync(ids);
            return ResponseResult(r);
        }
         [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HuAllowanceEmpDTO> request)
        {
            try
            {

                var response = await _unitOfWork.AllowanceEmpRepository.SinglePhaseQueryList(request);

                if (response.ErrorType != EnumErrorType.NONE)
                {
                    return Ok(new FormatedResponse()
                    {
                        ErrorType = response.ErrorType,
                        MessageCode = response.MessageCode ?? CommonMessageCode.NOT_ALL_CASES_CATCHED,
                        StatusCode = EnumStatusCode.StatusCode400,
                    });
                } else
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
        public async Task<ActionResult> DeleteIds(IdsRequest model)
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

        [HttpGet]
        public async Task<IActionResult> GetListAllowanceEmpByEmployee(long employeeId)
        {
            var allowance = _uow.Context.Set<HU_ALLOWANCE>().AsNoTracking();
            var allowanceEmp = _uow.Context.Set<HU_ALLOWANCE_EMP>().AsNoTracking();
            var employee = _uow.Context.Set<HU_EMPLOYEE>().AsNoTracking().AsQueryable();

            var result = await(from a in allowance
                               from ae in allowanceEmp.Where(c => c.ALLOWANCE_ID == a.ID).DefaultIfEmpty()
                               from e in employee.Where(e => e.ID == ae.EMPLOYEE_ID)
                               where ae.EMPLOYEE_ID == employeeId
                               select new HuAllowanceEmpDTO
                               {
                                   Id = ae.ID,
                                   EmployeeId = employeeId,
                                   AllowanceName = a.NAME,
                                   Coefficient = ae.COEFFICIENT,
                                   Monney = ae.MONNEY,
                                   DateStart = ae.DATE_START,
                                   DateEnd = ae.DATE_END
                               }).ToListAsync();
            return Ok(new FormatedResponse() { InnerBody = result });
        }
        [HttpGet]
        public async Task<ActionResult> GetList()
        {
            var r = await _unitOfWork.AllowanceEmpRepository.GetList();
            return Ok(new FormatedResponse() { InnerBody = r.Data });
        }

    }
}
