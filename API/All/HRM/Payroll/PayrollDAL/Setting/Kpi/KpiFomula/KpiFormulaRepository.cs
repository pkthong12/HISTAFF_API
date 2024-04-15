using Common.Paging;
using PayrollDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace PayrollDAL.Repositories
{
    public class KpiFormulaRepository : RepositoryBase<PA_KPI_FORMULA>, IKpiFormulaRepository
    {
        private PayrollDbContext _appContext => (PayrollDbContext)_context;
        public KpiFormulaRepository(PayrollDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<KpiFormulaDTO>> GetAll(KpiFormulaDTO param)
        {
            var queryable = from p in _appContext.KpiFormulas
                            join c in _appContext.KpiTargets on p.COL_NAME.Trim().ToUpper() equals c.CODE.Trim().ToUpper()
                             
                            orderby c.ORDERS ascending
                            select new KpiFormulaDTO
                            {
                                Id = p.ID,
                                ColName = p.COL_NAME,
                                Orders = c.ORDERS,
                            };

            if (!string.IsNullOrWhiteSpace(param.ColName))
            {
                queryable = queryable.Where(p => p.ColName.ToUpper().Contains(param.ColName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.Note))
            {
                queryable = queryable.Where(p => p.Note.ToUpper().Contains(param.Note.ToUpper()));
            }
            return await PagingList(queryable, param);
        }
        /// <summary>
        /// CMS Get Detail for System
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> GetById(long id)
        {
            try
            {
                var r = await (from p in _appContext.KpiFormulas
                               where p.ID == id 
                               select new
                               {
                                   Id = p.ID,
                                   ColName = p.COL_NAME,
                                   Orders = p.ORDERS,
                                   IsActive = p.IS_ACTIVE,
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> GetListGroup()
        {
            try
            {
                var r = await (from p in _appContext.ElementGroups
                               where p.IS_ACTIVE == true
                               select new
                               {
                                   Id = p.ID,
                                   Code = p.CODE,
                                   Name = p.NAME,

                               }).ToListAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> GetList()
        {
            try
            {
                var r = await (from p in _appContext.KpiFormulas
                               where p.IS_ACTIVE == true 
                               orderby p.ORDERS
                               select new
                               {
                                   Id = p.ID,
                                   ColName = p.COL_NAME,
                               }).ToListAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// CMS Edit Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(KpiFormulaCreateDTO param)
        {
            try
            {
                var collection = await _appContext.KpiTargets
                    .Where(c => c.IS_ACTIVE == true ).ToListAsync();
                var fomulaName = param.Formula;


                foreach (var item in collection)
                {
                    param.Formula = param.Formula.Replace('[' + item.NAME + ']', "NVL(" + item.CODE+ ",0)");
                }

                if (param == null)
                {
                    return new ResultWithError(400);
                }

                if (param.Id == null)
                {
                    var data = Map(param, new PA_KPI_FORMULA());
                    data.IS_ACTIVE = true;
                    data.FORMULA_NAME = fomulaName;
                    var result = await _appContext.KpiFormulas.AddAsync(data);
                    await _appContext.SaveChangesAsync();
                    return new ResultWithError(data);
                }
                else
                {
                    var r = _appContext.KpiFormulas.Where(x => x.ID == param.Id ).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }

                    var data = Map(param, r);
                    data.FORMULA_NAME = fomulaName;
                    var result = _appContext.KpiFormulas.Update(data);
                    await _appContext.SaveChangesAsync();
                    return new ResultWithError(200);
                }

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
                    var r = _appContext.KpiFormulas.Where(x => x.ID == item).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    r.IS_ACTIVE = !r.IS_ACTIVE;
                    var result = _appContext.KpiFormulas.Update(r);
                }
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
