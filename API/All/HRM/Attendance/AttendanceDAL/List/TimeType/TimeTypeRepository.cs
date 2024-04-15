using Common.Paging;
using AttendanceDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace AttendanceDAL.Repositories
{
    public class TimeTypeRepository : RepositoryBase<AT_TIME_TYPE>, ITimeTypeRepository
    {
        private AttendanceDbContext _appContext => (AttendanceDbContext)_context;
        public TimeTypeRepository(AttendanceDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<TimeTypeDTO>> GetAll(TimeTypeDTO param)
        {
            var queryable = from p in _appContext.TimeTypes
                            from m in _appContext.Symbols.Where(x => x.ID == p.MORNING_ID)
                            from a in _appContext.Symbols.Where(x => x.ID == p.AFTERNOON_ID)
                            select new TimeTypeDTO
                            {
                                Id = p.ID,
                                Code = p.CODE,
                                Name = p.NAME,
                                MorningId = p.MORNING_ID,
                                MorningName = "[" + m.CODE + "] " + m.NAME,
                                //MorningName = m.NAME,
                                AfternoonId = p.AFTERNOON_ID,
                                AfternoonName = "[" + a.CODE + "] " + a.NAME,
                                //AfternoonName = a.NAME,
                                IsOff = p.IS_OFF,
                                IsActive = p.IS_ACTIVE,
                                Orders = p.ORDERS,
                                Note = p.NOTE,
                                CreatedBy = p.CREATED_BY,
                                UpdatedBy = p.UPDATED_BY,
                                CreatedDate = p.CREATED_DATE,
                                UpdatedDate = p.UPDATED_DATE
                            };


            if (!string.IsNullOrWhiteSpace(param.Name))
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.Code))
            {
                queryable = queryable.Where(p => p.Code.ToUpper().Contains(param.Code.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.Note))
            {
                queryable = queryable.Where(p => p.Note.ToUpper().Contains(param.Note.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.MorningName))
            {
                queryable = queryable.Where(p => p.MorningName.ToUpper().Contains(param.MorningName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.AfternoonName))
            {
                queryable = queryable.Where(p => p.AfternoonName.ToUpper().Contains(param.AfternoonName.ToUpper()));
            }
            if (param.MorningId != null)
            {
                queryable = queryable.Where(p => p.MorningId == param.MorningId);
            }
            if (param.AfternoonId != null)
            {
                queryable = queryable.Where(p => p.AfternoonId == param.AfternoonId);
            }
            if (param.IsOff != null)
            {
                queryable = queryable.Where(p => p.IsOff == param.IsOff);
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
                var r = await (from p in _appContext.TimeTypes
                               where p.ID == id
                               select new
                               {
                                   Id = p.ID,
                                   Code = p.CODE,
                                   Name = p.NAME,
                                   IsActive = p.IS_ACTIVE,
                                   AfternoonId = p.AFTERNOON_ID,
                                   MorningId = p.MORNING_ID,
                                   IsOff = p.IS_OFF,
                                   Note = p.NOTE
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
        public async Task<ResultWithError> CreateAsync(TimeTypeInputDTO param)
        {
            try
            {
                var r = _appContext.TimeTypes.Where(x => x.CODE == param.Code.ToUpper().Trim()).Count();
                if (r > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }

                var m = _appContext.Symbols.Where(x => x.ID == param.MorningId).Count();
                if (m == 0)
                {
                    return new ResultWithError("MORNING_NOT_EXISTS");
                }
                var a = _appContext.Symbols.Where(x => x.ID == param.AfternoonId).Count();
                if (a == 0)
                {
                    return new ResultWithError("AFTERNOON_NOT_EXISTS");
                }

                var data = Map(param, new AT_TIME_TYPE());
                data.CODE = param.Code.Trim().ToUpper();
                data.IS_ACTIVE = true;
                var result = await _appContext.TimeTypes.AddAsync(data);
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
        public async Task<ResultWithError> UpdateAsync(TimeTypeInputDTO param)
        {
            try
            {
                // check code
                var c = _appContext.TimeTypes.Where(x => x.CODE.ToLower() == param.Code.ToLower() && x.ID != param.Id).Count();
                if (c > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }

                var r = _appContext.TimeTypes.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                var m = _appContext.Symbols.Where(x => x.ID == param.MorningId).Count();
                if (m == 0)
                {
                    return new ResultWithError("MORNING_NOT_EXISTS");
                }
                var a = _appContext.Symbols.Where(x => x.ID == param.AfternoonId).Count();
                if (a == 0)
                {
                    return new ResultWithError("AFTERNOON_NOT_EXISTS");
                }
                var data = Map(param, r);
                var result = _appContext.TimeTypes.Update(data);
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
                    var r = _appContext.TimeTypes.Where(x => x.ID == item ).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    r.IS_ACTIVE = !r.IS_ACTIVE;
                    var result = _appContext.TimeTypes.Update(r);
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
                var queryable = await (from p in _appContext.TimeTypes
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
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> GetListOff()
        {
            try
            {
                var queryable = await (from p in _appContext.TimeTypes
                                       where p.IS_ACTIVE == true  && p.IS_OFF == true
                                       orderby p.NAME
                                       select new
                                       {
                                           Id = p.ID,
                                           Code = p.CODE,
                                           Name = "[" + p.CODE + "] " + p.NAME
                                       }).ToListAsync();
                return new ResultWithError(queryable);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> PortalGetListOff()
        {
            try
            {
                var r = await QueryData.ExecuteStore<TimeTypeView>("PKG_TIMESHEET_V2.LIST_TIMETYPE", new
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
