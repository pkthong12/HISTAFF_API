using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface IBlogInternalRepository : IRepository<PT_BLOG_INTERNAL>
    {
        Task<PagedResult<BlogInternalDTO>> GetAll(BlogInternalDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(BlogInternalInputDTO param);
        Task<ResultWithError> UpdateAsync(BlogInternalInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<PagedResult<BlogPortalDTO>> PortalGetList(Pagings param);
        Task<ResultWithError> PortalGetById(long id);
        Task<ResultWithError> PortalGetNewest();
        Task<List<NotifyView>> PortalNotify();
        Task<ResultWithError> PortalWatched(long id);
        Task<ResultWithError> PortalHomeNew();
        Task<ResultWithError> PortalHomeNotify();
        Task<ResultWithError> PortalApproveNotify();
    }
}
