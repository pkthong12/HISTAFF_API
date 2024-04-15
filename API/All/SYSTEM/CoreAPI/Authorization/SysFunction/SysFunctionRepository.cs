using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Authorization.SysAction;
using API.All.SYSTEM.CoreAPI.Authorization.SysFunction;

namespace API.Controllers.SysFunction
{
    public class SysFunctionRepository : ISysFunctionRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<SYS_FUNCTION, SysFunctionDTO> _genericRepository;
        private readonly GenericReducer<SYS_FUNCTION, SysFunctionDTO> _genericReducer;

        public SysFunctionRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<SYS_FUNCTION, SysFunctionDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<SysFunctionDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysFunctionDTO> request)
        {
            var joined = from p in _dbContext.SysFunctions.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from m in _dbContext.SysModules.Where(c => c.ID == p.MODULE_ID).DefaultIfEmpty()
                         from g in _dbContext.SysFunctionGroups.Where(c => c.ID == p.GROUP_ID).DefaultIfEmpty()
                         select new SysFunctionDTO
                         {
                             Id = p.ID,
                             GroupId = p.GROUP_ID,
                             Code = p.CODE,
                             Name = p.NAME,
                             ModuleId = p.MODULE_ID,
                             ModuleName = m.NAME,
                             GroupName = g.NAME,
                             IsActive = p.IS_ACTIVE,
                             Path = p.PATH,
                             PathFullMatch = p.PATH_FULL_MATCH,
                             CreatedBy = p.CREATED_BY,
                             CreatedDate = p.CREATED_DATE,
                             UpdatedBy = p.UPDATED_BY,
                             UpdatedDate = p.UPDATED_DATE,
                             Status = (p.IS_ACTIVE.HasValue && p.IS_ACTIVE.Value) ? "ACTIVE" : "INACTIVE"
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<GenericPhaseTwoListResponse<SysFunctionDTO>> FunctionPermissionList(GenericQueryListDTO<SysFunctionDTO> request)
        {
            try
            {
                string? userId = null;
                if (request.Filter != null)
                {
                    userId = request.Filter.UserId;
                }

                var HEADER_VIEW_ABSTRACT = _dbContext.SysActions.Single(x => x.CODE == "HEADER_VIEW_ABSTRACT");

                var joined = from f in _dbContext.SysFunctions.AsNoTracking()
                             from g in _dbContext.SysFunctionGroups.AsNoTracking().Where(x => x.ID == f.GROUP_ID).DefaultIfEmpty()
                             from m in _dbContext.SysModules.AsNoTracking().Where(x => x.ID == f.MODULE_ID).DefaultIfEmpty()

                             where f.ROOT_ONLY != true

                             select new SysFunctionDTO
                             {
                                 UserId = userId,
                                 Id = f.ID,
                                 GroupCode = g.CODE,
                                 GroupName = g.NAME,
                                 ModuleCode = m.CODE,
                                 ModuleName = m.NAME,
                                 Code = f.CODE,
                                 Path = f.PATH,
                                 Name = f.NAME
                             };
                var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);

                if (singlePhaseResult != null)
                {
                    if (singlePhaseResult.List != null)
                    {
                        List<SysFunctionDTO> list = singlePhaseResult.List.ToList();
                        list.ForEach(f =>
                        {
                            var actions = (from fa in _dbContext.SysFunctionActions.AsNoTracking().Where(x => x.FUNCTION_ID == f.Id)
                                           from a in _dbContext.SysActions.AsNoTracking().Where(x => x.ID == fa.ACTION_ID).DefaultIfEmpty()
                                           select new
                                           {
                                               ActionId = fa.ACTION_ID,
                                               ActionCode = a.CODE
                                           }).ToList();

                            if (actions.Count == 0)
                            {
                                actions.Add(new
                                {
                                    ActionId = HEADER_VIEW_ABSTRACT.ID,
                                    ActionCode = HEADER_VIEW_ABSTRACT.CODE
                                });
                            }

                            List<SysActionTagDTO> appActions = new();
                            actions.ForEach(item =>
                            {
                                appActions.Add(new()
                                {
                                    Id = item.ActionId,
                                    Text = item.ActionCode
                                });
                            });
                            f.AppActions = appActions;
                            List<long> userActions = new();

                            var currentUserActions = (from a in _dbContext.SysUserFunctionActions.AsNoTracking()
                                              .Where(x => x.USER_ID == userId && x.FUNCTION_ID == f.Id)
                                                      select new
                                                      {
                                                          ActionId = a.ACTION_ID,
                                                      }).ToList();
                            currentUserActions.ForEach(item =>
                            {
                                userActions.Add(item.ActionId);
                            });
                            f.UserActions = userActions;
                        });
                        singlePhaseResult.List = list.AsQueryable();
                        return singlePhaseResult;
                    }
                    else
                    {
                        return new()
                        {
                            ErrorType = EnumErrorType.CATCHABLE,
                            MessageCode = CommonMessageCode.LIST_OF_SINGLE_QUERY_LIST_RESULT_WAS_NULL,
                            ErrorPhase = 1,
                        };
                    }
                }
                else
                {
                    return new()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.SINGLE_QUERY_LIST_RESULT_WAS_NULL,
                        ErrorPhase = 1,
                    };
                }
            }
            catch (Exception ex)
            {
                return new()
                {
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    MessageCode = ex.Message,
                    ErrorPhase = 1,
                };
            }

        }

        public async Task<FormatedResponse> ReadAll()
        {
            var response = await (
                            from l in _dbContext.SysFunctions.AsNoTracking()
                            from m in _dbContext.SysModules.AsNoTracking().Where(x => x.ID == l.MODULE_ID)
                            select new SysFunctionDTO
                            {
                                Id = l.ID,
                                ModuleId = l.MODULE_ID,
                                ModuleCode = m.CODE,
                                GroupId = l.GROUP_ID,
                                Code = l.CODE,
                                Name = l.NAME,
                                Path = l.PATH,
                                PathFullMatch = l.PATH_FULL_MATCH,
                                IsActive = l.IS_ACTIVE,
                                CreatedDate = l.CREATED_DATE,
                                CreatedBy = l.CREATED_BY,
                                UpdatedDate = l.UPDATED_DATE,
                                UpdatedBy = l.UPDATED_BY
                            }).ToListAsync();
            return new() { InnerBody = response };
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, long value)
        {
            var resposne = await _genericRepository.ReadAllByKey(key, value);
            return new() { InnerBody = resposne };
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, string value)
        {
            var resposne = await _genericRepository.ReadAllByKey(key, value);
            return new() { InnerBody = resposne };
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var response = await _genericRepository.GetById(id);
            return response;
        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, SysFunctionDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<SysFunctionDTO> dtos, string sid)
        {
            var add = new List<SysFunctionDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, SysFunctionDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<SysFunctionDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(_uow, dtos, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, long id)
        {
            var response = await _genericRepository.Delete(_uow, id);
            return response;
        }

        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<long> ids)
        {
            var response = await _genericRepository.DeleteIds(_uow, ids);
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> CreateFunctionThenUpdateFunctionIdForMenu(CreateUpdateFunctionThenAsignFunctionIdToMenuDTO request, string sid)
        {
            try
            {
                _uow.CreateTransaction();
                if (request.Function.ModuleId == null || request.Function.Code == null || request.Function.Name == null || request.Function.Path == null)
                {
                    _uow.Rollback();
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DTO_MODEL_IS_INVALID, StatusCode = EnumStatusCode.StatusCode400 };
                }
                var now = DateTime.UtcNow;
                SYS_FUNCTION function = new()
                {
                    MODULE_ID = (long)request.Function.ModuleId,
                    CODE = request.Function.Code,
                    NAME = request.Function.Name,
                    GROUP_ID = request.Function.GroupId,
                    PATH = request.Function.Path,
                    PATH_FULL_MATCH = request.Function.PathFullMatch,
                    IS_ACTIVE = request.Function.IsActive,
                    CREATED_DATE = now,
                    UPDATED_DATE = now,
                    CREATED_BY = sid,
                    UPDATED_BY = sid,
                };

                await _dbContext.SysFunctions.AddAsync(function);
                await _dbContext.SaveChangesAsync();
                var functionId = function.ID;
                var menu = await _dbContext.SysMenus.SingleAsync(x => x.ID == request.MenuId);

                menu.FUNCTION_ID = functionId;
                _dbContext.SysMenus.Update(menu);
                await _dbContext.SaveChangesAsync();
                _uow.Commit();

                return new()
                {
                    InnerBody = new
                    {
                        Function = function,
                        Menu = menu
                    }
                };

            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public async Task<FormatedResponse> UpdateFunctionThenUpdateFunctionIdForMenu(CreateUpdateFunctionThenAsignFunctionIdToMenuDTO request, string sid)
        {
            try
            {
                _uow.CreateTransaction();
                if (request.Function.Id == null || request.Function.ModuleId == null || request.Function.Code == null || request.Function.Name == null || request.Function.Path == null)
                {
                    _uow.Rollback();
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DTO_MODEL_IS_INVALID, StatusCode = EnumStatusCode.StatusCode400 };
                }
                var now = DateTime.UtcNow;
                SYS_FUNCTION function = _dbContext.SysFunctions.Single(x => x.ID == request.Function.Id);
                function.MODULE_ID = (long)request.Function.ModuleId;
                function.CODE = request.Function.Code;
                function.NAME = request.Function.Name;
                function.GROUP_ID = request.Function.GroupId;
                function.PATH = request.Function.Path;
                function.PATH_FULL_MATCH = request.Function.PathFullMatch;
                function.IS_ACTIVE = request.Function.IsActive;
                function.UPDATED_DATE = now;
                function.UPDATED_BY = sid;

                _dbContext.SysFunctions.Update(function);
                await _dbContext.SaveChangesAsync();
                var functionId = function.ID;
                var menu = await _dbContext.SysMenus.SingleAsync(x => x.ID == request.MenuId);
                menu.FUNCTION_ID = functionId;
                _dbContext.SysMenus.Update(menu);
                await _dbContext.SaveChangesAsync();
                _uow.Commit();

                return new()
                {
                    InnerBody = new
                    {
                        Function = function,
                        Menu = menu
                    }
                };

            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public async Task<FormatedResponse> ReadAllWithAllActions()
        {
            try
            {
                var functions = await (
                                from l in _dbContext.SysFunctions.AsNoTracking()
                                from m in _dbContext.SysModules.AsNoTracking().Where(x => x.ID == l.MODULE_ID)

                                //where l.ROOT_ONLY != true // to be filtered on Frontend

                                select new SysFunctionDTO
                                {
                                    Id = l.ID,
                                    ModuleId = l.MODULE_ID,
                                    ModuleCode = m.CODE,
                                    GroupId = l.GROUP_ID,
                                    Code = l.CODE,
                                    RootOnly = l.ROOT_ONLY,
                                    Name = l.NAME,
                                    Path = l.PATH,
                                    PathFullMatch = l.PATH_FULL_MATCH,
                                    IsActive = l.IS_ACTIVE,
                                    //CreatedDate = l.CREATED_DATE,
                                    //CreatedBy = l.CREATED_BY,
                                    //UpdatedDate = l.UPDATED_DATE,
                                    //UpdatedBy = l.UPDATED_BY
                                }).ToListAsync();

                functions.ForEach(function =>
                {
                    List<string> codes = new();
                    var actions = from fa in _dbContext.SysFunctionActions.AsNoTracking().Where(x => x.FUNCTION_ID == function.Id)
                                  from a in _dbContext.SysActions.AsNoTracking().Where(x => x.ID == fa.ACTION_ID).DefaultIfEmpty()
                                  select new
                                  {
                                      code = a.CODE
                                  };
                    actions?.ToList().ForEach(a => codes.Add(a.code));;
                    function.actionCodes = codes;

                });

                return new() { InnerBody = functions };
            } catch (Exception ex)
            {
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

    }
}

