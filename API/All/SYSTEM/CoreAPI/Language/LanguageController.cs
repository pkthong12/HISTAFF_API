using API.All.DbContexts;
using CORE.DTO;
using CORE.GenericUOW;
using Microsoft.AspNetCore.Mvc;
using API.All.SYSTEM.CoreDAL.System.Language.Models;
using CORE.Enum;
using CORE.StaticConstant;
using API.Main;
using Microsoft.Extensions.Options;

namespace API.All.SYSTEM.CoreAPI.Language
{
    [ApiExplorerSettings(GroupName = "002-SYSTEM-LANGUAGE")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class LanguageController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly CoreDbContext _coreDbContext;
        private IGenericRepository<SYS_LANGUAGE, SysLanguageDTO> _genericRepository;
        private readonly GenericReducer<SYS_LANGUAGE, SysLanguageDTO> genericReducer;
        private readonly AppSettings _appSettings;
        protected readonly ILogger<LanguageController> _logger;

        public LanguageController(CoreDbContext coreDbContext, IOptions<AppSettings> options, ILogger<LanguageController> logger)
        {
            _uow = new GenericUnitOfWork(coreDbContext);
            _coreDbContext = coreDbContext;
            _genericRepository = _uow.GenericRepository<SYS_LANGUAGE, SysLanguageDTO>();
            _appSettings = options.Value;
            genericReducer = new();
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ReadAll()
        {
            try
            {
                var response = await _genericRepository.ReadAll();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ReadAllMini()
        {
            try
            {
                var response = await (_coreDbContext.SysLanguages.AsNoTracking().Select(x => new
                {
                    Key = x.KEY,
                    Vi = x.VI,
                    En = x.EN
                }).OrderBy(x => x.Key)).ToListAsync();
                return Ok(new FormatedResponse() { InnerBody = response, MessageCode = CommonMessageCode.QUERY_LIST_SUCCESS });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<SysLanguageDTO> request)
        {

            try
            {
                var entity = _uow.Context.Set<SYS_LANGUAGE>().AsNoTracking().AsQueryable();
                var joined = from l in entity
                             from c in _uow.Context.Set<SYS_USER>().AsNoTracking().AsQueryable().Where(x => x.ID == l.CREATED_BY).DefaultIfEmpty()
                             from u in _uow.Context.Set<SYS_USER>().AsNoTracking().AsQueryable().Where(x => x.ID == l.UPDATED_BY).DefaultIfEmpty()

                             select new SysLanguageDTO
                             {
                                 Id = l.ID,
                                 Key = l.KEY,
                                 Vi = l.VI,
                                 En = l.EN,
                                 CreatedDate = l.CREATED_DATE,
                                 CreatedByUsername = c.USERNAME,
                                 UpdatedDate = l.UPDATED_DATE,
                                 UpdatedByUsername = u.USERNAME
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

                var joined = await (from l in _coreDbContext.SysLanguages.Where(x => x.ID == id)
                              from c in _coreDbContext.SysUsers.Where(x => x.ID == l.CREATED_BY).DefaultIfEmpty()
                              from u in _coreDbContext.SysUsers.Where(x => x.ID == l.UPDATED_BY).DefaultIfEmpty()
                              select new SysLanguageDTO
                              {
                                  Id = l.ID,
                                  Key = l.KEY,
                                  Vi = l.VI,
                                  En = l.EN,
                                  Note = l.NOTE,
                                  CreatedDate = l.CREATED_DATE,
                                  CreatedByUsername = c.USERNAME,
                                  UpdatedDate = l.UPDATED_DATE,
                                  UpdatedByUsername = u.USERNAME,
                              }).FirstOrDefaultAsync();

                if (joined != null)
                {
                    return Ok(new FormatedResponse() { InnerBody = joined });
                } else
                {
                    return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 });
                }

                
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(SysLanguageDTO model)
        {
            try
            {
                var now = DateTime.UtcNow;
                var sid = Request.Sid(_appSettings);

                if (sid == null) return Unauthorized();

                var response = await _genericRepository.Create(_uow, model, sid);
                _logger.LogError($"{model.Key} added to SYS_LANGUAGE");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(SysLanguageDTO model)
        {
            try
            {
                var now = DateTime.UtcNow;
                var sid = Request.Sid(_appSettings);

                if (sid == null) return Unauthorized();

                var response = await _genericRepository.Update(_uow, model, sid);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SysLanguageDTO model)
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
    }
}
