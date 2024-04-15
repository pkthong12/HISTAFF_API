using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.SysMenu
{
    public class SysMenuRepository : ISysMenuRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<SYS_MENU, SysMenuDTO> _genericRepository;
        private readonly GenericReducer<SYS_MENU, SysMenuDTO> _genericReducer;

        public SysMenuRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = uow.GenericRepository<SYS_MENU, SysMenuDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<SysMenuDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysMenuDTO> request)
        {
            var joined = from p in _dbContext.SysMenus.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                             // FOR EXAMPLE
                         from f in _dbContext.SysFunctions.AsNoTracking().Where(x => x.ID == p.FUNCTION_ID).DefaultIfEmpty()
                         from c in _dbContext.SysUsers.AsNoTracking().Where(x => x.ID == p.CREATED_BY).DefaultIfEmpty()
                         from u in _dbContext.SysUsers.AsNoTracking().Where(x => x.ID == p.CREATED_BY).DefaultIfEmpty()
                         select new SysMenuDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             FunctionName = f.CODE + " - " + f.NAME,
                             Url = p.URL,
                             Parent = p.PARENT,
                             IconClass = p.ICON_CLASS,
                             OrderNumber = p.ORDER_NUMBER,
                             CreatedDate = p.CREATED_DATE,
                             CreatedByUsername = c.USERNAME,
                             UpdatedDate = p.UPDATED_DATE,
                             UpdatedByUsername = u.USERNAME
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAllActive()
        {
            try
            {
                var sysMenus = await (from sm in _dbContext.SysMenus.AsNoTracking().Where(x => x.INACTIVE != true)

                                      select new SysMenuDTO()
                                      {
                                          Id = sm.ID,
                                          Code = sm.CODE,
                                          OrderNumber = sm.ORDER_NUMBER ?? 9999,
                                          IconClass = sm.ICON_CLASS,
                                          SysMenuServiceMethod = sm.SYS_MENU_SERVICE_METHOD,
                                          Url = sm.URL,
                                          Parent = sm.PARENT,
                                          Protected = false,
                                          RootOnly = sm.ROOT_ONLY
                                      }).OrderByDescending(x => x.OrderNumber).ToListAsync();

                return new() { InnerBody = sysMenus };

            }
            catch (Exception ex)
            {
                return new() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public async Task<FormatedResponse> ReadAll()
        {
            var joined = await (from p in _dbContext.SysMenus.AsNoTracking()
                                    // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                                    // FOR EXAMPLE
                                from f in _dbContext.SysFunctions.AsNoTracking().Where(x => x.ID == p.FUNCTION_ID).DefaultIfEmpty()
                                from c in _dbContext.SysUsers.AsNoTracking().Where(x => x.ID == p.CREATED_BY).DefaultIfEmpty()
                                from u in _dbContext.SysUsers.AsNoTracking().Where(x => x.ID == p.CREATED_BY).DefaultIfEmpty()
                                select new SysMenuDTO
                                {
                                    Id = p.ID,
                                    Code = p.CODE,
                                    Url = p.URL,
                                    Parent = p.PARENT,
                                    IconClass = p.ICON_CLASS,
                                    OrderNumber = p.ORDER_NUMBER,
                                    FunctionName = f.CODE + " - " + f.NAME,
                                    CreatedDate = p.CREATED_DATE,
                                    CreatedByUsername = c.USERNAME,
                                    UpdatedDate = p.UPDATED_DATE,
                                    UpdatedByUsername = u.USERNAME
                                }).OrderBy(x => x.OrderNumber ?? 9999).ToListAsync();
            return new FormatedResponse()
            {
                InnerBody = joined
            };
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
            try
            {
                var res = await _genericRepository.GetById(id);
                if (res.InnerBody != null)
                {
                    var response = res.InnerBody;
                    var list = new List<SYS_MENU>
                    {
                        (SYS_MENU)response
                    };
                    var joined = (from l in list
                                      // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                                      // FOR EXAMPLE
                                  from c in _uow.Context.Set<SYS_USER>().AsNoTracking().ToList().Where(c => c.ID == l.CREATED_BY).DefaultIfEmpty()
                                  from u in _uow.Context.Set<SYS_USER>().AsNoTracking().ToList().Where(u => u.ID == l.UPDATED_BY).DefaultIfEmpty()
                                  select new SysMenuDTO
                                  {
                                      Id = l.ID,
                                      Code = l.CODE,
                                      OrderNumber = l.ORDER_NUMBER,
                                      IconFontFamily = l.ICON_FONT_FAMILY,
                                      IconClass = l.ICON_CLASS,
                                      SysMenuServiceMethod = l.SYS_MENU_SERVICE_METHOD,
                                      Url = l.URL,
                                      Parent = l.PARENT,
                                      FunctionId = l.FUNCTION_ID,
                                      Inactive = l.INACTIVE,
                                      CreatedDate = l.CREATED_DATE,
                                      UpdatedDate = l.UPDATED_DATE,
                                      CreatedByUsername = c?.USERNAME,
                                      UpdatedByUsername = u?.USERNAME
                                  }).FirstOrDefault();

                    return new FormatedResponse() { InnerBody = joined };
                }
                else
                {
                    return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, SysMenuDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<SysMenuDTO> dtos, string sid)
        {
            var response = await _genericRepository.CreateRange(_uow, dtos, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, SysMenuDTO dto, string sid, bool patchMode = true)
        {


            // if INACTIVE status changes, we need to update this for all children
            _uow.CreateTransaction();

            try
            {

                var response = await _genericRepository.Update(_uow, dto, sid, patchMode);

                if (response.StatusCode == EnumStatusCode.StatusCode200)
                {
                    var children = _dbContext.SysMenus.Where(x => x.PARENT == dto.Id);
                    if (children != null)
                    {
                        List<SYS_MENU> entities = children.ToList();
                        List<SysMenuDTO> dtos = new();
                        entities.ForEach(e =>
                        {
                            var dto = CoreMapper<SysMenuDTO, SYS_MENU>.EntityToDto(e, new SysMenuDTO());
                            if (dto != null)
                            {
                                dtos.Add(dto);
                            }
                        });

                        var response1 = await _genericRepository.UpdateRange(_uow, dtos, sid, patchMode);

                        if (response1.StatusCode == EnumStatusCode.StatusCode200)
                        {
                            _uow.Commit();
                            return response;
                        }
                        else
                        {
                            _uow.Rollback();
                            return new() { StatusCode = response.StatusCode, MessageCode = CommonMessageCode.POST_UPDATE_FAILLED, ErrorType = EnumErrorType.CATCHABLE };
                        }

                    }
                    else
                    {
                        return response;
                    }
                }
                else
                {
                    _uow.Rollback();
                    return new() { StatusCode = response.StatusCode, MessageCode = CommonMessageCode.PRE_UPDATE_FAILLED, ErrorType = EnumErrorType.CATCHABLE };
                }
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return new() { StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE };

            }
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<SysMenuDTO> dtos, string sid, bool patchMode = true)
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
            var response = await _genericRepository.Delete(_uow, id);
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<long> ids)
        {
            var response = await _genericRepository.DeleteIds(_uow, ids);
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            var response = await _genericRepository.DeleteIds(_uow, ids);
            return response;
        }

        public async Task<FormatedResponse> GetPermittedLinearList(string userId)
        {
            try
            {
                var user = await _dbContext.SysUsers.AsNoTracking().SingleAsync(x => x.ID == userId);
                if (user == null) return new()
                {
                    MessageCode = CommonMessageCode.USER_DOES_NOT_EXIST,
                    ErrorType = EnumErrorType.CATCHABLE,
                    StatusCode = EnumStatusCode.StatusCode400,
                };

                var readAllActive = await ReadAllActive();
                if (readAllActive.StatusCode != EnumStatusCode.StatusCode200) {
                    return new()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        StatusCode = EnumStatusCode.StatusCode400,
                        MessageCode = readAllActive.MessageCode
                    };
                }
                if (readAllActive.InnerBody == null)
                {
                    return new()
                    {
                        InnerBody = new List<SysMenuDTO>()
                    };
                }

                List<SysMenuDTO> currentList;
                List<SysMenuDTO> wholeList = (List<SysMenuDTO>)readAllActive.InnerBody;

                if (user.IS_ROOT == true)
                {
                    return new()
                    {
                        InnerBody = wholeList
                    };
                } else if (user.IS_ADMIN)
                {
                    return new()
                    {
                        InnerBody = wholeList.Where(x => x.RootOnly != true)
                    };
                }

                // By default given from SysUserFunctionActions
                var permittedFunctions = _dbContext.SysUserFunctionActions.Where(x => x.USER_ID == userId)
                                            .GroupBy(x => x.FUNCTION_ID).Select(x => new { Id = x.Key }).ToList();

                // But if no result given from SysGroupFunctionActions
                if (permittedFunctions == null || permittedFunctions.Count == 0)
                {
                    permittedFunctions = _dbContext.SysGroupFunctionActions.Where(x => x.GROUP_ID == user.GROUP_ID)
                                            .GroupBy(x => x.FUNCTION_ID).Select(x => new { Id = x.Key }).ToList();
                }


                if (permittedFunctions != null)
                {
                    currentList = new();
                    permittedFunctions.ForEach(function =>
                    {
                        var filtered = _dbContext.SysMenus.Where(x => x.FUNCTION_ID == function.Id && x.INACTIVE != true && x.ROOT_ONLY != true).Select(x => new SysMenuDTO()
                        {
                            Id = x.ID,
                            Code = x.CODE,
                            OrderNumber = x.ORDER_NUMBER,
                            IconFontFamily = x.ICON_FONT_FAMILY,
                            IconClass = x.ICON_CLASS,
                            SysMenuServiceMethod = x.SYS_MENU_SERVICE_METHOD,
                            Url = x.URL,
                            Parent = x.PARENT,
                            FunctionId = x.FUNCTION_ID,
                            Inactive = x.INACTIVE,
                            Protected = false
                        });
                        if (filtered != null)
                        {
                            currentList.AddRange(filtered);
                        }
                    });

                    var count = currentList.Count;

                    for (var i = 0; i < count; i++)
                    {
                        var currentChild = currentList[i];
                        LoopInsertProtectedParentMenuItem(ref wholeList, ref currentList, currentChild);
                    }

                    /*
                    currentList.ForEach(currentChild =>
                    {
                        LoopInsertProtectedParentMenuItem(ref wholeList, ref currentList, currentChild);
                    });
                    */


                    return new FormatedResponse()
                    {
                        InnerBody = currentList.OrderByDescending(x => x.OrderNumber)
                    };

                }
                else
                {
                    return new()
                    {
                        InnerBody = new List<SysMenuDTO>()
                    };
                }

            }
            catch (Exception ex)
            {
                return new()
                {
                    MessageCode = ex.Message,
                    ErrorType = EnumErrorType.UNCATCHABLE,
                    StatusCode = EnumStatusCode.StatusCode500,
                };
            }
        }

        private void LoopInsertProtectedParentMenuItem(ref List<SysMenuDTO> wholeList, ref List<SysMenuDTO> currentList, SysMenuDTO currentChild)
        {
            if (currentChild.Parent != null)
            {
                var parentInCurrentList = currentList.Where(x => x.Id == currentChild.Parent).FirstOrDefault();
                if (parentInCurrentList == null)
                {
                    var parentInWholeList = wholeList.Where(x => x.Id == currentChild.Parent).FirstOrDefault();
                    if (parentInWholeList != null)
                    {
                        parentInWholeList.Protected = true;
                        // And remove the URL for better protection
                        parentInWholeList.Url = null;
                        currentList.Add(parentInWholeList);
                        LoopInsertProtectedParentMenuItem(ref wholeList, ref currentList, parentInWholeList);
                    }
                }
                else
                {
                    // By rule, any parent do not have URL
                    parentInCurrentList.Url = null;
                    LoopInsertProtectedParentMenuItem(ref wholeList, ref currentList, parentInCurrentList);
                }
            }
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }
    }
}

