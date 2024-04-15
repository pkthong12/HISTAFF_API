using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public class ShiftSysRepository : RepositoryBase<SYS_SHIFT>, IShiftSysRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        public ShiftSysRepository(ProfileDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<ShiftSysDTO>> GetAll(ShiftSysDTO param)
        {
            var queryable = from p in _appContext.ShiftSyses
                            select new ShiftSysDTO
                            {
                                Id = p.ID,
                                Code = p.CODE,
                                Name = p.NAME,
                                HoursStart = p.HOURS_START,
                                HoursStop = p.HOURS_STOP,
                                BreaksFrom = p.BREAKS_FROM,
                                BreaksTo = p.BREAKS_TO,
                                IsNoon = p.IS_NOON,
                                IsBreak = p.IS_BREAK,
                                TimeLate = p.TIME_LATE,
                                TimeEarly = p.TIME_EARLY,
                                Note = p.NOTE,
                                AreaId = p.AREA_ID,
                                IsActive = p.IS_ACTIVE,
                                CreateBy = p.CREATED_BY,
                                UpdatedBy = p.UPDATED_BY,
                                CreateDate = p.CREATED_DATE,
                                UpdatedDate = p.UPDATED_DATE,
                            };


            if (!string.IsNullOrWhiteSpace(param.Name))
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.Note))
            {
                queryable = queryable.Where(p => p.Note.ToUpper().Contains(param.Note.ToUpper()));
            }

            if (param.AreaId != null)
            {
                queryable = queryable.Where(p => p.AreaId == param.AreaId);
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
                var r = await (from p in _appContext.ShiftSyses.Where(c => c.ID == id)
                               select new
                               {
                                   Id = p.ID,
                                   Code = p.CODE,
                                   Name = p.NAME,
                                   AreaId = p.AREA_ID,
                                   HoursStart = p.HOURS_START,
                                   HoursStop = p.HOURS_STOP,
                                   BreaksFrom = p.BREAKS_FROM,
                                   BreaksTo = p.BREAKS_TO,
                                   TimeTypeId = p.TIME_TYPE_ID,
                                   IsNoon = p.IS_NOON,
                                   IsBreak = p.IS_BREAK,
                                   TimeLate = p.TIME_LATE,
                                   TImeEarly = p.TIME_EARLY,
                                   Note = p.NOTE,
                                   MonId = p.MON_ID,
                                   TueId = p.TUE_ID,
                                   WedId = p.WED_ID,
                                   ThuId = p.THU_ID,
                                   FriId = p.FRI_ID,
                                   SatId = p.SAT_ID,
                                   SunId = p.SUN_ID,
                                   IsActive = p.IS_ACTIVE,
                                   CreateBy = p.CREATED_BY,
                                   UpdatedBy = p.UPDATED_BY,
                                   CreateDate = p.CREATED_DATE,
                                   UpdatedDate = p.UPDATED_DATE,
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> GetShiftCycle(int id)
        {
            try
            {
                var r = await (from p in _appContext.ShiftSyses.Where(c => c.ID == id)
                               select new
                               {
                                   Id = p.ID,
                                   MonId = p.MON_ID,
                                   TueId = p.TUE_ID,
                                   WedId = p.WED_ID,
                                   ThuId = p.THU_ID,
                                   FriId = p.FRI_ID,
                                   SatId = p.SAT_ID,
                                   SunId = p.SUN_ID
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
        public async Task<ResultWithError> CreateAsync(ShiftSysInputDTO param)
        {
            try
            {

                var a = _appContext.ShiftSyses.Where(x => x.CODE == param.Code ).Count();
                if (a > 0)
                {
                    return new ResultWithError(Message.CODE_EXIST);
                }
  
                var data = Map(param, new SYS_SHIFT());
                data.IS_ACTIVE = true;
                var result = await _appContext.ShiftSyses.AddAsync(data);
                await _appContext.SaveChangesAsync();
                param.Id = data.ID;
                return new ResultWithError(param);
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
        public async Task<ResultWithError> UpdateAsync(ShiftSysInputDTO param)
        {
            try
            {
                // check code

                var r = _appContext.ShiftSyses.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                param.Code = null;
                var data = Map(param, r);
                var result = _appContext.ShiftSyses.Update(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex);
            }
        }  /// <summary>
           /// CMS Edit ShiftSys cycle
           /// </summary>
           /// <param name="param"></param>
           /// <returns></returns>
        public async Task<ResultWithError> UpdateShiftCycle(ShiftCycleSysInput param)
        {
            try
            {
                // check code

                var r = _appContext.ShiftSyses.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                var data = Map(param, r);
                var result = _appContext.ShiftSyses.Update(data);
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
                    var r = _appContext.ShiftSyses.Where(x => x.ID == item ).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    r.IS_ACTIVE = !r.IS_ACTIVE;
                    var result = _appContext.ShiftSyses.Update(r);
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
                var queryable = await (from p in _appContext.ShiftSyses
                                       where p.IS_ACTIVE == true 
                                       select new
                                       {
                                           Id = p.ID,
                                           Name = p.NAME,
                                           MonId = p.MON_ID,
                                           TueId = p.TUE_ID,
                                           WedId = p.WED_ID,
                                           ThuId = p.THU_ID,
                                           FriId = p.FRI_ID,
                                           SatId = p.SAT_ID,
                                           SunId = p.SUN_ID
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
