using API.Entities;
using Common.Extensions;
using Common.Interfaces;
using Common.Paging;
using CoreDAL.ViewModels;

namespace CoreDAL.Repositories
{
    public interface IApproveProcessRepository : IRepository<SE_APP_PROCESS>
    {
        Task<PagedResult<ApproveProcessDTO>> GetAll(ApproveProcessDTO param, long application);
        Task<ResultWithError> GetById(long id, long application);
        Task<ResultWithError> UpdateAsync(ApproveProcessDTO param, long application);
    }
}
