using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface ICandidateRepository : IRepository<RC_CANDIDATE_SCANCV>
    {
        Task<PagedResult<CandidateScanCVDTO>> GetAll(CandidateScanCVDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(CandidateScanCVInputDTO param);
        Task<ResultWithError> UpdateAsync(CandidateScanCVInputDTO param);
        Task<ResultWithError> RemoveAsync(long id);

        Task<ResultWithError> ReadFileCVAsync(CandidateScanCVImportDTO param);

    }
}
