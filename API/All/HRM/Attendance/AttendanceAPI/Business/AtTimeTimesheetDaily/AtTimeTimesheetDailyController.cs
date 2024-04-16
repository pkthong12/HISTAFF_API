using API.All.DbContexts;
using API.All.HRM.Attendance.AttendanceAPI.Business.AtTimeTimesheetDaily;
using API.All.SYSTEM.CoreAPI.Excel;
using API.DTO;
using API.Main;
using AttendanceDAL.ViewModels;
using Common.DataAccess;
using Common.Extensions;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Data;

namespace API.Controllers.AtTimeTimesheetDaily
{
    [ApiExplorerSettings(GroupName = "033-ATTENDANCE-AT_TIME_TIMESHEET_DAILY")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class AtTimeTimesheetDailyController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IAtTimeTimesheetDailyRepository _AtTimeTimesheetDailyRepository;
        private readonly AppSettings _appSettings;
        private readonly IExcelRespository _excelRespsitory;
        private readonly FullDbContext _dbContext;
        private readonly IWebHostEnvironment _env;



        public AtTimeTimesheetDailyController(
            FullDbContext dbContext,
            IWebHostEnvironment env,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _AtTimeTimesheetDailyRepository = new AtTimeTimesheetDailyRepository(dbContext, _uow);
            _appSettings = options.Value;
            _dbContext = dbContext;
            _excelRespsitory = new ExcelRepository(_dbContext);
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _AtTimeTimesheetDailyRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _AtTimeTimesheetDailyRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _AtTimeTimesheetDailyRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<AtTimeTimesheetDailyDTO> request)
        {
            try
            {
                /* HERE WE CAN EITHER CHOOSE ONE OF THE 2 APPROACHS: TwoPhaseQueryList or SinglePhaseQueryList */
                /* WE WILL KNOW WHAT APPROACH IS FASTER IN THE FUTURE */

                var response = await _AtTimeTimesheetDailyRepository.SinglePhaseQueryList(request);

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
                    /*return Ok(new FormatedResponse()
                    {
                        InnerBody = response
                    });*/

                    return Ok(
                        response
                    );
                }
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _AtTimeTimesheetDailyRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByImportEdit(long id)
        {
            var response = await _AtTimeTimesheetDailyRepository.GetByImportEdit(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateImportEdit(AtTimesheetDailyImportDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtTimeTimesheetDailyRepository.UpdateImportEdit(_uow, model, sid);
            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _AtTimeTimesheetDailyRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AtTimeTimesheetDailyDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtTimeTimesheetDailyRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<AtTimeTimesheetDailyDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtTimeTimesheetDailyRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AtTimeTimesheetDailyDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtTimeTimesheetDailyRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<AtTimeTimesheetDailyDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtTimeTimesheetDailyRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AtTimeTimesheetDailyDTO model)
        {
            if (model.Id != null)
            {
                var response = await _AtTimeTimesheetDailyRepository.Delete(_uow, (long)model.Id);
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
            var response = await _AtTimeTimesheetDailyRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _AtTimeTimesheetDailyRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetListTimeSheet(AtTimeTimesheetDailyDTO model)
        {
            var response = await _AtTimeTimesheetDailyRepository.GetListTimeSheet(model, _appSettings);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Calculate(AtTimeTimesheetDailyDTO request)
        {
            try
            {
                var r = await _AtTimeTimesheetDailyRepository.Calculate(request);
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
        public async Task<IActionResult> Confirm(AtTimeTimesheetDailyDTO request)
        {
            try
            {
                var r = await _AtTimeTimesheetDailyRepository.Confirm(request);
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
        public async Task<IActionResult> ExportTempImportTimeSheet(ExportDTO dto)
        {
            try
            {
                var QueryData = new SqlQueryDataTemplate(_dbContext);
                DataSet dataSet = new DataSet("MyDataSet");
                string orgids = "," + string.Join(",", dto.lstOrg) + ",";

                dataSet = QueryData.ExecuteStoreToTable("PKG_GET_DATA_TIMESHEET_IMPORT", new
                {
                    P_PERIOD_ID = dto.periodId ?? 0,
                    P_ORG_ID = orgids,
                    P_PAGE_INDEX = 0,
                    P_PAGE_SIZE = 1000,
                    P_EMPLOYEE_ID = dto.employeeId ?? 0,
                    P_COLOR = 0
                }, false);


                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.ExcelTemplates);
                var relativePath = "Template_importTimesheet_CTT.xls";
                var absolutePath = Path.Combine(location, relativePath);
                var file = await _excelRespsitory.ExportTempImportTimeSheet(absolutePath, dataSet, dto.periodId);
                return File(file, "application/octet-stream", relativePath);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse()
                {
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    MessageCode = ex.Message,
                    StatusCode = EnumStatusCode.StatusCode400,
                });
            }

        }

        [HttpPost]
        public async Task<IActionResult> ImportTimeSheetDaily([FromBody] ImportDTO dto)
        {

            string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.ExcelTemplates);
            var relativePath = "Template_importTimesheet_CTT_error.xls";
            var absolutePath = Path.Combine(location, relativePath);

            var isValid = await _excelRespsitory.CheckValidImportTimeSheet(absolutePath, dto.base64, dto.periodId);
            if (isValid != null)
            {

                if (isValid != new byte[0])
                {
                    return Ok(await _excelRespsitory.ImportTimeSheetDaily(dto.base64, dto.periodId));
                }
                else
                {
                    return File(isValid, "application/octet-stream", "Error_ImportTimeSheet.xlsx");
                }
            }
            else
            {

                return Ok(new FormatedResponse()
                {
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    MessageCode = CommonMessageCode.IMPORT_FAIL,
                    StatusCode = EnumStatusCode.StatusCode400,
                });
            }
        }

    }
}

