using Microsoft.AspNetCore.Mvc;
using ProfileDAL.ViewModels;
using API.All.DbContexts;
using API.Entities;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using API.Main;
using API;
using Microsoft.Extensions.Options;
using AttendanceDAL.ViewModels;
using API.Entities.PORTAL;

namespace ProfileAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "003-ATTENDANCE-LIST-SYMBOL")]
    [ApiController]

    [Route("api/[controller]/[action]")]
    public class AtSymbolController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _coreDbContext;
        private IGenericRepository<AT_SYMBOL, SymbolDTO> _genericRepository;
        private readonly GenericReducer<AT_SYMBOL, SymbolDTO> genericReducer;
        private AppSettings _appSettings;
        public AtSymbolController(IOptions<AppSettings> options, FullDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
            _uow = new GenericUnitOfWork(_coreDbContext);
            _genericRepository = _uow.GenericRepository<AT_SYMBOL, SymbolDTO>();
            genericReducer = new();
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListStringIdDTO<SymbolDTO> request)
        {
            try
            {
                var entity = _uow.Context.Set<AT_SYMBOL>().AsNoTracking().AsQueryable();
                var joined = from p in entity
                             orderby p.CREATED_DATE descending
                             select new SymbolDTO
                             {
                                 Id = p.ID,
                                 Code = p.CODE,
                                 Name = p.NAME,
                                 Note = p.NOTE,
                                 IsHaveSal = p.IS_HAVE_SAL,
                                 IsHolidayCal = p.IS_HOLIDAY_CAL,
                                 IsInsArising = p.IS_INS_ARISING,
                                 IsOff = p.IS_OFF,
                                 IsPortal = p.IS_PORTAL,
                                 IsRegister = p.IS_REGISTER,
                                 IsActive = p.IS_ACTIVE,
                                 WorkingHour = p.WORKING_HOUR,
                                 UpdatedBy = p.UPDATED_BY,
                                 UpdatedDate = p.UPDATED_DATE,
                                 CreateBy = p.CREATED_BY,
                                 CreateDate = p.CREATED_DATE,
                                 Status = (p.IS_ACTIVE.HasValue && p.IS_ACTIVE.Value) ? "Áp dụng" : "Ngừng áp dụng",
                                 IsActiveStr = (p.IS_ACTIVE.HasValue && p.IS_ACTIVE.Value) ? "Áp dụng" : "Ngừng áp dụng",
                             };
                var response = await genericReducer.SinglePhaseReduce(joined, request);
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
            try
            {
                var entity = _uow.Context.Set<AT_SYMBOL>().AsNoTracking().AsQueryable();
                var joined = await (from p in entity
                                    where p.ID == id
                                    select new
                                    {
                                        Id = p.ID,
                                        Code = p.CODE,
                                        Name = p.NAME,
                                        Note = p.NOTE,
                                        IsHaveSal = p.IS_HAVE_SAL,
                                        IsHolidayCal = p.IS_HOLIDAY_CAL,
                                        IsInsArising = p.IS_INS_ARISING,
                                        IsOff = p.IS_OFF,
                                        IsPortal = p.IS_PORTAL,
                                        IsRegister = p.IS_REGISTER,
                                        IsActive = p.IS_ACTIVE,
                                        WorkingHour = p.WORKING_HOUR,
                                        UpdatedBy = p.UPDATED_BY,
                                        UpdatedDate = p.UPDATED_DATE,
                                        CreateBy = p.CREATED_BY,
                                        CreateDate = p.CREATED_DATE,
                                        Status = (p.IS_ACTIVE.HasValue && p.IS_ACTIVE.Value) ? "Áp dụng" : "Ngừng áp dụng"
                                    }).FirstOrDefaultAsync();
                return Ok(new FormatedResponse() { InnerBody = joined });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(SymbolDTO model)
        {
            try
            {
                model.IsActive = true;
                var sid = Request.Sid(_appSettings);
                var response = await _genericRepository.Create(_uow, model, sid ?? "");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(SymbolDTO model)
        {
            try
            {
                var response = await _genericRepository.Update(_uow, model, Request.Sid(_appSettings));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            try
            {
                // Check using
                var symbol = _uow.Context.Set<AT_SYMBOL>().AsNoTracking().AsQueryable();
                var timeType = _uow.Context.Set<AT_TIME_TYPE>().AsNoTracking().AsQueryable();

                var atRegisterLeave = _uow.Context.Set<AT_REGISTER_LEAVE>().AsNoTracking().AsQueryable();
                var portalRegisterOff = _uow.Context.Set<PORTAL_REGISTER_OFF>().AsNoTracking().AsQueryable();
                var timeTimesheetDaily = _uow.Context.Set<AT_TIME_TIMESHEET_DAILY>().AsNoTracking().AsQueryable();

                /*var listTimeTypeIds = from t in timeType
                                      join s in symbol on t.MORNING_ID equals s.ID
                                      where t.AFTERNOON_ID == s.ID && model.Ids.Contains(s.ID)
                                      select t.ID;*/
                var listTimeTypeIds = await (from t in timeType
                                             from s in symbol.Where(x => model.Ids.Contains(x.ID) == true && (x.ID == t.MORNING_ID || x.ID == t.AFTERNOON_ID))
                                             select t.ID).ToListAsync();


                var checkUsingRegiterLeave = await (from l in atRegisterLeave.Where(l => listTimeTypeIds.Contains((long)l.TYPE_ID) == true) select new {l.ID}).CountAsync();
                var checkUsingPortalRegisterOff = await (from l in portalRegisterOff.Where(l => listTimeTypeIds.Contains(l.TIME_TYPE_ID!.Value) == true) select new {l.ID}).CountAsync();
                var checkUsingTimeTimesheetDaily = await (from l in timeTimesheetDaily.Where(l => listTimeTypeIds.Contains(l.MANUAL_ID!.Value) == true) select new { l.ID }).CountAsync();
                var checkIsActive = await symbol.Where(p => model.Ids.Contains(p.ID) && p.IS_ACTIVE == true).CountAsync();
                if(checkIsActive != 0)
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE });
                }
                if (checkUsingRegiterLeave != 0)
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = CommonMessageCode.DATA_HAS_USED });
                }
                if (checkUsingPortalRegisterOff != 0)
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = CommonMessageCode.DATA_HAS_USED });
                }
                if (checkUsingTimeTimesheetDaily != 0)
                {
                    return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = CommonMessageCode.DATA_HAS_USED });
                }
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

        [HttpPost]
        public async Task<IActionResult> Delete(SymbolDTO model)
        {
            try
            {
                if (model.Id != null)
                {
                    var response = await _genericRepository.Delete(_uow, (long)model.Id);
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

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _genericRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(new FormatedResponse()
            {
                MessageCode = response.MessageCode,
                InnerBody = response.InnerBody,
                StatusCode = EnumStatusCode.StatusCode200
            });
        }
    }
}
