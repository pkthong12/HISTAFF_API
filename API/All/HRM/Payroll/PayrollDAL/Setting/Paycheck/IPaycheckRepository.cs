using PayrollDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace PayrollDAL.Repositories
{
    public interface IPaycheckRepository : IRepository<PA_SALARY_PAYCHECK>
    {
        Task<PagedResult<PaycheckDTO>> GetAll(PaycheckDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(PaycheckInputListDTO param);
        Task<ResultWithError> UpdateAsync(PaycheckInputDTO param);
        Task<ResultWithError> QuickUpdate(PaycheckInputDTO param);
        Task<ResultWithError> RemoveAsync(List<long> ids);
    }
}
