/*
using Common.Interfaces;
using CoreDAL.ViewModels;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace CoreDAL.Repositories
{
    public interface ISysFunctionRepository : IRepository<SYS_FUNCTION>
    {
        Task<PagedResult<SysFunctionDTO>> GetAll(SysFunctionDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(SysFunctionInputDTO param);
        Task<ResultWithError> UpdateAsync(SysFunctionInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
    }
}
*/