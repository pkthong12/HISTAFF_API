using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public interface IThemeBlogRepository : IRepository<THEME_BLOG>
    {
        Task<PagedResult<ThemeBlogDTO>> GetAll(ThemeBlogDTO param);
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> CreateAsync(ThemeBlogInputDTO param);
        Task<ResultWithError> UpdateAsync(ThemeBlogInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetList();
    }
}