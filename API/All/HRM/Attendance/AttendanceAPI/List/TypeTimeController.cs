using Microsoft.AspNetCore.Mvc;
using AttendanceDAL.ViewModels;
using API.Entities;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using API.All.DbContexts;
using CoreDAL.ViewModels;
using API.Main;

using API;
using Microsoft.Extensions.Options;

namespace AttendanceAPI.List
{
    [ApiExplorerSettings(GroupName = "3-ATTENDANCE-LIST-TIME-TYPE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class TimeTypeController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly GenericReducer<AT_TIME_TYPE, TimeTypeDTO> genericReducer;
        private IGenericRepository<AT_SYMBOL, SymbolDTO> _symbolGenericRepository;
        private IGenericRepository<AT_TIME_TYPE, TimeTypeDTO> _genericRepository;
        private AppSettings _appSettings;

        public TimeTypeController(CoreDbContext coreDbContext, IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(coreDbContext);
            genericReducer = new();
            _genericRepository = _uow.GenericRepository<AT_TIME_TYPE, TimeTypeDTO>();
            _symbolGenericRepository = _uow.GenericRepository<AT_SYMBOL, SymbolDTO>();
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadTimeSymbol()
        {
            try
            {
                var response = await _symbolGenericRepository.ReadAll();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<TimeTypeDTO> request)
        {

            try
            {
                var entity = _uow.Context.Set<AT_TIME_TYPE>().AsNoTracking().AsQueryable();
                var symbols = _uow.Context.Set<AT_SYMBOL>().AsNoTracking().AsQueryable();
                var joined = from p in entity
                             from m in symbols.Where(x => x.ID == p.MORNING_ID)
                             from a in symbols.Where(x => x.ID == p.AFTERNOON_ID)
                             select new TimeTypeDTO
                             {
                                 Id = p.ID,
                                 Code = p.CODE,
                                 Name = p.NAME,
                                 MorningId = p.MORNING_ID,
                                 MorningName = m.NAME,
                                 AfternoonId = p.AFTERNOON_ID,
                                 AfternoonName = a.NAME,
                                 IsOff = p.IS_OFF,
                                 IsActive = p.IS_ACTIVE,
                                 Note = p.NOTE,
                                 CreatedBy = p.CREATED_BY,
                                 UpdatedBy = p.UPDATED_BY,
                                 CreatedDate = p.CREATED_DATE,
                                 UpdatedDate = p.UPDATED_DATE,
                                 IsActiveStr = (p.IS_ACTIVE) ? "Áp dụng" : "Ngừng áp dụng",
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
                var response = await _genericRepository.GetById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(TimeTypeDTO model)
        {
            try
            {
                var sid = Request.Sid(_appSettings);
                AT_TIME_TYPE entity = new();
                model.IsActive = true;
                var response = await _genericRepository.Create(_uow, model, sid ?? "");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(TimeTypeDTO model)
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
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            try
            {
                if (model.Ids != null)
                {
                    var eL = _uow.Context.Set<AT_TIME_TYPE>().AsNoTracking().AsQueryable();
                    var check = await eL.AsNoTracking().Where(p => model.Ids.Contains(p.ID) && p.IS_ACTIVE == true).AnyAsync();
                    if (check) {
                        return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE, StatusCode = EnumStatusCode.StatusCode400 });
                    }
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
        public async Task<IActionResult> Update(TimeTypeDTO model)
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
