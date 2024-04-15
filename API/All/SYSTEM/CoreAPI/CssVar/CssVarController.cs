using API.All.DbContexts;
using API.DTO;
using API.Entities;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;

namespace API.All.SYSTEM.CoreAPI.CssVar
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "002-SYSTEM-CSS_VAR")]
    [HiStaffAuthorize]
    public class CssVarController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly CoreDbContext _coreDbContext;
        private IGenericRepository<CSS_VAR, CssVarDTO> _genericRepository;
        private readonly GenericReducer<CSS_VAR, CssVarDTO> genericReducer;
        private readonly AppSettings _appSettings;

        public CssVarController(CoreDbContext coreDbContext, IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(coreDbContext);
            _coreDbContext = coreDbContext;
            _genericRepository = _uow.GenericRepository<CSS_VAR, CssVarDTO>();
            _appSettings = options.Value;
            genericReducer = new();
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
        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<CssVarDTO> model)
        {
            try
            {
                var query = from v in _coreDbContext.CssVars.AsNoTracking().AsQueryable()
                            from u in _coreDbContext.SysUsers.AsNoTracking().AsQueryable()
                            where v.UPDATED_BY == u.ID && v.CREATED_BY == u.ID
                            select new CssVarDTO()
                            {
                                Id = v.ID,
                                Name = v.NAME,
                                Description = v.DESCRIPTION,
                                CreatedDate = v.CREATED_DATE,
                                UpdatedDate = v.UPDATED_DATE,
                                CreatedByUsername = u.USERNAME,
                                UpdatedByUsername = u.USERNAME,

                            };
                var response = await genericReducer.SinglePhaseReduce(query, model);
                if(response.ErrorType != EnumErrorType.NONE) 
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
                var res = await _genericRepository.GetById(id);
                if (res.InnerBody != null)
                {

                    var response = res.InnerBody;
                    var list = new List<CSS_VAR>
                    {
                        (CSS_VAR)response
                    };

                    var joined = (from l in list
                                  from c in _coreDbContext.SysUsers.ToList().Where(x => x.ID == l.CREATED_BY).DefaultIfEmpty()
                                  from u in _coreDbContext.SysUsers.ToList().Where(x => x.ID == l.UPDATED_BY).DefaultIfEmpty()
                                  select new CssVarDTO
                                  {
                                      Id = l.ID,
                                      Name = l.NAME,
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
        public async Task<IActionResult> Create(CssVarDTO model)
        {
            try
            {
                var sId = Request.Sid(_appSettings);
                var response = await _genericRepository.Create(_uow, model, sId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }
        [HttpPost] 
        public async Task<IActionResult> Update(CssVarDTO model)
        {
            try
            {
                model.UpdatedDate = DateTime.Now;
                var response = await _genericRepository.Update(_uow, model, Request.Sid(_appSettings));
                return Ok(response);
            }
            catch (Exception ex)
            {

                return Ok(new FormatedResponse()
                {
                    MessageCode = ex.Message, ErrorType=EnumErrorType.UNCATCHABLE, StatusCode=EnumStatusCode.StatusCode500
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            try
            {
                if(model.Ids != null)
                {
                    var response = await _genericRepository.DeleteIds(_uow, model.Ids);
                    return Ok(response);
                }
                else { return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DELETE_REQUEST_NULL_ID });}
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse()
                {
                    MessageCode = ex.Message,
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    StatusCode = EnumStatusCode.StatusCode500
                });
            }
        }
    }
}
