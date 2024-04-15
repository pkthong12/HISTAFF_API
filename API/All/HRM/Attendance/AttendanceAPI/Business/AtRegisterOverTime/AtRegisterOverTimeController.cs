using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Excel;
using API.DTO;
using API.Main;
using Common.DataAccess;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.Services.File;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data;
using System.Globalization;

namespace API.Controllers.AtDecleareSeniority
{
    [ApiExplorerSettings(GroupName = "097-ATTENDANCE-AT_OVER_TIME")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class AtRegisterOverTimeController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IAtRegisterOverTimeRepository _AtOvertimeRepository;
        private readonly AppSettings _appSettings;
        private readonly FullDbContext _dbContext;
        private readonly IExcelRespository _excelRespsitory;
        private readonly IWebHostEnvironment _env;

        public AtRegisterOverTimeController(
            FullDbContext dbContext,
            IOptions<AppSettings> options,
                        IWebHostEnvironment env,
            IFileService fileService)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _AtOvertimeRepository = new AtRegisterOverTimeRepository(dbContext, _uow, env, options, fileService);
            _appSettings = options.Value;
            _env = env;
            _dbContext = dbContext;
            _excelRespsitory = new ExcelRepository(dbContext);
        }


        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<AtOvertimeDTO> request)
        {

            try
            {
                var response = await _AtOvertimeRepository.SinglePhaseQueryList(request);

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
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _AtOvertimeRepository.GetById(id);
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> Create(AtOvertimeDTO model)
        {
            if (model.StartDate!.Value.Date> model.EndDate!.Value.Date)
            {
                return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.EFFECTIVEDATE_NOT_BIGGER_THAN_EXPIRATIONDATE, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 });
            }
            IFormatProvider culture = new CultureInfo("en-US", true);
            model.TimeEnd = DateTime.ParseExact(model.StartDate.Value.ToString("dd/MM/yyyy") + " " + model.TimeEndStr, "dd/MM/yyyy HH:mm", culture);
            model.TimeStart = DateTime.ParseExact(model.StartDate.Value.ToString("dd/MM/yyyy") + " " + model.TimeStartStr, "dd/MM/yyyy HH:mm", culture);

            //if (model.TimeStart > model.TimeEnd)
            //{
            //    return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.TIMEFROM_NOT_BIGGER_THAN_TIMETO, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 });
            //}
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtOvertimeRepository.CreateAsync(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AtOvertimeDTO model)
        {
            if (model.StartDate!.Value.Date > model.EndDate!.Value.Date)
            {
                return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.EFFECTIVEDATE_NOT_BIGGER_THAN_EXPIRATIONDATE, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 });
            }
            IFormatProvider culture = new CultureInfo("en-US", true);
            model.TimeEnd = DateTime.ParseExact(model.StartDate.Value.ToString("dd/MM/yyyy") + " " + model.TimeEndStr, "dd/MM/yyyy HH:mm", culture);
            model.TimeStart = DateTime.ParseExact(model.StartDate.Value.ToString("dd/MM/yyyy") + " " + model.TimeStartStr, "dd/MM/yyyy HH:mm", culture);
            //if (model.TimeStart > model.TimeEnd)
            //{
            //    return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.TIMEFROM_NOT_BIGGER_THAN_TIMETO, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            //}
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtOvertimeRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            var response = await _AtOvertimeRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> ExportTempImportBasic(ExportDTO dto)
        {
            try
            {
                var QueryData = new SqlQueryDataTemplate(_dbContext);
                DataSet dataSet = new DataSet("MyDataSet");
                dataSet = QueryData.ExecuteStoreToTable("PKG_GET_DATA_REGISTER_OT", new
                {
                    P_CURENT_USER_ID = _dbContext.CurrentUserId,
                    P_PERIOD_ID = dto.periodId,
                    P_IS_ADMIN = _dbContext.IsAdmin == true ? 1 : 0,
                    P_ORG_ID = dto.lstOrg![0],
                    P_ISDISSOLVE = 1
                }, false);
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.ExcelTemplates);
                var relativePath = "Template_ImportOT.xls";
                var absolutePath = Path.Combine(location, relativePath);
                var file = await _excelRespsitory.ExportTempImportBasic(absolutePath, dataSet);
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
        public async Task<IActionResult> ImportRegisterOT(ImportDTO dto)
        {

            string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.ExcelTemplates);
            var relativePath = "Template_ImportOT_error.xls";
            var absolutePath = Path.Combine(location, relativePath);

            var isValid = await _excelRespsitory.CheckValidImportRegisterOT(absolutePath, dto.base64);
            if (isValid != null)
            {
                if(isValid!= new byte[0])
                {
                    return File(isValid, "application/octet-stream", "Template_ImportOT_error.xls");
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
            else
            {
                return Ok(await _excelRespsitory.ImportRegisterOT(dto.base64));
            }
        }
    }
}

