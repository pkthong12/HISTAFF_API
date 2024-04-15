using Common.Paging;
using Common.Repositories;
using CoreDAL.ViewModels;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace CoreDAL.Repositories
{
    public class SysOtherListRepository : RepositoryBase<SYS_OTHER_LIST_TYPE>, ISysOtherListRepository
    {
        private CoreDbContext _appContext => (CoreDbContext)_context;
        public SysOtherListRepository(CoreDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Type Data Is Active
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CMSGetAllType(SysOtherListTypeDTO param)
        {
            var r = await (from p in _appContext.SysOtherListTypes
                           where p.IS_ACTIVE == true
                           orderby p.ORDERS
                           select new
                           {
                               Id = p.ID,
                               Name = p.NAME
                           }).ToListAsync();
            return new ResultWithError(r);
        }
        /// <summary>
        /// CMS Get All Data by Type
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<SYS_OTHER_LIST>> CMSGetByType(SysOtherListDTO param)
        {
            var queryable = from p in _appContext.SysOtherLists select p;
            queryable = queryable.Where(p => p.IS_ACTIVE == true);
            if (param.TypeId != null)
            {
                queryable = queryable.Where(p => p.TYPE_ID == param.TypeId);
            }
            if (param.Name != null)
            {
                queryable = queryable.Where(p => p.NAME.ToUpper().Contains(param.Name.ToUpper()));
            }

            if (param.Code != null)
            {
                queryable = queryable.Where(p => p.CODE.ToUpper().Contains(param.Code.ToUpper()));
            }

            return await PagingList(queryable, param);
        }
        /// <summary>
        /// CMS Create Type
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateTypeAsync(SysOtherListTypeInputDTO param)
        {
            var r = _appContext.SysOtherListTypes.Where(x => x.CODE.ToLower() == param.Code.ToLower()).Count();
            if (r > 0)
            {
                return new ResultWithError(409);
            }
            var data = Map(param, new SYS_OTHER_LIST_TYPE());
            data.IS_ACTIVE = true;
            var result = await _appContext.SysOtherListTypes.AddAsync(data);
            await _appContext.SaveChangesAsync();
            return new ResultWithError(data);
        }
        /// <summary>
        /// CMS Update Type
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateTypeAsync(SysOtherListTypeInputDTO param)
        {
            var r = _appContext.SysOtherListTypes.Where(x => x.ID == param.Id).FirstOrDefault();
            if (r == null)
            {
                return new ResultWithError(404);
            }
            var r1 = _appContext.SysOtherListTypes.Where(x => x.ID != param.Id && x.CODE.ToLower() == param.Code.ToLower()).Count();
            if (r1 > 0)
            {
                return new ResultWithError(409);
            }
            var data = Map(param, r);
            var result = _appContext.SysOtherListTypes.Update(data);
            await _appContext.SaveChangesAsync();
            return new ResultWithError(200);
        }
        /// <summary>
        /// CMS Change Status Type
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> ChangeStatusTypeAsync(List<long> ids)
        {
            foreach (var item in ids)
            {
                var r = _appContext.SysOtherListTypes.Where(x => x.ID == item).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(400);
                }
                r.IS_ACTIVE = !r.IS_ACTIVE;
                var result = _appContext.SysOtherListTypes.Update(r);
            }
            await _appContext.SaveChangesAsync();
            return new ResultWithError(200);
        }
        /// <summary>
        /// /// CMS Create Data OtherList
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateAsync(SysOtherListInputDTO param)
        {
            var r = _appContext.SysOtherLists.Where(x => x.CODE.ToLower() == param.Code.ToLower()).Count();
            if (r > 0)
            {
                return new ResultWithError(409);
            }
            var n = _appContext.SysOtherListTypes.Where(x => x.ID == param.TypeId).Count();
            if (n == 0)
            {
                return new ResultWithError("TYPE_NOT_EXIST");
            }
            var data = Map(param, new SYS_OTHER_LIST());
            data.IS_ACTIVE = true;
            var result = await _appContext.SysOtherLists.AddAsync(data);
            await _appContext.SaveChangesAsync();
            return new ResultWithError(data);
        }
        /// <summary>
        /// CMS Update Data OtherList 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(SysOtherListInputDTO param)
        {
            var r = _appContext.SysOtherLists.Where(x => x.ID == param.Id).FirstOrDefault();
            if (r == null)
            {
                return new ResultWithError(404);
            }
            var n = _appContext.SysOtherListTypes.Where(x => x.ID == param.TypeId).Count();
            if (n == 0)
            {
                return new ResultWithError("TYPE_NOT_EXIST");
            }

            var r1 = _appContext.SysOtherLists.Where(x => x.ID != param.Id && x.CODE.ToLower() == param.Code.ToLower()).Count();
            if (r1 > 0)
            {
                return new ResultWithError(409);
            }
            var data = Map(param, r);
            var result = _appContext.SysOtherLists.Update(data);
            await _appContext.SaveChangesAsync();
            return new ResultWithError(200);
        }
        /// <summary>
        /// CMS Change Status OtherList
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> ChangeStatusAsync(List<long> ids)
        {
            foreach (var item in ids)
            {
                var r = _appContext.SysOtherLists.Where(x => x.ID == item).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                r.IS_ACTIVE = !r.IS_ACTIVE;
                var result = _appContext.SysOtherLists.Update(r);
            }
            await _appContext.SaveChangesAsync();
            return new ResultWithError(200);
        }

        /// <summary>
        /// Backend Get Data is Active by Type
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetOtherListByType(string typeCode)
        {
            if (typeCode == null || typeCode.Trim().Length == 0)
            {
                return new ResultWithError("CODE_NOT_BLANK");
            }
            var r = await (from p in _appContext.SysOtherLists
                           join o in _appContext.SysOtherListTypes on p.TYPE_ID equals o.ID
                           where p.IS_ACTIVE == true && o.CODE == typeCode
                           select new
                           {
                               Id = p.ID,
                               Name = p.NAME
                           }).ToListAsync();
            return new ResultWithError(r);
        }

        /// <summary>
        /// Get OtherList Fix
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetSysConfixByType(string typeCode)
        {
            if (typeCode == null || typeCode.Trim().Length == 0)
            {
                return new ResultWithError("CODE_NOT_BLANK");
            }
            var r = await (from p in _appContext.SysConfigs
                           where p.IS_ACTIVE == true && p.TYPE == typeCode
                           select new
                           {
                               Id = p.ID,
                               Name = p.NAME,
                               Code = p.CODE
                           }).ToListAsync();
            return new ResultWithError(r);
        }
    }
}

