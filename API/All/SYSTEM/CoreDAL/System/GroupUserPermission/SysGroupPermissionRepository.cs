using Common.Paging;
using CoreDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace CoreDAL.Repositories
{
    public class SysGroupPermissionRepository : RepositoryBase<SYS_GROUP_PERMISSION>, ISysGroupPermissionRepository
    {
        private CoreDbContext _appContext => (CoreDbContext)_context;
        public SysGroupPermissionRepository(CoreDbContext context) : base(context)
        {

        }

        /// <summary>
        /// Get paging Data AspGroupPermission.
        /// </summary>
        public async Task<PagedResult<SysGroupPermissionDTO>> GetAll(SysGroupPermissionDTO param)
        {
            var queryable = (from p in _appContext.SysGroupPermissions
                             join g in _appContext.SysGroupUsers on p.GROUP_ID equals g.ID
                             join f in _appContext.SysFunctions on p.FUNCTION_ID equals f.ID
                             select new SysGroupPermissionDTO
                             {
                                 UserGroupId = p.GROUP_ID,
                                 FunctionId = p.FUNCTION_ID,
                                 ModuleId = f.MODULE_ID,
                                 UseGroupName = g.NAME,
                                 FunctionName = f.NAME,
                                 PermissionStr = p.PERMISSION_STRING
                             });
            if (param.UserGroupId > 0)
            {
                queryable = queryable.Where(p => p.UserGroupId == param.UserGroupId);
            }

            if (param.FunctionId > 0)
            {
                queryable = queryable.Where(p => p.FunctionId == param.FunctionId);
            }

            if (param.ModuleId > 0)
            {
                queryable = queryable.Where(p => p.ModuleId == param.ModuleId);
            }

            if (param.UseGroupName != null)
            {
                queryable = queryable.Where(p => p.UseGroupName.ToUpper().Contains(param.UseGroupName.ToUpper()));
            }

            if (param.FunctionName != null)
            {
                queryable = queryable.Where(p => p.FunctionName.ToUpper().Contains(param.FunctionName.ToUpper()));
            }

            if (param.PermissionStr != null)
            {
                queryable = queryable.Where(p => p.PermissionStr.ToUpper().Contains(param.PermissionStr.ToUpper()));
            }

            return await PagingList(queryable, param);
        }

        /// <summary>
        /// Get All Data By GroupUer Or/And Function, module.
        /// </summary>
        public async Task<ResultWithError> GetBy(SysGroupPermissionDTO param)
        {
            var queryable = (from p in _appContext.SysGroupPermissions
                             join g in _appContext.SysFunctions on p.FUNCTION_ID equals g.ID
                             join m in _appContext.SysModules on g.MODULE_ID equals m.ID
                             orderby m.NAME, g.NAME
                             select new SysGroupPermissionDTO
                             {
                                 UserGroupId = p.GROUP_ID,
                                 FunctionId = p.FUNCTION_ID,
                                 FunctionName = g.NAME,
                                 ModuleId = g.MODULE_ID,
                                 ModuleName = m.NAME,
                                 PermissionStr = p.PERMISSION_STRING,
                             });
            if (param.UserGroupId > 0)
            {
                queryable = queryable.Where(p => p.UserGroupId == param.UserGroupId);
            }

            if (param.FunctionId > 0)
            {
                queryable = queryable.Where(p => p.FunctionId == param.FunctionId);
            }

            if (param.ModuleId > 0)
            {
                queryable = queryable.Where(p => p.ModuleId == param.FunctionId);
            }

            var list = await queryable.ToListAsync();
            return new ResultWithError(list);
        }

        /// <summary>
        /// Update Or Create Data By GroupUser and Function.
        /// </summary>
        public async Task<ResultWithError> UpdateAsync(List<GroupPermissionInputDTO> param)
        {
            foreach (var item in param)
            {
                if (item.FunctionId != 0 && item.GroupId != 0)
                {
                    if (item.PermissionString.Trim().Length > 0)
                    {
                        var r = _appContext.SysGroupPermissions.Where(x => x.GROUP_ID == item.GroupId && x.FUNCTION_ID == item.FunctionId).FirstOrDefault();
                       
                        if (r != null)
                        {
                            r.PERMISSION_STRING = item.PermissionString;
                            // update 
                            var result1 = _appContext.SysGroupPermissions.Update(r);
                        }
                        else
                        {
                            // insert
                            var data = Map(item, new SYS_GROUP_PERMISSION());
                            await _appContext.SysGroupPermissions.AddAsync(data);
                        }
                    }
                    else
                    {
                        var r = _appContext.SysGroupPermissions.Where(x => x.GROUP_ID == item.GroupId && x.FUNCTION_ID == item.FunctionId).FirstOrDefault();
                        _appContext.SysGroupPermissions.Remove(r);
                    }
                }
            }
            await _appContext.SaveChangesAsync();
            return new ResultWithError(200);
        }

        /// <summary>
        /// Get paging Data AspGroupPermission.
        /// </summary>
        public async Task<PagedResult<GridFunctionOutput>> GridPermission(GridFunctionInput param)
        {
            try
            {
                var queryable = (from p in _appContext.SysFunctions
                                 from g in _appContext.SysGroupFunctions.Where(c => c.ID == p.GROUP_ID).DefaultIfEmpty()
                                 from m in _appContext.SysModules.Where(c => c.ID == p.MODULE_ID).DefaultIfEmpty()
                                 from f in _appContext.SysGroupPermissions.Where(c => c.FUNCTION_ID == p.ID && c.GROUP_ID == param.UserGroupId).DefaultIfEmpty()
                                 select new GridFunctionOutput
                                 {
                                     UserGroupId = param.UserGroupId,
                                     FunctionId = p.ID,
                                     FunctionCode = p.CODE,
                                     FunctionName = p.NAME,
                                     GroupFuntionId = g.ID,
                                     GroupFunctionName = g.NAME,
                                     ModuleName = m.NAME,
                                     IsAdd = f.PERMISSION_STRING.Contains("ADD") ? true : false,
                                     IsEdit = f.PERMISSION_STRING.Contains("EDIT") ? true : false,
                                     IsDelete = f.PERMISSION_STRING.Contains("DELETE") ? true : false,
                                 }

                         );

                if (param.FunctionId != null)
                {
                    queryable = queryable.Where(p => p.FunctionId == param.FunctionId);
                }

                if (param.FunctionName != null)
                {
                    queryable = queryable.Where(p => p.FunctionName.ToUpper().Contains(param.FunctionName.ToUpper()));
                }

                if (!string.IsNullOrWhiteSpace(param.FunctionCode))
                {
                    queryable = queryable.Where(p => p.FunctionCode.ToUpper().Contains(param.FunctionCode.ToUpper()));
                }

                if (!string.IsNullOrWhiteSpace(param.FunctionName))
                {
                    queryable = queryable.Where(p => p.FunctionName.ToUpper().Contains(param.FunctionName.ToUpper()));
                }

                return await PagingList(queryable, param);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
