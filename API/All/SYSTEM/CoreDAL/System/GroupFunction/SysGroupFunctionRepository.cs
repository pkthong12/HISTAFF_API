using Common.Paging;
using CoreDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace CoreDAL.Repositories
{
    public class AspGroupFunctionRepository : RepositoryBase<SYS_FUNCTION_GROUP>, ISysGroupFunctionRepository
    {
        private CoreDbContext _appContext => (CoreDbContext)_context;
        public AspGroupFunctionRepository(CoreDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<SysGroupFunctionDTO>> GetAll(SysGroupFunctionDTO param)
        {
            var queryable = from p in _appContext.SysGroupFunctions
                            from a in _appContext.SysConfigs.Where(x=>x.ID==p.APPLICATION_ID && x.TYPE == SystemConfig.APPLICATION)
                            select new SysGroupFunctionDTO
                            {
                                Id = p.ID,
                                AppName = a.NAME,
                                ApplicationId = p.APPLICATION_ID,
                                Name = p.NAME,
                                Code = p.CODE,
                                IsActive = p.IS_ACTIVE,
                                CreateBy = p.CREATED_BY,
                                CreateDate = p.CREATED_DATE,
                                UpdateBy = p.UPDATED_BY,
                                UpdateDate = p.UPDATED_DATE
                            };
            if (param.ApplicationId > 0)
            {
                queryable = queryable.Where(p => p.ApplicationId == param.ApplicationId);
            }

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

            return await PagingList(queryable, param);
        }
        /// <summary>
        /// CMS Get Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> GetById(long id)
        {
            var r = await (from p in _appContext.SysGroupFunctions
                    where p.ID == id
                    select new
                    {
                        Id = p.ID,
                        ApplicationId = p.APPLICATION_ID,
                        Name = p.NAME,
                        Code = p.CODE
                    }).FirstOrDefaultAsync();
            return new ResultWithError(r);
        }
        /// <summary>
        /// Create Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateAsync(SysGroupFunctionInputDTO param)
        {
            // check application
            var n = _appContext.SysConfigs.Where(x => x.ID == param.ApplicationId).Count();
            if (n == 0)
            {
                return new ResultWithError(Consts.APP_NOT_EXISTS);
            }
            // check code
            var r = _appContext.SysGroupFunctions.Where(x => x.CODE.ToLower() == param.Code.ToLower()).Count();
            if (r > 0)
            {
                return new ResultWithError(Consts.CODE_EXISTS);
            }
            var data = Map(param, new SYS_FUNCTION_GROUP());
            data.IS_ACTIVE = true;
            var result = await _appContext.SysGroupFunctions.AddAsync(data);
            await _appContext.SaveChangesAsync();
            return new ResultWithError(data);
        }
        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(SysGroupFunctionInputDTO param)
        {
            // check application
            var n = _appContext.SysConfigs.Where(x => x.ID == param.ApplicationId).Count();
            if (n == 0)
            {
                return new ResultWithError(Consts.APP_NOT_EXISTS);
            }
            // check code
            var c = _appContext.SysGroupFunctions.Where(x => x.CODE.ToLower() == param.Code.ToLower() && x.ID != param.Id).Count();
            if (c > 0)
            {
                return new ResultWithError(Consts.CODE_EXISTS);
            }

            var r = _appContext.SysGroupFunctions.Where(x => x.ID == param.Id).FirstOrDefault();
            if (r == null)
            {
                return new ResultWithError(404);
            }

            var data = Map(param, r);
            var result = _appContext.SysGroupFunctions.Update(data);
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
                var r = _appContext.SysGroupFunctions.Where(x => x.ID == item).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                r.IS_ACTIVE = !r.IS_ACTIVE;
                var result = _appContext.SysGroupFunctions.Update(r);
            }
            await _appContext.SaveChangesAsync();
            return new ResultWithError(200);
        }
    }
}
