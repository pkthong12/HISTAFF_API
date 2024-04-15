using Common.Interfaces;
using CoreDAL.ViewModels;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace CoreDAL.Repositories
{
    public interface ISysUserPermissionRepository : IRepository<SYS_USER_PERMISSION>
    {
        /// <summary>
        /// Get pagesing Data AspUserPermission.
        /// </summary>
        Task<PagedResult<SysUserPermissionDTO>> GetAll(SysUserPermissionDTO param);
        /// <summary>
        /// Get All Data By Uer Or/And Function.
        /// </summary>
        Task<ResultWithError> GetBy(SysUserPermissionDTO param);
        /// <summary>
        /// Update Or Create Data By User and Function.
        /// </summary>
        Task<ResultWithError> UpdateAsync(List<UserPermissionInputDTO> param);
        /// <summary>
        /// Get permission bu grroup user
        /// 
        /// </summary>
        Task<PagedResult<GridFunctionOutput>> GridPermission(GridFunctionInput param);
    }
}
