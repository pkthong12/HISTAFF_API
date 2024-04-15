using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;
using CORE.DTO;

namespace ProfileDAL.Repositories
{
    public interface IAllowanceRepository : IRepository<HU_ALLOWANCE>
    {
        Task<PagedResult<AllowanceViewDTO>> GetAll(AllowanceViewDTO param);
        Task<ResultWithError> GetById(long id);
        Task<FormatedResponse> CreateAsync(AllowanceInputDTO param);
        Task<FormatedResponse> UpdateAsync(AllowanceInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetList();
        Task<ResultWithError> CheckAllowIsUsed(string param);

        Task<ResultWithError> RemoteAsync(List<long> ids);
        Task<GenericPhaseTwoListResponse<AllowanceViewDTO>> SinglePhaseQueryList(GenericQueryListDTO<AllowanceViewDTO> request);
    }
}
