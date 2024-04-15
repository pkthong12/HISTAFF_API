using PayrollDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace PayrollDAL.Repositories
{
    public interface IKpiFormulaRepository : IRepository<PA_KPI_FORMULA>
    {
        Task<PagedResult<KpiFormulaDTO>> GetAll(KpiFormulaDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> UpdateAsync(KpiFormulaCreateDTO param);
        Task<ResultWithError> GetList();
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
    }
}
