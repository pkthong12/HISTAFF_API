using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public class PositionSysRepository : RepositoryBase<HU_POSITION_GROUP>, IPositionSysRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        public PositionSysRepository(ProfileDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<PositionSysViewDTO>> GetAll(PositionSysViewDTO param)
        {
            var queryable = from p in _appContext.PositionSyses
                            from g in _appContext.GroupPositionSyses.Where(c => c.ID == p.GROUP_ID)
                            orderby p.CREATED_DATE descending
                            select new PositionSysViewDTO
                            {
                                Id = p.ID,
                                Name = p.NAME,
                                GroupName = g.NAME,
                                GroupId = p.GROUP_ID,
                                AreaId = p.AREA_ID,
                                Code = p.CODE,
                                IsActive = p.IS_ACTIVE,
                                Note = p.NOTE,
                                JobDesc = p.JOB_DESC,
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
            if (!string.IsNullOrEmpty(param.GroupName))
            {
                queryable = queryable.Where(p => p.Note.ToUpper().Contains(param.Note.ToUpper()));
            }
            if (param.IsActive != null)
            {
                queryable = queryable.Where(p => p.IsActive == param.IsActive);
            }
            if (param.GroupId != null)
            {
                queryable = queryable.Where(p => p.GroupId == param.GroupId);
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
            try
            {
                var r = await (from p in _appContext.PositionSyses
                               where p.ID == id
                               select new
                               {
                                   Id = p.ID,
                                   Name = p.NAME,
                                   Code = p.CODE,
                                   Note = p.NOTE,
                                   GroupId = p.GROUP_ID,
                                   AreaId = p.AREA_ID,
                                   JobDesc = p.JOB_DESC
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
        public async Task<ResultWithError> CreateAsync(PositionSysInputDTO param)
        {
            try
            {
                var r1 = _appContext.PositionSyses.Where(x => x.CODE == param.Code ).Count();
                if (r1 > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }
                var r = _appContext.GroupPositionSyses.Where(x => x.ID == param.GroupId).Count();
                if (r == 0)
                {
                    return new ResultWithError("GROUP_NOT_EXISTS");
                }
                var data = Map(param, new SYS_POSITION());
                data.IS_ACTIVE = true;
                var result = await _appContext.PositionSyses.AddAsync(data);
                await _appContext.SaveChangesAsync();
                //return data
                var output = new PositionSysOutputDTO();
                var values = Map(data, output);
                return new ResultWithError(values);
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
        public async Task<ResultWithError> UpdateAsync(PositionSysInputDTO param)
        {
            try
            {
                // check code
                var c = _appContext.PositionSyses.Where(x => x.CODE.ToLower() == param.Code.ToLower()  && x.ID != param.Id).Count();
                if (c > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }
                var r = _appContext.GroupPositionSyses.Where(x => x.ID == param.GroupId ).Count();
                if (r == 0)
                {
                    return new ResultWithError("GROUP_NOT_EXISTS");
                }
                var a = _appContext.PositionSyses.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (a == null)
                {
                    return new ResultWithError(404);
                }

                var data = Map(param, a);
                var result = _appContext.PositionSyses.Update(data);
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
                    var r = _appContext.PositionSyses.Where(x => x.ID == item ).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    r.IS_ACTIVE = !r.IS_ACTIVE;
                    var result = _appContext.PositionSyses.Update(r);
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
        public async Task<ResultWithError> GetList(int groupId)
        {
            try
            {
                var queryable = from p in _appContext.PositionSyses
                                where p.IS_ACTIVE == true 
                                orderby p.CODE
                                select new {p};

                if(groupId != 0)
                {
                    queryable = queryable.Where(c => c.p.GROUP_ID == groupId);
                }

                var data = await queryable.Select(c => new {
                    Id = c.p.ID,
                    Code = c.p.CODE,
                    Name = c.p.NAME
                }).ToListAsync();
                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// Delete by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultWithError> Delete(long id)
        {
            try
            {
                var r = await _appContext.Employees.Where(x => x.POSITION_ID == id ).CountAsync();
                if (r > 0)
                {
                    return new ResultWithError(Message.DATA_IS_USED);
                }
                var item = await _appContext.PositionSyses.Where(x => x.ID == id ).FirstOrDefaultAsync();
                _appContext.PositionSyses.Remove(item);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(item);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
    }
}
