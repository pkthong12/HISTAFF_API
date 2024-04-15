using API.All.DbContexts;
using API.DTO;
using API.Entities;
using API.Main;
using AttendanceDAL.ViewModels;
using Common.Extensions;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace API.Controllers.AtShift
{
    [ApiExplorerSettings(GroupName = "096-ATTENDANCE-AT-SHIFT")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class AtShiftController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly IAtShiftRepository _AtShiftRepository;
        private readonly AppSettings _appSettings;


        public AtShiftController(
            FullDbContext dbContext,
            IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(dbContext);
            _AtShiftRepository = new AtShiftRepository(dbContext, _uow);
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var response = await _AtShiftRepository.ReadAll();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByNumberKey(ReadAllByKeyLongRequest request)
        {
            var response = await _AtShiftRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAllByStringKey(ReadAllByKeyStringRequest request)
        {
            var response = await _AtShiftRepository.ReadAllByKey(request.Key, request.Value);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<AtShiftDTO> request)
        {
            var response = await _AtShiftRepository.SinglePhaseQueryList(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _AtShiftRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStringId(string id)
        {
            var response = await _AtShiftRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AtShiftDTO model)
        {
            IFormatProvider culture = new CultureInfo("en-US", true);
            if (!string.IsNullOrEmpty(model.BreaksFromStr) && !string.IsNullOrEmpty(model.BreaksToStr))
            {
                model.BreaksFrom = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy") + " " + model.BreaksFromStr, "dd/MM/yyyy HH:mm", culture);
                model.BreaksTo = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy") + " " + model.BreaksToStr, "dd/MM/yyyy HH:mm", culture);
                if (model.IsNight == true)
                {
                    model.BreaksTo = model.BreaksTo.Value.AddDays(1);
                }
                if (model.BreaksFrom > model.BreaksTo)
                {
                    return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.TIMEFROM_NOT_BIGGER_THAN_TIMETO, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 });
                }
            }
            model.HoursStart = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy") + " " + model.HoursStartStr, "dd/MM/yyyy HH:mm", culture);
            model.HoursStop = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy") + " " + model.HoursStopStr, "dd/MM/yyyy HH:mm", culture);
            if (model.IsNight == true)
            {
                model.HoursStop = model.HoursStop.Value.AddDays(1);
            }
            if (model.HoursStart > model.HoursStop)
            {
                return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.TIMEFROM_NOT_BIGGER_THAN_TIMETO, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 });
            }
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtShiftRepository.Create(_uow, model, sid);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<AtShiftDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtShiftRepository.CreateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AtShiftDTO model)
        {
            IFormatProvider culture = new CultureInfo("en-US", true);
            if (!string.IsNullOrEmpty(model.BreaksFromStr) && !string.IsNullOrEmpty(model.BreaksToStr))
            {
                model.BreaksFrom = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy") + " " + model.BreaksFromStr, "dd/MM/yyyy HH:mm", culture);
                model.BreaksTo = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy") + " " + model.BreaksToStr, "dd/MM/yyyy HH:mm", culture);
                if (model.IsNight == true)
                {
                    model.BreaksTo = model.BreaksTo.Value.AddDays(1);
                }
                if (model.BreaksFrom > model.BreaksTo)
                {
                    return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.TIMEFROM_NOT_BIGGER_THAN_TIMETO, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 });
                }
            }

            model.HoursStart = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy") + " " + model.HoursStartStr, "dd/MM/yyyy HH:mm", culture);
            model.HoursStop = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy") + " " + model.HoursStopStr, "dd/MM/yyyy HH:mm", culture);
            if (model.IsNight == true)
            {
                model.HoursStop = model.HoursStop.Value.AddDays(1);
            }
            if (model.HoursStart > model.HoursStop)
            {
                return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.TIMEFROM_NOT_BIGGER_THAN_TIMETO, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 });
            }
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtShiftRepository.Update(_uow, model, sid, false);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<AtShiftDTO> models)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtShiftRepository.UpdateRange(_uow, models, sid);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AtShiftDTO model)
        {
            if (model.Id != null)
            {
                var response = await _AtShiftRepository.Delete(_uow, (long)model.Id);
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

            var entity = _uow.Context.Set<AT_WORKSIGN>().AsNoTracking().AsQueryable();
            var entity2 = _uow.Context.Set<AT_SIGN_DEFAULT>().AsNoTracking().AsQueryable();

            var check1 = (from p in entity
                          where model.Ids.Contains((long)p.SHIFT_ID)
                          select p).Count() > 0 ? true : false;

            var check2 = (from p in entity2
                          where model.Ids.Contains((long)p.SIGN_DEFAULT)
                          select p).Count() > 0 ? true : false;

            var check3 = (from p in _uow.Context.Set<AT_TIME_EXPLANATION>().AsNoTracking().AsQueryable()
                          where model.Ids.Contains((long)p.SHIFT_ID)
                          select p).Count() > 0 ? true : false;

            var check4 = (from p in _uow.Context.Set<AT_SHIFT>().AsNoTracking().AsQueryable()
                          where model.Ids.Contains((long)p.ID) && p.IS_ACTIVE == true
                          select p).Count() > 0 ? true : false;
            if (check1 || check2 || check3 )
                return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE });
            if (check4)
            {
                return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.CAN_NOT_DELETE_RECORDS_HAVE_IS_ACTIVE });

            }
            var response = await _AtShiftRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStringIds(StringIdsRequest model)
        {
            var response = await _AtShiftRepository.DeleteIds(_uow, model.Ids);
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult> GetListToImport()
        {
            try
            {
                var entity = _uow.Context.Set<AT_SHIFT>().AsNoTracking().AsQueryable();
                var joined = from p in entity
                             where p.IS_ACTIVE == true
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
        public async Task<ActionResult> GetListTimeType()
        {
            try
            {
                var entity = _uow.Context.Set<AT_TIME_TYPE>().AsNoTracking().AsQueryable();
                var joined = from p in entity
                             where p.IS_ACTIVE == true
                             orderby p.CREATED_DATE descending
                             select new TimeTypeDTO
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
        public async Task<ActionResult> GetListSymbol()
        {
            try
            {
                var entity = _uow.Context.Set<AT_SYMBOL>().AsNoTracking().AsQueryable();
                var joined = from p in entity
                             where p.IS_ACTIVE == true
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
        public async Task<IActionResult> GetListTimeTypeById(long id)
        {
            var entity = _uow.Context.Set<AT_SYMBOL>().AsNoTracking().AsQueryable();
            var joined = from p in entity
                         where p.ID == id
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

        [HttpGet]
        public async Task<IActionResult> GetTimeTypeById(long id)
        {
            var entity = _uow.Context.Set<AT_TIME_TYPE>().AsNoTracking().AsQueryable();
            var joined = from p in entity
                         where p.ID == id
                         select new TimeTypeDTO
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
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _AtShiftRepository.ToggleActiveIds(_uow, model.Ids, model.ValueToBind, sid);
            return Ok(new FormatedResponse()
            {
                MessageCode = response.MessageCode,
                InnerBody = response.InnerBody,
                StatusCode = EnumStatusCode.StatusCode200
            });
        }
    }
}

