using Common.Interfaces;
using CoreDAL.ViewModels;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace CoreDAL.Repositories
{
    public interface ISysPermissionRepository : IRepository<SYS_PERMISSION>
    {
        Task<PagedResult<SysPermissionDTO>> GetAll(SysPermissionDTO param);
        Task<ResultWithError> CreateAsync(SysPermissionInputDTO param);
        Task<ResultWithError> UpdateAsync(SysPermissionInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetListPermission();
    }
}
