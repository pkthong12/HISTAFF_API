using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Excel;
using API.DTO;
using API.Main;
using Azure;
using Common.DataAccess;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data;

namespace API.All.HRM.Payroll.PayrollAPI.PaSalImportBackdate
{
    [ApiExplorerSettings(GroupName = "991-PAYROLL-PA_SAL_IMPORT_BACKDATE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]

    public class PaSalImportBackdateController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IPaSalImportBackdateRepository _PaSalImportBackdateRepository;
        private readonly AppSettings _appSettings;
        private readonly FullDbContext _dbContext;
        private readonly IExcelRespository _excelRespsitory;
        private readonly IWebHostEnvironment _env;

        public PaSalImportBackdateController(
            FullDbContext dbContext,
                        IWebHostEnvironment env,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _PaSalImportBackdateRepository = new PaSalImportBackdateRepository(dbContext, _uow);
            _appSettings = options.Value;

            // lấy dbContext
            _dbContext = dbContext;
            _excelRespsitory = new ExcelRepository(_dbContext);
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> GetShiftDefault(PaSalImportBackdateDTO param)
        {
            var response = await _PaSalImportBackdateRepository.GetShiftDefault(param);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentPeriodSalary()
        {
            var response = await _PaSalImportBackdateRepository.GetCurrentPeriodSalary();
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<PaSalImportBackdateDTO> request)
        {
            var response = await _PaSalImportBackdateRepository.SinglePhaseQueryList(request);
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

        [HttpPost]
        public async Task<IActionResult> GetEmployeeInfo(PaSalImportBackdateDTO model)
        {
            var response = await _PaSalImportBackdateRepository.GetEmployeeInfo(model);
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
                dataSet = QueryData.ExecuteStoreToTable("PKG_GET_SALARY_IMPORT_BACKDATE", new
                {
                    P_PERIOD_ID = dto.periodId,
                    P_PERIOD_ADD_ID = dto.periodAddId,
                    P_ORG_IDS = "," + orgids + ",",
                    P_OBJ_SAL_ID = dto.salObjId,
                    P_EMPLOYEE_ID = dto.employeeId ?? 0
                }, false);
                var response = await (from p in _dbContext.PaListsalariess.AsNoTracking().Where(p => p.IS_VISIBLE == true)
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
            catch(Exception ex)
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
        public async Task<IActionResult> ImportTempSalary([FromBody] ImportDTO dto)
        {
            var response = await (from p in _dbContext.PaListsalariess.AsNoTracking().Where(p => p.IS_VISIBLE == true)
                                  from c in _dbContext.PaListSals.AsNoTracking().Where(c => c.ID == p.CODE_SAL).DefaultIfEmpty()
                                  where dto.lstColVal.Contains(p.ID)
                                  select c.CODE_LISTSAL).ToListAsync();


            var r = await _excelRespsitory.ImportTempSalary(dto.salObj, dto.periodId,dto.periodAddId, dto.base64, response, dto.recordSuccess, dto.year, "BACK_DATE");
            return Ok(r);
        }

        [HttpPost]
        public async Task<IActionResult> GetListSalaryInYear(AtSalaryPeriodDTO param)
        {
            var response = await _PaSalImportBackdateRepository.GetListSalaryInYear(param);
            return Ok(response);
        }
    }
}