/*
using Microsoft.AspNetCore.Mvc;
using CoreDAL.ViewModels;
using API.All.DbContexts;
using API.Entities;
using CORE.DTO;
using CORE.Enum;
using CORE.GenericUOW;
using CORE.StaticConstant;
using Common.Extensions;

namespace CoreAPI.Authorization
{
    [ApiExplorerSettings(GroupName = "002-SYSTEM-SYS_FUNCTION")]
    [ApiController]
    [HiStaffAuthorize]
    [Route("api/[controller]/[action]")]
    public class SysFunctionController : ControllerBase
    {
        private readonly GenericUnitOfWork _uow;
        private readonly CoreDbContext _coreDbContext;
        private IGenericRepository<SYS_FUNCTION, SysFunctionDTO> _genericRepository;
        private readonly GenericReducer<SYS_FUNCTION, SysFunctionDTO> genericReducer;
        public SysFunctionController(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
            _uow = new GenericUnitOfWork(coreDbContext);
            _genericRepository = _uow.GenericRepository<SYS_FUNCTION, SysFunctionDTO>();
            genericReducer = new();
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(GenericQueryListStringIdDTO<SysFunctionDTO> request)
        {
            try
            {
                var entity = _coreDbContext.Set<SYS_FUNCTION>().AsNoTracking().AsQueryable();
                var joined = from p in entity
                             from m in _coreDbContext.SysModules.Where(c => c.ID == p.MODULE_ID)
                             from g in _coreDbContext.SysGroupFunctions.Where(c => c.ID == p.GROUP_ID)
                             select new SysFunctionDTO
                             {
                                 Id = p.ID,
                                 GroupId = p.GROUP_ID,
                                 Code = p.CODE,
                                 Name = p.NAME,
                                 Link = p.LINK,
                                 ModuleName = m.NAME,
                                 GroupName = g.NAME,
                                 IsActive = p.IS_ACTIVE,
                                 CreatedBy = p.CREATED_BY,
                                 CreatedDate = p.CREATED_DATE,
                                 UpdatedBy = p.UPDATED_BY,
                                 UpdatedDate = p.UPDATED_DATE,
                                 Status = (p.IS_ACTIVE.HasValue && p.IS_ACTIVE.Value) ? "Áp dụng" : "Ngừng áp dụng"
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
                var objRes = await (from p in _coreDbContext.SysFunctions
                                    where p.ID == id
                                    select new SysFunctionDTO
                                    {
                                        Id = p.ID,
                                        Code = p.CODE,
                                        Name = p.NAME,
                                        Link = p.LINK,
                                        GroupId = p.GROUP_ID,
                                        ModuleId = p.MODULE_ID
                                    }).FirstOrDefaultAsync();
                var actions = await (from p in _coreDbContext.SysFunctionAction
                                     where p.FUNCTION_ID == id
                                     select p.ACTION_ID).ToListAsync();
                if (objRes != null) objRes.Actions = actions == null ? new List<long>() : actions;
                return Ok(new FormatedResponse() { InnerBody = objRes });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(SysFunctionInputDTO request)
        {
            try
            {
                var check = _coreDbContext.SysFunctions.Where(x => x.CODE.ToUpper().Equals(request.Code.ToUpper())).Any();
                if (check) return Ok(new FormatedResponse() { MessageCode = "SYS_FUNCTION_CODE_EXISTED", ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
                SYS_FUNCTION objData = new()
                {
                    CODE = request.Code,
                    NAME = request.Name,
                    MODULE_ID = request.ModuleId,
                    GROUP_ID = request.GroupId,
                    LINK = request.Link,
                    IS_ACTIVE = true,
                    CREATED_DATE = DateTime.Now
                };
                _coreDbContext.SysFunctions.Add(objData);
                await _coreDbContext.SaveChangesAsync();

                if (request.Actions != null)
                {
                    foreach (var item in request.Actions)
                    {
                        SYS_FUNCTION_ACTION oAction = new()
                        {
                            FUNCTION_ID = objData.ID,
                            ACTION_ID = item
                        };
                        _coreDbContext.SysFunctionAction.Add(oAction);
                    }
                }
                await _coreDbContext.SaveChangesAsync();
                return Ok(new FormatedResponse()
                {
                    MessageCode = CommonMessageCode.CREATE_SUCCESS
                });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(SysFunctionInputDTO request)
        {
            try
            {
                if (request.Id == null) return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode404 });
                var objData = _coreDbContext.SysFunctions.Where(x => x.ID == request.Id.Value).FirstOrDefault();
                if (objData == null) return Ok(new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode404 });

                var check = _coreDbContext.SysFunctions.Where(x => x.CODE.ToUpper().Equals(request.Code.ToUpper()) && x.ID != request.Id.Value).Any();
                if (check) return Ok(new FormatedResponse() { MessageCode = "SYS_FUNCTION_CODE_EXISTED", ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });

                objData.CODE = request.Code;
                objData.NAME = request.Name;
                objData.GROUP_ID = request.GroupId;
                objData.MODULE_ID = request.ModuleId;
                objData.LINK = request.Link;
                var actions = _coreDbContext.SysFunctionAction.Where(x => x.FUNCTION_ID == objData.ID);
                foreach (var item in actions)
                {
                    _coreDbContext.SysFunctionAction.Remove(item);
                }
                if (request.Actions != null)
                {
                    foreach (var item in request.Actions)
                    {
                        SYS_FUNCTION_ACTION oAction = new()
                        {
                            FUNCTION_ID = objData.ID,
                            ACTION_ID = item
                        };
                        _coreDbContext.SysFunctionAction.Add(oAction);
                    }
                }
                await _coreDbContext.SaveChangesAsync();
                return Ok(new FormatedResponse()
                {
                    MessageCode = CommonMessageCode.UPDATE_SUCCESS
                });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest req)
        {
            try
            {
                if (req.Ids != null)
                {
                    var response = await _genericRepository.DeleteIds(_uow, req.Ids);
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

        [HttpGet]
        public async Task<IActionResult> GetModules()
        {
            try
            {
                var query = await (from p in _coreDbContext.SysModules.Where(x => x.IS_ACTIVE.HasValue && x.IS_ACTIVE.Value)
                                   select new SysModuleDTO
                                   {
                                       Id = p.ID,
                                       Name = p.NAME
                                   }).ToListAsync();
                return Ok(new FormatedResponse()
                {
                    InnerBody = query
                });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetActions()
        {
            try
            {
                var query = await (from p in _coreDbContext.SysActions
                                   select new
                                   {
                                       Id = p.ID,
                                       Name = p.NAME_VN
                                   }).ToListAsync();
                return Ok(new FormatedResponse()
                {
                    InnerBody = query
                });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var query = await (from p in _coreDbContext.SysFunctions
                                   select new
                                   {
                                       Id = p.ID,
                                       Name = p.NAME
                                   }).ToListAsync();
                return Ok(new FormatedResponse()
                {
                    InnerBody = query
                });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFuncGroups()
        {
            try
            {
                var query = await (from p in _coreDbContext.SysGroupFunctions.Where(x => x.IS_ACTIVE.HasValue && x.IS_ACTIVE.Value)
                                   select new
                                   {
                                       Id = p.ID,
                                       Name = p.NAME
                                   }).ToListAsync();
                return Ok(new FormatedResponse()
                {
                    InnerBody = query
                });
            }
            catch (Exception ex)
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message });
            }
        }

    }
}
*/