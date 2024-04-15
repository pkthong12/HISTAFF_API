using API.All.DbContexts;
using API.DTO;
using API.Entities;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;




namespace API.All.SYSTEM.CoreAPI.CssTheme
{
    [ApiExplorerSettings(GroupName = "002-SYSTEM-CSS_THEME")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class CssThemeController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly CoreDbContext _coreDbContext;
        private readonly IGenericRepository<CSS_THEME, CssThemeDTO> _genericRepository;
        private readonly GenericReducer<CSS_THEME, CssThemeDTO> genericReducer;
        private readonly AppSettings _appSettings;

        public CssThemeController(CoreDbContext coreDbContext, IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(coreDbContext);
            _coreDbContext = coreDbContext;
            _genericRepository = _uow.GenericRepository<CSS_THEME, CssThemeDTO>();
            _appSettings = options.Value;
            genericReducer = new();
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<CssThemeDTO> model)
        {
            try
            {
                var query = from t in _coreDbContext.CssThemes.AsNoTracking().AsQueryable()
                            from u in _coreDbContext.SysUsers.AsNoTracking().AsQueryable()
                            where t.CREATED_BY == u.ID && t.UPDATED_BY == u.ID
                            select new CssThemeDTO()
                            {
                                Id = t.ID,
                                Code = t.CODE,
                                Description = t.DESCRIPTION,
                                CreatedDate = t.CREATED_DATE,
                                UpdatedDate = t.UPDATED_DATE,
                                CreatedByUsername = u.USERNAME,
                                UpdatedByUsername = u.USERNAME,
                            };
                var response = await genericReducer.SinglePhaseReduce(query, model);
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
        [AllowAnonymous]
        public async Task<IActionResult> ReadAll()
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var res = await _genericRepository.GetById(id);
                if (res.InnerBody != null)
                {

                    var response = res.InnerBody;
                    var list = new List<CSS_THEME>
                    {
                        (CSS_THEME)response
                    };

                    var joined = (from l in list
                                  from c in _coreDbContext.SysUsers.ToList().Where(x => x.ID == l.CREATED_BY).DefaultIfEmpty()
                                  from u in _coreDbContext.SysUsers.ToList().Where(x => x.ID == l.UPDATED_BY).DefaultIfEmpty()
                                  select new CssThemeDTO
                                  {
                                      Id = l.ID,
                                      Code = l.CODE,
                                      Description = l.DESCRIPTION,
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
        public async Task<IActionResult> Create(CssThemeDTO model)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Update(CssThemeDTO model)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CssThemeDTO model)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
    }
}
