using Common.Paging;
using PayrollDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using System.Dynamic;
using System.Data;
using API.All.DbContexts;
using API.Entities;

namespace PayrollDAL.Repositories
{
    public class FormulaRepository : RepositoryBase<PA_FORMULA>, IFormulaRepository
    {
        private PayrollDbContext _appContext => (PayrollDbContext)_context;
        public FormulaRepository(PayrollDbContext context) : base(context)
        {

        }

        /// <summary>
        /// Setting Formular Payroll
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> Update(FormulaInputDTO param)
        {
            try
            {

                dynamic r = await QueryData.ExecuteObject("PKG_PAYROLL.ADD_FORMULA",
                    new
                    {
                        
                        P_FORMULAR_NAME = param.FormulaName,
                        P_COL_NAME = param.ColName,
                        P_SALARY_TYPE_ID = param.SalaryTypeId,
                        P_CUR = QueryData.OUT_CURSOR
                    }, false);
                if (r.STATUS == 400)
                {
                    return new ResultWithError(r.MESSAGE);
                }

                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Get List grid ben phai
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<FormulaDTO>> GetElementCal(FormulaDTO param)
        {
            try
            {
                var r = from p in _appContext.SalaryStructures
                        join e in _appContext.SalaryElements on p.ELEMENT_ID equals e.ID
                        join s in _appContext.Formulas on e.CODE equals s.COL_NAME
                        where p.SALARY_TYPE_ID == param.SalaryTypeId  && p.IS_CALCULATE == true  && s.SALARY_TYPE_ID == param.SalaryTypeId
                        orderby s.ORDERS
                        select new FormulaDTO
                        {
                            Id = s.ID,
                            // SalaryTypeId = param.SalaryTypeId,
                            Name = e.NAME,
                            ColName = e.CODE,
                            FormulaName = s.FORMULA_NAME,
                            Orders = s.ORDERS
                        };
                return await PagingList(r, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResultWithError> ListPayrollSum(PayrollSumDTO param)
        {
            try
            {
                if (param.PageNo == 0)
                {
                    param.PageNo = 1;
                }
                if (param.PageSize == 0)
                {
                    param.PageSize = 20;
                }
                var r =  await QueryData.ExecutePaging(Procedures.PKG_PAYROLL_LIST_PAYROLL_SUM,
                    new
                    {
                        p_is_admin = _appContext.IsAdmin == true ? 1 : 0,
                        p_org_id = param.OrgId,
                        p_period_id = param.PeriodId,
                        P_IS_QUIT = param.IS_QUIT,
                        P_SALARY_TYPE_ID = param.SalaryTypeId,
                        P_CODE = param.EMPLOYEE_CODE,
                        P_NAME = param.EMPLOYEE_NAME,
                        P_ORG_NAME = param.ORG_NAME,
                        P_POS_NAME = param.POSITION_NAME,
                        p_page_no = param.PageNo,
                        p_page_size = param.PageSize,
                        P_CUR = QueryData.OUT_CURSOR,
                        P_CUR_PAGE = QueryData.OUT_CURSOR
                    }, true);
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<List<ExpandoObject>> MBPayrollSum(PayrollInputMobile param)
        {
            try
            {

                return await QueryData.ExecuteList(Procedures.PKG_PAYROLL_M_PAYROLL_SUM,
                    new
                    {
                        
                        P_EMP_ID = _appContext.EmpId,
                        P_PERIOD_ID = param.PeriodId,
                        P_SALARY_TYPE_ID = param.SalaryTypeId,
                        P_CUR = QueryData.OUT_CURSOR,
                    }, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultWithError> PayrollCal(PayrollInputDTO param)
        {
            try
            {
                // check log time sheet
                dynamic r = await QueryData.ExecuteObject(Procedures.PKG_PAYROLL_CHECK_TIMESHEET_LOCK, new
                {
                    
                    p_org_id = param.OrgId,
                    p_period_id = param.PeriodId,
                    p_cur = QueryData.OUT_CURSOR
                }, false);
                if (r.STATUS == "UNLOCK")
                {
                    return new ResultWithError(Message.TIME_SHEET_UNLOCKED);
                }
                if (r.STATUS == "LOCK")
                {
                    try
                    {
                        await QueryData.Execute("PKG_PAYROLL_V2.CAL_PAYROLL", new
                        {
                            p_is_admin = _appContext.IsAdmin == true ? 1 : 0,
                            p_org_id = param.OrgId,
                            p_period_id = param.PeriodId,
                            p_type_sal = param.SalaryTypeId // Bang luong
                        }, true);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    //var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                    //     new
                    //     {
                    //         P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                    //         
                    //         P_ORG_ID = param.OrgId,
                    //         P_CURENT_USER_ID = _appContext.CurrentUserId,
                    //         P_CUR = QueryData.OUT_CURSOR
                    //     }, false);


                    //List<decimal?> ids = orgIds.Select(c => (decimal?)((dynamic)c).ID).ToList();
                    //ids.Add(param.OrgId);

                    //var query = (from e in _appContext.Employees.Where(c =>  ids.Contains(c.ORG_ID))
                    //             join a in _appContext.Workings on e.ID equals a.EMPLOYEE_ID into tmp1
                    //             from a2 in tmp1.DefaultIfEmpty()
                    //             select new
                    //             {
                    //                 ID = e.ID,
                    //                 NAME = e.FULLNAME,
                    //                 CODE = e.CODE,
                    //                 WORKING_ID = (long?)a2.EMPLOYEE_ID
                    //             }).ToList();
                    //var x = query.Where(d => d.WORKING_ID == null).Select(f => f).ToList();
                    return new ResultWithError(200);
                }
                return new ResultWithError(400);

            }
            catch (Exception ex)
            {

                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> CheckTimesheetLock(PayrollInputDTO param)
        {
            try
            {
                // check log time sheet
                string r = await QueryData.ExecuteStoreString(Procedures.PKG_PAYROLL_CHECK_TIMESHEET_LOCK, new
                {
                    p_org_id = param.OrgId,
                    p_period_id = param.PeriodId,
                    p_cur = QueryData.OUT_CURSOR
                });
                return new ResultWithError(new { status = r });
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex.Message);
            }
        }

        public async Task<List<ValuesDTO>> PortalGetBy(int periodId)
        {

            try
            {

                var r = await QueryData.ExecuteStore<ValuesDTO>
               ("PKG_PAYROLL.PORTAL_PAYCHECK",
               new
               {
                   
                   P_EMP_ID = _appContext.EmpId,
                   P_PERIOD_ID = periodId,
                   P_CUR = QueryData.OUT_CURSOR
               });
                return r;
            }
            catch (Exception ex)
            {
                return null;
            }

            //try
            //{

            //    var r = await QueryData.ExecuteList("PKG_PAYROLL.PORTAL_PAYCHECK",
            //        new
            //        {
            //            
            //            P_EMP_ID = _appContext.EmpId,
            //            P_PERIOD_ID = periodId,
            //            P_CUR = QueryData.OUT_CURSOR
            //        }, false);

            //    var x = new List<object>();
            //    foreach (var item in r)
            //    {
            //        var y = item.FirstOrDefault(v => v.Key == "NAME");


            //        var values = item.GetType().GetProperties();
            //       var C =  values.GetValue(0);
            //        x.Add(item);
            //    }

            //    return x;
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}



            //try
            //{
            //    var x = new List<object>()
            //    {
            //        new
            //        {
            //            NAME = "Bao hiem that nghiep nguoi lao dong",
            //            VALUE = 60000.0
            //        },
            //                       new  {
            //            NAME= "Cong di lam thuc te",
            //            VALUE= 5.50
            //        },
            //       new {
            //                    NAME= "Bao hiem that nghiep cong ty",
            //            VALUE= 60000.0
            //        },
            //       new {
            //                    NAME= "Bao hiem xa hoi cong ty ",
            //            VALUE= 60000.0
            //        },
            //       new {
            //                    NAME= "Luong chinh",
            //            VALUE= 12000000.0
            //        },
            //       new {
            //                    NAME= "OT dem ngay nghi",
            //            VALUE = 0
            //        },
            //       new {
            //                    NAME= "Tong cac khoan giam tru",
            //            VALUE= 22630000.0
            //        },
            //       new {
            //                    NAME= "Bao hiem y te nguoi lao dong",
            //            VALUE= 90000.0
            //        },
            //       new {
            //                    NAME= "Doan phi cong doan",
            //            VALUE= 60000.0
            //        },
            //      new  {
            //                    NAME= "Thu nhap tinh thue",
            //            VALUE= 0.0
            //        },
            //       new {
            //                    NAME= "Bao hiem xa hoi nguoi  lao dong",
            //            VALUE= 480000.0
            //        },
            //       new {
            //                    NAME= "Thue thu nhap ca nhan",
            //            VALUE= 0.0
            //        },
            //       new {
            //                    NAME= "Luong co ban dong bao hiem",
            //            VALUE= 6000000.0
            //        },
            //        new{
            //                    NAME= "Phu cap di lai",
            //            VALUE= 325000.0
            //        }

            // };


            //    return x;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public async Task<ResultWithError> PortalGetBy1(int periodId)
        {
            try
            {
                string r = await QueryData.ExecuteStoreString("PKG_PAYROLL.PORTAL_PAYCHECK", new
                {
                    P_EMP_ID = _appContext.EmpId,
                    P_PERIOD_ID = periodId,
                    P_CUR = QueryData.OUT_CURSOR
                });
                return new ResultWithError(new { status = r });
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }

        }

        public async Task<ResultWithError> LockPayroll(LockInputDTO param)
        {
            try
            {
                var r = await QueryData.ExecuteObject("PKG_PAYROLL.LOCK_PAYROLL",
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
        public async Task<ResultWithError> IsLockPayroll(LockInputDTO param)
        {
            try
            {
                var r = await _appContext.CalculateLocks.Where(c =>  c.PERIOD_ID == param.PERIOD_ID && c.ORG_ID == param.ORG_ID).AnyAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// Sắp xếp công thưc lương
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> MoveTableIndex(List<TempSortInputDTO> param, int type)
        {
            try
            {
                var lst = new List<SYS_TMP_SORT>();
                var guid = Guid.NewGuid().ToString();
                foreach (var item in param)
                {
                    var data = Map(item, new SYS_TMP_SORT());
                    data.REF_CODE = guid;
                    lst.Add(data);
                }

                await _appContext.SysTempSorts.AddRangeAsync(lst);
                await _appContext.SaveChangesAsync();
                await QueryData.Execute("PKG_PAYROLL_SETTING.SORT_TABLE_PAYROLL", new { P_REF_CODE = guid, P_TYPE = type }, true);
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
    }
}
