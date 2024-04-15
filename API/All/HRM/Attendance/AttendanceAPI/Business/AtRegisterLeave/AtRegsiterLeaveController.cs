using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Excel;
using API.DTO;
using API.Main;
using AttendanceDAL.ViewModels;
using Common.DataAccess;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.Services.File;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data;
using System.Drawing;
using System.Linq;

namespace API.Controllers.AtRegisterLeave
{
    [ApiExplorerSettings(GroupName = "036-ATTENDANCE-AT_REGISTER_LEAVE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class AtRegisterLeaveController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IAtRegisterLeaveRepository _AtRegisterLeaveRepository;
        private readonly AppSettings _appSettings;
        private readonly FullDbContext _dbContext;
        private readonly IExcelRespository _excelRespsitory;
        private readonly IWebHostEnvironment _env;

        public AtRegisterLeaveController(
            FullDbContext dbContext,
            IOptions<AppSettings> options,
                        IWebHostEnvironment env,
            IFileService fileService)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _AtRegisterLeaveRepository = new AtRegisterLeaveRepository(dbContext, _uow, env, options, fileService);
            _appSettings = options.Value;
            _env = env;
            _dbContext = dbContext;
            _excelRespsitory = new ExcelRepository(dbContext);
        }


        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<AtRegisterLeaveDTO> request)
        {

            try
            {
                var response = await _AtRegisterLeaveRepository.SinglePhaseQueryList(request);

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
            var response = await _AtRegisterLeaveRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdVer2(long id)
        {
            var response = await _AtRegisterLeaveRepository.GetByIdVer2(id);
            
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AtRegisterLeaveDTO model)
        {
            if (model.DateStart > model.DateEnd)
            {
                return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.EFFECTIVEDATE_NOT_BIGGER_THAN_EXPIRATIONDATE, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
            if (model.EmployeeIds.Count() == 0)
            {
                return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.NO_ID_PROVIED, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtRegisterLeaveRepository.CreateAsync(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVer2(DynamicDTO model)
        {
            if (DateTime.Parse(model["dateStart"].ToString()) > DateTime.Parse(model["dateEnd"].ToString()))
            {
                return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.EFFECTIVEDATE_NOT_BIGGER_THAN_EXPIRATIONDATE, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtRegisterLeaveRepository.CreateVer2Async(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVer2(DynamicDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtRegisterLeaveRepository.UpdateVer2(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AtRegisterLeaveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtRegisterLeaveRepository.Update(_uow, model, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            var response = await _AtRegisterLeaveRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult> GetListTypeOff()
        {
            try
            {
                var entity = _uow.Context.Set<AT_TIME_TYPE>().AsNoTracking().AsQueryable();
                var joined = from p in entity
                             where p.IS_ACTIVE == true && p.IS_OFF == true
                             orderby p.CREATED_DATE descending
                             select new SymbolDTO
                             {
                                 Id = p.ID,
                                 Code = p.CODE,
                                 Name = "[" + p.CODE + "] " + p.NAME
                             };
                var response = await joined.ToListAsync();
                return Ok(new FormatedResponse()
                {
                    InnerBody = response
                });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetListTypeOffById(long id)
        {
            var entity = _uow.Context.Set<AT_TIME_TYPE>().AsNoTracking().AsQueryable();
            var joined = from p in entity
                         where p.ID == id && p.IS_ACTIVE == true
                         select new SymbolDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Name = "[" + p.CODE + "] " + p.NAME
                         };
            var response = await joined.FirstOrDefaultAsync();
            return Ok(new FormatedResponse()
            {
                InnerBody = response
            });
        }

        [HttpPost]
        public async Task<IActionResult> ExportTempImportBasic(ExportDTO dto)
        {
            try
            {
                var QueryData = new SqlQueryDataTemplate(_dbContext);
                DataSet dataSet = new DataSet("MyDataSet");
                dataSet = QueryData.ExecuteStoreToTable("PKG_GET_DATA_REGISTER_LEAVE", new
                {
                    P_CURENT_USER_ID = _dbContext.CurrentUserId,
                    P_PERIOD_ID = dto.periodId,
                    P_IS_ADMIN = _dbContext.IsAdmin == true ? 1 : 0,
                    P_ORG_ID = dto.lstOrg![0],
                    P_ISDISSOLVE = 1
                }, false);
                string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.ExcelTemplates);
                var relativePath = "AT_IMPORT_REGISTER_CO.xlsx";
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
        public async Task<IActionResult> ImportRegisterLeave(ImportDTO dto)
        {

            string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.ExcelTemplates);
            var relativePath = "AT_IMPORT_REGISTER_CO_Error.xlsx";
            var absolutePath = Path.Combine(location, relativePath);

            var isValid = await _excelRespsitory.CheckValidImportRegisterLeave(absolutePath, dto.base64);
            if (isValid != null)
            {

                if (isValid != new byte[0])
                {
                    return File(isValid, "application/octet-stream", "AT_IMPORT_REGISTER_CO_Error.xlsx");
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
                return Ok(await _excelRespsitory.ImportRegisterLeave(dto.base64));
            }
        }


        [HttpPost]
        public async Task<ActionResult> GetWorkSignName(AtWorksignDTO dto)
        {
            try
            {
                var entity = _uow.Context.Set<AT_WORKSIGN>().AsNoTracking();
                var shift = _uow.Context.Set<AT_SHIFT>().AsNoTracking();

                var joined = await (from p in entity.Where(p =>p.EMPLOYEE_ID == dto.EmployeeId)
                                    from s in shift.Where(x=>x.ID== p.SHIFT_ID).DefaultIfEmpty()
                                    select new
                                    {
                                        id = p.SHIFT_ID,
                                        workingday= p.WORKINGDAY,
                                        name= s.NAME,
                                        code = s.CODE,
                                    }).ToListAsync();

                var response = joined.Where(x => x.workingday.Value.Date == dto.Workingday.Value.Date).ToList().FirstOrDefault();
                return Ok(new FormatedResponse()
                {
                    InnerBody = response
                });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse()
                {
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    StatusCode = EnumStatusCode.StatusCode500,
                    MessageCode = ex.Message
                });
            }
        }
    }
}

