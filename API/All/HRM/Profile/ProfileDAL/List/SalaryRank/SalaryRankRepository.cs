using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public class SalaryRankRepository : RepositoryBase<HU_SALARY_RANK>, ISalaryRankRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        public SalaryRankRepository(ProfileDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<SalaryRankDTO>> GetAll(SalaryRankDTO param)
        {
            var queryable = from p in _appContext.SalaryRanks
                            join r in _appContext.SalaryScales on p.SALARY_SCALE_ID equals r.ID
                            
                            orderby p.ORDERS ascending
                            select new SalaryRankDTO
                            {
                                Id = p.ID,
                                Code = p.CODE,
                                Name = p.NAME,
                                Orders = p.ORDERS,
                                SalaryScaleId = p.SALARY_SCALE_ID,
                                SalaryScaleName = r.NAME,
                                IsActive = p.IS_ACTIVE,
                                Note = p.NOTE,
                                CreateBy = p.CREATED_BY,
                                UpdatedBy = p.UPDATED_BY,
                                CreateDate = p.CREATED_DATE,
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
            if (!string.IsNullOrWhiteSpace(param.SalaryScaleName))
            {
                queryable = queryable.Where(p => p.SalaryScaleName.ToUpper().Contains(param.SalaryScaleName.ToUpper()));
            }
            if (param.IsActive != null)
            {
                queryable = queryable.Where(p => p.IsActive == param.IsActive);
            }
            if (param.SalaryScaleId != null)
            {
                queryable = queryable.Where(p => p.SalaryScaleId == param.SalaryScaleId);
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
                var r = await (from p in _appContext.SalaryRanks
                               where p.ID == id 
                               select new
                               {
                                   Id = p.ID,
                                   Code = p.CODE,
                                   Name = p.NAME,
                                   Orders = p.ORDERS,
                                   SalaryScaleId = p.SALARY_SCALE_ID,
                                   LevelStart = p.LEVEL_START,
                                   IsActive = p.IS_ACTIVE,
                                   Note = p.NOTE,
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
        /// <summary>
        /// Create Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateAsync(SalaryRankInputDTO param)
        {

            try
            {
                // check code
                var c = _appContext.SalaryRanks.Where(x => x.CODE.ToLower() == param.Code.ToLower() ).Count();
                if (c > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }
                var data = Map(param, new HU_SALARY_RANK());
                data.IS_ACTIVE = true;
                var result = await _appContext.SalaryRanks.AddAsync(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(data);
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
        public async Task<ResultWithError> UpdateAsync(SalaryRankInputDTO param)
        {
            try
            {
                // check code
                var c = _appContext.SalaryRanks.Where(x => x.CODE.ToLower() == param.Code.ToLower() && x.ID != param.Id ).Count();
                if (c > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }

                var r = _appContext.SalaryRanks.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                var data = Map(param, r);
                data.IS_ACTIVE = true;
                var result = _appContext.SalaryRanks.Update(data);
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
                    var r = _appContext.SalaryRanks.Where(x => x.ID == item ).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    bool? status = !r.IS_ACTIVE;
                    r.IS_ACTIVE = status;
                    var result = _appContext.SalaryRanks.Update(r);
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
        public async Task<ResultWithError> GetListByScale(int? scaleId)
        {
            try
            {
                var queryable = await (from p in _appContext.SalaryRanks
                                       where p.SALARY_SCALE_ID == scaleId  && p.IS_ACTIVE == true
                                       orderby p.CODE
                                       select new
                                       {
                                           Id = p.ID,
                                           Name = p.NAME
                                       }).ToListAsync();

                return new ResultWithError(queryable);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// lấy bậc lương theo ngạch lương
        /// </summary>
        /// <returns></returns>
        public async Task<ResultWithError> GetRankList()
        {
            try
            {
                var r = await (from p in _appContext.SalaryScales
                               
                               select new
                               {
                                   Id = p.ID,
                                   Name = p.NAME,
                                   Code = p.CODE,
                                   Orders = p.ORDERS,
                                   Note = p.NOTE,
                                   LstRank = (from c in _appContext.SalaryRanks
                                              where  c.SALARY_SCALE_ID == p.ID && c.IS_ACTIVE == true
                                              select new {
                                                  Id = c.ID,
                                                  Name = c.NAME,
                                                  Code = c.CODE,
                                                  Orders = c.ORDERS,
                                                  Note = c.NOTE,
                                                  SalaryScaleId = c.SALARY_SCALE_ID,
                                                  IdScale = p.ID,
                                                  NameScale = p.NAME,
                                                  NoteScale = p.NOTE,
                                                  OrderScale = p.ORDERS,
                                                  CodeScale = p.CODE,
                                                  IsShow = false,
                                                  CountRank = _appContext.SalaryRanks.Where(x => x.SALARY_SCALE_ID == p.ID && x.IS_ACTIVE == true ).Count(),
                                                  CountLevel = _appContext.SalaryLevels.Where(x => x.SALARY_RANK_ID == c.ID && x.IS_ACTIVE == true ).Count(),
                                                  Lstlevel = _appContext.SalaryLevels.Where(x => x.SALARY_RANK_ID == c.ID  && x.IS_ACTIVE == true).OrderBy(y => y.NAME).ToList()
                                              }).ToList()
                               }).ToListAsync();

             

                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// lấy bậc lương theo ngạch lương
        /// </summary>
        /// <returns></returns>
        public async Task<ResultWithError> GetRankListAll()
        {
            try
            {
                var r = await (from p in _appContext.SalaryScales
                               
                               select new
                               {
                                   Id = p.ID,
                                   Name = p.NAME,
                               }).ToListAsync();



                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// lupdate level start
        /// </summary>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateLevelStart(SalaryRankStart param)
        {
            try
            {
                var r =  _appContext.SalaryRanks.Where(x => x.CODE.ToLower() == param.Code.ToLower() ).FirstOrDefault();
                if(r == null)
                {
                    return new ResultWithError(404);
                }
                r.LEVEL_START = param.LevelStart;
                 _appContext.SalaryRanks.Update(r);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
    }
}
