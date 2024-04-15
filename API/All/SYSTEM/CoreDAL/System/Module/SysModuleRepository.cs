using Common.Paging;
using CoreDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;

namespace CoreDAL.Repositories
{
    public class SysModuleRepository : RepositoryBase<SYS_MODULE>, ISysModuleRepository
    {
        private CoreDbContext _appContext => (CoreDbContext)_context;
        public SysModuleRepository(CoreDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS GetAll Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<SysModuleDTO>> GetAll(SysModuleDTO param)
        {
            var queryable = from p in _appContext.SysModules
                            from o in _appContext.SysConfigs.Where(x => x.ID == p.APPLICATION_ID && x.TYPE == SystemConfig.APPLICATION)
                            select new SysModuleDTO
                            {
                                Id = p.ID,
                                Code = p.CODE,
                                Name = p.NAME,
                                ApplicationId = p.APPLICATION_ID,
                                AppName = o.NAME,
                                Note = p.NOTE,
                                Price = p.PRICE,
                                IsActive = p.IS_ACTIVE,
                                Orders = p.ORDERS
                            };
            if (param.Name != null)
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }

            if (param.Code != null)
            {
                queryable = queryable.Where(p => p.Code.ToUpper().Contains(param.Code.ToUpper()));
            }

            if (param.AppName != null)
            {
                queryable = queryable.Where(p => p.AppName.ToUpper().Contains(param.AppName.ToUpper()));
            }

            if (param.ApplicationId != null)
            {
                queryable = queryable.Where(p => p.ApplicationId == param.ApplicationId);
            }

            return await PagingList(queryable, param);
        }
        /// <summary>
        /// CMS Get Detail Data
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetById(long Id)
        {
            var r = await (from p in _appContext.SysModules
                           where p.ID == Id
                           select new SysModuleDTO
                           {
                               Id = p.ID,
                               Code = p.CODE,
                               Name = p.NAME,
                               ApplicationId = p.APPLICATION_ID,
                               Note = p.NOTE,
                               IsActive = p.IS_ACTIVE,
                               Price = p.PRICE,
                               Orders = p.ORDERS
                           }).FirstOrDefaultAsync();

            return new ResultWithError(r);
        }
        /// <summary>
        /// CMS Create Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateAsync(SysModuleInputDTO param)
        {
            // kiểm tra xem có tồn tại ứng dụng không?
            var n = _appContext.SysConfigs.Where(x => x.ID == param.ApplicationId).Count();
            if (n == 0)
            {
                return new ResultWithError(Consts.APP_NOT_EXISTS);
            }
            var r = _appContext.SysModules.Where(x => x.CODE.ToLower() == param.Code.ToLower()).Count();
            if (r > 0)
            {
                return new ResultWithError(Consts.CODE_EXISTS);
            }
            var data = Map(param, new SYS_MODULE());
            data.IS_ACTIVE = true;
            var result = await _appContext.SysModules.AddAsync(data);
            await _appContext.SaveChangesAsync();
            return new ResultWithError(data);
        }
        /// <summary>
        /// CMSs Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(SysModuleInputDTO param)
        {
            // kiểm tra xem có tồn tại ứng dụng không?
            var n = _appContext.SysConfigs.Where(x => x.ID == param.ApplicationId).Count();
            if (n == 0)
            {
                return new ResultWithError(Consts.APP_NOT_EXISTS);
            }
            var r = _appContext.SysModules.Where(x => x.ID == param.Id).FirstOrDefault();
            if (r == null)
            {
                return new ResultWithError(404);
            }
            var c = _appContext.SysModules.Where(x => x.CODE.ToLower() == param.Code.ToLower() && x.ID != param.Id).Count();
            if (c > 0)
            {
                return new ResultWithError(Consts.CODE_EXISTS);
            }

            var data = Map(param, r);
            var result = _appContext.SysModules.Update(data);
            await _appContext.SaveChangesAsync();
            return new ResultWithError(200);
        }
        /// <summary>
        /// CMS Change Status 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> ChangeStatusAsync(List<long> ids)
        {
            foreach (var item in ids)
            {
                var r = _appContext.SysModules.Where(x => x.ID == item).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                r.IS_ACTIVE = !r.IS_ACTIVE;
                var result = _appContext.SysModules.Update(r);
            }
            await _appContext.SaveChangesAsync();
            return new ResultWithError(200);
        }
    }
}
