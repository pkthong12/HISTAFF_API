using Common.Paging;
using AttendanceDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using System.Data;
using System.Dynamic;
using Common.EPPlus;
using API.All.DbContexts;
using API.Entities;
using CORE.GenericUOW;
using API.DTO;
using CORE.DTO;
using ProfileDAL.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using Common.DataAccess;

namespace AttendanceDAL.Repositories
{
    public class TimeSheetMonthlyRepository : RepositoryBase<AT_TIMESHEET_MONTHLY>, ITimeSheetMonthlyRepository
    {
        private AttendanceDbContext _appContext;
        private readonly GenericReducer<AT_TIMESHEET_MONTHLY, TimeSheetMonthlyDTO> genericReducer;
        public TimeSheetMonthlyRepository(AttendanceDbContext context) : base(context)
        {
            _appContext = context;
            genericReducer = new();
        }
        public async Task<GenericPhaseTwoListResponse<TimeSheetMonthlyDTO>> SinglePhaseQueryList(GenericQueryListDTO<TimeSheetMonthlyDTO> request)
        {
            var joined = from p in _appContext.TimeSheetMonthlies
                         from e in _appContext.Employees.Where(f => p.EMPLOYEE_ID == f.ID)
                         from w in _appContext.Workings.Where(f => f.ID == p.DECISION_ID).DefaultIfEmpty()
                         from t in _appContext.Positions.Where(f => p.POSITION_ID == f.ID).DefaultIfEmpty()
                         from j in _appContext.Jobs.AsNoTracking().Where(j => j.ID == t.JOB_ID).DefaultIfEmpty()
                         from o in _appContext.Organizations.Where(f => p.ORG_ID == f.ID).DefaultIfEmpty()
                         from eo in _appContext.OtherLists.Where(f => p.PA_EMPLOYEE_OBJECT_ID == f.ID).DefaultIfEmpty()
                         from s in _appContext.SalaryPeriods.Where(f => p.PERIOD_ID == f.ID).DefaultIfEmpty()
                         from op in _appContext.OrgPeriods.Where(op => op.PERIOD_ID == p.PERIOD_ID && op.ORG_ID == p.ORG_ID).DefaultIfEmpty()
                         select new TimeSheetMonthlyDTO
                         {
                             PositionId = j.ID,
                             Id = p.ID,
                             EmployeeCode = e.CODE,
                             EmployeeName = e.Profile.FULL_NAME,
                             PositionName = t.NAME,
                             OrgId = p.ORG_ID,
                             OrgName = o.NAME,
                             WorkingStandard = p.WORKING_STANDARD,
                             TotalLate = p.TOTAL_LATE,
                             TotalDmPhat = p.TOTAL_DM_PHAT,
                             TotalComebackout = p.TOTAL_COMEBACKOUT,
                             TotalVsPhat = p.TOTAL_VS_PHAT,
                             WorkingLcb = p.WORKING_LCB,
                             WorkingX = p.WORKING_X,
                             TotalWorkingXj = p.TOTAL_WORKING_XJ,
                             ObjEmpName = eo.NAME,
                             WorkingCt = p.WORKING_CT,
                             WorkingCtn = p.WORKING_CTN,
                             WorkingRo = p.WORKING_RO,
                             WorkingH = p.WORKING_H,
                             WorkingL = p.WORKING_L,
                             WorkingP = p.WORKING_P,
                             WorkingO = p.WORKING_O,
                             WorkingTs = p.WORKING_TS,
                             WorkingD = p.WORKING_D,
                             TotalWorkingCt = (p.WORKING_CT == null ? 0 : p.WORKING_CT) + (p.WORKING_CTN == null ? 0 : p.WORKING_CTN),
                             FromDate = s.START_DATE,
                             EndDate = s.END_DATE,
                             Year = s.YEAR,
                             PeriodId = p.PERIOD_ID,
                             TotalOtWeekday = p.TOTAL_OT_WEEKDAY,
                             TotalOtSunday = p.TOTAL_OT_SUNDAY,
                             TotalOtHoliday = p.TOTAL_OT_HOLIDAY,
                             TotalOtWeeknight = p.TOTAL_OT_WEEKNIGHT,
                             TotalOtSundaynigth = p.TOTAL_OT_SUNDAYNIGTH,
                             TotalOtHolidayNigth = p.TOTAL_OT_HOLIDAY_NIGTH,
                             JobOrderNum = (int)(j.ORDERNUM ?? 999),
                             OrgOrderNum = (int)(o.ORDER_NUM ?? 999),
                             IsLock = op.STATUSCOLEX == 1 ? true : false,
                         };
            var phase2 = await genericReducer.SinglePhaseReduce(joined, request);
            return phase2;
        }
        /// <summary>
        /// CMS Get Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> GetByEmployeeId(long empId)
        {
            try
            {
                var r = await (from p in _appContext.TimeSheetMonthlies
                               from e in _appContext.Employees.Where(f => p.EMPLOYEE_ID == f.ID)
                               from t in _appContext.Positions.Where(f => p.POSITION_ID == f.ID).DefaultIfEmpty()
                               from o in _appContext.Organizations.Where(f => p.ORG_ID == f.ID).DefaultIfEmpty()
                               where p.EMPLOYEE_ID == empId
                               select new TimeSheetMonthlyDTO
                               {
                                   Id = p.ID,
                                   EmployeeCode = e.CODE,
                                   EmployeeName = e.Profile.FULL_NAME,
                                   PositionName = t.NAME,
                                   OrgId = p.ORG_ID,
                                   OrgName = o.NAME,
                                   WorkingStandard = p.WORKING_STANDARD,
                                   TotalLate = p.TOTAL_LATE,
                                   TotalDmPhat = p.TOTAL_DM_PHAT,
                                   TotalComebackout = p.TOTAL_COMEBACKOUT,
                                   TotalVsPhat = p.TOTAL_VS_PHAT,
                                   WorkingLcb = p.WORKING_LCB,
                                   WorkingX = p.WORKING_X,
                                   TotalWorkingXj = p.TOTAL_WORKING_XJ,
                                   WorkingCt = p.WORKING_CT,
                                   WorkingCtn = p.WORKING_CTN,
                                   WorkingRo = p.WORKING_RO,
                                   WorkingH = p.WORKING_H,
                                   WorkingL = p.WORKING_L,
                                   WorkingP = p.WORKING_P,
                                   WorkingO = p.WORKING_O,
                                   WorkingTs = p.WORKING_TS,
                                   WorkingD = p.WORKING_D,
                                   TotalWorkingCt = (p.WORKING_CT == null ? 0 : p.WORKING_CT) + (p.WORKING_CTN == null ? 0 : p.WORKING_CTN)

                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> Calculate(TimeSheetInputDTO request)
        {
            try
            {

                var checkLockOrg = (from p in _appContext.OrgPeriods where request.OrgIds.Contains(p.ORG_ID) && p.STATUSCOLEX == 1 && p.PERIOD_ID == request.PeriodId select p).ToList();
                if (checkLockOrg != null && checkLockOrg.Count() > 0)
                {
                    return new ResultWithError("ORG_LOCKED");
                }
                var x = new
                {
                    P_USER_ID = _appContext.CurrentUserId,
                    P_PERIOD_ID = request.PeriodId,
                    P_ORG_ID = request.OrgId,
                    P_ISDISSOLVE = -1,
                    P_EMPLOYEE_ID = request.EmployeeId == null ? 0 : request.EmployeeId
                };
                var data = QueryData.ExecuteStoreToTable(Procedures.PKG_ATTENDANCE_CALCULATE_CAL_TIME_TIMESHEET_MACHINE,
                    x, false);
                if (Convert.ToDouble(data.Tables[0].Rows[0][0]) > 0)
                {
                    var x1 = new
                    {
                        P_USER_ID = _appContext.CurrentUserId,
                        P_PERIOD_ID = request.PeriodId,
                        P_ORG_ID = request.OrgId,
                        P_ISDISSOLVE = -1,
                    };
                    var data1 = QueryData.ExecuteStoreToTable(Procedures.PKG_ATTENDANCE_CALCULATE_CAL_TIME_TIMESHEET_MONTHLY,
                        x1, false);
                    if (Convert.ToDouble(data.Tables[0].Rows[0][0]) > 0)
                    {
                        return new ResultWithError(200, data.Tables[0].Rows[0][0]);
                    }
                    else
                    {

                        return new ResultWithError(400);
                    }
                }
                else
                {
                    return new ResultWithError(400);
                }
                
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> Lock(TimeSheetInputDTO request)
        {
            try
            {

                var lstLockOrg = (from p in _appContext.OrgPeriods
                                  where request.PeriodId == p.PERIOD_ID
                                  select p).ToList();
                foreach (var item in request.OrgIds)
                {
                    var check = lstLockOrg.Where(f => f.ORG_ID == item);
                    if (check.Count() > 0)
                    {
                        if(check.Count() > 1)
                        {
                            foreach (var item1 in check)
                            {
                                item1.STATUSCOLEX = request.StatusColex;
                                var rs = _appContext.OrgPeriods.Update(item1);
                            }
                        }
                        if(check.Count() ==1)
                        {
                            var itemUpdate = check.FirstOrDefault();
                            itemUpdate.STATUSCOLEX = request.StatusColex;
                            var rs = _appContext.OrgPeriods.Update(itemUpdate);
                        }
                        
                    }
                    else
                    {
                        var data = Map(request, new AT_ORG_PERIOD());
                        data.ORG_ID = item;
                        await _appContext.OrgPeriods.AddAsync(data);
                    }
                }
                await _appContext.SaveChangesAsync();

                return new ResultWithError(request);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> ImportSwipeMachine(List<SwipeDataInput> param)
        {
            try
            {
                var data = new List<AT_SWIPE_DATA_SYNC>();
                var guid = Guid.NewGuid().ToString();
                var lstError = new List<SwipeDataInput>();
                var count = param.Count();

                foreach (var item in param)
                {
                    var a = new AT_SWIPE_DATA_SYNC();
                    try
                    {
                        a.REF_CODE = guid;
                        a.TIME_POINT_STR = item.TIME;
                        a.ITIME_ID = item.CODE;
                        data.Add(a);
                    }
                    catch
                    {
                        item.TIME = "!Sai định dạng dd/MM/yyyy HH:mm:ss";
                        lstError.Add(item);
                    }
                }
                if (lstError.Count > 0)
                {
                    return new ResultWithError(lstError);
                }
                else
                {
                    if (data.Count > 0)
                    {

                        await _appContext.SwipeDataSyncs.AddRangeAsync(data);
                        await _appContext.SaveChangesAsync();

                        var x = new
                        {
                            P_USER_ID = _appContext.CurrentUserId,
                            P_REF_CODE = guid

                        };
                        var ds = QueryData.ExecuteStoreToTable(Procedures.PKG_SYNC_SWIPE_SYNC_MACHINE,
                            x, false);

                    }

                    return new ResultWithError(200);
                }
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> CheckLock(LockDataInput request)
        {
            try
            {
                // 1-khoa, 0/null-mo
                //get phong ban ton tai
                var listOrg = await _appContext.Organizations.AsNoTracking().Where(p => request.OrgIds.Contains(p.ID)).Select(p => p.ID).ToListAsync();
                var lstLockOrg = await (from p in _appContext.OrgPeriods
                                        where listOrg.Contains(p.ORG_ID.Value) && request.PeriodId == p.PERIOD_ID
                                        select new
                                        {
                                            Id = p.ORG_ID,
                                            Status = p.STATUSCOLEX ?? 0,
                                        }).ToListAsync();
                var lockUnlock = true;
                var lockLock = true;
                if(lstLockOrg.Count > 0) 
                {
                    var group = lstLockOrg.GroupBy(x => x.Status).ToList();
                    if(group.Count() > 1)
                    {
                        lockUnlock = false;
                        lockLock = false;
                    }
                    else
                    {
                        if (group[0].Key == 1)
                        {
                            lockLock = true;
                            lockUnlock = false;
                        }
                        else
                        {
                            lockLock = false;
                            lockUnlock = true;
                        }
                    }
                }
                else
                {
                    lockLock = false;
                    lockUnlock = true;
                }
                return new ResultWithError(new
                {
                    Lock = lockLock,
                    UnLock = lockUnlock
                });
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex.Message);
            }
        }

        //public async Task<PagedResult<ExpandoObject>> ListTimeSheetMonthly(TimeSheetMonthlyDTO param)
        //{
        //    try
        //    {
        //        if (param.PageNo == 0)
        //        {
        //            param.PageNo = 1;
        //        }
        //        if (param.PageSize == 0)
        //        {
        //            param.PageSize = 20;
        //        }

        //        var r = await QueryData.ExecutePaging("PKG_TIMESHEET.LIST_TIMESHEET_MONTHLY",
        //            new
        //            {
        //                P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
        //                p_org_id = param.ORG_ID,
        //                p_period_id = param.PERIOD_ID,
        //                P_IS_QUIT = param.IS_QUIT,
        //                P_CODE = param.EMPLOYEE_CODE,
        //                P_NAME = param.EMPLOYEE_NAME,
        //                P_ORG_NAME = param.ORG_NAME,
        //                P_POS_NAME = param.POSITION_NAME,
        //                p_page_no = param.PageNo,
        //                p_page_size = param.PageSize,
        //                P_CUR = QueryData.OUT_CURSOR,
        //                P_CUR_PAGE = QueryData.OUT_CURSOR
        //            }, true);
        //        return r;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public async Task<PagedResult<TimeSheetFomulaDTO>> GetListFormula(TimeSheetFomulaDTO param)
        //{
        //    try
        //    {
        //        var queryable = from p in _appContext.TimeSheetFomulas

        //                        orderby p.ORDERS
        //                        select new TimeSheetFomulaDTO
        //                        {
        //                            Id = p.ID,
        //                            Name = p.NAME,
        //                            FormulaName = p.FORMULA_NAME,
        //                            Orders = p.ORDERS,
        //                        };

        //        return await PagingList(queryable, param);

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        //public async Task<ResultWithError> UpdateFormula(TimeSheetFomulaInputDTO param)
        //{
        //    try
        //    {

        //        dynamic r = await QueryData.ExecuteObject("PKG_PAYROLL.ADD_FORMULA_ATTENDANCE",
        //            new
        //            {

        //                P_ID = param.Id,
        //                P_FORMULA_NAME = param.FormulaName,
        //                P_CUR = QueryData.OUT_CURSOR
        //            }, false);

        //        if (r.STATUS == 400)
        //        {
        //            return new ResultWithError(r.MESSAGE);
        //        }

        //        return new ResultWithError(200);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultWithError(ex.Message);
        //    }
        //}
        ///// <summary>
        ///// Tông hợp công từ bảng TimeSheet_Daily insert vào bảng TimeSheet_DTL và bảng Timesheet_monthly
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //public async Task<ResultWithError> SumWork(TimeSheetInputDTO param)
        //{
        //    try
        //    {
        //        // check log time sheet
        //        Boolean isLock = await _appContext.TimeSheetLocks.Where(c => c.PERIOD_ID == param.PeriodId && c.ORG_ID == param.OrgId ).AnyAsync();
        //        if (isLock == true)
        //        {
        //            return new ResultWithError(Message.TIME_SHEET_LOCKED);
        //        }
        //        else
        //        {
        //            await QueryData.Execute("PKG_TIMESHEET.CAL_TIMESHEET_MONTHLY",
        //               new
        //               {
        //                   P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
        //                   p_org_id = param.OrgId,
        //                   p_period_id = param.PeriodId
        //               }, true);
        //            return new ResultWithError(200);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultWithError(ex.Message);
        //    }
        //}

        //public async Task<ResultWithError> ImportSwipeData(List<SwipeDataInput> param)
        //{
        //    try
        //    {
        //        // check log time sheet
        //        var listEmp = param.Select(c => c.CODE.Trim()).Distinct();
        //        var err = "";
        //        foreach (var item in listEmp)
        //        {
        //            var x = _appContext.Employees.Where(c => c.ITIME_CODE == item ).Any();
        //            if (x == false)
        //            {
        //                err += item + ",";
        //            }
        //        }
        //        if (err != "")
        //        {
        //            return new ResultWithError(err);
        //        }
        //        var data = new List<AT_SWIPE_DATA>();
        //        for (int i = 0; i < param.Count(); i++)
        //        {
        //            var a = new AT_SWIPE_DATA();
        //            a.EMP_ID = _appContext.Employees.Where(c => c.ITIME_CODE == param[i].CODE.Trim() ).Select(c => c.ID).FirstOrDefault();
        //            a.TIME_POINT = Convert.ToDateTime(param[i].TIME);
        //            data.Add(a);
        //        }
        //        if (data.Count > 0)
        //        {
        //            await _appContext.SwipeDatas.AddRangeAsync(data);
        //            await _appContext.SaveChangesAsync();
        //        }
        //        return new ResultWithError(200);

        //    }
        //    catch (Exception ex)
        //    {

        //        return new ResultWithError(ex.Message);
        //    }
        //}

        //public async Task<ResultWithError> ImportSwipeDataNew(SwipeImportnput param)
        //{
        //    try
        //    {
        //        var data = new List<AT_SWIPE_DATA_TMP>();
        //        var guid = Guid.NewGuid().ToString();
        //        var lstError = new List<SwipeDataInput>();
        //        var count = param.Data.Count();
        //        DateTime[] timePoint = new DateTime[count];
        //        string[] refCode = new string[count];
        //        string[] timeCode = new string[count];
        //        decimal[] tenant_id = new decimal[count];
        //        int i = 0;
        //        foreach (var item in param.Data)
        //        {
        //            var a = new AT_SWIPE_DATA_TMP();
        //            try
        //            {
        //                refCode[i] = guid;
        //                timeCode[i] = item.CODE;
        //                timePoint[i] = DateTime.ParseExact(item.TIME.Trim(), "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        //                i += 1;
        //            }
        //            catch
        //            {
        //                item.TIME = "!Sai định dạng dd/MM/yyyy HH:mm:ss";
        //                lstError.Add(item);
        //            }
        //        }
        //        if (lstError.Count > 0)
        //        {
        //            return new ResultWithError(lstError);
        //        }
        //        else
        //        {
        //            // xử lý fill dữ liệu vào master data
        //            var ds = QueryData.ExecuteStoreToTable("PKG_IMPORT.SWIPE_IMPORT",
        //            new
        //            {
        //                P_PERIOD_ID = param.PeriodId,
        //                P_REF_CODE = guid,
        //                P_CUR = QueryData.OUT_CURSOR
        //            }, true);

        //            if (ds.Tables.Count > 0)
        //            {
        //                return new ResultWithError(ds.Tables[0]);
        //            }
        //            else
        //            {
        //                return new ResultWithError(200);
        //                //await QueryData.Execute("PKG_TIMESHEET.CAL_TIMESHEET_SWIPE",
        //                //    new
        //                //    {
        //                //        p_is_admin = _appContext.IsAdmin == true ? 1 : 0,
        //                //        p_org_id = param.OrgId,
        //                //        p_period_id = param.PeriodId
        //                //    }, true);
        //            }

        //            return new ResultWithError(200);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return new ResultWithError(ex.Message);
        //    }
        //}


        ////public async Task<ResultWithError> ImportSwipeDataNew(SwipeImportnput param)
        ////{
        ////    try
        ////    {
        ////        var data = new List<SwipeDataTmp>();
        ////        var guid = Guid.NewGuid().ToString();
        ////        var lstError = new List<SwipeDataInput>();

        ////        foreach (var item in param.Data)
        ////        {
        ////            var a = new SwipeDataTmp();
        ////            try
        ////            {
        ////                a.TIME_POINT = DateTime.ParseExact(item.TIME.Trim(), "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        ////                a.REF_CODE = guid;
        ////                a.ITIME_ID = item.CODE;
        ////                data.Add(a);
        ////            }
        ////            catch
        ////            {
        ////                item.TIME = "!Sai định dạng dd/MM/yyyy HH:mm:ss";
        ////                lstError.Add(item);
        ////            }
        ////        }
        ////        if (lstError.Count > 0)
        ////        {
        ////            return new ResultWithError(lstError);
        ////        }
        ////        else
        ////        {
        ////            if (data.Count > 0)
        ////            {
        ////                // BulkInsert


        ////                await _appContext.SwipeDataTmps.AddRangeAsync(data);
        ////                await _appContext.SaveChangesAsync();

        ////                // xử lý fill dữ liệu vào master data
        ////                var ds = QueryData.ExecuteStoreToTable("PKG_IMPORT.SWIPE_IMPORT",
        ////                new
        ////                {
        ////                    P_PERIOD_ID = param.PeriodId,
        ////                    P_REF_CODE = guid,
        ////                    P_CUR = QueryData.OUT_CURSOR
        ////                }, true);

        ////                if (ds.Tables.Count > 0)
        ////                {
        ////                    return new ResultWithError(ds.Tables[0]);
        ////                }
        ////                else
        ////                {
        ////                    await QueryData.Execute("PKG_TIMESHEET.CAL_TIMESHEET_SWIPE",
        ////                        new
        ////                        {
        ////                            p_is_admin = _appContext.IsAdmin == true ? 1 : 0,
        ////                            p_org_id = param.OrgId,
        ////                            p_period_id = param.PeriodId
        ////                        }, true);
        ////                }
        ////            }
        ////            return new ResultWithError(200);
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {

        ////        return new ResultWithError(ex.Message);
        ////    }
        ////}
        //public async Task<ResultWithError> ListSwipeData(SwipeDataDTO param)
        //{
        //    try
        //    {
        //        if (param.PageNo == 0)
        //        {
        //            param.PageNo = 1;
        //        }
        //        if (param.PageSize == 0)
        //        {
        //            param.PageSize = 20;
        //        }

        //        var r = await QueryData.ExecutePaging("PKG_TIMESHEET.LIST_SWIPEDATA",
        //            new
        //            {
        //                p_is_admin = _appContext.IsAdmin == true ? 1 : 0,
        //                p_org_id = param.ORG_ID,
        //                p_period_id = param.PERIOD_ID,
        //                P_IS_QUIT = param.IS_QUIT,
        //                P_CODE = param.EMPLOYEE_CODE,
        //                P_NAME = param.EMPLOYEE_NAME,
        //                P_ORG_NAME = param.ORG_NAME,
        //                P_POS_NAME = param.POSITION_NAME,
        //                p_page_no = param.PageNo,
        //                p_page_size = param.PageSize,
        //                P_CUR = QueryData.OUT_CURSOR,
        //                P_CUR_PAGE = QueryData.OUT_CURSOR
        //            }, true);
        //        return new ResultWithError(r);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultWithError(ex.Message);
        //    }
        //}
        //public async Task<ResultWithError> LockTimeSheet(TimeSheetLockInputDTO param)
        //{
        //    try
        //    {
        //        var r = await QueryData.ExecuteObject("PKG_TIMESHEET.LOCK_TIMESHEET",
        //            new
        //            {
        //                p_is_admin = _appContext.IsAdmin == true ? 1 : 0,
        //                p_org_id = param.ORG_ID,
        //                p_period_id = param.PERIOD_ID,
        //                P_CUR = QueryData.OUT_CURSOR
        //            }, true);
        //        return new ResultWithError(r);

        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultWithError(ex.Message);
        //    }
        //}
        //public async Task<ResultWithError> IsLockTimeSheet(TimeSheetLockInputDTO param)
        //{
        //    try
        //    {
        //        var r = await _appContext.TimeSheetLocks.Where(c =>  c.PERIOD_ID == param.PERIOD_ID && c.ORG_ID == param.ORG_ID).AnyAsync();
        //        return new ResultWithError(r);

        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultWithError(ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Portal Get By Id
        ///// </summary>
        ///// <param name="periodId"></param>
        ///// <returns></returns>
        //public async Task<ResultWithError> PortalGetBY(int periodId)
        //{
        //    try
        //    {
        //        var r = await (from p in _appContext.TimeSheetMonthlies
        //                       join o in _appContext.SalaryPeriods on p.PERIOD_ID equals o.ID
        //                       where p.EMPLOYEE_ID == _appContext.EmpId && p.PERIOD_ID == periodId
        //                       select new TimeSheetPortal
        //                       {
        //                           WorkingX = p.WORKING_X,
        //                           WorkingLpay = p.WORKING_LPAY,
        //                           WorkingPay = p.WORKING_PAY,
        //                           WorkingN = p.WORKING_N,
        //                           WorkingStand = o.STANDARD_WORKING
        //                       }).FirstOrDefaultAsync();

        //        r.Detail = (from p in _appContext.TimesheetMachines
        //                    from a in _appContext.TimeTypes.Where(x => x.ID == p.TIMETYPE_ID)
        //                    where p.EMPLOYEE_ID == _appContext.EmpId && p.PERIOD_ID == periodId
        //                    orderby p.WORKINGDAY descending
        //                    select new TimeSheetPortalDtl
        //                    {
        //                        WorkingDay = p.WORKINGDAY.Value,
        //                        TimeTypeName = "[" + a.CODE + "] " + a.NAME,
        //                        TimePoint1 = p.TIME_POINT1,
        //                        TimePoint4 = p.TIME_POINT4,
        //                        LateInEarlyOut = p.LATE_IN + p.EARLY_OUT,
        //                        LateIn = p.LATE_IN,
        //                        EarlyOut = p.EARLY_OUT,
        //                        OT = p.OT_TIME.Value + p.OT_TIME_NIGHT.Value == null ? 0 : p.OT_TIME.Value + p.OT_TIME_NIGHT.Value
        //                    }).ToList();
        //        return new ResultWithError(r);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultWithError(ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Update
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //public async Task<ResultWithError> UpdateTimeSheetMachine(MaChineInput param)
        //{
        //    try
        //    {
        //        var data = new AT_TIMESHEET_MACHINE_EDIT();
        //        var r = _appContext.TimesheetMachines.Where(x => x.ID == param.Id).FirstOrDefault();
        //        if (param.TYPE_EDIT == "IN")
        //        {
        //            data.TIME_POINT1 = param.TIME_EDIT;
        //            data.TIME_POINT4 = r.TIME_POINT4;
        //            data.IS_EDIT_IN = true;
        //            r.TIME_POINT1 = param.TIME_EDIT;
        //            r.IS_EDIT_IN = true;
        //        }
        //        if (param.TYPE_EDIT == "OUT")
        //        {
        //            data.TIME_POINT1 = r.TIME_POINT1;
        //            data.TIME_POINT4 = param.TIME_EDIT;
        //            data.IS_EDIT_OUT = true;
        //            r.TIME_POINT4 = param.TIME_EDIT;
        //            r.IS_EDIT_OUT = true;
        //        }
        //        r.NOTE = param.NOTE;
        //        data.NOTE = param.NOTE;
        //        data.WORKINGDAY = r.WORKINGDAY;
        //        data.PERIOD_ID = r.PERIOD_ID;
        //        data.EMPLOYEE_ID = r.EMPLOYEE_ID;
        //        _appContext.TimesheetMachineEdits.Add(data);
        //        _appContext.TimesheetMachines.Update(r);
        //        await _appContext.SaveChangesAsync();
        //        await QueryData.Execute("PKG_TIMESHEET.MACHINE_KEEPING",
        //             new
        //             {

        //                 P_TIME = r.WORKINGDAY,
        //                 P_EMP_ID = r.EMPLOYEE_ID,
        //                 P_PERIOD_ID = r.PERIOD_ID
        //             }, false);
        //        ;
        //        return new ResultWithError(200);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultWithError(ex.Message);
        //    }
        //}
        //public async Task<PagedResult<ExpandoObject>> ListEntitlement(EntitlementDTO param)
        //{
        //    try
        //    {
        //        if (param.PageNo == 0)
        //        {
        //            param.PageNo = 1;
        //        }
        //        if (param.PageSize == 0)
        //        {
        //            param.PageSize = 20;
        //        }

        //        var r = await QueryData.ExecutePaging("PKG_ENTITLEMENT.GET_ENTITLEMENT",
        //            new
        //            {
        //                p_is_admin = _appContext.IsAdmin == true ? 1 : 0,
        //                p_org_id = param.OrgId,
        //                p_year = param.Year,
        //                P_code = param.Code,
        //                P_NAME = param.FullName,
        //                p_page_no = param.PageNo,
        //                p_page_size = param.PageSize,
        //                P_CUR = QueryData.OUT_CURSOR,
        //                P_CUR_PAGE = QueryData.OUT_CURSOR
        //            }, true);
        //        return r;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<ResultWithError> ReportSwipeData(SwipeDataReport param)
        //{

        //    try
        //    {
        //        // get list data emp
        //        var ds = await QueryData.ExecuteStore<RptAT001>("PKG_REPORT.RPT_AT001",
        //                                   new
        //                                   {

        //                                       P_USER_ID = _appContext.CurrentUserId,
        //                                       P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
        //                                       P_ORG_ID = param.OrgId,
        //                                       P_FROM_DATE = param.FromDate,
        //                                       P_TO_DATE = param.ToDate,
        //                                       P_CUR = QueryData.OUT_CURSOR
        //                                   });
        //        return new ResultWithError(ds);
        //        //var ds = QueryData.ExecuteStoreToTable("PKG_REPORT.RPT_AT001",
        //        //                           new
        //        //                           {
        //        //                               P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
        //        //                               P_ORG_ID = param.OrgId,
        //        //                               P_FROM_DATE = param.FromDate,
        //        //                               P_TO_DATE = param.ToDate,
        //        //                               P_CUR = QueryData.OUT_CURSOR
        //        //                           }, true);
        //        //if (ds.Tables[0].Rows.Count <= 0)
        //        //{
        //        //    return new ResultWithError("DATA_EMPTY");
        //        //}
        //        //ds.Tables[0].TableName = "DATA_EMP";
        //        //var pathTemp = _appContext._config["urlAT001"];
        //        //var memoryStream = Template.FillReport(pathTemp, ds);
        //        //return new ResultWithError(memoryStream);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultWithError(ex.Message);
        //    }
        //}
        //public async Task<ResultWithError> ReportSwipeDataExp(SwipeDataReport param)
        //{

        //    try
        //    {
        //        var ds = QueryData.ExecuteStoreToTable("PKG_REPORT.RPT_AT001",
        //                                   new
        //                                   {
        //                                       P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
        //                                       P_ORG_ID = param.OrgId,
        //                                       P_FROM_DATE = param.FromDate,
        //                                       P_TO_DATE = param.ToDate,
        //                                       P_CUR = QueryData.OUT_CURSOR
        //                                   }, true);
        //        if (ds.Tables[0].Rows.Count <= 0)
        //        {
        //            return new ResultWithError("DATA_EMPTY");
        //        }
        //        ds.Tables[0].TableName = "Data";
        //        var dt = new DataTable();
        //        dt.TableName = "Title";
        //        dt.Columns.Add("NAME");
        //        if (param.FromDate == param.ToDate)
        //        {
        //            dt.Rows.Add("BÁO CÁO DỮ LIỆU CHẤM CÔNG CHI TIẾT NGÀY " + param.FromDate.ToString("dd/MM/yyyy"));
        //        }
        //        else
        //        {
        //            dt.Rows.Add("BÁO CÁO DỮ LIỆU CHẤM CÔNG CHI TIẾT NGÀY " + param.FromDate.ToString("dd/MM/yyyy") + " - " + param.ToDate.ToString("dd/MM/yyyy"));
        //        }

        //        ds.Tables.Add(dt);
        //        var pathTemp = _appContext._config["urlAT001"];
        //        var memoryStream = Template.FillReport(pathTemp, ds);
        //        return new ResultWithError(memoryStream);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultWithError(ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Tông hợp Phép
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //public async Task<ResultWithError> CalEntitlement(TimeSheetInputDTO param)
        //{
        //    try
        //    {

        //        await QueryData.Execute("PKG_ENTITLEMENT.CAL_ENTITLEMENT",
        //           new
        //           {
        //               P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
        //               P_ORG_ID = param.OrgId,
        //               P_PERIOD_ID = param.PeriodId
        //           }, true);
        //        return new ResultWithError(200);

        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultWithError(ex.Message);
        //    }
        //}


        //public async Task<ResultWithError> ReadMCC(TimeSheetInputDTO param)
        //{

        //    // Tam thoi bo qua MCC
        //    return await Task.Run(() => new ResultWithError(200));

        //    // lấy thông tin máy chấm công:
        //    /*
        //    try
        //    {
        //        var lst = new List<SwipeDataTmp>();
        //        int mv_MachineNumber = 1;
        //        int sv_ErrorNo = 0;
        //        int rowNumber = 0;
        //        int sv_year = 0;
        //        int sv_month = 0;
        //        int sv_day = 0;
        //        int sv_hour = 0;
        //        int sv_minute = 0;
        //        int sv_second = 0;
        //        int sv_dworkcode = 0;
        //        int sv_verifyStatus = 0;
        //        int sv_inOutStatus = 0;
        //        string dwEnrollNumber;
        //        var objZkeeper = new CZKEM();
        //        objZkeeper.SetCommPassword(12345);
        //        objZkeeper.Connect_Net("10.5.4.204", 4370);
        //        objZkeeper.EnableDevice(mv_MachineNumber, false);
        //        objZkeeper.GetDeviceStatus(mv_MachineNumber, 6, rowNumber);
        //        if (objZkeeper.ReadGeneralLogData(mv_MachineNumber))
        //        {
        //            objZkeeper.ReadMark = true;
        //            objZkeeper.GetLastError(sv_ErrorNo);
        //            while (objZkeeper.SSR_GetGeneralLogData(mv_MachineNumber, out dwEnrollNumber, out sv_verifyStatus, out sv_inOutStatus, out sv_year, out sv_month, out sv_day, out sv_hour, out sv_minute, out sv_second, ref sv_dworkcode))
        //            {
        //                var inputDate = new DateTime(sv_year, sv_month, sv_day, sv_hour, sv_minute, sv_second);
        //                var objSwipe = new SwipeDataTmp();
        //                objSwipe.ITIME_ID = dwEnrollNumber;
        //                objSwipe.TIME_POINT = inputDate;
        //                lst.Add(objSwipe);
        //            }
        //            if (lst.Count > 0)
        //            {
        //                await _appContext.SwipeDataTmps.AddRangeAsync(lst);
        //                await _appContext.SaveChangesAsync();
        //            }
        //        }
        //        return new ResultWithError(200);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    */

        //}

    }
}
