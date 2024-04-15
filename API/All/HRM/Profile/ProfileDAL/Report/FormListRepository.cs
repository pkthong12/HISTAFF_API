using API.All.DbContexts;
using API.Entities;
using Common.Extensions;
using Common.Repositories;
using CORE.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using ProfileDAL.ViewModels;

namespace ProfileDAL.Repositories
{
    public class FormListRepository : RepositoryBase<HU_FORM_LIST>, IFormListRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        public FormListRepository(ProfileDbContext context) : base(context)
        {

        }

        public async Task<ResultWithError> GetById(long id)
        {
            try
            {
                // Get ra bieu mau
                var r = await _appContext.FormLists.Where(c => c.ID_MAP == id )
                    .Select(d => d.TEXT)
                    .FirstOrDefaultAsync();
                // Get list cac phan tu 
                while (id >= 10) id /= 10;
                var x = await QueryData.ExecuteStore<ReferParam>(Procedures.PKG_COMMON_FORM_ELEMENT, new
                {
                    P_TYPE_ID = id,
                    
                    P_CUR = QueryData.OUT_CURSOR
                });



                return new ResultWithError(new { Text = r, Elements = x });
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> GetTreeView()
        {
            try
            {

                var r = await QueryData.ExecuteStore<FormListDTO>
               (Procedures.PKG_COMMON_FORM_LIST,
               new
               {
                   
                   p_user_id = _appContext.CurrentUserId,
                   P_CUR = QueryData.OUT_CURSOR
               });


                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> PrintForm(int id, int typeId)
        {
            try
            {

                var r = await QueryData.ExecuteObject
                ("PKG_COMMON.FORM_PRINT",
                new
                {
                    P_ID = id,
                    P_TYPE_ID = typeId,
                    
                    P_CUR = QueryData.OUT_CURSOR
                }, false);


                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> PrintFormProfile(int id)
        {
            try
            {
                var data = await QueryData.ExecuteStoreObject("PKG_DEMO.PRINT_PROFILE",
                    new
                    {
                        P_EMP_ID = id,
                        
                        P_EMP_DATA = QueryData.OUT_CURSOR,
                        P_FORMULA = QueryData.OUT_CURSOR,
                        P_DECISION = QueryData.OUT_CURSOR,
                        P_CONTRACT = QueryData.OUT_CURSOR,
                        P_COMMENT = QueryData.OUT_CURSOR,
                        P_DISCIPLINE = QueryData.OUT_CURSOR,
                        P_INSCHANGE = QueryData.OUT_CURSOR
                    }, false);
                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResultWithError> PrintFormSalary(PayrollInputDTO param)
        {
            try
            {
                var data = await QueryData.ExecuteStoreObject(Procedures.PKG_COMMON_FORM_PRINT_SALARY,
                    new
                    {
                        p_is_admin = _appContext.IsAdmin == true ? 1 : 0,
                        p_org_id = param.OrgId,
                        p_period_id = param.PeriodId,
                        P_SALARY_TYPE_ID = param.SalaryTypeId,
                        P_CUR = QueryData.OUT_CURSOR,
                        P_FORMULA = QueryData.OUT_CURSOR,
                        P_CUR2 = QueryData.OUT_CURSOR,
                    }, true);
                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResultWithError> PrintFormAttendance(PayrollInputDTO param)
        {
            try
            {
                var data = await QueryData.ExecuteStoreObject(Procedures.PKG_COMMON_FORM_PRINT_ATTENDANCE,
                    new
                    {
                        P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                        P_ORG_ID = param.OrgId,
                        P_PERIOD_ID = param.PeriodId,
                        P_CUR = QueryData.OUT_CURSOR,
                        P_FORMULA = QueryData.OUT_CURSOR
                    }, true);
                return new ResultWithError(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResultWithError> GetListRemind()
        {
            try
            {

                var r = await (from p in _appContext.OtherListFixs
                               join a in _appContext.SettingReminds on p.CODE equals a.CODE into tmp1
                               from a2 in tmp1.DefaultIfEmpty()
                               where p.TYPE == SystemConfig.SETTING_REMIND
                               orderby p.ORDERS
                               select new SettingRemindDTO
                               {
                                   Code = p.CODE,
                                   Name = p.NAME,
                                   Day = a2.DAY,
                                   IsActive = a2.IS_ACTIVE
                               }).ToArrayAsync();

                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> GetRemind()
        {
            try
            {
                var listRemind = await (from p in _appContext.OtherListFixs.Where(c => c.TYPE == SystemConfig.SETTING_REMIND)
                                        from a in _appContext.SettingReminds.Where(c => c.IS_ACTIVE == true && c.CODE == p.CODE ).DefaultIfEmpty()
                                        orderby p.ORDERS
                                        select new RemindDTO
                                        {
                                            Name = p.NAME,
                                            Code = p.CODE,
                                            Day = a.DAY
                                        }
                    ).ToArrayAsync();
                var d = DateTime.Now.DayOfYear;


                var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                     new
                     {
                         P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                         
                         P_ORG_ID = 0,
                         P_CURENT_USER_ID = _appContext.CurrentUserId,
                         P_CUR = QueryData.OUT_CURSOR
                     }, false);

                List<long?> ids = orgIds.Select(c => (long?)((dynamic)c).ID).ToList();

                foreach (var item in listRemind)
                {
                    switch (item.Code)
                    {
                        case SystemConfig.EMP_BIRTH_DAY:
                            var x = await (from p in _appContext.Employees.Where(c => c.Profile.BIRTH_DATE != null)
                                           where ((DateTime)p.Profile.BIRTH_DATE).DayOfYear - d <= item.Day && ((DateTime)p.Profile.BIRTH_DATE).DayOfYear - d >= 0
                                            && p.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE
                                           && ids.Contains(p.ORG_ID)
                                           select new ReferParam
                                           {
                                               Id = (int)p.ID,
                                               Name = p.Profile.FULL_NAME,
                                               Code = p.CODE
                                           }).ToArrayAsync();
                            item.Value = new List<ReferParam>();
                            item.Value.AddRange(x);
                            item.Count = x.Count();
                            break;
                        case SystemConfig.EMP_EXPIRE_CONTRACT:// Het han hop dong
                            var a = await (from p in _appContext.Contracts.Where(c => c.EXPIRE_DATE != null)
                                           join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                                           where ((DateTime)p.EXPIRE_DATE).DayOfYear - d < item.Day && ((DateTime)p.EXPIRE_DATE).DayOfYear - d > 0
                                              && e.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE
                                             && ids.Contains(e.ORG_ID)
                                           select new ReferParam
                                           {
                                               Id = (int)e.ID,
                                               Name = e.Profile.FULL_NAME,
                                               Code = e.CODE
                                           }).ToArrayAsync();
                            item.Value = new List<ReferParam>();
                            item.Value.AddRange(a);
                            item.Count = a.Count();
                            break;
                        case SystemConfig.EMP_REGISTER_CONTRACT:// Nhan vien chua lam hop dong
                            var b = await (from p in _appContext.Employees
                                           join e in _appContext.Contracts on p.ID equals e.EMPLOYEE_ID into tmp1
                                           from e2 in tmp1.DefaultIfEmpty()
                                           where e2.CONTRACT_NO == null  && p.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE
                                           && ids.Contains(p.ORG_ID)
                                           select new ReferParam
                                           {
                                               Id = (int)p.ID,
                                               Name = p.Profile.FULL_NAME,
                                               Code = p.CODE
                                           }).ToArrayAsync();
                            item.Value = new List<ReferParam>();
                            item.Value.AddRange(b);
                            item.Count = b.Count();
                            break;
                        case SystemConfig.EMP_REGISTER_WORKING:// Nhan vien chua co quyet dinh
                            var f = await (from p in _appContext.Employees
                                           join e in _appContext.Workings on p.ID equals e.EMPLOYEE_ID into tmp1
                                           from e2 in tmp1.DefaultIfEmpty()
                                           where e2.DECISION_NO == null  && p.WORK_STATUS_ID != OtherConfig.EMP_STATUS_TERMINATE
                                           && ids.Contains(p.ORG_ID)
                                           select new ReferParam
                                           {
                                               Id = (int)p.ID,
                                               Name = p.Profile.FULL_NAME,
                                               Code = p.CODE
                                           }).ToArrayAsync();
                            item.Value = new List<ReferParam>();
                            item.Value.AddRange(f);
                            item.Count = f.Count();
                            break;
                        case SystemConfig.EMP_METTTING_ROOM:// Đặt phòng họp
                            var g = await (from p in _appContext.Employees
                                           join e in _appContext.Bookings on p.ID equals e.EMP_ID
                                           where e.STATUS_ID == OtherConfig.STATUS_WAITING 
                                           && ids.Contains(p.ORG_ID)
                                           select new ReferParam
                                           {
                                               Id = (int)p.ID,
                                               Name = p.Profile.FULL_NAME,
                                               Code = p.CODE
                                           }).ToArrayAsync();
                            item.Value = new List<ReferParam>();
                            item.Value.AddRange(g);
                            item.Count = g.Count();
                            break;
                        case SystemConfig.EMP_PAPER:// Đặt phòng họp
                            var r = await QueryData.ExecuteStore<InfoView>(Procedures.PKG_PROFILE_GET_INFO, new
                            {
                                
                                P_EMP_ID = _appContext.EmpId,
                                P_CUR = QueryData.OUT_CURSOR
                            });


                            //var g = await (from p in _appContext.Employees
                            //               join e in _appContext.EmployeePaperses on p.ID equals e.EMP_ID
                            //               join a in _appContext.PostionPaperses on p.POSITION_ID equals a.POS_ID
                            //               where e.STATUS_ID == OtherConfig.STATUS_WAITING 
                            //               && ids.Contains(p.ORG_ID)
                            //               select new ReferParam
                            //               {
                            //                   Id = (int)p.ID,
                            //                   Name = p.FULLNAME,
                            //                   Code = p.CODE
                            //               }).ToArrayAsync();
                            //item.Value = new List<ReferParam>();
                            //item.Value.AddRange(g);
                            //item.Count = g.Count();
                            break;
                        default:

                            break;
                    }
                }
                return new ResultWithError(listRemind);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> UpdateRemind(List<SettingRemindDTO> param)
        {
            try
            {
                var rms = await _appContext.SettingReminds.ToArrayAsync();
                _appContext.SettingReminds.RemoveRange(rms);
                var list = new List<HU_SETTING_REMIND>();
                for (int i = 0; i < param.Count; i++)
                {
                    if (param[i].IsActive == true && param[i].Day != null)
                    {
                        var r = Map(param[i], new HU_SETTING_REMIND());
                        list.Add(r);
                    }

                }

                await _appContext.SettingReminds.AddRangeAsync(list);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex);
            }
        }
        public async Task<ResultWithError> UpdateAsync(FormListDTO param)
        {
            try
            {
                var x = await _appContext.FormLists.Where(c => c.ID_MAP == param.IdMap ).FirstOrDefaultAsync();
                int? parentId = param.IdMap;
                while (parentId >= 10) parentId /= 10;
                if (x == null)
                {
                    var r = new HU_FORM_LIST();
                    r.ID_MAP = param.IdMap;
                    r.TEXT = param.Text;
                    _appContext.FormLists.Add(r);
                }
                else
                {
                    x.TEXT = param.Text;
                    _appContext.FormLists.Update(x);
                }
                _appContext.SaveChanges();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex);
            }
        }
        public async Task<FormatedResponse> GetDashboard()
        {
            try
            {
                var r = await QueryData.ExecuteStoreObject(Procedures.PKG_DASHBOARD_STATISTIC_COMMON,
                new
                {
                    P_TENANT_ID = 0,
                    P_USER_ID = _appContext.IsAdmin == true ? "admin" : _appContext.CurrentUserId,
                    P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                    P_CUR_GENDER = QueryData.OUT_CURSOR,
                    P_CUR_CONTRACT = QueryData.OUT_CURSOR,
                    P_CUR_LEARNING = QueryData.OUT_CURSOR,
                    P_CUR_EMP_MONTH = QueryData.OUT_CURSOR,
                    P_CUR_SENIORITY = QueryData.OUT_CURSOR
                }, false); 

                return new FormatedResponse() { InnerBody = r};
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }


        public async Task<ResultWithError> REPORT_HU001()
        {
            try
            {
                var r = await QueryData.ExecuteStoreObject(Procedures.PKG_REPORT_REPORT_HU001,
                new
                {
                    P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                    P_DATE = QueryData.OUT_CURSOR,
                    P_ORG_ID = QueryData.OUT_CURSOR,
                    P_CUR = QueryData.OUT_CURSOR
                }, true);

                return new ResultWithError(r);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> REPORT_HU009()
        {
            try
            {
                var r = await QueryData.ExecuteStoreObject(Procedures.PKG_REPORT_REPORT_HU009,
                new
                {
                    P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                    P_DATE = QueryData.OUT_CURSOR,
                    P_ORG_ID = QueryData.OUT_CURSOR,
                    P_CUR = QueryData.OUT_CURSOR
                }, true);

                return new ResultWithError(r);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex.Message);
            }
        }

    }
}
