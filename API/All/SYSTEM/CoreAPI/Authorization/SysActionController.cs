using API.All.DbContexts;
using API.Entities;
using CORE.DTO;
using CORE.GenericUOW;
using Microsoft.AspNetCore.Mvc;
using CORE.Enum;
using CORE.StaticConstant;
using Microsoft.Extensions.Options;
using CoreDAL.ViewModels;
using API.Main;

namespace API.All.SYSTEM.CoreAPI.Language
{
    [ApiExplorerSettings(GroupName = "002-SYSTEM-ACTION")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/actions/")]
    public class SysActionController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly CoreDbContext _coreDbContext;
        private readonly AppSettings _appSettings;
        private IGenericRepository<SYS_ACTION, SysActionDTO> _genericRepository;
        private readonly GenericReducer<SYS_ACTION, SysActionDTO> genericReducer;

        public SysActionController(CoreDbContext coreDbContext, IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(coreDbContext);
            _coreDbContext = coreDbContext;
            _genericRepository = _uow.GenericRepository<SYS_ACTION, SysActionDTO>();
            genericReducer = new();
            _appSettings = options.Value;
        }

        [HttpPost]
        [Route("QueryList")]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<SysActionDTO> request)
        {
            try
            {
                var entity = _uow.Context.Set<SYS_ACTION>().AsNoTracking().AsQueryable();
                var joined = from l in entity
                             select new SysActionDTO
                             {
                                 Id = l.ID,
                                 Code = l.CODE,
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
        [Route("funcsAcitons")]
        public async Task<IActionResult> GetFuncsActions()
        {
            try
            {
                string connector = BackendCodePrefix.SIGN_CONNECTOR;
                string prefix = BackendCodePrefix.BASE + connector + BackendCodePrefix.PERMISSION;
                var joined = from f in _coreDbContext.SysFunctions
                             where f.IS_ACTIVE.HasValue && f.IS_ACTIVE.Value
                             select new
                             {
                                 FunctionId = f.ID,
                                 AppActions = (from l in _coreDbContext.SysActions
                                               from a in _coreDbContext.SysFunctionAction.Where(x => x.ACTION_ID == l.ID)
                                               from m in _coreDbContext.SysModules.Where(x => x.ID == f.MODULE_ID)
                                               from g in _coreDbContext.SysGroupFunctions.Where(x => x.ID == f.GROUP_ID)
                                               where a.FUNCTION_ID == f.ID
                                               select new
                                               {
                                                   Id = l.ID,
                                                   Code = l.CODE,
                                                   ActionNameCode = prefix + connector + f.CODE.ToUpper() + connector + g.CODE.ToUpper() + connector + f.CODE.ToUpper() + connector + l.CODE.ToUpper(),
                                               }).ToList()
                             };
                var lst = await joined.ToListAsync();
                return Ok(new FormatedResponse()
                {
                    InnerBody = lst
                });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO request)
        {
            var sid = Request.Sid(_appSettings);
            if (sid == null) return Unauthorized();
            var response = await _genericRepository.ToggleActiveIds(_uow, request.Ids, request.ValueToBind, sid);
            return Ok(response);
        }

    }
}
