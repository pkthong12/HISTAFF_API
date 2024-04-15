using Common.Paging;
using AttendanceDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace AttendanceDAL.Repositories
{
    public class OverTimeRepository : RepositoryBase<AT_OVERTIME>, IOverTimeRepository
    {
        private AttendanceDbContext _appContext => (AttendanceDbContext)_context;
        public OverTimeRepository(AttendanceDbContext context) : base(context)
        {

        }


        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<OverTimeDTO>> GetAll(OverTimeDTO param)
        {
            try
            {
                var queryable = from p in _appContext.RegisterOffs
                                join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                                join o in _appContext.Organizations on e.ORG_ID equals o.ID
                                join po in _appContext.Positions on e.POSITION_ID equals po.ID
                                
                                orderby p.ID descending
                                select new OverTimeDTO
                                {
                                    Id = p.ID,
                                    EmployeeId = p.EMPLOYEE_ID,
                                    EmployeeName = e.Profile.FULL_NAME,
                                    EmployeeCode = e.CODE,
                                    OrgId = e.ORG_ID,
                                    OrgName = o.NAME,
                                    PositionName = po.NAME,
                                    WorkingDay = p.WORKING_DAY,
                                    TimeStart = p.TIME_START,
                                    TimeEnd = p.TIME_END,
                                    StatusId = p.STATUS_ID,
                                    Note = p.NOTE
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

                if (!string.IsNullOrWhiteSpace(param.EmployeeName))
                {
                    queryable = queryable.Where(p => p.EmployeeName.ToUpper().Contains(param.EmployeeName.ToUpper()));
                }

                if (!string.IsNullOrWhiteSpace(param.OrgName))
                {
                    queryable = queryable.Where(p => p.OrgName.ToUpper().Contains(param.OrgName.ToUpper()));
                }
                if (!string.IsNullOrWhiteSpace(param.PositionName))
                {
                    queryable = queryable.Where(p => p.PositionName.ToUpper().Contains(param.PositionName.ToUpper()));
                }
                if (param.EmployeeId != null)
                {
                    queryable = queryable.Where(p => p.EmployeeId == param.EmployeeId);
                }



                return await PagingList(queryable, param);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ResultWithError> CreateAsync(OverTimeCreateDTO param)
        {
            try
            {
                if (param.EmpIds.Count == 0)
                {
                    return new ResultWithError(Message.EMP_NOT_EXIST);
                }
                foreach (var item in param.EmpIds)
                {
                    var data = Map(param, new AT_REGISTER_OFF());
                    data.STATUS_ID = OtherConfig.STATUS_WAITING;
                    data.TYPE_ID = Consts.REGISTER_OT;
                    data.IS_ACTIVE = true;
                    data.EMPLOYEE_ID = item;
                    data.DATE_START = param.WorkingDay;
                    data.DATE_END = param.WorkingDay;
                    var result = await _appContext.RegisterOffs.AddAsync(data);
                    // insert record to Table Approved
                    var app = new AT_APPROVED();
                    app.REGISTER_ID = data.ID;
                    app.EMP_RES_ID = data.EMPLOYEE_ID;
                    app.STATUS_ID = OtherConfig.STATUS_WAITING;
                    app.APPROVE_DAY = DateTime.Now;
                    app.IS_READ = 0;
                    app.TYPE_ID = Consts.REGISTER_OT;
                    await _appContext.Approveds.AddAsync(app);
                    // tạo notifi
                    // lấy fmc token
                    var Mng = await (from o in _appContext.Organizations
                                     join e in _appContext.Employees on o.ID equals e.ORG_ID
                                     where e.ID == app.EMP_RES_ID
                                     select new
                                     {
                                         mngId = o.MNG_ID,
                                         orgParentId = o.PARENT_ID
                                     }
                                   ).FirstOrDefaultAsync();
                    if (Mng != null)
                    {
                        var mngId = Mng.mngId.ToString();
                        if (Mng.mngId == app.EMP_RES_ID) // người phê duyệt là người đăng ký Up lên 1 lvl
                        {
                            var lstEmpId = await _appContext.Organizations.Where(x => x.ID == Mng.orgParentId).Select(v => v.MNG_ID).FirstOrDefaultAsync();
                            if (lstEmpId != null)
                            {
                                mngId = lstEmpId.ToString();  
                            }
                            else
                            {
                                mngId = null;
                            }
                        }

                        if (mngId!=null)
                        {
                            var lstToken = await QueryData.ExecuteList("connectionString",
                            "PKG_NOTIFI.GET_FMC_TOKEN", new
                            {
                                
                                P_EMP_ID = mngId,
                                P_CUR = QueryData.OUT_CURSOR
                            });
                            var IsRead = new List<bool>(lstToken.Count);

                            List<string> lstDevice = lstToken.Select(c => (string)((dynamic)c).FCM_TOKEN).ToList();
                            if (lstDevice.Count > 0)
                            {
                                //lstDevice.Add()
                                for (int i = 0; i < lstToken.Count; i++)
                                {
                                    IsRead.Add(false);
                                }
                                // 
                                var no = new AT_NOTIFICATION();
                                no.TYPE = Consts.REGISTER_OT;
                                no.ACTION = Consts.ACTION_CREATE;
                                no.NOTIFI_ID = data.ID;
                                no.EMP_CREATE_ID = app.EMP_RES_ID;
                                no.FMC_TOKEN = string.Join(";", lstDevice);
                                //no.IS_READ = string.Join(";", IsRead);
                                //no.EMP_NOTIFY_ID = Mng.mngId.ToString();
                                no.CREATED_DATE = DateTime.Now;
                                // sau khi lưu gửi thông báo
                                var FullName = _appContext.Employees.Where(x => x.ID == data.EMPLOYEE_ID).Select(e => e.Profile.FULL_NAME).FirstOrDefault();
                                var Title = "";
                                var Body = "";

                                Title = FullName + " đăng ký OT ngày " + param.WorkingDay.ToString("dd/MM/yyyy");
                                Body = "Từ " + param.TimeStart.Value.ToString("HH:MM") + " đến " + param.TimeEnd.Value.ToString("HH:MM");
                                await _appContext.Notifications.AddAsync(no);

                                

                               await SendNotification(new NotificationModel
                                {
                                    Devices = lstDevice,
                                    Title = Title,
                                    Body = Body
                                });

                            }
                        }
                    }
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(param);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> UpdateAsync(OverTimeInputDTO param)
        {
            try
            {
                var r = _appContext.RegisterOffs.Where(c => c.ID == param.Id).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(Message.RECORD_NOT_FOUND);
                }
                if (r.STATUS_ID == OtherConfig.STATUS_APPROVE)
                {
                    return new ResultWithError("APPROVED");
                }
                var data = Map(param, r);
                data.DATE_START = param.WorkingDay;
                data.DATE_END = param.WorkingDay;
                //data.STATUS_ID = OtherConfig.STATUS_WAITING;
                var result = _appContext.RegisterOffs.Update(data);
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
        public async Task<ResultWithError> ChangeStatusAsync(List<long> ids, int StatusId)
        {
            try
            {
                foreach (var item in ids)
                {
                    var r = _appContext.OverTimes.Where(x => x.ID == item ).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    if (r.STATUS_ID == OtherConfig.STATUS_APPROVE)
                    {
                        return new ResultWithError("APPROVED");
                    }
                    if (r.STATUS_ID == OtherConfig.STATUS_DECLINE)
                    {
                        return new ResultWithError("DENIED");
                    }
                    r.STATUS_ID = StatusId;
                    var result = _appContext.OverTimes.Update(r);
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
                    var r = _appContext.OverTimes.Where(x => x.ID == item ).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    if (r.STATUS_ID == OtherConfig.STATUS_APPROVE)
                    {
                        return new ResultWithError("APPROVED");
                    }
                    r.IS_ACTIVE = false;
                    _appContext.OverTimes.Update(r);
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
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateConfig(OverTimeConfigDTO param)
        {
            try
            {

                var r = await _appContext.OverTimeConfigs.FindAsync(param.Id);
                if (r != null)
                {
                    var m = Map(param, r);
                    _appContext.OverTimeConfigs.Update(m);
                }
                else
                {
                    var m = Map(param, new AT_OVERTIME_CONFIG());
                    await _appContext.OverTimeConfigs.AddAsync(m);
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(param);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> GetConfig()
        {
            try
            {
                var r = await (from p in _appContext.OverTimeConfigs
                               
                               select new
                               {
                                   Id = p.ID,
                                   HourMin = p.HOUR_MIN,
                                   HourMax = p.HOUR_MAX,
                                   FactorNt = p.FACTOR_NT,
                                   FactorNn = p.FACTOR_NN,
                                   FactorNl = p.FACTOR_NL,
                                   FactorDnt = p.FACTOR_DNT,
                                   FactorDnn = p.FACTOR_DNN,
                                   FactorDnl = p.FACTOR_DNL
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// Portal GetBY Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetBy(long id)
        {
            try
            {
                var r = await (from p in _appContext.RegisterOffs
                               where p.EMPLOYEE_ID == _appContext.EmpId && p.ID == id
                               select new
                               {
                                   WorkingDay = p.DATE_START,
                                   DateStart = p.DATE_START,
                                   DateEnd = p.DATE_END,
                                   Note = p.NOTE

                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
    }
}
