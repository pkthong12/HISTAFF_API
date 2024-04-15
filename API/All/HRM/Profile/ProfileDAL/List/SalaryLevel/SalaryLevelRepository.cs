using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public class SalaryLevelRepository : RepositoryBase<HU_SALARY_LEVEL>, ISalaryLevelRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        public SalaryLevelRepository(ProfileDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<SalaryLevelDTO>> GetAll(SalaryLevelDTO param)
        {
            var queryable = from p in _appContext.SalaryLevels
                            join v in _appContext.SalaryRanks on p.SALARY_RANK_ID equals v.ID
                            join x in _appContext.SalaryScales on v.SALARY_SCALE_ID equals x.ID

                            select new SalaryLevelDTO
                            {
                                Id = p.ID,
                                Code = p.CODE,
                                Name = p.NAME,
                                Orders = p.ORDERS,
                                SalaryRankId = p.SALARY_RANK_ID,
                                SalaryRankName = v.NAME,
                                SalaryScaleId = x.ID,
                                SalaryScaleName = x.NAME,
                                //Monney = p.MONNEY,
                                IsActive = p.IS_ACTIVE,
                                Note = p.NOTE
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
                queryable = queryable.Where(p => p.SalaryRankName.ToUpper().Contains(param.SalaryRankName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.SalaryRankName))
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
            if (param.SalaryRankId != null)
            {
                queryable = queryable.Where(p => p.SalaryRankId == param.SalaryRankId);
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
                var r = await (from p in _appContext.SalaryLevels
                               join v in _appContext.SalaryRanks on p.SALARY_RANK_ID equals v.ID
                               where p.ID == id
                               select new
                               {
                                   Id = p.ID,
                                   code = p.CODE,
                                   name = p.NAME,
                                   orders = p.ORDERS,
                                   salaryRankId = p.SALARY_RANK_ID,
                                   salaryScaleId = v.SALARY_SCALE_ID,
                                   //monney = p.MONNEY,
                                   isActive = p.IS_ACTIVE,
                                   note = p.NOTE,
                                   Coefficient = p.COEFFICIENT
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
        public async Task<ResultWithError> CreateAsync(SalaryLevelInputDTO param)
        {
            try
            {
                // check code
                var c = _appContext.SalaryLevels.Where(x => x.CODE.ToLower() == param.Code.ToLower()).Count();
                if (c > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }
                var data = Map(param, new HU_SALARY_LEVEL());
                data.IS_ACTIVE = true;
                var result = await _appContext.SalaryLevels.AddAsync(data);
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
        public async Task<ResultWithError> UpdateAsync(SalaryLevelInputDTO param)
        {
            try
            {
                // check code
                var c = _appContext.SalaryLevels.Where(x => x.CODE.ToLower() == param.Code.ToLower() && x.ID != param.Id).Count();
                if (c > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }

                var r = _appContext.SalaryLevels.Where(x => x.ID == param.Id).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                var data = Map(param, r);
                var result = _appContext.SalaryLevels.Update(data);
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
                    var r = _appContext.SalaryLevels.Where(x => x.ID == item).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    r.IS_ACTIVE = !r.IS_ACTIVE;
                    var result = _appContext.SalaryLevels.Update(r);
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
        public async Task<ResultWithError> GetList(int rankId)
        {
            try
            {
                var queryable = await (from p in _appContext.SalaryLevels
                                       where p.IS_ACTIVE == true && p.SALARY_RANK_ID == rankId
                                       orderby p.ORDERS
                                       select new
                                       {
                                           Id = p.ID,
                                           Name = p.NAME,
                                           //SalBasic = p.MONNEY,
                                           Coefficient = p.COEFFICIENT
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
