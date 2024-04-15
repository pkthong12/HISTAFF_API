using Common.Paging;
using Common.Repositories;
using CoreDAL.Models;
using Microsoft.AspNetCore.Identity;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;
using CoreDAL.Utilities;
using Common.Interfaces;
//using Common.Interfaces;

namespace CoreDAL.Repositories
{
    public class SysUserRepository : RepositoryBase<SYS_USER>, ISysUserRepository
    {
        private CoreDbContext _appContext => (CoreDbContext)_context;
        private readonly UserManager<SysUser> _userManager;
        private IRefreshTokenService _refreshTokenService;
        public SysUserRepository(CoreDbContext context, UserManager<SysUser> userManager, IRefreshTokenService refreshTokenService) : base(context)
        {
            _userManager = userManager;
            _refreshTokenService = refreshTokenService;
        }

        async Task<PagedResult<SysUserDTO>> ISysUserRepository.GetAll(CoreDAL.Models.SysUserDTO param)
        {
            var queryable = (from p in _appContext.SysUsers
                             join g in _appContext.SysGroupUsers on p.GROUP_ID equals g.ID
                             select new SysUserDTO
                             {
                                 Id = p.ID,
                                 UserName = p.USERNAME,
                                 FullName = p.FULLNAME,
                                 GroupId = g.ID,
                                 Email = p.EMAIL,
                                 Avatar = p.AVATAR,
                                 GroupName = g.NAME,
                                 Lock = p.LOCKOUTENABLED,
                                 CreatedBy = p.CREATED_BY,
                                 CreatedDate = p.CREATED_DATE,
                                 UpdatedBy = p.UPDATED_BY,
                                 UpdatedDate = p.UPDATED_DATE
                             });
            if (param.UserName != null)
            {
                queryable = queryable.Where(p => p.UserName.ToUpper().Contains(param.UserName.ToUpper()));
            }

            if (param.FullName != null)
            {
                queryable = queryable.Where(p => p.FullName.ToUpper().Contains(param.FullName.ToUpper()));
            }

            if (param.GroupName != null)
            {
                queryable = queryable.Where(p => p.GroupName.ToUpper().Contains(param.GroupName.ToUpper()));
            }

            if (param.GroupId != null)
            {
                queryable = queryable.Where(p => p.GroupId == param.GroupId);
            }

            return await PagingList(queryable, param);
        }
        async Task<object> ISysUserRepository.CreateUserAsync(SysUserInputDTO param)
        {
            var data = Map(param, new SysUser());
            data.UserName = data.UserName.Trim().ToLower();
            data.LockoutEnabled = false;
            var r = _appContext.SysUsers.Where(x => x.USERNAME == param.UserName).Count();
            if (r > 0)
            {
                return new ResultWithError(409);
            }

            var result = await _userManager.CreateAsync(data, param.Password);
            var user = await _userManager.FindByNameAsync(data.UserName);
            var res = new { data = user, statusCode = result.Succeeded ? 200 : 400, message = result.Errors };

            return res;
        }

        async Task<ResultWithError> ISysUserRepository.UpdateUserAsync(SysUserInputDTO  param)
        {
            //var r = _appContext.SysUsers.Where(x => x.Id == param.Id).FirstOrDefault();
            //if (r == null)
            //{
            //    return new ResultWithError(404);
            //}
            //var data = Map(param, r);
            //if (data != null)
            //{
            //    var result = await _userManager.UpdateAsync(data);
            //}
            
            return new ResultWithError(200);
        }

        async Task<ResultWithError> ISysUserRepository.ChangePasswordAsync(string user, string currentPassword, string newPassword)
        {
            var r = await _userManager.FindByNameAsync(user.ToLower());
            if (r == null)
            {
                return new ResultWithError(404);
            }
            var result = await _userManager.CheckPasswordAsync(r, currentPassword);
            if (!result)
            {
                return new ResultWithError("CURRENT_PASS_NOT_CORRECT");
            }
            try
            {
                var x = await _userManager.ChangePasswordAsync(r, currentPassword, newPassword);
            }
            catch
            {
                return new ResultWithError("PASS_VALID");
            }

            return new ResultWithError(200);
        }
        async Task<ResultWithError> ISysUserRepository.SetLockoutEnabledAsync(string user, bool enable)
        {
            var r = await _userManager.FindByNameAsync(user.ToLower());
            if (r == null)
            {
                return new ResultWithError(404);
            }
            var x = await _userManager.SetLockoutEnabledAsync(r, enable);
            return new ResultWithError(200);
        }
        /// <summary>
        /// Get Data Permission by User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        async Task<ResultWithError> ISysUserRepository.GetPermissonByUser(string userId)
        {
            var r = await (from p in _appContext.SysUsers
                           join g in _appContext.SysGroupUsers on p.GROUP_ID equals g.ID
                           where p.ID == userId
                           select new LoginParam
                           {
                               IsAdmin = g.IS_ADMIN,
                               FullName = p.FULLNAME,
                               UserName = p.USERNAME,
                               Avatar = p.AVATAR
                           }).FirstOrDefaultAsync();
            if (r == null)
            {
                return new ResultWithError("USER_NOT_EXISTS");
            }
            r.Id = userId;
            if (r.IsAdmin == true)
            {
                var dataPermission = await (from p in _appContext.SysFunctions
                                            join m in _appContext.SysModules on p.MODULE_ID equals m.ID
                                            join g in _appContext.SysGroupFunctions on p.GROUP_ID equals g.ID
                                            where m.APPLICATION_ID == Consts.APPLICATION_SYSTEM
                                            select new PermissionParam
                                            {
                                                ModuleCode = m.CODE,
                                                GroupFuncCode = g.CODE,
                                                FunctionCode = p.CODE
                                            }).ToListAsync();
                r.PermissionParams = dataPermission;


            }
            else
            {
                var dataPermission = await (from p in _appContext.AspUserPermissions
                                            join f in _appContext.SysFunctions on p.FUNCTION_ID equals f.ID
                                            join m in _appContext.SysModules on f.MODULE_ID equals m.ID
                                            join g in _appContext.SysGroupFunctions on f.GROUP_ID equals g.ID
                                            where p.USER_ID == userId
                                            select new PermissionParam
                                            {
                                                ModuleCode = m.CODE,
                                                GroupFuncCode = g.CODE,
                                                FunctionCode = f.CODE
                                            }).ToListAsync();
                r.PermissionParams = dataPermission;


            }

            return new ResultWithError(r);
        }
        /// <summary>
        /// CMS Get Detail Data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        async Task<ResultWithError> ISysUserRepository.GetList()
        {
            var r = await (from p in _appContext.SysUsers
                           from c in _appContext.SysGroupUsers.Where(f => f.ID == p.GROUP_ID)
                           where c.IS_ACTIVE == true && c.IS_ADMIN == false
                           orderby p.USERNAME
                           select new
                           {
                               Id = p.ID,
                               Name = p.USERNAME
                           }).ToListAsync();
            return new ResultWithError(r);
        }
        /// <summary>
        /// Tenant Login 
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="password"></param>
        /// <param name="ipAddress"></param>
        /// <param name="alreadtHased"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        async Task<ResultWithError> ISysUserRepository.ClientsLogin(string UserName, string password, string ipAddress, bool alreadtHased = false, SYS_REFRESH_TOKEN refreshToken = null)
        {
            try
            {
                UserName = UserName.ToLower().Trim();
                try
                {
                    var r = await _appContext.SysUsers.Where(p => p.USERNAME!.ToLower() == UserName).FirstOrDefaultAsync();
                    if (r != null)
                    {

                        if (BCrypt.Net.BCrypt.Verify(password, r.PASSWORDHASH))
                        {
                            var userID = r.ID;
                            var data = new AuthResponse()
                            {
                                Id = r.ID,
                                UserName = r.USERNAME!,
                                FullName = r.FULLNAME!,
                                IsAdmin = r.IS_ADMIN,
                                IsRoot = r.IS_ROOT,
                                IsLock = r.IS_LOCK,
                                IsWebapp = r.IS_WEBAPP,
                                IsPortal = r.IS_PORTAL,
                                Avatar = r.AVATAR!,
                                IsFirstLogin = r.IS_FIRST_LOGIN,
                                EmployeeId = r.EMPLOYEE_ID,
                            };
                            /* PermissionParams depricated */
                            /*
                            data.PermissionParams = await _appContext.AspUserPermissions.Where(x => x.USER_ID == r.ID).Select(o => new PermissionParam
                            {
                                PermissionString = o.PERMISSION_STRING

                            }).ToListAsync();
                            */
                            if (refreshToken == null) refreshToken = await _refreshTokenService.UpdateRefreshTokens(userID, ipAddress);
                            data.RefreshToken = refreshToken;

                            if (refreshToken != null)
                            {
                                // neu lan dau dang nhap thi them bang payroll_sum vao DB
                                return new ResultWithError(data);
                            }
                            else
                            {
                                return new ResultWithError("Could not update RefreshToken");
                            }
                        }
                        else
                        {
                            return new ResultWithError("ERROR_PASSWORD_INCORRECT");
                        }
                    }
                    else
                    {
                        return new ResultWithError("ERROR_USERNAME_INCORRECT");
                    }
                }
                catch (Exception ex)
                {
                    return new ResultWithError(ex.Message);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        async Task<ResultWithError> ISysUserRepository.GetByAccountName(string UserName,  bool alreadtHased = false, SYS_REFRESH_TOKEN refreshToken = null)
        {
            try
            {
                UserName = UserName.ToLower().Trim();
                try
                {
                    var r = await _appContext.SysUsers.Where(p => p.USERNAME!.ToLower() == UserName).FirstOrDefaultAsync();
                    if (r != null)
                    {
                        var userID = r.ID;
                        var data = new AuthResponse()
                        {
                            Id = r.ID,
                            UserName = r.USERNAME!,
                            FullName = r.FULLNAME!,
                            IsAdmin = r.IS_ADMIN,
                            IsRoot = r.IS_ROOT,
                            IsLock = r.IS_LOCK,
                            IsWebapp = r.IS_WEBAPP,
                            IsPortal = r.IS_PORTAL,
                            Avatar = r.AVATAR!,
                            IsFirstLogin = r.IS_FIRST_LOGIN,
                            EmployeeId = r.EMPLOYEE_ID,
                        };

                        return new ResultWithError(data);

                    }
                    else
                    {
                        return new ResultWithError("ERROR_USERNAME_INCORRECT");
                    }
                }
                catch (Exception ex)
                {
                    return new ResultWithError(ex.Message);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Task<SysUser> IRepository<SysUser>.Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultWithError> Add(SysUser entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddRange(IEnumerable<SysUser> entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(SysUser entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> Remove(SysUser entity)
        {
            throw new NotImplementedException();
        }
    }

}

