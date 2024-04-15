using Common.Interfaces;
using CoreDAL.ViewModels;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace CoreDAL.Repositories
{
    public interface ISysGroupFunctionRepository : IRepository<SYS_FUNCTION_GROUP>
    {
        Task<PagedResult<SysGroupFunctionDTO>> GetAll(SysGroupFunctionDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(SysGroupFunctionInputDTO param);
        Task<ResultWithError> UpdateAsync(SysGroupFunctionInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
    }
}
