using Common.Interfaces;
using CoreDAL.ViewModels;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace CoreDAL.Repositories
{
    public interface ISysGroupUserRepository : IRepository<SYS_GROUP>
    {
        Task<PagedResult<SysGroupUserDTO>> GetAll(SysGroupUserDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(SysGroupUserInputDTO param);
        Task<ResultWithError> UpdateAsync(SysGroupUserInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetList();
    }
}
