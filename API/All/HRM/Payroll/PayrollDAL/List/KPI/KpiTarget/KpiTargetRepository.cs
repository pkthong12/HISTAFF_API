using Common.Paging;
using PayrollDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace PayrollDAL.Repositories
{
    public class KpiTargetRepository : RepositoryBase<PA_KPI_TARGET>, IKpiTargetRepository
    {
        private PayrollDbContext _appContext => (PayrollDbContext)_context;
        public KpiTargetRepository(PayrollDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<KpiTargetOutDTO>> GetAll(KpiTargetDTO param)
        {
            var queryable = from p in _appContext.KpiTargets
                            where p.KPI_GROUP_ID == param.GroupId
                            orderby p.KPI_GROUP_ID, p.NAME
                            select new KpiTargetOutDTO
                            {

                                Id = p.ID,
                                Code = p.CODE,
                                Name = p.NAME,
                                IsImportKpi = p.IS_IMPORT_KPI,
                                IsPaySalary = p.IS_PAY_SALARY,
                                IsRealValue = p.IS_REAL_VALUE,
                                SalaryElement = p.COL_NAME,
                                Unit = p.UNIT,
                                Orders = p.ORDERS,
                                IsActive = p.IS_ACTIVE,
                            };

            if (!string.IsNullOrWhiteSpace(param.Name))
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.Code))
            {
                queryable = queryable.Where(p => p.Code.ToUpper().Contains(param.Code.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.Unit))
            {
                queryable = queryable.Where(p => p.Unit.ToUpper().Contains(param.Unit.ToUpper()));
            }
            if (param.Orders != null)
            {
                queryable = queryable.Where(p => p.Orders == param.Orders);
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
                var r = await (from p in _appContext.KpiTargets
                               where p.ID == id 
                               select new
                               {
                                   Id = p.ID,
                                   Code = p.CODE,
                                   Name = p.NAME,
                                   KpiGroupId = p.KPI_GROUP_ID,
                                   Unit = p.UNIT,
                                   MaxValue = p.MAX_VALUE,
                                   IsRealValue = p.IS_REAL_VALUE,
                                   IsPaySalary = p.IS_PAY_SALARY,
                                   IsImportKpi = p.IS_IMPORT_KPI,
                                   IsActive = p.IS_ACTIVE,
                                   Orders = p.ORDERS,
                                   Note = p.NOTE,
                                   ColName = p.COL_NAME,
                                   ColId = p.COL_ID,
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        //public async Task<ResultWithError> GetList(int groupid, int? typeId)
        public async Task<ResultWithError> GetList()
        {
            try
            {

                var r = await (from p in _appContext.KpiTargets
                               where p.IS_ACTIVE == true 
                               orderby p.NAME
                               select new
                               {
                                   Id = p.ID,
                                   Code = p.CODE,
                                   Name = p.NAME,
                               }).ToListAsync();
                return new ResultWithError(r);

                //if (groupid == 0)
                //{
                //    var r = await (from p in _appContext.KpiTargets
                //                   where p.IS_ACTIVE == true  && p.TYPE_ID == typeId
                //                   select new
                //                   {
                //                       Id = p.ID,
                //                       Code = p.CODE,
                //                       Name = p.NAME,
                //                   }).ToListAsync();
                //    return new ResultWithError(r);
                //}
                //else
                //{
                //    var r = await (from p in _appContext.KpiTargets
                //                   where p.IS_ACTIVE == true && p.KPI_GROUP_ID == groupid 
                //                   select new
                //                   {
                //                       Id = p.ID,
                //                       Code = p.CODE,
                //                       Name = p.NAME,
                //                   }).ToListAsync();
                //    return new ResultWithError(r);
                //}

            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> GetListFomula()
        {
            try
            {
                var r = await (from p in _appContext.KpiFormulas
                               join t in _appContext.KpiTargets on p.COL_NAME equals t.CODE
                               where p.IS_ACTIVE == true
                               orderby p.ORDERS
                               select new
                               {
                                   Id = p.ID,
                                   //ColName = t.CODE,
                                   Name = t.NAME,
                                   Formula = p.FORMULA_NAME,
                                   Orders = p.ORDERS
                               }).ToListAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Create Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateAsync(KpiTargetInputDTO param)
        {
            try
            {
                // check group exist
                var g = _appContext.KpiGroups.Where(c => c.ID == param.KpiGroupId ).Count();
                if (g == 0)
                {
                    return new ResultWithError(Message.RECORD_NOT_FOUND);
                }
                // Check code
                var r = _appContext.KpiTargets.Where(x => x.CODE == param.Code.Trim().ToUpper() ).Count();
                if (r > 0)
                {
                    return new ResultWithError(Consts.CODE_EXISTS);
                }
                await _appContext.Database.BeginTransactionAsync();
                var data = Map(param, new PA_KPI_TARGET());
                data.CODE = data.CODE.Trim().ToUpper();
                data.IS_ACTIVE = true;
                var result = await _appContext.KpiTargets.AddAsync(data);
                await _appContext.SaveChangesAsync();
                //Tạo formular
                var obj = new PA_KPI_FORMULA();
                obj.COL_NAME = data.CODE;
                obj.ORDERS = 0;
                obj.IS_ACTIVE = param.IsPaySalary.Value;
                await _appContext.KpiFormulas.AddAsync(obj);
                await _appContext.SaveChangesAsync();
                _appContext.Database.CommitTransaction();
                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                _appContext.Database.RollbackTransaction();
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// CMS Edit Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(KpiTargetInputDTO param)
        {
            try
            {
               
                var r = _appContext.KpiTargets.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                
                var data = Map(param, r);
                data.CODE = r.CODE;
                if (param.ColId == null)
                {
                    data.COL_ID = null;
                    data.COL_NAME = null;
                }
                var result = _appContext.KpiTargets.Update(data);
                await _appContext.SaveChangesAsync();
                var a = QueryData.Execute(Procedures.PKG_PAYROLL_SETTING_ELEMENT_FORMULA_KPI_CHANGE,
                   new
                   {
                       P_COL_NAME = r.CODE,
                       P_STATUS_ID = param.IsPaySalary == true ? 1 : 0
                   }, true);

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
                    var r = _appContext.KpiTargets.Where(x => x.ID == item).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    r.IS_ACTIVE = !r.IS_ACTIVE;
                    var result = _appContext.KpiTargets.Update(r);
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
        /// CMS Edit Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> QuickUpdate(KpiTargetQickDTO param)
        {
            try
            {
                var r = _appContext.KpiTargets.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                if (param.IsImportKpi != null)
                {
                    r.IS_IMPORT_KPI = param.IsImportKpi.Value;
                }
                if (param.IsPaySalary != null)
                {
                    r.IS_PAY_SALARY = param.IsPaySalary.Value;
                }

                var result = _appContext.KpiTargets.Update(r);
                await _appContext.SaveChangesAsync();
                if (param.IsPaySalary != null)
                {
                    var a = QueryData.Execute(Procedures.PKG_PAYROLL_SETTING_ELEMENT_FORMULA_KPI_CHANGE,
                    new
                    {
                        P_COL_NAME = r.CODE,
                        P_STATUS_ID = param.IsPaySalary == true ? 1 : 0
                    }, true);
                }

                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }
    }
}
