using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;
using CORE.DTO;

namespace ProfileDAL.Repositories
{
    public interface IHuJobRepository : IRepository<HU_JOB>
    {
        Task<PagedResult<HUJobInputDTO>> GetJobs(HUJobInputDTO param);
        Task<ResultWithError> GetJob(int id);
        Task<ResultWithError> UpdateAsync(HUJobEditDTO param);
        Task<bool> ValidateJob(HUJobEditDTO param);
        Task<ResultWithError> ChangeStatusAsync(HUJobDTO request);
        Task<ResultWithError> DeleteAsync(List<long> ids);
        Task<GenericPhaseTwoListResponse<HUJobInputDTO>> SinglePhaseQueryList(GenericQueryListDTO<HUJobInputDTO> request);
        Task<ResultWithError> GetJobById(long id);
        Task<ResultWithError> GetList();
        Task<FormatedResponse> GetCodeByJobFamily(long id);

    }
}
