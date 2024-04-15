using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public class BlogInternalRepository : RepositoryBase<PT_BLOG_INTERNAL>, IBlogInternalRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        public BlogInternalRepository(ProfileDbContext context) : base(context)
        {

        }


        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<BlogInternalDTO>> GetAll(BlogInternalDTO param)
        {
            var queryable = from p in _appContext.BlogInternals
                            
                            select new BlogInternalDTO
                            {
                                Id = p.ID,
                                Title = p.TITLE,
                                Description = p.DESCRIPTION,
                                Content = p.CONTENT,
                                IsActive = p.IS_ACTIVE,
                                CreateBy = p.CREATED_BY,
                                CreateDate = p.CREATED_DATE,
                                UpdateBy = p.UPDATED_BY,
                                UpdateDate = p.UPDATED_DATE
                            };

            if (param.Title != null)
            {
                queryable = queryable.Where(p => p.Title.ToUpper().Contains(param.Title.ToUpper()));
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
                var r = await (from p in _appContext.BlogInternals
                               where p.ID == id
                               select new
                               {
                                   Id = p.ID,
                                   Title = p.TITLE,
                                   Description = p.DESCRIPTION,
                                   Content = p.CONTENT,
                                   ThemeId = p.THEME_ID,
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
        public async Task<ResultWithError> CreateAsync(BlogInternalInputDTO param)
        {

            try
            {
                var data = Map(param, new PT_BLOG_INTERNAL());
                data.IS_ACTIVE = true;
                var result = await _appContext.BlogInternals.AddAsync(data);
                await _appContext.SaveChangesAsync();

                // tạo notifi
                // lấy fmc token
                var lstEmpId = await (from e in _appContext.Employees
                                      select e.ID
                               ).ToListAsync();
                var lstToken = await QueryData.ExecuteList("connectionString",
                    "PKG_NOTIFI_GET_FMC_TOKEN", new
                    {
                        P_EMP_ID = string.Join(",", lstEmpId),
                    });
                var IsRead = new List<bool>(lstToken.Count);
                List<string> lstDevice = lstToken.Select(c => (string)((dynamic)c).FCM_TOKEN).ToList();
                if (lstDevice.Count > 0)
                {
                    for (int i = 0; i < lstToken.Count; i++)
                    {
                        IsRead.Add(false);
                    }
                    // 
                    var no = new AT_NOTIFICATION();
                    no.TYPE = Consts.REGISTER_NEW;
                    no.ACTION = Consts.ACTION_APPROVE;
                    no.NOTIFI_ID = data.ID;
                    no.EMP_CREATE_ID = _appContext.EmpId;
                    no.FMC_TOKEN = string.Join(";", lstDevice);
                    //no.IS_READ = string.Join(";", IsRead);
                    //no.EMP_NOTIFY_ID = string.Join(";", lstEmpId);
                    
                    no.CREATED_DATE = DateTime.Now;
                    no.TITLE = data.TITLE;
                    await _appContext.Notifications.AddAsync(no);
                    await SendNotification(new NotificationModel
                    {
                        Devices = lstDevice,
                        Title = data.TITLE,
                        Body = data.DESCRIPTION
                    });
                }
                param.Id = data.ID;
                return new ResultWithError(param);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }

        }
        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(BlogInternalInputDTO param)
        {
            var r = _appContext.BlogInternals.Where(x => x.ID == param.Id).FirstOrDefault();
            if (r == null)
            {
                return new ResultWithError(404);
            }

            var data = Map(param, r);
            var result = _appContext.BlogInternals.Update(data);
            await _appContext.SaveChangesAsync();
            return new ResultWithError(200);
        }
        /// <summary>
        /// CMS Change Status Data
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> ChangeStatusAsync(List<long> ids)
        {
            foreach (var item in ids)
            {
                var r = _appContext.BlogInternals.Where(x => x.ID == item).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                r.IS_ACTIVE = !r.IS_ACTIVE;
                var result = _appContext.BlogInternals.Update(r);
            }
            await _appContext.SaveChangesAsync();
            return new ResultWithError(200);
        }
        /// <summary>
        /// Get List Group is Activce
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<PagedResult<BlogPortalDTO>> PortalGetList(Pagings param)
        {
            var queryable = from p in _appContext.BlogInternals
                            where p.IS_ACTIVE == true 
                            orderby p.CREATED_DATE descending
                            select new BlogPortalDTO
                            {
                                Id = p.ID,
                                Title = p.TITLE,
                                Description = p.DESCRIPTION,
                                CreateDate = p.CREATED_DATE,
                                Img = "http://gohr.vn:6868/upload/gohr/profile/profile-picture.png"
                            };
            return await PagingList(queryable, param);
        }

        /// <summary>
        /// PortalGetById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultWithError> PortalGetById(long id)
        {
            var r = await (from p in _appContext.BlogInternals
                           where p.ID == id 
                           select new
                           {
                               Title = p.TITLE,
                               Description = p.DESCRIPTION,
                               Content = p.CONTENT,
                               ThemeId = p.THEME_ID
                           }).FirstOrDefaultAsync();
            return new ResultWithError(r);
        }


        /// <summary>
        /// Portal Get Newest 
        /// </summary>
        /// <returns></returns>
        public async Task<ResultWithError> PortalGetNewest()
        {
            var r = await (from p in _appContext.BlogInternals
                           join t in _appContext.ThemeBlogs on p.THEME_ID equals t.ID
                           where p.IS_ACTIVE == true
                           orderby p.CREATED_DATE descending
                           select new
                           {
                               Id = p.ID,
                               Title = p.TITLE,
                               BackGroud = t.IMG_URL,
                               Color = t.COLOR
                           }).FirstOrDefaultAsync();
            return new ResultWithError(r);
        }



        public async Task<List<NotifyView>> PortalNotify()
        {

            try
            {
                var r = await QueryData.ExecuteStore<NotifyView>(Procedures.PKG_PROFILE_GET_NOTIFY, new
                {
                    
                    P_EMP_ID = _appContext.EmpId,
                    P_CUR = QueryData.OUT_CURSOR
                });
                return r;
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }
        /// Version 2
        /// <summary>
        /// Portal check Watched
        /// </summary>
        /// <returns></returns>
        public async Task<ResultWithError> PortalWatched(long id)
        {
            var r = _appContext.Notifications.Where(p => p.ID == id).FirstOrDefault();
            // chuyển 2 trường clop thành list;
            // emp-notification
            //List<long> lstEmpNoti = r.EMP_NOTIFY_ID.Split(",").Select(long.Parse).ToList();
            // is_read
            //List<bool> lstIsRead = r.IS_READ.Split(",").Select(bool.Parse).ToList();
            // vị trí nhân viên đã xem
            //var IndexEmp = lstEmpNoti.IndexOf(_appContext.EmpId);
            // cap nhat
            //lstIsRead[IndexEmp] = true;

            //r.IS_READ = string.Join(";", lstIsRead);
            _appContext.Notifications.Update(r);

            await _appContext.SaveChangesAsync();

            return new ResultWithError(200);
        }

        /// <summary>
        /// Portal Get slider for Home 
        /// </summary>
        /// <returns></returns>
        public async Task<ResultWithError> PortalHomeNew()
        {
            var r = await (from p in _appContext.BlogInternals
                           join t in _appContext.ThemeBlogs on p.THEME_ID equals t.ID
                           where p.IS_ACTIVE == true
                           orderby p.CREATED_DATE descending
                           select new
                           {
                               Id = p.ID,
                               Title = p.TITLE,
                               background = p.IMG_URL == null ? t.IMG_URL : p.IMG_URL,
                               Color = t.COLOR
                           }).Skip(0).Take(3).ToListAsync();
            return new ResultWithError(r);
        }

        /// <summary>
        /// Portal Home Notify
        /// </summary>
        /// <returns></returns>
        public async Task<ResultWithError> PortalHomeNotify()
        {
            try
            {
                var r = await QueryData.ExecuteStore<NotifyHomeView>(Procedures.PKG_NOTIFY_PORTAL_COUNT_HOME, new
                {
                    
                    P_EMP_ID = _appContext.EmpId,
                    P_CUR = QueryData.OUT_CURSOR
                });

                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// Portal Home Notify
        /// </summary>
        /// <returns></returns>
        public async Task<ResultWithError> PortalApproveNotify()
        {
            try
            {
                var r = await QueryData.ExecuteStore<NotifyApproveView>(Procedures.PKG_NOTIFY_PORTAL_COUNT_REG, new
                {
                    
                    P_EMP_ID = _appContext.EmpId,
                    P_CUR = QueryData.OUT_CURSOR
                });
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
    }
}
