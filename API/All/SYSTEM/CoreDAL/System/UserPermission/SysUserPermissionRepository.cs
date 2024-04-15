using Common.Paging;
using CoreDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace CoreDAL.Repositories
{
    public class SysUserPermissionRepository : RepositoryBase<SYS_USER_PERMISSION>, ISysUserPermissionRepository
    {
        private CoreDbContext _appContext => (CoreDbContext)_context;
        public SysUserPermissionRepository(CoreDbContext context) : base(context)
        {

        }

        /// <summary>
        /// Get pagesing Data AspGroupPermission.
        /// </summary>
        public async Task<PagedResult<SysUserPermissionDTO>> GetAll(SysUserPermissionDTO param)
        {
            var queryable = (from p in _appContext.AspUserPermissions
                             join g in _appContext.SysUsers on p.USER_ID equals g.ID
                             join f in _appContext.SysFunctions on p.FUNCTION_ID equals f.ID
                             join m in _appContext.SysModules on f.MODULE_ID equals m.ID
                             select new SysUserPermissionDTO
                             {
                                 UserId = p.USER_ID,
                                 FunctionId = p.FUNCTION_ID,
                                 UseName = g.USERNAME,
                                 FunctionName = f.NAME,
                                 PermissionStr = p.PERMISSION_STRING,
                                 ModuleId = f.MODULE_ID,
                                 ModuleName = m.NAME
                             });
            if (param.UserId != null)
            {
                queryable = queryable.Where(p => p.UserId == param.UserId);
            }

            if (param.ModuleId > 0)
            {
                queryable = queryable.Where(p => p.ModuleId == param.ModuleId);
            }

            if (param.ModuleName != null)
            {
                queryable = queryable.Where(p => p.ModuleName.ToUpper().Contains(param.ModuleName.ToUpper()));
            }

            if (param.FunctionId > 0)
            {
                queryable = queryable.Where(p => p.FunctionId == param.FunctionId);
            }

            if (param.FunctionName != null)
            {
                queryable = queryable.Where(p => p.FunctionName.ToUpper().Contains(param.FunctionName.ToUpper()));
            }

            return await PagingList(queryable, param);
        }
        /// <summary>
        /// Get All Data By User Or/And Function.
        /// </summary>
        public async Task<ResultWithError> GetBy(SysUserPermissionDTO param)
        {
            var queryable = (from p in _appContext.AspUserPermissions
                             join f in _appContext.SysFunctions on p.FUNCTION_ID equals f.ID
                             join m in _appContext.SysModules on f.MODULE_ID equals m.ID
                             select new SysUserPermissionDTO
                             {
                                 UserId = p.USER_ID,
                                 FunctionId = p.FUNCTION_ID,
                                 FunctionName = f.NAME,
                                 PermissionStr = p.PERMISSION_STRING,
                                 ModuleId = f.MODULE_ID,
                                 ModuleName = m.NAME
                             });
            if (param.UserId != null)
            {
                queryable = queryable.Where(p => p.UserId == param.UserId);
            }

            if (param.FunctionId > 0)
            {
                queryable = queryable.Where(p => p.FunctionId == param.FunctionId);
            }

            if (param.FunctionName != null)
            {
                queryable = queryable.Where(p => p.FunctionName.ToUpper().Contains(param.FunctionName.ToUpper()));
            }

            if (param.ModuleName != null)
            {
                queryable = queryable.Where(p => p.ModuleName.ToUpper().Contains(param.ModuleName.ToUpper()));
            }


            var list = await queryable.ToListAsync();
            return new ResultWithError(list);
        }
        /// <summary>
        /// Update Or Create Data By GroupUser and Function.
        /// </summary>
        public async Task<ResultWithError> UpdateAsync(List<UserPermissionInputDTO> param)
        {
            foreach (var item in param)
            {
                if (item.FunctionId != 0 && item.UserId != null)
                {
                    if (item.PermissionString.Trim().Length > 0)
                    {
                        var r = _appContext.AspUserPermissions.Where(x => x.USER_ID == item.UserId && x.FUNCTION_ID == item.FunctionId).FirstOrDefault();
                        if (r != null)
                        {
                            // update 
                            r.PERMISSION_STRING = item.PermissionString;
                            var result1 = _appContext.AspUserPermissions.Update(r);
                        }
                        else
                        {
                            // insert
                            var data = Map(item, new SYS_USER_PERMISSION());
                            await _appContext.AspUserPermissions.AddAsync(data);
                        }
                    }
                    else
                    {
                        var r = _appContext.AspUserPermissions.Where(x => x.USER_ID == item.UserId && x.FUNCTION_ID == item.FunctionId).FirstOrDefault();
                        if (r != null)
                        {
                            _appContext.AspUserPermissions.Remove(r);
                        }
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
                                 join g in _appContext.SysGroupFunctions on p.GROUP_ID equals g.ID
                                 join m in _appContext.SysModules on p.MODULE_ID equals m.ID
                                 join f in _appContext.AspUserPermissions.Where(c => c.USER_ID == param.UserId) on p.ID equals f.FUNCTION_ID into tmp1
                                 from f2 in tmp1.DefaultIfEmpty()
                                 select new GridFunctionOutput
                                 {
                                     UserId = param.UserId,
                                     FunctionId = p.ID,
                                     FunctionCode = p.CODE,
                                     FunctionName = p.NAME,
                                     GroupFuntionId = g.ID,
                                     GroupFunctionName = g.NAME,
                                     ModuleName = m.NAME,
                                     IsAdd = f2.PERMISSION_STRING.Contains("ADD") ? true : false,
                                     IsEdit = f2.PERMISSION_STRING.Contains("EDIT") ? true : false,
                                     IsDelete = f2.PERMISSION_STRING.Contains("DELETE") ? true : false,
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
