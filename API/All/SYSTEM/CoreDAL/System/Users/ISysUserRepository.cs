using API.Entities;
using Common.Extensions;
using Common.Interfaces;
using Common.Paging;
using CoreDAL.Models;

namespace CoreDAL.Repositories
{
    public interface ISysUserRepository : IRepository<SysUser>
    {
        Task<PagedResult<SysUserDTO>> GetAll(SysUserDTO param);
        Task<object> CreateUserAsync(SysUserInputDTO user);
        Task<ResultWithError> UpdateUserAsync(SysUserInputDTO user);
        Task<ResultWithError> ChangePasswordAsync(string user, string currentPassword, string newPassword);
        Task<ResultWithError> SetLockoutEnabledAsync(string user, bool enable);
        Task<ResultWithError> GetPermissonByUser(string userId);
        Task<ResultWithError> GetList();
        Task<ResultWithError> ClientsLogin(string UserName, string password, string ipAddress, bool alreadtHased = false, SYS_REFRESH_TOKEN newRefreshToken = null);

        Task<ResultWithError> GetByAccountName(string UserName, bool alreadtHased = false, SYS_REFRESH_TOKEN newRefreshToken = null);


    }
}
