using API.All.DbContexts;
using API.Entities;
using CORE.DTO;
using CORE.GenericUOW;
using Microsoft.AspNetCore.Mvc;
using CORE.Enum;
using CORE.StaticConstant;
using API.Main;
using Microsoft.Extensions.Options;
using API.DTO;

namespace API.All.SYSTEM.CoreAPI.DuoCssThemeVar
{
    [ApiExplorerSettings(GroupName = "002-SYSTEM-DUO_CSS_THEME_VAR")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class DuoCssThemeVarController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly CoreDbContext _coreDbContext;
        private IDuoGenericRepository<CSS_THEME, CssThemeDTO, CSS_THEME_VAR, CssThemeVarDTO> _duoGenericRepository;
        private readonly AppSettings _appSettings;

        public DuoCssThemeVarController(CoreDbContext coreDbContext, IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(coreDbContext);
            _coreDbContext = coreDbContext;
            _duoGenericRepository = _uow.DuoGenericRepository<CSS_THEME, CssThemeDTO, CSS_THEME_VAR, CssThemeVarDTO>();
            _appSettings = options.Value;
        }


        [HttpGet]
        public async Task<IActionResult> GetChildrenById(long id)
        {
            try
            {
                var res = await _duoGenericRepository.GetChildrenById(id);
                if (res.InnerBody != null)
                {

                    var response = (List<CSS_THEME_VAR>)res.InnerBody;
                    var joined = (from l in response
                                  from t in _coreDbContext.CssThemes.AsNoTracking().Where(x => x.ID == l.CSS_THEME_ID).DefaultIfEmpty()
                                  from v in _coreDbContext.CssVars.AsNoTracking().Where(x => x.ID == l.CSS_VAR_ID).DefaultIfEmpty()
                                  from c in _coreDbContext.SysUsers.AsNoTracking().Where(x => x.ID == l.CREATED_BY).DefaultIfEmpty()
                                  from u in _coreDbContext.SysUsers.AsNoTracking().Where(x => x.ID == l.UPDATED_BY).DefaultIfEmpty()
                                  select new CssThemeVarDTO
                                  {
                                      Id = l.ID,
                                      ThemeCode = t.CODE,
                                      VarName = v.NAME,
                                      CreatedDate = l.CREATED_DATE,
                                      CreatedByUsername = c.USERNAME,
                                      UpdatedDate = l.UPDATED_DATE,
                                      UpdatedByUsername = u.USERNAME,
                                  }).FirstOrDefault();

                    return Ok(new FormatedResponse() { InnerBody = joined });
                }
                else
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
        public async Task<IActionResult> Create(DuoGenericCreateUpdateRequest<CssThemeDTO, CssThemeVarDTO> model)
        {
            try
            {
                var sid = Request.Sid(_appSettings);

                if (sid == null) return Unauthorized();

                var response = await _duoGenericRepository.Create(_uow, model.Parent, model.Children, sid);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(DuoGenericCreateUpdateRequest<CssThemeDTO, CssThemeVarDTO> model)
        {
            try
            {
                var sid = Request.Sid(_appSettings);

                if (sid == null) return Unauthorized();

                var response = await _duoGenericRepository.Update(_uow, model.Parent, model.Children, sid);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }
    }
}
