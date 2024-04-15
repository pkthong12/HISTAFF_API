using Common.Paging;
using PayrollDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace PayrollDAL.Repositories
{
    public class PaycheckRepository : RepositoryBase<PA_SALARY_PAYCHECK>, IPaycheckRepository
    {
        private PayrollDbContext _appContext => (PayrollDbContext)_context;
        public PaycheckRepository(PayrollDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<PaycheckDTO>> GetAll(PaycheckDTO param)
        {

            try
            {
                var queryable = from p in _appContext.Paychecks
                                join t in _appContext.SalaryTypes on p.SALARY_TYPE_ID equals t.ID
                                join e in _appContext.SalaryElements on p.ELEMENT_ID equals e.ID
                                orderby p.SALARY_TYPE_ID, p.ORDERS
                                select new PaycheckDTO
                                {
                                    Id = p.ID,
                                    Name = p.NAME,
                                    SalaryTypeName = t.NAME,
                                    ElementName = e.NAME,
                                    Orders = p.ORDERS,
                                    IsVisible = p.IS_VISIBLE,
                                    SalaryId = p.SALARY_TYPE_ID
                                };

                if (param.SalaryId != 0)
                {
                    queryable = queryable.Where(p => p.SalaryId == param.SalaryId);
                }
                if (!string.IsNullOrWhiteSpace(param.Name))
                {
                    queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
                }

                if (!string.IsNullOrWhiteSpace(param.SalaryTypeName))
                {
                    queryable = queryable.Where(p => p.SalaryTypeName.ToUpper().Contains(param.SalaryTypeName.ToUpper()));
                }

                if (!string.IsNullOrWhiteSpace(param.ElementName))
                {
                    queryable = queryable.Where(p => p.ElementName.ToUpper().Contains(param.ElementName.ToUpper()));
                }
                return await PagingList(queryable, param);

            }
            catch (Exception ex)
            {

                throw ex;
            }
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
                var r = await (from p in _appContext.Paychecks
                               join s in _appContext.SalaryTypes on p.SALARY_TYPE_ID equals s.ID
                               join e in _appContext.SalaryElements on p.ELEMENT_ID equals e.ID
                               where p.ID == id 
                               select new
                               {
                                   Id = p.ID,
                                   Name = p.NAME,
                                   TypeName = s.NAME,
                                   ElementName = e.NAME,
                                   Orders = p.ORDERS
                               }).FirstOrDefaultAsync();
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
        public async Task<ResultWithError> CreateAsync(PaycheckInputListDTO param)
        {
            try
            {
                string lst = "";
                foreach (var item in param.ElementId)
                {
                    lst = lst + "," + item.ToString();
                }

                lst = lst.Substring(1);
                var ds = await QueryData.ExecuteNonQuery("PKG_IMPORT.PAYCHECK_IMPORT",
                             new
                             {
                                 P_SALARY_TYPE_ID = param.SalaryTypeId,
                                 P_ELEMENT_ID = lst
                             }, true);
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
        public async Task<ResultWithError> UpdateAsync(PaycheckInputDTO param)
        {
            try
            {
                var r = _appContext.Paychecks.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }

                // var data = Map<PaycheckInputDTO, Paycheck>(param, r);
                r.NAME = param.Name;
                r.ORDERS = param.Orders;
                var result = _appContext.Paychecks.Update(r);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }


        /// <summary>
        /// CMS Edit Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> QuickUpdate(PaycheckInputDTO param)
        {
            try
            {
                var r = _appContext.Paychecks.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }

                if (param.IsVisible != null)
                {
                    r.IS_VISIBLE = param.IsVisible.Value;
                }

                var result = _appContext.Paychecks.Update(r);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }

        /// <summary>
        /// REMOVE LIST
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> RemoveAsync(List<long> ids)
        {
            try
            {
                string lst = "";
                foreach (var item in ids)
                {
                    lst = lst + "," + item.ToString();
                }

                lst = lst.Substring(1);
                var ds = await QueryData.ExecuteNonQuery("PKG_IMPORT.PAYCHECK_REMOVE",
                             new
                             {
                                 P_IDS = lst
                             }, true);
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
    }
}
