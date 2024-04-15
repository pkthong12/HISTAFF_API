using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using API.All.SYSTEM.CoreAPI.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Controllers.SysGroup
{
    public class SysGroupRepository : ISysGroupRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<SYS_GROUP, SysGroupDTO> _genericRepository;
        private readonly GenericReducer<SYS_GROUP, SysGroupDTO> _genericReducer;

        public SysGroupRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<SYS_GROUP, SysGroupDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<SysGroupDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysGroupDTO> request)
        {
            var joined = from p in _dbContext.SysGroups.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         where p.IS_SYSTEM != true
                         select new SysGroupDTO
                         {
                             Id = p.ID,
                             Name = p.NAME,
                             Note = p.NOTE,
                             Code = p.CODE
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            var response = await _genericRepository.ReadAll();
            return response;
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
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<SYS_GROUP>
                    {
                        (SYS_GROUP)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new SysGroupDTO
                              {
                                  Id = l.ID,
                                  Name = l.NAME,
                                  Note = l.NOTE,
                                  Code = l.CODE
                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, SysGroupDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> Clone(GenericUnitOfWork _uow, SysGroupDTO dto, string sid)
        {
            _uow.CreateTransaction();
            try
            {
                var response = await _genericRepository.Create(_uow, dto, sid);
                if (response.StatusCode == EnumStatusCode.StatusCode200)
                {
                    if (response.InnerBody != null)
                    {
                        SYS_GROUP newEntity = (SYS_GROUP)response.InnerBody;
                        var groupId = newEntity.ID;

                        var cloneSrc = _dbContext.SysGroups.Single(x => x.NAME == dto.CloneFrom);

                        // insert org permission
                        var cloneOrgPermissions = await _dbContext.SysUserGroupOrgs.Where(x => x.GROUP_ID == cloneSrc.ID).ToListAsync();
                        if (cloneOrgPermissions != null)
                        {
                            cloneOrgPermissions?.ForEach(item =>
                            {
                                item.ID = 0;
                                item.ORG_ID = groupId;
                            });
                            await _dbContext.SysUserGroupOrgs.AddRangeAsync(cloneOrgPermissions);
                            _dbContext.SaveChanges();
                        }

                        // insert function-action permission
                        var cloneFunctionActionPermissions = await _dbContext.SysGroupFunctionActions.Where(x => x.GROUP_ID == cloneSrc.ID).ToListAsync();
                        if (cloneFunctionActionPermissions != null)
                        {
                            cloneFunctionActionPermissions?.ForEach(item =>
                            {
                                item.ID = 0;
                                item.GROUP_ID = groupId;
                            });
                            await _dbContext.SysGroupFunctionActions.AddRangeAsync(cloneFunctionActionPermissions);
                            _dbContext.SaveChanges();
                        }

                        // Commit the changes
                        _uow.Commit();

                        return new() { InnerBody = newEntity };

                    }
                    else
                    {
                        _uow.Rollback();
                        return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.CREATED_FAILD };
                    }
                }
                else
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<SysGroupDTO> dtos, string sid)
        {
            var add = new List<SysGroupDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, SysGroupDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<SysGroupDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> QueryOrgPermissionList(long groupId)
        {
            try
            {
                var entity = _dbContext.HuOrganizations.AsNoTracking().AsQueryable();
                var orgsForCurGroup = await (from suo in _dbContext.SysUserGroupOrgs.AsQueryable()
                                             where suo.GROUP_ID == groupId
                                             select new
                                             {
                                                 OrgId = suo.ORG_ID
                                             }).OrderBy(x => x.OrgId).ToListAsync();

                List<long> orgIds = new();
                orgsForCurGroup.ForEach(item => orgIds.Add(item.OrgId));

                return new()
                {
                    InnerBody = orgIds
                };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }
        public async Task<FormatedResponse> QueryFunctionActionPermissionList(long groupId)
        {
            try
            {
                var raw = from gfa in _dbContext.SysGroupFunctionActions.AsNoTracking()
                                .Where(x => x.GROUP_ID == groupId)
                          from fa in _dbContext.SysFunctionActions.AsNoTracking().Where(x => x.FUNCTION_ID == gfa.FUNCTION_ID && x.ACTION_ID == gfa.ACTION_ID)
                          from f in _dbContext.SysFunctions.AsNoTracking().Where(x => x.ID == gfa.FUNCTION_ID).DefaultIfEmpty()
                          from m in _dbContext.SysModules.AsNoTracking().Where(x => x.ID == f.MODULE_ID).DefaultIfEmpty()
                          from a in _dbContext.SysActions.AsNoTracking().Where(x => x.ID == gfa.ACTION_ID).DefaultIfEmpty()

                          where fa != null && f.ROOT_ONLY != true

                          select new SysGroupFunctionActionDTO()
                          {
                              ModuleCode = m.CODE,
                              FunctionId = gfa.FUNCTION_ID,
                              FunctionCode = f.CODE,
                              ActionId = gfa.ACTION_ID,
                              ActionCode = a.CODE
                          };

                List<FunctionActionPermissionDTO> result = new();
                var list = await raw.ToListAsync();

                long? functionId = null;
                string moduleCode = "";
                string functionCode = "";
                string functionUrl = "";
                List<long> actionIds = new();
                List<string> actionCodes = new();
                list.ForEach(r =>
                {
                    moduleCode = r.ModuleCode ?? "";
                    functionCode = r.FunctionCode ?? "";
                    functionUrl = r.FunctionUrl ?? "";
                    if (r.FunctionId != functionId)
                    {
                        if (functionId != null)
                        {
                            result.Add(new FunctionActionPermissionDTO()
                            {
                                FunctionId = (long)functionId,
                                AllowedActionIds = actionIds,
                                ModuleCode = moduleCode,
                                FunctionCode = functionCode,
                                FunctionUrl = functionUrl,
                                AllowedActionCodes = actionCodes
                            });
                            actionIds = new();
                            actionCodes = new();
                            functionId = r.FunctionId;
                        }
                    }
                    functionId = r.FunctionId;
                    actionIds.Add((r.ActionId ?? 0));
                    actionCodes.Add(r.ActionCode ?? "");
                });
                // The tail
                if (functionId != null)
                {
                    result.Add(new FunctionActionPermissionDTO()
                    {
                        FunctionId = (long)functionId,
                        AllowedActionIds = actionIds,
                        ModuleCode = moduleCode,
                        FunctionCode = functionCode,
                        FunctionUrl = functionUrl,
                        AllowedActionCodes = actionCodes
                    });
                }
                return new() { InnerBody = result };
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500, MessageCode = ex.Message };
            }
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> DeleteAllFunctionActionPermissionByGroupId(long groupId)
        {
            try
            {
                var getListFunctionActionPermission = await _dbContext.SysGroupFunctionActions.Where(x => x.GROUP_ID == groupId).ToListAsync();
                var getListSysUserGroupOrg = await _dbContext.SysUserGroupOrgs.Where(x => x.GROUP_ID == groupId).ToListAsync();
                if (getListFunctionActionPermission.Count > 0 || getListSysUserGroupOrg.Count > 0)
                {
                    _dbContext.SysGroupFunctionActions.RemoveRange(getListFunctionActionPermission);
                    _dbContext.SysUserGroupOrgs.RemoveRange( getListSysUserGroupOrg);
                    _dbContext.SaveChanges();
                    return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode200, MessageCode = CommonMessageCode.DELETE_MULTIPLE_RECORD_SUCCESS };
                }
                else
                {
                    return new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ENTITIES_NOT_FOUND};
                }
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }

    }

}

