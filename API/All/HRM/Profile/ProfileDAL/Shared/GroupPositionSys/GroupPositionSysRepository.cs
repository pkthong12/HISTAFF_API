using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public class GroupPositionSysRepository : RepositoryBase<SYS_POSITION_GROUP>, IGroupPositionSysRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        public GroupPositionSysRepository(ProfileDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<GroupPositionSysDTO>> GetAll(GroupPositionSysDTO param)
        {
            var queryable = from p in _appContext.GroupPositionSyses
                            select new GroupPositionSysDTO
                            {
                                Id = p.ID,
                                Name = p.NAME,
                                Code = p.CODE,
                                AreaId = p.AREA_ID,
                                IsActive = p.IS_ACTIVE,
                                Note = p.NOTE,
                                CreateBy = p.CREATED_BY,
                                CreateDate = p.CREATED_DATE,
                                UpdateBy = p.UPDATED_BY,
                                UpdateDate = p.UPDATED_DATE
                            };

            if (param.Name != null)
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }

            if (param.Code != null)
            {
                queryable = queryable.Where(p => p.Code.ToUpper().Contains(param.Code.ToUpper()));
            }

            if (param.Note != null)
            {
                queryable = queryable.Where(p => p.Note.ToUpper().Contains(param.Note.ToUpper()));
            }

            if (param.IsActive != null)
            {
                queryable = queryable.Where(p => p.IsActive == param.IsActive);
            }
            if (param.AreaId != null)
            {
                queryable = queryable.Where(p => p.AreaId == param.AreaId);
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
            var r = await (from p in _appContext.GroupPositionSyses
                           where p.ID == id
                           select new GroupPositionSysDTO
                           {
                               Id = p.ID,
                               Name = p.NAME,
                               Code = p.CODE,
                               Note = p.NOTE,
                               AreaId = p.AREA_ID,
                               IsActive = p.IS_ACTIVE
                           }).FirstOrDefaultAsync();
            return new ResultWithError(r);
        }
        /// <summary>
        /// Create Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateAsync(GroupPositionSysInputDTO param)
        {
            var r = _appContext.GroupPositionSyses.Where(x => x.CODE == param.Code).Count();
            if (r > 0)
            {
                return new ResultWithError(Consts.CODE_EXISTS);
            }
            var data = Map(param, new SYS_POSITION_GROUP());
            data.IS_ACTIVE = true;
            var result = await _appContext.GroupPositionSyses.AddAsync(data);
            try
            {
                await _appContext.SaveChangesAsync();
                param.Id = data.ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new ResultWithError(param);
        }
        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(GroupPositionSysInputDTO param)
        {
            // check code
            var c = _appContext.GroupPositionSyses.Where(x => x.CODE.ToLower() == param.Code.ToLower()  && x.ID != param.Id).Count();
            if (c > 0)
            {
                return new ResultWithError(Consts.CODE_EXISTS);
            }

            var r = _appContext.GroupPositionSyses.Where(x => x.ID == param.Id ).FirstOrDefault();
            if (r == null)
            {
                return new ResultWithError(404);
            }

            var data = Map(param, r);
            var result = _appContext.GroupPositionSyses.Update(data);
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
                var r = _appContext.GroupPositionSyses.Where(x => x.ID == item ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                r.IS_ACTIVE = !r.IS_ACTIVE;
                var result = _appContext.GroupPositionSyses.Update(r);
            }
            await _appContext.SaveChangesAsync();
            return new ResultWithError(200);
        }
        /// <summary>
        /// Get List Group is Activce
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetList()
        {
            var queryable = await (from p in _appContext.GroupPositionSyses
                                   where p.IS_ACTIVE == true 
                                   orderby p.CODE
                                   select new
                                   {
                                       Id = p.ID,
                                       Name = p.NAME,
                                       Code = p.CODE
                                   }).ToListAsync();
            return new ResultWithError(queryable);
        }

        public async Task<ResultWithError> Delete(long id)
        {
            try
            {
                var r = await _appContext.Positions.Where(x => x.GROUP_ID == id).CountAsync();
                if (r > 0)
                {
                    return new ResultWithError(Message.DATA_IS_USED);
                }
                var item = await _appContext.GroupPositionSyses.Where(x => x.ID == id).FirstOrDefaultAsync();
                _appContext.GroupPositionSyses.Remove(item);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(item);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Get List Group is Activce and by areaId 
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetListByArea(int areaId)
        {
            var queryable = await (from p in _appContext.GroupPositionSyses
                                   where p.IS_ACTIVE == true && p.AREA_ID == areaId
                                   orderby p.CODE
                                   select new
                                   {
                                       Id = p.ID,
                                       Name = p.NAME,
                                       Code = p.CODE
                                   }).ToListAsync();
            return new ResultWithError(queryable);
        }
    }
}
