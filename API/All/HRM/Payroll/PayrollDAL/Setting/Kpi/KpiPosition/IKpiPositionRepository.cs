using PayrollDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace PayrollDAL.Repositories
{
    public interface IKpiPositionRepository : IRepository<PA_KPI_POSITION>
    {
        Task<PagedResult<KpiPositionDTO>> GetAll(KpiPositionDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(KpiPositionInputDTO param);
        Task<ResultWithError> Removes(List<long> ids);
    }
}
