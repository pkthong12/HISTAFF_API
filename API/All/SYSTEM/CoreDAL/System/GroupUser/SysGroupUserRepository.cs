using Common.Paging;
using CoreDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace CoreDAL.Repositories
{
    public class SysGroupUserRepository : RepositoryBase<SYS_GROUP>, ISysGroupUserRepository
    {
        private CoreDbContext _appContext => (CoreDbContext)_context;
        public SysGroupUserRepository(CoreDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<SysGroupUserDTO>> GetAll(SysGroupUserDTO param)
        {
            var queryable = from p in _appContext.SysGroupUsers
                            where p.IS_ADMIN == false
                            select new SysGroupUserDTO
                            {
                                Id = (int)p.ID,
                                Name = p.NAME,
                                Code = p.CODE,
                                IsActive = p.IS_ACTIVE
                            };
            if (param.Name != null)
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }

            if (param.Code != null)
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Code.ToUpper()));
            }

            return await PagingList(queryable, param);
        }
        /// <summary>
        /// CMS Get Detail Data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetById(long id)
        {
            var r = await (from p in _appContext.SysGroupUsers
                           where p.ID == id
                           select new
                           {
                               Id = p.ID,
                               Name = p.NAME,
                               Code = p.CODE,
                               IsActive = p.IS_ACTIVE
                           }).FirstOrDefaultAsync();
            return new ResultWithError(r);
        }
        /// <summary>
        /// CMS Create Data GroupUser
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateAsync(SysGroupUserInputDTO param)
        {
            var r = _appContext.SysGroupUsers.Where(x => x.CODE.ToLower() == param.Code.ToLower()).Count();
            if (r > 0)
            {
                return new ResultWithError(409);
            }
            param.Id = null;
            var data = Map(param, new SYS_GROUP());
            data.IS_ACTIVE = true;
            data.IS_ADMIN = false;
            var result = await _appContext.SysGroupUsers.AddAsync(data);
            await _appContext.SaveChangesAsync();
            return new ResultWithError(data);
        }
        /// <summary>
        /// CMS Edit GroupUser
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(SysGroupUserInputDTO param)
        {
            var r = _appContext.SysGroupUsers.Where(x => x.ID == param.Id).FirstOrDefault();
            if (r == null)
            {
                return new ResultWithError(404);
            }
            var data = Map(param, r);
            var result = _appContext.SysGroupUsers.Update(data);
            await _appContext.SaveChangesAsync();
            return new ResultWithError(200);
        }

        public async Task<ResultWithError> ChangeStatusAsync(List<long> ids)
        {
            foreach (var item in ids)
            {
                var r = _appContext.SysGroupUsers.Where(x => x.ID == item).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                r.IS_ACTIVE = !r.IS_ACTIVE;
                var result = _appContext.SysGroupUsers.Update(r);
            }
            await _appContext.SaveChangesAsync();
            return new ResultWithError(200);
        }

        /// <summary>
        /// CMS Get Detail Data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetList()
        {
            var r = await (from p in _appContext.SysGroupUsers
                           where p.IS_ACTIVE == true && p.IS_ADMIN == false
                           orderby p.NAME
                           select new
                           {
                               Id = p.ID,
                               Name = p.NAME
                           }).ToListAsync();
            return new ResultWithError(r);
        }
    }
}
