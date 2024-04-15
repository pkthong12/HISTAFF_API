using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;
using CORE.DTO;
using API.DTO;

namespace ProfileDAL.Repositories
{
    public interface IAllowanceEmpRepository : IRepository<HU_ALLOWANCE_EMP>
    {
        Task<PagedResult<AllowanceEmpDTO>> GetAll(AllowanceEmpDTO param);
        Task<ResultWithError> GetById(long id);
        Task<FormatedResponse> CreateAsync(AllowanceEmpInputDTO param);
        Task<FormatedResponse> UpdateAsync(AllowanceEmpInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> RemoteAsync(List<long> ids);

        Task<GenericPhaseTwoListResponse<HuAllowanceEmpDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuAllowanceEmpDTO> request);
        Task<ResultWithError> GetList();


    }
}
