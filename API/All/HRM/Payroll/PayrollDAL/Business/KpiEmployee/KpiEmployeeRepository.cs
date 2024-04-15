using Common.Paging;
using PayrollDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using System.Data;
using Common.EPPlus;
using System.Dynamic;
using API.All.DbContexts;
using API.Entities;

namespace PayrollDAL.Repositories
{
    public class KpiEmployeeRepository : RepositoryBase<PA_KPI_SALARY_DETAIL>, IKpiEmployeeRepository
    {
        private PayrollDbContext _appContext => (PayrollDbContext)_context;
        public KpiEmployeeRepository(PayrollDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data for System
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<KpiEmployeeDTO>> GetAll(KpiEmployeeDTO param)
        {
            var queryable = from p in _appContext.KpiEmployees.Where(c => c.PERIOD_ID == param.PeriodId)
                            join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                            join po in _appContext.Positions on e.POSITION_ID equals po.ID
                            join t in _appContext.KpiTargets on p.KPI_TARGET_ID equals t.ID
                            
                            orderby t.NAME
                            select new KpiEmployeeDTO
                            {
                                Id = p.ID,
                                EmployeeId = p.EMPLOYEE_ID,
                                EmployeeCode = e.CODE,
                                EmployeeName = e.Profile.FULL_NAME,
                                OrgId = e.ORG_ID,
                                PeriodId = p.PERIOD_ID,
                                PositionName = po.NAME,
                                KpiTargetId = p.KPI_TARGET_ID,
                                KpiTargetName = t.NAME,
                                RealValue = p.REAL_VALUE,
                                StartValue = p.START_VALUE,
                                EqualValue = t.IS_REAL_VALUE == true ? null : p.EQUAL_VALUE,
                                KpiSalary = p.KPI_SALARY == null ? 0 : p.KPI_SALARY,
                                IsPaySalary = p.IS_PAY_SALARY,
                                Note = p.NOTE,
                            };

            var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                    new
                    {
                        P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                        P_ORG_ID = param.OrgId,
                        P_CURENT_USER_ID = _appContext.CurrentUserId,
                        P_CUR = QueryData.OUT_CURSOR
                    }, false);


            List<long?> ids = orgIds.Select(c => (long?)((dynamic)c).ID).ToList();
            if (param.OrgId != null)
            {
                ids.Add(param.OrgId);
            }
            queryable = queryable.Where(p => ids.Contains(p.OrgId));
            // queryable = queryable.Where(p => p.KpiSalary != null && p.KpiSalary != 0);
            if (!string.IsNullOrWhiteSpace(param.KpiTargetName))
            {
                queryable = queryable.Where(p => p.KpiTargetName.ToUpper().Contains(param.KpiTargetName.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.EmployeeCode))
            {
                queryable = queryable.Where(p => p.EmployeeCode.ToUpper().Contains(param.EmployeeCode.ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(param.EmployeeName))
            {
                queryable = queryable.Where(p => p.EmployeeName.ToUpper().Contains(param.EmployeeName.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.PositionName))
            {
                queryable = queryable.Where(p => p.PositionName.ToUpper().Contains(param.PositionName.ToUpper()));
            }
            if (param.KpiTargetId != null)
            {
                queryable = queryable.Where(p => p.KpiTargetId == param.KpiTargetId);
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
                var r = await (from p in _appContext.KpiEmployees
                               where p.ID == id 
                               select new
                               {


                                   Note = p.NOTE,
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
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
                var r = await (
                               from p in _appContext.KpiEmployees
                               where p.IS_PAY_SALARY == true
                               select new
                               {

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
        public async Task<ResultWithError> CreateAsync(KpiEmployeeInputDTO param)
        {
            try
            {
                // check group exist
                var g = _appContext.ElementGroups.Where(c => c.ID == param.KpiGroupId).Count();
                if (g == 0)
                {
                    return new ResultWithError(Message.RECORD_NOT_FOUND);
                }


                // 
                var data = Map(param, new PA_KPI_SALARY_DETAIL());
                var result = await _appContext.KpiEmployees.AddAsync(data);
                await _appContext.SaveChangesAsync();

                return new ResultWithError(data);
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
        public async Task<ResultWithError> UpdateAsync(KpiEmployeeInputDTO param)
        {
            try
            {

                var r = _appContext.KpiEmployees.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }

                var data = Map(param, r);
                var result = _appContext.KpiEmployees.Update(data);
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
                    var r = _appContext.KpiEmployees.Where(x => x.ID == item).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    var result = _appContext.KpiEmployees.Update(r);
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> Delete(List<long> ids)
        {
            try
            {
                foreach (var item in ids)
                {
                    var r = _appContext.KpiEmployees.Where(x => x.ID == item).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    var result = _appContext.Remove(r);
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> ExportTemplate(KpiEmployeeInput param)
        {

            var ds = QueryData.ExecuteStoreToTable(Procedures .PKG_KPI_GET_EMP_KPI_IMP,
                new
                {
                    
                    P_USER_ID = _appContext.IsAdmin == true ? "admin" : _appContext.CurrentUserId,
                    P_ORG_ID = param.OrgId,
                    P_KPI_ID = param.KpiTargetId,
                    P_PERIOD = param.PeriodId,
                    P_CUR = QueryData.OUT_CURSOR
                }, false);

            if (ds.Tables[0].Rows.Count <= 0)
            {
                return new ResultWithError("DATA_EMPTY");
            }
            ds.Tables[0].TableName = Consts.DATA_KPI;
            var pathTemp = _appContext._config["urlTemplate"];

            var memoryStream = Template.FillReport(pathTemp, ds);
            return new ResultWithError(memoryStream);

        }
        public async Task<ResultWithError> CaclKpiSalary(KpiEmployeeInput param)
        {

            await QueryData.Execute(Procedures.PKG_KPI_CALC_KPI_SALARY,
                  new
                  {
                      
                      P_USER_ID = _appContext.IsAdmin == true ? "admin" : _appContext.CurrentUserId,
                      P_ORG_ID = param.OrgId,
                      P_PERIOD_ID = param.PeriodId,
                  }, false);
            return new ResultWithError(200);
        }

        public async Task<ResultWithError> ImportFromTemplate(KpiEmployeeImport param)
        {
            List<object> message = new List<object>();
            var period = await _appContext.SalaryPeriods.Where(c => c.ID == param.PeriodId).FirstOrDefaultAsync();

            if (param.file == null || param.file.Length <= 0)
            {
                return new ResultWithError("File empty");
            }

            if (!Path.GetExtension(param.file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase)
                && !Path.GetExtension(param.file.FileName).Equals(".xls", StringComparison.OrdinalIgnoreCase)
                )
            {
                return new ResultWithError("Not Support file extension");
            }
            if (period == null)
            {
                return new ResultWithError("PERIOD_NOT_FOUND");
            }
            using (var stream = new MemoryStream())
            {
                await param.file.CopyToAsync(stream);

                var data = Template.ConvertToDataSet(stream);
                var dt = data.Tables["Table1"];
                if (dt == null)
                {
                    return new ResultWithError("NOT_FOUND_TABLE: Table1");
                }
                var ListData = QueryData.ConvertToListByOrder<KpiEmployeeOutput>(dt);

                var dataImport = new List<PA_KPI_SALARY_DETAIL_TMP>();
                //Validate data
                var key = true;
                List<ExpandoObject> resp = new List<ExpandoObject>();

                foreach (var item in ListData)
                {

                    if (item.RealValue != 0)
                    {
                        dynamic res = new ExpandoObject();
                        res.Order = item.Order;
                        //var empId = _appContext.Employees.Where(c => c.ID == item.EmployeeId).FirstOrDefault();
                        //var kpiId = _appContext.KpiTargets.Where(c => c.ID == item.KpiTargetId).FirstOrDefault();
                        if (item.EmployeeId == 0)
                        {
                            res.EmployeeId = 0;
                            res.EmployeeIdMessage = "ID_NOT_FOUND";
                            key = false;
                        }
                        if (item.KpiTargetId == 0)
                        {
                            res.kpiId = 0;
                            res.kpiIdMessage = "ID_NOT_FOUND";
                            key = false;
                        }

                        if (item.RealValue == 0)
                        {
                            res.kpiId = 0;
                            res.kpiIdMessage = "REQUIRED";
                            key = false;
                        }
                        resp.Add(res);


                        var kpiItem = new PA_KPI_SALARY_DETAIL_TMP();
                        kpiItem.PERIOD_ID = param.PeriodId;
                        kpiItem.KPI_TARGET_ID = item.KpiTargetId;
                        kpiItem.EMPLOYEE_ID = item.EmployeeId;
                        if (item.StartValue != 0)
                        {

                            kpiItem.START_VALUE = item.StartValue;
                        }
                        kpiItem.REAL_VALUE = item.RealValue;

                        //if (kpiId.IS_IMPORT_KPI == true && kpiId.IS_REAL_VALUE == false)
                        //{
                        //    try
                        //    {
                        //        kpiItem.EQUAL_VALUE = Math.Round(kpiItem.REAL_VALUE / kpiItem.START_VALUE ?? 0, 2);
                        //    }
                        //    catch (Exception)
                        //    {

                        //        kpiItem.EQUAL_VALUE = 0;
                        //    }

                        //}
                        dataImport.Add(kpiItem);
                    }
                }
                if (key == false)
                {
                    return new ResultWithError(400, resp);
                }

                //remove data old 

                //var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                //        new
                //        {
                //            P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                //            
                //            P_ORG_ID = param.OrgId,
                //            P_CURENT_USER_ID = _appContext.CurrentUserId,
                //            P_CUR = QueryData.OUT_CURSOR
                //        }, false);

                //List<int?> ids = orgIds.Select(c => (int?)((dynamic)c).ID).ToList();
                //ids.Add(param.OrgId);
                ////filtter data import 
                //var dataImportFilter = from p in dataImport
                //                       join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                //                       join o in _appContext.Organizations on e.ORG_ID equals o.ID
                //                       where ids.Contains(o.ID)
                //                       select p;
                //var empIds = dataImportFilter.Select(f => f.EMPLOYEE_ID).Distinct().ToList();

                //_appContext.KpiEmployees.RemoveRange(_appContext.KpiEmployees
                //    .Where(c => c.PERIOD_ID == param.PeriodId && empIds.Contains(c.EMPLOYEE_ID)));

                await _appContext.KpiEmployeeTmps.AddRangeAsync(dataImport);
                try
                {
                    await _appContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                var r = await QueryData.ExecuteObject(Procedures.PKG_KPI_IMPORT_KPI,
                   new
                   {
                       P_PERIOD_ID = param.PeriodId
                   }, true);
                return new ResultWithError(200);
            }

        }

        public async Task<ResultWithError> LockKPI(LockInputDTO param)
        {
            try
            {
                var r = await QueryData.ExecuteObject(Procedures.PKG_PAYROLL_LOCK_KPI,
                    new
                    {
                        p_is_admin = _appContext.IsAdmin == true ? 1 : 0,
                        p_org_id = param.ORG_ID,
                        p_period_id = param.PERIOD_ID,
                        P_CUR = QueryData.OUT_CURSOR
                    }, true);
                return new ResultWithError(r);

            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> IsLockKPI(LockInputDTO param)
        {
            try
            {
                var r = await _appContext.KpiLocks.Where(c =>  c.PERIOD_ID == param.PERIOD_ID && c.ORG_ID == param.ORG_ID).AnyAsync();
                return new ResultWithError(r);

            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

    }
}
