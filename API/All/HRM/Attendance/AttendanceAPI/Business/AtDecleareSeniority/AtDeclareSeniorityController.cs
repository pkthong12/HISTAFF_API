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

namespace API.Controllers.AtDecleareSeniority
{
    [ApiExplorerSettings(GroupName = "099-ATTENDANCE-AT_DECLARE_SENIORITY")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class AtDeclareSeniorityController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IAtDeclareSeniorityRepository _AtDeclareSeniorityRepository;
        private readonly AppSettings _appSettings;
        private readonly FullDbContext _dbContext;
        private readonly IExcelRespository _excelRespsitory;
        private readonly IWebHostEnvironment _env;

        public AtDeclareSeniorityController(
            FullDbContext dbContext,
            IOptions<AppSettings> options,
                        IWebHostEnvironment env,
            IFileService fileService)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _AtDeclareSeniorityRepository = new AtDeclareSeniorityRepository(dbContext, _uow, env, options, fileService);
            _appSettings = options.Value;
            _env = env;
            _dbContext = dbContext;
            _excelRespsitory = new ExcelRepository(_dbContext);

        }


        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<AtDeclareSeniorityDTO> request)
        {

            try
            {
                var response = await _AtDeclareSeniorityRepository.SinglePhaseQueryList(request);

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
            var response = await _AtDeclareSeniorityRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AtDeclareSeniorityDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtDeclareSeniorityRepository.Create(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AtDeclareSeniorityDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtDeclareSeniorityRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            var response = await _AtDeclareSeniorityRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> CalculateTotal(long employeeId)
        {
            var response = await _AtDeclareSeniorityRepository.CalculateTotal(employeeId);
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> ExportTempImportBasic(ExportDTO dto)
        {
            try
            {
                var QueryData = new SqlQueryDataTemplate(_dbContext);
                DataSet dataSet = new DataSet("MyDataSet");
                dataSet = QueryData.ExecuteStoreToTable("PKG_GET_DECLARE_SENIORITY_IMPORT", new
                {
                    P_CURENT_USER_ID = _dbContext.CurrentUserId,
                    P_IS_ADMIN = _dbContext.IsAdmin == true ? 1 : 0,
                    P_ORG_ID = dto.lstOrg![0],
                    P_ISDISSOLVE = 1
                }, false);
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.ExcelTemplates);
                var relativePath = "ImportDeclareSeniority.xls";
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
        public async Task<IActionResult> ImportDeclareSeniority(ImportDTO dto)
        {

            string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.ExcelTemplates);
            var relativePath = "ImportDeclareSeniorityError.xls";
            var absolutePath = Path.Combine(location, relativePath);

            var isValid = await _excelRespsitory.CheckValidImportDeclareSeniority(absolutePath, dto.base64);
            if (isValid != null)
            {

                if (isValid != new byte[0])
                {
                    return File(isValid, "application/octet-stream", "Error_DeclareSeniority.xlsx");
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
                return Ok(await _excelRespsitory.ImportDeclareSeniority(dto.base64));
            }
        }

    }
}

