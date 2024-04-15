using Microsoft.AspNetCore.Mvc;
using AttendanceDAL.Repositories;
using AttendanceDAL.ViewModels;
using CORE.GenericUOW;
using API.All.DbContexts;
using CORE.DTO;
using CORE.Enum;
using CORE.StaticConstant;

namespace AttendanceAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "003-ATTENDANCE-LIST-SALARY-PERIOD")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class SalaryPeriodController : BaseController2
    {
        private readonly GenericUnitOfWork _uow;
        private IGenericRepository<AT_SALARY_PERIOD, SalaryPeriodInputDTO> _genericRepository;
        private readonly GenericReducer<AT_SALARY_PERIOD, SalaryPeriodInputDTO> genericReducer;
        public SalaryPeriodController(IAttendanceUnitOfWork unitOfWork, CoreDbContext coreDbContext) : base(unitOfWork)
        {
            _uow = new GenericUnitOfWork(coreDbContext);
            _genericRepository = _uow.GenericRepository<AT_SALARY_PERIOD, SalaryPeriodInputDTO>();
            genericReducer = new();
        }
        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<SalaryPeriodDTO> request)
        {
            try
            {
                /* HERE WE CAN EITHER CHOOSE ONE OF THE 2 APPROACHS: TwoPhaseQueryList or SinglePhaseQueryList */
                /* WE WILL KNOW WHAT APPROACH IS FASTER IN THE FUTURE */

                var response = await _unitOfWork.SalaryPeriodRepository.TwoPhaseQueryList(request);

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
        public async Task<ActionResult> GetAll(SalaryPeriodDTO param)
        {
            var r = await _unitOfWork.SalaryPeriodRepository.GetAll(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetById(long Id)
        {
            var r = await _unitOfWork.SalaryPeriodRepository.GetById(Id);
            return ResponseResult(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetList(int? Id)
        {
            var r = await _unitOfWork.SalaryPeriodRepository.GetList(Id);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] SalaryPeriodInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.SalaryPeriodRepository.CreateAsync(param);
            return ResponseResult(r);
        }
        [HttpPost]
        public async Task<ActionResult> Update([FromBody] SalaryPeriodInputDTO param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.SalaryPeriodRepository.UpdateAsync(param);
            return ResponseResult(r);

        }

        /// <summary>
        /// Portal
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetYear()
        {
            var r = await _unitOfWork.SalaryPeriodRepository.GetYear();
            return ResponseResult(r);
        }
        /// <summary>
        /// Portal
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> PortalGetYear()
        {
            var r = await _unitOfWork.SalaryPeriodRepository.PortalGetYear();
            return ResponseResult(r);
        }

        /// <summary>
        /// PortalByYear
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> PortalByYear(int year)
        {
            var r = await _unitOfWork.SalaryPeriodRepository.PortalByYear(year);
            return ResponseResult(r);
        }

    }
}
