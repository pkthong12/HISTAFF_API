using API.Entities;
using Common.Extensions;
using Common.Interfaces;
using Common.Paging;
using CoreDAL.ViewModels;
namespace CoreDAL.Repositories
{
    public interface ISysModuleRepository : IRepository<SYS_MODULE>
    {
        Task<PagedResult<SysModuleDTO>> GetAll(SysModuleDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(SysModuleInputDTO param);
        Task<ResultWithError> UpdateAsync(SysModuleInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
    }
}
