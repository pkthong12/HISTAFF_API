using Common.Paging;
using PayrollDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace PayrollDAL.Repositories
{
    public class SalaryStructureRepository : RepositoryBase<PA_SALARY_STRUCTURE>, ISalaryStructureRepository
    {
        private PayrollDbContext _appContext => (PayrollDbContext)_context;
        public SalaryStructureRepository(PayrollDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<SalaryStructureDTO>> GetAll(SalaryStructureDTO param)
        {

            try
            {
                var queryable = from p in _appContext.SalaryStructures
                                join t in _appContext.SalaryTypes on p.SALARY_TYPE_ID equals t.ID
                                join e in _appContext.SalaryElements on p.ELEMENT_ID equals e.ID
                                join g in _appContext.ElementGroups on e.GROUP_ID equals g.ID
                                orderby p.SALARY_TYPE_ID, p.ORDERS
                                select new SalaryStructureDTO
                                {
                                    Id = p.ID,
                                    ColName = e.CODE,
                                    Name = e.NAME,
                                    SalaryTypeName = t.NAME,
                                    GroupName = g.NAME,
                                    ElementName = e.NAME,
                                    Orders = p.ORDERS,
                                    IsCalculate = p.IS_CALCULATE,
                                    IsVisible = p.IS_VISIBLE,
                                    IsImport = p.IS_IMPORT,
                                    IsSum = p.IS_SUM,
                                    IsChange = p.IS_CHANGE,
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
                if (!string.IsNullOrWhiteSpace(param.ColName))
                {
                    queryable = queryable.Where(p => p.ColName.ToUpper().Contains(param.ColName.ToUpper()));
                }
                if (!string.IsNullOrWhiteSpace(param.SalaryTypeName))
                {
                    queryable = queryable.Where(p => p.SalaryTypeName.ToUpper().Contains(param.SalaryTypeName.ToUpper()));
                }
                if (!string.IsNullOrWhiteSpace(param.GroupName))
                {
                    queryable = queryable.Where(p => p.GroupName.ToUpper().Contains(param.GroupName.ToUpper()));
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
                var r = await (from p in _appContext.SalaryStructures
                               join e in _appContext.SalaryElements on p.ELEMENT_ID equals e.ID into tmp1
                               from e2 in tmp1.DefaultIfEmpty()
                               where p.ID == id 
                               select new
                               {
                                   Id = p.ID,
                                   ColName = e2.CODE,
                                   Name = e2.NAME,
                                   SalaryTypeId = p.SALARY_TYPE_ID,
                                   GroupId = e2.GROUP_ID,
                                   ElementId = p.ELEMENT_ID,
                                   Orders = p.ORDERS,
                                   IsCalculate = p.IS_CALCULATE,
                                   IsVisible = p.IS_VISIBLE,
                                   IsImport = p.IS_IMPORT,
                                   IsSum = p.IS_SUM,
                                   IsChange = p.IS_CHANGE,
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> GetList(int id)
        {
            try
            {
                var r = await (from p in _appContext.SalaryStructures
                               join e in _appContext.SalaryElements on p.ELEMENT_ID equals e.ID
                               where p.SALARY_TYPE_ID == id  && p.IS_VISIBLE == true
                               orderby p.ORDERS
                               select new
                               {
                                   Name = e.NAME,
                                   Code = e.CODE
                               }).ToListAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// Get List Element by Import
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetListImport(int id)
        {
            try
            {
                var r = await (from p in _appContext.SalaryStructures
                               join e in _appContext.SalaryElements on p.ELEMENT_ID equals e.ID
                               where p.SALARY_TYPE_ID == id  && p.IS_IMPORT == true
                               orderby p.ORDERS
                               select new
                               {
                                   Name = e.NAME,
                                   Code = e.CODE
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
        public async Task<ResultWithError> CreateAsync(SalaryStructureInputDTO param)
        {
            try
            {
                await _appContext.Database.BeginTransactionAsync();
                // check đã tạo structure cho bảng lương
                var p = await _appContext.SalaryStructures.Where(c => c.ELEMENT_ID == param.ElementId && c.SALARY_TYPE_ID == param.SalaryTypeId ).AnyAsync();
                if (p == true)
                {
                    return new ResultWithError(Message.DATA_IS_EXISTS);
                }
                var data = Map(param, new PA_SALARY_STRUCTURE());
                
                var result = await _appContext.SalaryStructures.AddAsync(data);
                await _appContext.SaveChangesAsync();
                _appContext.Database.CommitTransaction();
                var a = QueryData.Execute(Procedures.PKG_PAYROLL_SETTING_ELEMENT_FORMULA_CHANGE,
                  new
                  {
                      P_SALARY_TYPE = param.SalaryTypeId,
                      P_ELEMENT_ID = param.ElementId,
                      P_STATUS_ID = param.IsCalculate == true ? 1 : 0
                  }, true);


                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                _appContext.Database.RollbackTransaction();
                return new ResultWithError(ex);
            }
        }

        /// <summary>
        /// CMS Edit Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(SalaryStructureInputDTO param)
        {
            try
            {
                var r = _appContext.SalaryStructures.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }

                var data = Map(param, r);
                var result = _appContext.SalaryStructures.Update(data);
                await _appContext.SaveChangesAsync();
                await QueryData.Execute(Procedures.PKG_PAYROLL_SETTING_ELEMENT_FORMULA_CHANGE,
                   new
                   {
                       P_SALARY_TYPE = param.SalaryTypeId,
                       P_ELEMENT_ID = param.ElementId,
                       P_STATUS_ID = param.IsCalculate == true ? 1 : 0
                   }, true);

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
        public async Task<ResultWithError> QuickUpdate(SalaryStructureInputDTO param)
        {
            try
            {
                var r = _appContext.SalaryStructures.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                if (param.IsCalculate != null)
                {
                    r.IS_CALCULATE = param.IsCalculate.Value;
                }
                if (param.IsImport != null)
                {
                    r.IS_IMPORT = param.IsImport.Value;
                }
                if (param.IsVisible != null)
                {
                    r.IS_VISIBLE = param.IsVisible.Value;
                }
                if (param.IsSum != null)
                {
                    r.IS_SUM = param.IsSum.Value;
                }
                if (param.IsChange != null)
                {
                    r.IS_CHANGE = param.IsChange.Value;
                }
                var result = _appContext.SalaryStructures.Update(r);
                await _appContext.SaveChangesAsync();
                if (param.IsCalculate != null)
                {
                    await QueryData.Execute(Procedures.PKG_PAYROLL_SETTING_ELEMENT_FORMULA_CHANGE,
                                      new
                                      {
                                          P_SALARY_TYPE = r.SALARY_TYPE_ID,
                                          P_ELEMENT_ID = r.ELEMENT_ID,
                                          P_STATUS_ID = param.IsCalculate == true ? 1 : 0
                                      }, true);
                }
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
        public async Task<ResultWithError> Delete(long id)
        {
            try
            {
                await QueryData.Execute(Procedures.PKG_PAYROLL_SETTING_STRUCT_REMOVE_ELEMENT,
                                  new
                                  {
                                      P_ID = id
                                  }, true);
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }


        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetElement(long salaryTypeId)
        {
            var data = await (from p in _appContext.SalaryElements
                              from s in _appContext.SalaryStructures.Where(x => x.ELEMENT_ID == p.ID)
                              where s.SALARY_TYPE_ID == salaryTypeId
                              orderby p.ORDERS
                              select new
                              {
                                  Id = p.ID,
                                  ColName = p.CODE,
                                  Name = p.NAME
                              }).ToListAsync();

            return new ResultWithError(data);

        }
    }
}
