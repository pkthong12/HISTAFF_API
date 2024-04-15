using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Excel;
using API.All.SYSTEM.CoreAPI.Word;
using API.DTO;
using API.Main;
using Aspose.Cells;
using Common.DataAccess;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data;

namespace API.Controllers.PaSalImport
{
    [ApiExplorerSettings(GroupName = "142-PAYROLL-PA_SAL_IMPORT")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class PaSalImportController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IPaSalImportRepository _PaSalImportRepository;
        private readonly AppSettings _appSettings;
        private readonly FullDbContext _dbContext;
        private readonly IWebHostEnvironment _env;
        private readonly IExcelRespository _excelRespsitory;
        public PaSalImportController(
            FullDbContext dbContext,
            IWebHostEnvironment env,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _PaSalImportRepository = new PaSalImportRepository(dbContext, _uow);
            _appSettings = options.Value;
            _dbContext = dbContext;
            _env = env;
            _excelRespsitory = new ExcelRepository(_dbContext);
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _PaSalImportRepository.ReadAll();
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _PaSalImportRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _PaSalImportRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<PaSalImportDTO> request)
        {
            try
            {
                var response = await _PaSalImportRepository.SinglePhaseQueryList(request);

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
            var response = await _PaSalImportRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _PaSalImportRepository.GetById(id);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaSalImportDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaSalImportRepository.Create(_uow, model, sid);
            return Ok(new FormatedResponse() { InnerBody = response });
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<PaSalImportDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaSalImportRepository.CreateRange(_uow, models, sid);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Update(PaSalImportDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaSalImportRepository.Update(_uow, model, sid);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<PaSalImportDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _PaSalImportRepository.UpdateRange(_uow, models, sid);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PaSalImportDTO model)
        {
            if (model.Id != null)
            {
                var response = await _PaSalImportRepository.Delete(_uow, (long)model.Id);
                return Ok(new FormatedResponse() { InnerBody = response });
            }
            else
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            var response = await _PaSalImportRepository.DeleteIds(_uow, model.Ids);
            return Ok(new FormatedResponse() { InnerBody = response });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _PaSalImportRepository.DeleteIds(_uow, model.Ids);
            return Ok(new FormatedResponse() { InnerBody = response });
        }
        [HttpGet]
        public async Task<IActionResult> GetListSalaries(long id)
        {
            var response = await _PaSalImportRepository.GetListSalaries(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ExportTempImportSalary([FromBody] ExportDTO dto)
        {
            try
            {
                var QueryData = new SqlQueryDataTemplate(_dbContext);
                DataSet dataSet = new DataSet("MyDataSet");
                string orgids = "," + string.Join(",", dto.lstOrg) + ",";
                dataSet = QueryData.ExecuteStoreToTable("PKG_GET_SALARY_IMPORT", new
                {
                    P_PERIOD_ID = dto.periodId,
                    P_ORG_IDS = "," + orgids + ",",
                    P_OBJ_SAL_ID = dto.salObjId,
                    P_EMPLOYEE_ID = dto.employeeId ?? 0
                }, false);
                var response = await (from p in _dbContext.PaListsalariess.AsNoTracking()
                                      from c in _dbContext.PaListSals.AsNoTracking().Where(c => c.ID == p.CODE_SAL).DefaultIfEmpty()
                                      where dto.lstRankCode.Contains(p.ID)
                                      select new
                                      {
                                          Id = p.ID,
                                          Name = c.CODE_LISTSAL + " : " + p.NAME,
                                          Code = c.CODE_LISTSAL,
                                      }).ToListAsync();


                System.Data.DataTable dtColName = new System.Data.DataTable();
                dtColName.Columns.Add("COLVAL");
                dtColName.Columns.Add("COLNAME");
                dtColName.Columns.Add("COLDATA");
                foreach (var item in response)
                {
                    DataRow row = dtColName.NewRow();
                    row["COLVAL"] = item.Code;
                    row["COLNAME"] = item.Name.Split(':')[1].Trim();
                    row["COLDATA"] = "&=Table." + item.Code;
                    dtColName.Rows.Add(row);
                }
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.ExcelTemplates);

                var relativePath = "TEMP_IMPORT_SALARY.xlsx";
                var absolutePath = Path.Combine(location, relativePath);
                var file = await _excelRespsitory.ExportTempImportSalary(absolutePath, dataSet, dtColName);
                return File(file, "application/octet-stream", relativePath);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    MessageCode = ex.Message,
                    StatusCode = EnumStatusCode.StatusCode400,
                });
            }

        }

        [HttpPost]
        public async Task<IActionResult> ImportTempSalary([FromBody] ImportDTO dto)
        {

            // check valid file
            var response = await (from p in _dbContext.PaListsalariess.AsNoTracking()
                                  from c in _dbContext.PaListSals.AsNoTracking().Where(c => c.ID == p.CODE_SAL).DefaultIfEmpty()
                                  where dto.lstColVal.Contains(p.ID)
                                  select c.CODE_LISTSAL).ToListAsync();


            var r = await _excelRespsitory.ImportTempSalary(dto.salObj, dto.periodId,0, dto.base64, response, dto.recordSuccess, dto.year, "");
            return Ok(r);
        }
    }
}

