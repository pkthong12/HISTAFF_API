using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;
using CORE.DTO;

namespace ProfileDAL.Repositories
{
    public interface ITerminateRepository : IRepository<HU_TERMINATE>
    {
        Task<GenericPhaseTwoListResponse<TerminateDTO>> TwoPhaseQueryList(GenericQueryListDTO<TerminateDTO> request);
        Task<PagedResult<TerminateView>> GetAll(TerminateDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> GetTerminateByEmployee(long id);
        Task<ResultWithError> CreateAsync(TerminateInputDTO param, string sid);
        Task<ResultWithError> UpdateAsync(TerminateInputDTO param, string sid);
        Task<bool> ApproveList(string sid);
        Task<bool> Approve(HU_TERMINATE obj);
        Task<ResultWithError> RemoveAsync(List<long> ids);
        Task<ResultWithError> CalculateSeniority(string? dStart1, string? dStop1);
        void ScanApproveTerminate();
        Task<bool> Approve2(HU_TERMINATE obj);
    }
}
