using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface IPositionPaperRepository : IRepository<HU_POS_PAPER>
    {
        Task<PagedResult<PostionPaperDTO>> GetAll(PostionPaperDTO param);
        Task<ResultWithError> CreateAsync(PosPaperInputDTO param);
        Task<ResultWithError> DeleteAsync(List<long> ids);
    }
}
