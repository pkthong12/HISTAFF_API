using Common.Paging;
using Common.Repositories;
using Common.Extensions;
using CoreDAL.ViewModels;
using API.All.DbContexts;
using API.Entities;

namespace CoreDAL.Repositories
{
    public class SysPermissionRepository : RepositoryBase<SYS_PERMISSION>, ISysPermissionRepository
    {
        private CoreDbContext _appContext => (CoreDbContext)_context;
        public SysPermissionRepository(CoreDbContext context) : base(context)
        {

        }
        /// <summary>
        /// Get All For CMS
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<SysPermissionDTO>> GetAll(SysPermissionDTO param)
        {
            var queryable = from p in _appContext.SysPermissions
                            select new SysPermissionDTO
                            {
                                Id = p.ID,
                                Name = p.NAME,
                                Code = p.CODE,
                                Orders = p.ORDERS,
                                IsActive = p.IS_ACTIVE
                            };
            if (param.Name != null)
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }

            if (param.Code != null)
            {
                queryable = queryable.Where(p => p.Code.ToUpper().Contains(param.Code.ToUpper()));
            }

            return await PagingList(queryable, param);
        }
        /// <summary>
        /// CMS Create Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> CreateAsync(SysPermissionInputDTO param)
        {
            var r = _appContext.SysPermissions.Where(x => x.CODE.ToLower() == param.Code.ToLower()).Count();
            if (r > 0)
            {
                return new ResultWithError(Consts.CODE_EXISTS);
            }
            var data = Map(param, new SYS_PERMISSION());
            data.IS_ACTIVE = true;
            var result = await _appContext.SysPermissions.AddAsync(data);
            await _appContext.SaveChangesAsync();
            return new ResultWithError(data);
        }
        /// <summary>
        /// CMS Update Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(SysPermissionInputDTO param)
        {
            var r = _appContext.SysPermissions.Where(x => x.ID == param.Id).FirstOrDefault();
            if (r == null)
            {
                return new ResultWithError(404);
            }

            var r1 = _appContext.SysPermissions.Where(x => x.ID != param.Id && x.CODE.ToLower() == param.Code.ToLower()).Count();
            if (r1 > 0)
            {
                return new ResultWithError(Consts.CODE_EXISTS);
            }

            var data = Map(param, r);
            var result = _appContext.SysPermissions.Update(data);
            await _appContext.SaveChangesAsync();
            return new ResultWithError(200);
        }
        /// <summary>
        /// CMS ChangeStatus 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> ChangeStatusAsync(List<long> ids)
        {
            foreach (var item in ids)
            {
                var r = _appContext.SysPermissions.Where(x => x.ID == item).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                r.IS_ACTIVE = !r.IS_ACTIVE;
                var result = _appContext.SysPermissions.Update(r);
            }
            await _appContext.SaveChangesAsync();
            return new ResultWithError(200);
        }
        /// <summary>
        /// Backend Get List Permission is Active
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetListPermission()
        {
            var r = await (from p in _appContext.SysPermissions.Where(x => x.IS_ACTIVE == true)
                           orderby p.ORDERS
                           select new
                           {
                               Id = p.ID,
                               Code = p.CODE,
                               Name = p.NAME
                           }).ToListAsync();
            return new ResultWithError(r); ;
        }
    }
}
