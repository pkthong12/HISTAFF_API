using Common.Interfaces;
using CoreDAL.ViewModels;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace CoreDAL.Repositories
{
    public interface ISysGroupPermissionRepository : IRepository<SYS_GROUP_PERMISSION>
    {
        /// <summary>
        /// Get pagesing Data AspGroupPermission.
        /// </summary>
        Task<PagedResult<SysGroupPermissionDTO>> GetAll(SysGroupPermissionDTO param);
        /// <summary>
        /// Get All Data By GroupUer Or/And Function.
        /// </summary>
        Task<ResultWithError> GetBy(SysGroupPermissionDTO param);
        /// <summary>
        /// Update Or Create Data By GroupUser and Function.
        /// </summary>
        Task<ResultWithError> UpdateAsync(List<GroupPermissionInputDTO> param);

        /// <summary>
        /// Get permission bu grroup user
        /// 
        /// </summary>
        Task<PagedResult<GridFunctionOutput>> GridPermission(GridFunctionInput param);
    }
}
