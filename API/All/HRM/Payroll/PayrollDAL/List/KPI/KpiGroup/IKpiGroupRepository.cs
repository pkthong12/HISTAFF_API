using PayrollDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;
using CORE.DTO;

namespace PayrollDAL.Repositories
{
    public interface IKpiGroupRepository : IRepository<PA_KPI_GROUP>
    {

        Task<GenericPhaseTwoListResponse<KpiGroupDTO>> TwoPhaseQueryList(GenericQueryListDTO<KpiGroupDTO> request);
        Task<PagedResult<KpiGroupOutDTO>> GetAll(KpiGroupDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(KpiGroupInputDTO param);
        Task<ResultWithError> UpdateAsync(KpiGroupInputDTO param);
        Task<ResultWithError> GetList();
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
    }
}
