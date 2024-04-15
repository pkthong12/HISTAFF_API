using PayrollDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace PayrollDAL.Repositories
{
    public interface IKpiEmployeeRepository : IRepository<PA_KPI_SALARY_DETAIL>
    {
        Task<PagedResult<KpiEmployeeDTO>> GetAll(KpiEmployeeDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(KpiEmployeeInputDTO param);
        Task<ResultWithError> UpdateAsync(KpiEmployeeInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> Delete(List<long> ids);
        Task<ResultWithError> ExportTemplate(KpiEmployeeInput param);
        Task<ResultWithError> ImportFromTemplate(KpiEmployeeImport param);
        Task<ResultWithError> CaclKpiSalary(KpiEmployeeInput param);
        Task<ResultWithError> LockKPI(LockInputDTO param);
        Task<ResultWithError> IsLockKPI(LockInputDTO param);
    }
}
