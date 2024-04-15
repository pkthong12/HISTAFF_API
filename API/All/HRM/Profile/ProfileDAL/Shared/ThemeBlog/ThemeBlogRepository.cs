using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public class ThemeBlogRepository : RepositoryBase<THEME_BLOG>, IThemeBlogRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        public ThemeBlogRepository(ProfileDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<ThemeBlogDTO>> GetAll(ThemeBlogDTO param)
        {
            var queryable = from p in _appContext.ThemeBlogs
                            select new ThemeBlogDTO
                            {
                                Id = p.ID,
                                Name = p.NAME,
                                CreateDate = p.CREATED_DATE,
                                ImgUrl = p.IMG_URL,
                                IsActive = p.IS_ACTIVE
                            };

          
            if (!string.IsNullOrWhiteSpace(param.Name))
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }
            if (param.IsActive != null)
            {
                queryable = queryable.Where(p => p.IsActive == param.IsActive);
            }
            return await PagingList(queryable, param);
        }
        /// <summary>
        /// CMS Get Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> GetById(long id)
        {
            try
            {
                var r = await (from p in _appContext.ThemeBlogs
                               where p.ID == id
                               select new
                               {
                                   Id = p.ID,
                                   Name = p.NAME,
                                   ImgUrl = p.IMG_URL,
                                   Color = p.COLOR,
                                   IsActive = p.IS_ACTIVE
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Create Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateAsync(ThemeBlogInputDTO param)
        {
            try
            {
                var data = Map(param, new THEME_BLOG());
                data.IS_ACTIVE = true;
                var result = await _appContext.ThemeBlogs.AddAsync(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }
        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(ThemeBlogInputDTO param)
        {
            try
            {
                var r = _appContext.ThemeBlogs.Where(x => x.ID == param.Id).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }

                var data = Map(param, r);
                var result = _appContext.ThemeBlogs.Update(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex);
            }
        }
        /// <summary>
        /// CMS Change Status Data
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> ChangeStatusAsync(List<long> ids)
        {
            try
            {
                foreach (var item in ids)
                {
                    var r = _appContext.ThemeBlogs.Where(x => x.ID == item).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    r.IS_ACTIVE = !r.IS_ACTIVE;
                    var result = _appContext.ThemeBlogs.Update(r);
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Get List Group is Activce
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetList()
        {
            try
            {
                var queryable = await (from p in _appContext.ThemeBlogs
                                       where p.IS_ACTIVE == true
                                       orderby p.NAME
                                       select new
                                       {
                                           Id = p.ID,
                                           Name = p.NAME,
                                           ImgUrl = p.IMG_URL,
                                           Color = p.COLOR
                                       }).ToListAsync();
                return new ResultWithError(queryable);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
    }
}
