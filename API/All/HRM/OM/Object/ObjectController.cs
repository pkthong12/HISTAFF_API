using API.All.DbContexts;
using API.Entities;
using API.Main;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq.Dynamic.Core;

namespace API.All.HRM.OM.Object
{
    [ApiExplorerSettings(GroupName = "002-OM-HRM_OBJECT")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class ObjectController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly CoreDbContext _coreDbContext;
        private IGenericRepository<HRM_OBJECT, HrmObjectDTO> _genericRepository;
        private readonly GenericReducer<HRM_OBJECT, HrmObjectDTO> genericReducer;
        private readonly AppSettings _appSettings;

        public ObjectController(CoreDbContext coreDbContext, IOptions<AppSettings> options)
        {
            _uow = new GenericUnitOfWork(coreDbContext);
            _coreDbContext = coreDbContext;
            _genericRepository = _uow.GenericRepository<HRM_OBJECT, HrmObjectDTO>();
            _appSettings = options.Value;
            genericReducer = new();
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListDTO<HrmObjectDTO> request)
        {

            try
            {
                var entity = _uow.Context.Set<HRM_OBJECT>().AsNoTracking().AsQueryable();
                var joined = from o in entity
                             from c in _uow.Context.Set<SYS_USER>().AsNoTracking().AsQueryable().Where(x => x.ID == o.CREATED_BY).DefaultIfEmpty()
                             from u in _uow.Context.Set<SYS_USER>().AsNoTracking().AsQueryable().Where(x => x.ID == o.UPDATED_BY).DefaultIfEmpty()
                             select new HrmObjectDTO
                             {
                                 Id = o.ID,
                                 Name = o.NAME,
                                 CreatedDate = o.CREATED_DATE,
                                 CreatedByUsername = c.USERNAME,
                                 UpdatedDate = o.UPDATED_DATE,
                                 UpdatedByUsername = u.USERNAME,
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
                var res = await _genericRepository.GetById(id);

                if (res.InnerBody != null)
                {

                    var response = res.InnerBody;
                    var list = new List<HRM_OBJECT>
                    {
                        (HRM_OBJECT)response
                    };

                    var joined = (from o in list
                                  from c in _coreDbContext.SysUsers.ToList().Where(x => x.ID == o.CREATED_BY).DefaultIfEmpty()
                                  from u in _coreDbContext.SysUsers.ToList().Where(x => x.ID == o.UPDATED_BY).DefaultIfEmpty()
                                  select new HrmObjectDTO
                                  {
                                      Id = o.ID,
                                      Name = o.NAME,
                                      CreatedDate = o.CREATED_DATE,
                                      CreatedByUsername = c.USERNAME,
                                      UpdatedDate = o.UPDATED_DATE,
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
        public async Task<IActionResult> Create(HrmObjectDTO model)
        {
            try
            {

                var now = DateTime.UtcNow;
                var sid = Request.Sid(_appSettings);

                if (sid == null) return Unauthorized();

                var response = await _genericRepository.Create(_uow, model, sid);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(HrmObjectDTO model)
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
        public async Task<IActionResult> Delete(HrmObjectDTO model)
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
        public async Task<IActionResult> DeleteIds(StringIdsRequest model)
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
