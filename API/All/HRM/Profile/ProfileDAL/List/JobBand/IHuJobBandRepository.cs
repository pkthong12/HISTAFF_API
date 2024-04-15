using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface IHuJobBandRepository : IRepository<HU_JOB_BAND>
    {
        Task<PagedResult<HUJobBandDTO>> GetJobBands(HUJobBandDTO param);
        Task<ResultWithError> GetJobBand(int id);
        Task<ResultWithError> UpdateAsync(HUJobBandInputDTO param);
        Task<bool> ValidateJobBand(HUJobBandInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids, bool status);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> DeleteAsync(List<long> ids);
        Task<ResultWithError> GetCboJobBand();
    }
}
