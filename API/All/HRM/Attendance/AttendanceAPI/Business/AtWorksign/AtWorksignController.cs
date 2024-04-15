using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Excel;
using API.DTO;
using API.Main;
using Common.DataAccess;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Data;

namespace API.Controllers.AtWorksign
{
    [ApiExplorerSettings(GroupName = "034-ATTENDANCE-AT_WORKSIGN")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class AtWorksignController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IAtWorksignRepository _AtWorksignRepository;
        private readonly AppSettings _appSettings;
        private readonly FullDbContext _dbContext;
        private readonly IExcelRespository _excelRespsitory;
        private readonly IWebHostEnvironment _env;

        public AtWorksignController(
            FullDbContext dbContext,
            IWebHostEnvironment env,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _AtWorksignRepository = new AtWorksignRepository(dbContext, _uow);
            _appSettings = options.Value;
            _dbContext = dbContext;
            _env = env;
            _excelRespsitory = new ExcelRepository(_dbContext);
        }

        [HttpPost]
        public async Task<IActionResult> GetShiftDefault(AtWorksignDTO param)
        {
            var response = await _AtWorksignRepository.GetShiftDefault(param);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetList(AtWorksignDTO param)
        {
            var response = await _AtWorksignRepository.GetList(param);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentPeriodSalary()
        {
            var response = await _AtWorksignRepository.GetCurrentPeriodSalary();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _AtWorksignRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _AtWorksignRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _AtWorksignRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<AtWorksignDTO> request)
        {
            var response = await _AtWorksignRepository.SinglePhaseQueryList(request);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _AtWorksignRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _AtWorksignRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AtWorksignDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtWorksignRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<AtWorksignDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtWorksignRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AtWorksignDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtWorksignRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<AtWorksignDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtWorksignRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AtWorksignDTO model)
        {
            if (model.Id != null)
            {
                var response = await _AtWorksignRepository.Delete(_uow, (long)model.Id);
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
            var response = await _AtWorksignRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _AtWorksignRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteWorksigns(AtWorksignDTO model)
        {
            var response = await _AtWorksignRepository.DeleteWorksigns(_uow, model);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetEmployeeInfo(AtWorksignDTO model)
        {
            var response = await _AtWorksignRepository.GetEmployeeInfo(model);
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> ExportTempImportShiftSort([FromBody] ExportDTO dto)
        {
            try
            {
                var QueryData = new SqlQueryDataTemplate(_dbContext);
                DataSet dataSet = new DataSet("MyDataSet");
                dataSet = QueryData.ExecuteStoreToTable("PKG_GET_DATA_SHIFT_SORT_IMPORT", new
                {
                    P_CURENT_USER_ID = _dbContext.CurrentUserId,
                    P_PERIOD_ID = dto.periodId,
                    P_IS_ADMIN = _dbContext.IsAdmin == true ? 1 : 0,
                    P_ORG_ID = dto.lstOrg![0],
                    P_ISDISSOLVE = 1
                }, false);
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.ExcelTemplates);
                var relativePath = "Template_ImportShift.xls";
                var absolutePath = Path.Combine(location, relativePath);
                var file = await _excelRespsitory.ExportTempImportShiftSort(absolutePath, dataSet, dto.periodId);
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
        public async Task<IActionResult> ImportShiftSort([FromBody] ImportDTO dto)
        {

            string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.ExcelTemplates);
            var relativePath = "Template_ImportShift_error.xls";
            var absolutePath = Path.Combine(location, relativePath);

            var isValid = await _excelRespsitory.CheckValidImportShiftSort(absolutePath, dto.base64, dto.periodId);
            if (isValid != null)
            {

                if (isValid != new byte[0])
                {
                    return File(isValid, "application/octet-stream", "Error_ImportShiftSort.xlsx");
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
                return Ok(await _excelRespsitory.ImportShiftSort(dto.base64, dto.periodId));
            }
        }
    }
}

