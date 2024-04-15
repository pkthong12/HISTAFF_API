using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public class RoomRepository : RepositoryBase<AD_ROOM>, IRoomRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        public RoomRepository(ProfileDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<RoomDTO>> GetAll(RoomDTO param)
        {
            var queryable = from p in _appContext.Rooms
                            
                            select new RoomDTO
                            {
                                Id = p.ID,
                                Name = p.NAME,
                                Code = p.CODE,
                                IsActive = p.IS_ACTIVE,
                                Orders = p.ORDERS,
                                Address = p.ADDRESS,
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
            if (param.Address != null)
            {
                queryable = queryable.Where(p => p.Address.ToUpper().Contains(param.Address.ToUpper()));
            }
            if (param.Note != null)
            {
                queryable = queryable.Where(p => p.Note.ToUpper().Contains(param.Note.ToUpper()));
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
            var r = await (from p in _appContext.Rooms
                           where p.ID == id 
                           select new
                           {
                               Id = p.ID,
                               Name = p.NAME,
                               Code = p.CODE,
                               IsActive = p.IS_ACTIVE,
                               Orders = p.ORDERS,
                               Address = p.ADDRESS,
                               Note = p.NOTE
                           }).FirstOrDefaultAsync();
            return new ResultWithError(r);
        }
        /// <summary>
        /// Create Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateAsync(RoomInputDTO param)
        {
            var r = _appContext.Rooms.Where(x => x.CODE == param.Code ).Count();
            if (r > 0)
            {
                return new ResultWithError(Consts.CODE_EXISTS);
            }
            var data = Map(param, new AD_ROOM());
            data.IS_ACTIVE = true;
            var result = await _appContext.Rooms.AddAsync(data);
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
        public async Task<ResultWithError> UpdateAsync(RoomInputDTO param)
        {
            // check code
            var c = _appContext.Rooms.Where(x => x.CODE.ToLower() == param.Code.ToLower()  && x.ID != param.Id).Count();
            if (c > 0)
            {
                return new ResultWithError(Consts.CODE_EXISTS);
            }

            var r = _appContext.Rooms.Where(x => x.ID == param.Id ).FirstOrDefault();
            if (r == null)
            {
                return new ResultWithError(404);
            }

            var data = Map(param, r);
            var result = _appContext.Rooms.Update(data);
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
                var r = _appContext.Rooms.Where(x => x.ID == item ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                r.IS_ACTIVE = !r.IS_ACTIVE;
                var result = _appContext.Rooms.Update(r);
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
            var queryable = await (from p in _appContext.Rooms
                                   where p.IS_ACTIVE == true 
                                   orderby p.ORDERS
                                   select new
                                   {
                                       Id = p.ID,
                                       Name = p.NAME,
                                       Code = p.CODE
                                   }).ToListAsync();
            return new ResultWithError(queryable);
        }

        public async Task<ResultWithError> Remove(int id)
        {
            try
            {
                var r = _appContext.Bookings.Where(x => x.ROOM_ID == id).Count();
                if (r > 0)
                {
                    return new ResultWithError("DATA_IS_USE_BOOKING");
                }
                var item = await _appContext.Rooms.Where(x => x.ID == id ).FirstOrDefaultAsync();
                _appContext.Rooms.Remove(item);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
    }
}
