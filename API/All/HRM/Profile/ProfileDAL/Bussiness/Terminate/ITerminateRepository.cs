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
        Task<ResultWithError> CreateAsync(TerminateInputDTO param);
        Task<ResultWithError> UpdateAsync(TerminateInputDTO param);
        Task<bool> ApproveList(string sid);
        Task<bool> Approve(HU_TERMINATE obj);
        Task<ResultWithError> RemoveAsync(List<long> ids);
        Task<ResultWithError> CalculateSeniority(string? dStart1, string? dStop1);
        void ScanApproveTerminate();
        
    }
}
