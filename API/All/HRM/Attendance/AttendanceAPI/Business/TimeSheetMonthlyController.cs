using Microsoft.AspNetCore.Mvc;
using AttendanceDAL.Repositories;
using AttendanceDAL.ViewModels;
using CORE.Enum;
using CORE.DTO;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.Services.File;
using CORE.GenericUOW;
using API;
using Microsoft.Extensions.Options;

namespace AttendanceAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "3-ATTENDANCE-TIME-SHEET-MONTHLY")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class TimeSheetMonthlyController : BaseController2
    {
        private readonly GenericUnitOfWork _uow;
        private IGenericRepository<AT_TIMESHEET_MONTHLY, TimeSheetInputDTO> _genericRepository;
        private readonly GenericReducer<AT_TIMESHEET_MONTHLY, TimeSheetMonthlyDTO> genericReducer;
        private readonly IFileService _fileService;
        private AppSettings _appSettings;
        private readonly IWebHostEnvironment _env;
        private AttendanceDbContext _coreDbContext;
        public TimeSheetMonthlyController(IAttendanceUnitOfWork unitOfWork, IOptions<AppSettings> options, IWebHostEnvironment env, IFileService fileService, AttendanceDbContext coreDbContext) : base(unitOfWork)
        {
            _uow = new GenericUnitOfWork(coreDbContext);
            _genericRepository = _uow.GenericRepository<AT_TIMESHEET_MONTHLY, TimeSheetInputDTO>();
            genericReducer = new();
            _appSettings = options.Value;
            _env = env;
            _fileService = fileService;
            _coreDbContext = coreDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<TimeSheetMonthlyDTO> request)
        {
            try
            {
                /* HERE WE CAN EITHER CHOOSE ONE OF THE 2 APPROACHS: TwoPhaseQueryList or SinglePhaseQueryList */
                /* WE WILL KNOW WHAT APPROACH IS FASTER IN THE FUTURE */

                var response = await _unitOfWork.TimeSheetMonthlyRepository.SinglePhaseQueryList(request);

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
        public async Task<ActionResult> GetByEmployeeId(long Id)
        {
            try
            {
                var r = await _unitOfWork.TimeSheetMonthlyRepository.GetByEmployeeId(Id);
                return Ok(new FormatedResponse() { InnerBody = r.Data });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Calculate(TimeSheetInputDTO request)
        {
            try
            {
                var r = await _unitOfWork.TimeSheetMonthlyRepository.Calculate(request);
                if (r.StatusCode == "200")
                {
                    return Ok(new FormatedResponse() { InnerBody = r.Data, StatusCode = EnumStatusCode.StatusCode200 });
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = r.Error, StatusCode = EnumStatusCode.StatusCode400 });
                }

            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Lock(TimeSheetInputDTO request)
        {
            try
            {
                var r = await _unitOfWork.TimeSheetMonthlyRepository.Lock(request);
                if (r.StatusCode == "200")
                {
                    return Ok(new FormatedResponse() { InnerBody = r.Data, StatusCode = EnumStatusCode.StatusCode200 });
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = r.Error, StatusCode = EnumStatusCode.StatusCode400 });
                }

            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> CheckLock(LockDataInput request)
        {
            try
            {
                var r = await _unitOfWork.TimeSheetMonthlyRepository.CheckLock(request);
                if (r.StatusCode == "200")
                {
                    return Ok(new FormatedResponse() { InnerBody = r.Data, StatusCode = EnumStatusCode.StatusCode200 });
                }
                else
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = r.Error, StatusCode = EnumStatusCode.StatusCode400 });
                }

            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpPost]
        public async Task<ActionResult> ImportSwipeMachine([FromBody] List<SwipeDataInput> param)
        {
            if (!ModelState.IsValid)
            {
                return ResponseValidation();
            }
            var r = await _unitOfWork.TimeSheetMonthlyRepository.ImportSwipeMachine(param);
            return ImportResult(r);
            
        }

        //[HttpGet]
        //public async Task<ActionResult> ListTimeSheetMonthly(TimeSheetMonthlyDTO param)
        //{
        //    var r = await _unitOfWork.TimeSheetMonthlyRepository.ListTimeSheetMonthly(param);
        //    return Ok(r);
        //}

        //[HttpGet]
        //public async Task<ActionResult> GetListFormula(TimeSheetFomulaDTO param)
        //{
        //    var r = await _unitOfWork.TimeSheetMonthlyRepository.GetListFormula(param);
        //    return Ok(r);
        //}
        //[HttpPost]
        //public async Task<ActionResult> UpdateFormula([FromBody] TimeSheetFomulaInputDTO param)
        //{
        //    var r = await _unitOfWork.TimeSheetMonthlyRepository.UpdateFormula(param);
        //    return Ok(r);
        //}


        //[HttpPost]
        //public async Task<ActionResult> SumWork([FromBody] TimeSheetInputDTO param)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return ResponseValidation();
        //    }
        //    var r = await _unitOfWork.TimeSheetMonthlyRepository.SumWork(param);
        //    return ResponseResult(r);
        //}
        //[HttpPost]
        //public async Task<ActionResult> ImportSwipeData([FromBody] List<SwipeDataInput> param)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return ResponseValidation();
        //    }
        //    var r = await _unitOfWork.TimeSheetMonthlyRepository.ImportSwipeData(param);
        //    return ResponseResult(r);
        //}

        //[HttpPost]
        //public async Task<ActionResult> ImportSwipeDataNew([FromBody] SwipeImportnput param)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return ResponseValidation();
        //    }
        //    var r = await _unitOfWork.TimeSheetMonthlyRepository.ImportSwipeDataNew(param);
        //    return ResponseResult(r);
        //}

        //[HttpGet]
        //public async Task<ActionResult> ListSwipeData(SwipeDataDTO param)
        //{
        //    var r = await _unitOfWork.TimeSheetMonthlyRepository.ListSwipeData(param);
        //    return Ok(r.Data);
        //}

        //[HttpGet]
        //public async Task<ActionResult> LockTimeSheet(TimeSheetLockInputDTO param)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return ResponseValidation();
        //    }
        //    var r = await _unitOfWork.TimeSheetMonthlyRepository.LockTimeSheet(param);
        //    return Ok(r);
        //}
        //[HttpGet]
        //public async Task<ActionResult> IsLockTimeSheet(TimeSheetLockInputDTO param)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return ResponseValidation();
        //    }
        //    var r = await _unitOfWork.TimeSheetMonthlyRepository.IsLockTimeSheet(param);
        //    return Ok(r);
        //}

        //[HttpGet]
        //public async Task<ActionResult> PortalGetBY(int periodId)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return ResponseValidation();
        //    }
        //    try
        //    {
        //        var r = await _unitOfWork.TimeSheetMonthlyRepository.PortalGetBY(periodId);
        //        return ResponseResult(r);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Ok(ex.Message);
        //    }

        //}

        //[HttpPost]
        //public async Task<ActionResult> UpdateTimeSheetMachine([FromBody] MaChineInput param)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return ResponseValidation();
        //    }
        //    try
        //    {
        //        var r = await _unitOfWork.TimeSheetMonthlyRepository.UpdateTimeSheetMachine(param);
        //        return ResponseResult(r);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Ok(ex.Message);
        //    }

        //}
        //[HttpGet]
        //public async Task<ActionResult> ListEntitlement(EntitlementDTO param)
        //{
        //    var r = await _unitOfWork.TimeSheetMonthlyRepository.ListEntitlement(param);
        //    return Ok(r);
        //}

        //[HttpGet]
        //public async Task<ActionResult> ReportSwipeData(SwipeDataReport param)
        //{
        //    var r = await _unitOfWork.TimeSheetMonthlyRepository.ReportSwipeData(param);
        //    return Ok(r);
        //}

        //[HttpPost]
        //public async Task<ActionResult> ReportSwipeDataExp([FromBody] SwipeDataReport param)
        //{
        //    try
        //    {
        //        var stream = await _unitOfWork.TimeSheetMonthlyRepository.ReportSwipeDataExp(param);
        //        var fileName = "BaoCaoChamCong.xlsx";
        //        if (stream.StatusCode == "200")
        //        {
        //            return new FileStreamResult(stream.memoryStream, "application/octet-stream") { FileDownloadName = fileName };
        //        }
        //        return ResponseResult(stream);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ResponseResult(ex.Message);
        //    }
        //}


        //[HttpPost]
        //public async Task<ActionResult> CalEntitlement([FromBody] TimeSheetInputDTO param)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return ResponseValidation();
        //    }
        //    var r = await _unitOfWork.TimeSheetMonthlyRepository.CalEntitlement(param);
        //    return ResponseResult(r);
        //}

        //[HttpPost]
        //public async Task<ActionResult> ReadMCC([FromBody] TimeSheetInputDTO param)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return ResponseValidation();
        //    }
        //    var r = await _unitOfWork.TimeSheetMonthlyRepository.ReadMCC(param);
        //    return ResponseResult(r);
        //}

    }
}
