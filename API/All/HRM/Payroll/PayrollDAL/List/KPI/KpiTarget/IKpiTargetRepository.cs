using PayrollDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace PayrollDAL.Repositories
{
    public interface IKpiTargetRepository : IRepository<PA_KPI_TARGET>
    {
        Task<PagedResult<KpiTargetOutDTO>> GetAll(KpiTargetDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(KpiTargetInputDTO param);
        Task<ResultWithError> UpdateAsync(KpiTargetInputDTO param);
        // Task<ResultWithError> GetList(int groupid, int? typeId);
        Task<ResultWithError> GetList();
        Task<ResultWithError> GetListFomula();
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> QuickUpdate(KpiTargetQickDTO param);
    }
}
