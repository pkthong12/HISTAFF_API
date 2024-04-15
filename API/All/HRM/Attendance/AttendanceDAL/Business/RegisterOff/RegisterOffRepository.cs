using Common.Paging;
using AttendanceDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace AttendanceDAL.Repositories
{
    public class RegisterOffRepository : RepositoryBase<AT_REGISTER_OFF>, IRegisterOffRepository
    {
        private AttendanceDbContext _appContext => (AttendanceDbContext)_context;
        public RegisterOffRepository(AttendanceDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<RegisterOffDTO>> GetAll(RegisterOffDTO param)
        {
            try
            {
                var queryable = from e in _appContext.Employees
                                join o in _appContext.Organizations on e.ORG_ID equals o.ID
                                join a in _appContext.Positions on e.POSITION_ID equals a.ID
                                join p in _appContext.RegisterOffs on e.ID equals p.EMPLOYEE_ID
                                join s in _appContext.TimeTypes on p.TIMETYPE_ID equals s.ID
                                orderby p.STATUS_ID, p.DATE_START descending
                                select new RegisterOffDTO
                                {
                                    Id = p.ID,
                                    EmployeeId = p.EMPLOYEE_ID,
                                    EmployeeName = e.Profile.FULL_NAME,
                                    EmployeeCode = e.CODE,
                                    OrgId = e.ORG_ID,
                                    OrgName = o.NAME,
                                    PositionName = a.NAME,
                                    DateStart = p.DATE_START,
                                    DateEnd = p.DATE_END,
                                    TimetypeId = p.TIMETYPE_ID,
                                    TimetypeCode = "[" + s.CODE + "] " + s.NAME,
                                    StatusId = p.STATUS_ID,
                                    TypeId = p.TYPE_ID,
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
                if (param.TypeId != null)
                {
                    queryable = queryable.Where(p => p.TypeId == param.TypeId);
                }

                return await PagingList(queryable, param);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// CMS Get All Data OT and EarlyLate
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<RegisterOffDTO>> GetAllOTEL(RegisterOffDTO param)
        {
            try
            {
                var queryable = from p in _appContext.RegisterOffs
                                join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                                join o in _appContext.Organizations on e.ORG_ID equals o.ID
                                join a in _appContext.Positions on e.POSITION_ID equals a.ID

                                orderby p.ID descending
                                select new RegisterOffDTO
                                {
                                    Id = p.ID,
                                    EmployeeId = p.EMPLOYEE_ID,
                                    EmployeeName = e.Profile.FULL_NAME,
                                    EmployeeCode = e.CODE,
                                    OrgId = e.ORG_ID,
                                    OrgName = o.NAME,
                                    PositionName = a.NAME,
                                    DateStart = p.DATE_START,
                                    DateEnd = p.DATE_END,
                                    TimetypeId = p.TIMETYPE_ID,
                                    StatusId = p.STATUS_ID,
                                    TypeId = p.TYPE_ID,
                                    TimeLate = p.TIME_LATE,
                                    TimeEarly = p.TIME_EARLY,
                                    TimeStart = p.TIME_START,
                                    TimeEnd = p.TIME_END,
                                    WorkingDay = p.WORKING_DAY,
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

                if (!string.IsNullOrWhiteSpace(param.EmployeeCode))
                {
                    queryable = queryable.Where(p => p.EmployeeCode.ToUpper().Contains(param.EmployeeCode.ToUpper()));
                }
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
                if (param.TypeId != null)
                {
                    queryable = queryable.Where(p => p.TypeId == param.TypeId);
                }

                return await PagingList(queryable, param);

            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                var r = await (from p in _appContext.RegisterOffs
                               where p.ID == id
                               select new
                               {
                                   Id = p.ID,
                                   EmployeeId = p.EMPLOYEE_ID,
                                   DateStart = p.DATE_START,
                                   DateEnd = p.DATE_END,
                                   TimetypeId = p.TIMETYPE_ID,
                                   Note = p.NOTE,
                                   StatusId = p.STATUS_ID
                               }).FirstOrDefaultAsync();


                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        public async Task<ResultWithError> CreateAsync(RegisterOffInputDTO param, int type)
        {
            var r = await Register(param, type, param.EmployeeId);
            return new ResultWithError(r);
        }
        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(RegisterOffInputDTO param)
        {
            try
            {
                var r = _appContext.RegisterOffs.Where(c => c.ID == param.Id).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(Message.RECORD_NOT_FOUND);
                }

                if (r.STATUS_ID == OtherConfig.STATUS_APPROVE && !_appContext.IsAdmin)
                {
                    return new ResultWithError(Message.RECORD_IS_APPROVED);
                }

                var data = Map(param, r);
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
                var emp = await (from p in _appContext.Employees
                                 join o in _appContext.Positions on p.POSITION_ID equals o.ID
                                 where p.ID == _appContext.EmpId
                                 select new
                                 {
                                     fullName = p.Profile.FULL_NAME,
                                     PosName = o.NAME
                                 }).FirstOrDefaultAsync();
                foreach (var item in ids)
                {
                    var r = _appContext.RegisterOffs.Where(x => x.ID == item).FirstOrDefault();
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
                    var result = _appContext.RegisterOffs.Update(r);

                    // insert record to Table Approved               
                    var app = new AT_APPROVED();
                    app.REGISTER_ID = r.ID;
                    app.EMP_RES_ID = r.EMPLOYEE_ID;
                    app.APPROVE_ID = _appContext.EmpId;
                    app.STATUS_ID = StatusId;
                    app.TYPE_ID = r.TYPE_ID;
                    app.IS_READ = 0;
                    app.IS_REG = 0;
                    app.APPROVE_DAY = DateTime.Now;
                    if (emp != null)
                    {
                        app.APPROVE_NAME = emp.fullName;
                        app.APPROVE_POS = emp.PosName;
                    }
                    else
                    {
                        app.APPROVE_NAME = "Admin";
                        app.APPROVE_POS = "Admin";
                    }

                    await _appContext.Approveds.AddAsync(app);
                    // tạo notifi
                    // lấy fmc token
                    var lstEmpId = new List<long>() { r.EMPLOYEE_ID };
                    var lstToken = await QueryData.ExecuteList("connectionString",
                        "PKG_NOTIFI.GET_FMC_TOKEN", new
                        {

                            P_EMP_ID = string.Join(",", lstEmpId),
                            P_CUR = QueryData.OUT_CURSOR
                        });
                    var IsRead = new List<bool>(lstToken.Count);
                    List<string> lstDevice = lstToken.Select(c => (string)((dynamic)c).FCM_TOKEN).ToList();
                    if (lstDevice.Count > 0)
                    {
                        for (int i = 0; i < lstToken.Count; i++)
                        {
                            IsRead.Add(false);
                        }
                        // 
                        var no = new AT_NOTIFICATION();
                        no.TYPE = r.TYPE_ID;

                        no.NOTIFI_ID = r.ID;
                        no.EMP_CREATE_ID = _appContext.EmpId;
                        no.FMC_TOKEN = string.Join(";", lstDevice);
                        //no.IS_READ = string.Join(";", IsRead);
                        //no.EMP_NOTIFY_ID = string.Join(";", lstEmpId);
                        no.CREATED_DATE = DateTime.Now;

                        // sau khi lưu gửi thông báo
                        var Title = "";
                        var Body = "";
                        if (StatusId == OtherConfig.STATUS_APPROVE)
                        {
                            no.ACTION = Consts.ACTION_APPROVE;
                            if (r.TYPE_ID == Consts.REGISTER_OFF)
                            {
                                Title = app.APPROVE_NAME + " đã phê duyệt";
                                if (r.DATE_START == r.DATE_END)
                                {
                                    Body = "Đăng ký nghỉ ngày " + r.DATE_START.ToString("dd/MM/yyyy");
                                }
                                else
                                {
                                    Body = "Đăng ký nghỉ từ " + r.DATE_START.ToString("dd/MM/yyyy") + " - " + r.DATE_END.ToString("dd/MM/yyyy");
                                }

                            }
                            else if (r.TYPE_ID == Consts.REGISTER_LATE)
                            {
                                Title = app.APPROVE_NAME + " đã phê duyệt";
                                if (r.DATE_START == r.DATE_END)
                                {
                                    Body = "Đi muộn về sớm ngày" + r.DATE_START.ToString("dd/MM/yyyy");
                                }
                                else
                                {
                                    Body = "Đi muộn về sớm từ " + r.DATE_START.ToString("dd/MM/yyyy") + " - " + r.DATE_END.ToString("dd/MM/yyyy");
                                }
                            }
                            else
                            {
                                Title = app.APPROVE_NAME + " đã phê duyệt";
                                if (r.DATE_START == r.DATE_END)
                                {
                                    Body = "OT ngày" + r.DATE_START.ToString("dd/MM/yyyy");
                                }
                                else
                                {
                                    Body = "OT từ " + r.DATE_START.ToString("dd/MM/yyyy") + " - " + r.DATE_END.ToString("dd/MM/yyyy");
                                }
                            }
                        }
                        else
                        {
                            no.ACTION = Consts.ACTION_DENIED;
                            if (r.TYPE_ID == Consts.REGISTER_OFF)
                            {
                                Title = app.APPROVE_NAME + " đã từ chối";
                                if (r.DATE_START == r.DATE_END)
                                {
                                    Body = "Đăng ký nghỉ ngày " + r.DATE_START.ToString("dd/MM/yyyy");
                                }
                                else
                                {
                                    Body = "Đăng ký nghỉ từ " + r.DATE_START.ToString("dd/MM/yyyy") + " - " + r.DATE_END.ToString("dd/MM/yyyy");
                                }
                            }
                            else if (r.TYPE_ID == Consts.REGISTER_LATE)
                            {
                                Title = app.APPROVE_NAME + " đã từ chối";
                                if (r.DATE_START == r.DATE_END)
                                {
                                    Body = "Đi muộn về sớm ngày" + r.DATE_START.ToString("dd/MM/yyyy");
                                }
                                else
                                {
                                    Body = "Đi muộn về sớm từ " + r.DATE_START.ToString("dd/MM/yyyy") + " - " + r.DATE_END.ToString("dd/MM/yyyy");
                                }
                            }
                            else
                            {
                                Title = app.APPROVE_NAME + " đã từ chối";
                                if (r.DATE_START == r.DATE_END)
                                {
                                    Body = "OT ngày" + r.DATE_START.ToString("dd/MM/yyyy");
                                }
                                else
                                {
                                    Body = "OT từ " + r.DATE_START.ToString("dd/MM/yyyy") + " - " + r.DATE_END.ToString("dd/MM/yyyy");
                                }
                            }
                        }

                        await _appContext.Notifications.AddAsync(no);
                        await SendNotification(new NotificationModel
                        {
                            Devices = lstDevice,
                            Title = Title,
                            Body = Body

                        });
                    }
                    await _appContext.SaveChangesAsync();
                }

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
                    var r = _appContext.RegisterOffs.Where(x => x.ID == item).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    //if (r.STATUS_ID == OtherConfig.STATUS_APPROVE)
                    //{
                    //    return new ResultWithError("APPROVED");
                    //}
                    if (!_appContext.IsAdmin)
                    {
                        if (r.STATUS_ID == OtherConfig.STATUS_APPROVE)
                        {
                            return new ResultWithError(Message.RECORD_IS_APPROVED);
                        }
                    }
                    _appContext.RegisterOffs.Remove(r);
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
        /// Portal Register
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>

        public async Task<ResultWithError> PortalReg(RegisterOffInputDTO param, int type)
        {
            var r = await Register(param, type, _appContext.EmpId);
            return new ResultWithError(r);
        }
        /// <summary>
        /// Approve
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>

        public async Task<ResultWithError> PortalApprove(PortalApproveDTO param, int type, int status)
        {
            try
            {
                var day = DateTime.Now;
                var emp = await (from p in _appContext.Employees
                                 join o in _appContext.Positions on p.POSITION_ID equals o.ID
                                 where p.ID == _appContext.EmpId
                                 select new
                                 {
                                     fullName = p.Profile.FULL_NAME,
                                     PosName = o.NAME
                                 }).FirstOrDefaultAsync();
                // Approved
                var r = await _appContext.RegisterOffs.Where(x => x.ID == param.Id).FirstOrDefaultAsync();
                if (r != null)
                {
                    if (r.STATUS_ID == OtherConfig.STATUS_APPROVE)
                    {
                        return new ResultWithError(Message.RECORD_IS_APPROVED);
                    }
                }
                else
                {
                    return new ResultWithError(Message.RECORD_NOT_FOUND);
                }
                r.STATUS_ID = status;
                r.APPROVE_NAME = emp.fullName;
                r.APPROVE_POS = emp.PosName;
                r.APPROVE_DAY = day;
                r.APPROVE_NOTE = param.Note;
                var result = _appContext.RegisterOffs.Update(r);
                // insert record to Table Approved               
                var app = new AT_APPROVED();
                app.REGISTER_ID = r.ID;
                app.EMP_RES_ID = r.EMPLOYEE_ID;
                app.APPROVE_ID = _appContext.EmpId;
                app.STATUS_ID = status;
                app.TYPE_ID = type;
                app.IS_READ = 0;
                app.IS_REG = 0;
                app.APPROVE_DAY = day;
                app.APPROVE_NAME = emp.fullName;
                app.APPROVE_POS = emp.PosName;
                app.APPROVE_NOTE = param.Note;
                await _appContext.Approveds.AddAsync(app);

                if (type == Consts.REGISTER_OFF)
                {
                    await QueryData.Execute("PKG_TIMESHEET.UPDATE_DAYOFF",
                    new
                    {
                        P_EMP_ID = r.EMPLOYEE_ID,
                        P_TIME_TYPE = r.TIMETYPE_ID,
                        P_START = r.DATE_START,
                        P_END = r.DATE_END
                    }, true);
                }
                // tạo notifi
                // lấy fmc token
                var lstEmpId = new List<long>() { r.EMPLOYEE_ID };
                var lstToken = await QueryData.ExecuteList("connectionString",
                    "PKG_NOTIFI.GET_FMC_TOKEN", new
                    {

                        P_EMP_ID = string.Join(",", lstEmpId),
                        P_CUR = QueryData.OUT_CURSOR
                    });
                var IsRead = new List<bool>(lstToken.Count);
                List<string> lstDevice = lstToken.Select(c => (string)((dynamic)c).FCM_TOKEN).ToList();
                if (lstDevice.Count > 0)
                {
                    for (int i = 0; i < lstToken.Count; i++)
                    {
                        IsRead.Add(false);
                    }
                    // 
                    var no = new AT_NOTIFICATION();
                    no.NOTIFI_ID = r.ID;
                    no.EMP_CREATE_ID = _appContext.EmpId;
                    no.FMC_TOKEN = string.Join(";", lstDevice);
                    //no.IS_READ = string.Join(";", IsRead);
                    //no.EMP_NOTIFY_ID = string.Join(";", lstEmpId);
                    no.CREATED_DATE = DateTime.Now;

                    // sau khi lưu gửi thông báo
                    var Title = "";
                    var Body = "";
                    if (status == OtherConfig.STATUS_APPROVE)
                    {
                        no.ACTION = Consts.ACTION_APPROVE;
                        if (type == Consts.REGISTER_OFF)
                        {
                            Title = app.APPROVE_NAME + " đã phê duyệt";
                            no.TYPE = 1;
                            if (r.DATE_START != r.DATE_END)
                            {
                                Body = "Đăng ký nghỉ ngày " + r.DATE_START.ToString("dd/MM/yyyy") + " - " + r.DATE_END.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                Body = "Đăng ký nghỉ ngày " + r.DATE_START.ToString("dd/MM/yyyy");
                            }
                        }
                        else if (type == Consts.REGISTER_LATE)
                        {
                            no.TYPE = 2;
                            Title = app.APPROVE_NAME + " đã phê duyệt";
                            if (r.DATE_START != r.DATE_END)
                            {
                                Body = "Đi muộn về sớm " + r.DATE_START.ToString("dd/MM/yyyy") + " - " + r.DATE_END.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                Body = "Đi muộn về sớm ngày " + r.DATE_START.ToString("dd/MM/yyyy");
                            }
                        }
                        else
                        {
                            no.TYPE = 3;
                            Title = app.APPROVE_NAME + " đã phê duyệt";
                            if (r.DATE_START != r.DATE_END)
                            {
                                Body = "Đăng ký OT ngày " + r.DATE_START.ToString("dd/MM/yyyy") + " - " + r.DATE_END.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                Body = "Đăng ký OT ngày " + r.DATE_START.ToString("dd/MM/yyyy");
                            }

                        }
                    }
                    else
                    {
                        no.ACTION = Consts.ACTION_DENIED;
                        if (type == Consts.REGISTER_OFF)
                        {
                            no.TYPE = 1;
                            Title = app.APPROVE_NAME + " đã từ chối";
                            if (r.DATE_START != r.DATE_END)
                            {
                                Body = "Đăng ký nghỉ ngày " + r.DATE_START.ToString("dd/MM/yyyy") + " - " + r.DATE_END.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                Body = "Đăng ký nghỉ ngày " + r.DATE_START.ToString("dd/MM/yyyy");
                            }
                        }
                        else if (type == Consts.REGISTER_LATE)
                        {
                            no.TYPE = 2;
                            Title = app.APPROVE_NAME + " đã từ chối";
                            if (r.DATE_START != r.DATE_END)
                            {
                                Body = "Đi muộn về sớm " + r.DATE_START.ToString("dd/MM/yyyy") + " - " + r.DATE_END.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                Body = "Đi muộn về sớm ngày " + r.DATE_START.ToString("dd/MM/yyyy");
                            }
                        }
                        else
                        {
                            no.TYPE = 3;
                            Title = app.APPROVE_NAME + " đã từ chối";
                            if (r.DATE_START != r.DATE_END)
                            {
                                Body = "Đăng ký OT ngày " + r.DATE_START.ToString("dd/MM/yyyy") + " - " + r.DATE_END.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                Body = "Đăng ký OT ngày " + r.DATE_START.ToString("dd/MM/yyyy");
                            }
                        }
                    }

                    await _appContext.Notifications.AddAsync(no);
                    await SendNotification(new NotificationModel
                    {
                        Devices = lstDevice,
                        Title = Title,
                        Body = Body

                    });


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
        /// Danh sách chờ phê duyệt
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<ResultWithError> PortalWaitList(int type)
        {
            try
            {
                if (type == Consts.REGISTER_OFF) // Đăng ký nghỉ
                {
                    var r = await (from p in _appContext.RegisterOffs
                                   join t in _appContext.TimeTypes on p.TIMETYPE_ID equals t.ID
                                   join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                                   join o in _appContext.Organizations on e.ORG_ID equals o.ID
                                   join o1 in _appContext.Organizations on o.PARENT_ID equals o1.ID
                                   where ((o.MNG_ID == _appContext.EmpId && p.EMPLOYEE_ID != _appContext.EmpId) || (o.MNG_ID == p.EMPLOYEE_ID && o1.MNG_ID == _appContext.EmpId)) && p.STATUS_ID == OtherConfig.STATUS_WAITING && p.TYPE_ID == type
                                   orderby p.CREATED_DATE descending
                                   select new
                                   {
                                       Id = p.ID,
                                       dateStart = p.DATE_START,
                                       dateEnd = p.DATE_END,
                                       workingDay = p.CREATED_DATE, // phiên bản cũ
                                       TypeName = "[" + t.CODE + "] " + t.NAME,
                                       FullName = e.Profile.FULL_NAME,
                                       Avatar = e.Profile.AVATAR
                                   }).ToListAsync();
                    return new ResultWithError(r);
                }
                else // Danh sách đăng ký đi muộn về sớm, OT
                {
                    var r = await (from p in _appContext.RegisterOffs
                                   join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                                   join o in _appContext.Organizations on e.ORG_ID equals o.ID
                                   join o1 in _appContext.Organizations on o.PARENT_ID equals o1.ID
                                   where ((o.MNG_ID == _appContext.EmpId && p.EMPLOYEE_ID != _appContext.EmpId) || (o.MNG_ID == p.EMPLOYEE_ID && o1.MNG_ID == _appContext.EmpId)) && p.STATUS_ID == OtherConfig.STATUS_WAITING && p.TYPE_ID == type
                                   orderby p.CREATED_DATE descending
                                   select new
                                   {
                                       Id = p.ID,
                                       dateStart = p.DATE_START,
                                       dateEnd = p.DATE_END,
                                       timeStart = p.TIME_START,
                                       timeEnd = p.TIME_END,
                                       timeLate = p.TIME_LATE,
                                       timeEarly = p.TIME_EARLY,
                                       FullName = e.Profile.FULL_NAME,
                                       Avatar = e.Profile.AVATAR
                                   }).ToListAsync();
                    return new ResultWithError(r);
                }
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        ///  Lịch sử đăng ký by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">Loại đăng ký</param>
        /// <returns></returns>       
        public async Task<ResultWithError> PortalHistoryBy(int id, int type)
        {
            try
            {
                if (type == Consts.REGISTER_OFF)
                {
                    var r = await (from p in _appContext.RegisterOffs
                                   join t in _appContext.TimeTypes on p.TIMETYPE_ID equals t.ID
                                   where p.ID == id
                                   select new
                                   {
                                       FromDate = p.DATE_START,
                                       ToDate = p.DATE_END,
                                       Name = t.NAME,
                                       Note = p.NOTE,
                                       Status = p.STATUS_ID,
                                       AppDate = p.APPROVE_DAY,
                                       AppName = p.APPROVE_NAME,
                                       AppPos = p.APPROVE_POS,
                                       AppNote = p.APPROVE_NOTE
                                   }).FirstOrDefaultAsync();
                    return new ResultWithError(r);
                }
                else
                {
                    var r = await (from p in _appContext.RegisterOffs
                                   where p.ID == id
                                   select new
                                   {
                                       WorkingDay = p.WORKING_DAY,
                                       FromDate = p.DATE_START, // chọn 1 trong 2
                                       ToDate = p.DATE_END, // chọn 1 trong 2
                                       TimeStart = p.TIME_START,
                                       TimeEnd = p.TIME_END,
                                       TimeEarly = p.TIME_EARLY,
                                       TimeLate = p.TIME_LATE,
                                       Note = p.NOTE,
                                       Status = p.STATUS_ID,
                                       AppDate = p.APPROVE_DAY,
                                       AppName = p.APPROVE_NAME,
                                       AppPos = p.APPROVE_POS,
                                       AppNote = p.APPROVE_NOTE
                                   }).FirstOrDefaultAsync();
                    return new ResultWithError(r);
                }

            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// Lịch sử phê duyệt GetBy ID Version 1 --> Update xong bỏ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> PortalAppBy(int id, int type)
        {
            try
            {
                if (type == Consts.REGISTER_OFF)
                {
                    var r = await (from a in _appContext.Approveds
                                   join p in _appContext.RegisterOffs on a.REGISTER_ID equals p.ID
                                   join t in _appContext.TimeTypes on p.TIMETYPE_ID equals t.ID
                                   where p.ID == id
                                   orderby a.STATUS_ID ascending, a.CREATED_DATE descending
                                   select new
                                   {
                                       Id = p.ID,
                                       FromDate = p.DATE_START,
                                       ToDate = p.DATE_END,
                                       Name = t.NAME,
                                       Note = p.NOTE,
                                       Status = p.STATUS_ID,
                                       AppDate = a.APPROVE_DAY,
                                       AppName = a.APPROVE_NAME,
                                       AppPos = a.APPROVE_POS,
                                       AppNote = a.APPROVE_NOTE,
                                   }).FirstOrDefaultAsync();
                    return new ResultWithError(r);
                }
                else
                {
                    var r = await (from a in _appContext.Approveds
                                   join p in _appContext.RegisterOffs on a.REGISTER_ID equals p.ID
                                   where p.ID == id
                                   orderby a.STATUS_ID ascending, a.CREATED_DATE descending
                                   select new
                                   {
                                       Id = p.ID,
                                       workingDay = p.WORKING_DAY, // old
                                       DateStart = p.DATE_END, // chọn 1 trong 2
                                       DateEnd = p.DATE_END, // chọn 1 trong 2
                                       FromDate = p.DATE_START, // chọn 1 trong 2
                                       ToDate = p.DATE_END, // chọn 1 trong 2
                                       TimeStart = p.TIME_START,
                                       TimeEnd = p.TIME_END,
                                       TimeEarly = p.TIME_EARLY,
                                       TimeLate = p.TIME_LATE,
                                       Name = "a",
                                       Note = p.NOTE,
                                       Status = p.STATUS_ID,
                                       AppDate = a.APPROVE_DAY,
                                       AppName = a.APPROVE_NAME,
                                       AppPos = a.APPROVE_POS,
                                       AppNote = a.APPROVE_NOTE
                                   }).FirstOrDefaultAsync();
                    return new ResultWithError(r);
                }
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// GetBy ID for Approve
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> PortalAppGetBy(int id, int type)
        {
            try
            {
                if (type == Consts.REGISTER_OFF)
                {
                    var r = await (from p in _appContext.RegisterOffs
                                   join t in _appContext.TimeTypes on p.TIMETYPE_ID equals t.ID
                                   where p.ID == id
                                   select new
                                   {
                                       Id = p.ID,
                                       FromDate = p.DATE_START,
                                       ToDate = p.DATE_END,
                                       Name = t.NAME,
                                       Note = p.NOTE,
                                       Status = p.STATUS_ID
                                   }).FirstOrDefaultAsync();
                    if (r.Status == OtherConfig.STATUS_APPROVE)
                    {
                        return new ResultWithError(Message.RECORD_IS_APPROVED);
                    }
                    return new ResultWithError(r);
                }
                else
                {
                    var r = await (from p in _appContext.RegisterOffs
                                   where p.ID == id
                                   select new
                                   {

                                       Id = p.ID,
                                       FromDate = p.DATE_START,
                                       ToDate = p.DATE_END,
                                       TimeStart = p.TIME_START,
                                       TimeEnd = p.TIME_END,
                                       TimeEarly = p.TIME_EARLY,
                                       TimeLate = p.TIME_LATE,
                                       Note = p.NOTE,
                                       Status = p.STATUS_ID
                                   }).FirstOrDefaultAsync();
                    if (r.Status == OtherConfig.STATUS_APPROVE)
                    {
                        return new ResultWithError(Message.RECORD_IS_APPROVED);
                    }
                    return new ResultWithError(r);
                }
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách lịch sử đăng ký 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<HistoryRegDTO>> GetRegister(Pagings param)
        {
            try
            {
                var queryable = (from p in _appContext.RegisterOffs
                                 where p.EMPLOYEE_ID == _appContext.EmpId
                                 orderby p.STATUS_ID, p.CREATED_DATE descending
                                 select new HistoryRegDTO
                                 {
                                     Id = p.ID,
                                     Date = p.CREATED_DATE, // build xong IOS bỏ
                                     DateStart = p.DATE_START,
                                     DateEnd = p.DATE_END,
                                     Status = p.STATUS_ID,
                                     TypeId = p.TYPE_ID
                                 });

                return await PagingList(queryable, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Danh sách Lịch sử phê duyệt --> bỏ
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<AcceptDTO>> GetAccept(Pagings param)
        {
            try
            {
                var queryable = (from p in _appContext.Approveds
                                 join e in _appContext.Employees on p.EMP_RES_ID equals e.ID
                                 where p.APPROVE_ID == _appContext.EmpId
                                 orderby p.STATUS_ID, p.CREATED_DATE descending
                                 select new AcceptDTO
                                 {
                                     Id = p.ID,
                                     Name = e.Profile.FULL_NAME,
                                     Avatar = e.Profile.AVATAR,
                                     Date = p.CREATED_DATE,
                                     Status = p.STATUS_ID,
                                     TypeId = p.TYPE_ID
                                 }).OrderByDescending(c => c.Date);
                return await PagingList(queryable, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Danh sách chờ phê duyệt
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<AcceptDTO>> GetAppHistotyList(Pagings param)
        {
            try
            {
                var queryable = (from p in _appContext.Approveds
                                 join e in _appContext.Employees on p.EMP_RES_ID equals e.ID
                                 join o in _appContext.Organizations on e.ORG_ID equals o.ID
                                 where p.APPROVE_ID == _appContext.EmpId
                                 orderby p.CREATED_DATE descending
                                 select new AcceptDTO
                                 {
                                     Id = p.ID,
                                     Name = e.Profile.FULL_NAME,
                                     //Avatar = e.AVATAR,
                                     OrgName = o.NAME,
                                     Date = p.CREATED_DATE,
                                     Status = p.STATUS_ID,
                                     TypeId = p.TYPE_ID
                                 }).OrderByDescending(c => c.Date).OrderBy(c => c.Status);
                return await PagingList(queryable, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lịch sử phê duyệt GetBy ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> PortalAppHistoryBy(int id)
        {
            try
            {
                var r = await (from a in _appContext.Approveds
                               join p in _appContext.RegisterOffs on a.REGISTER_ID equals p.ID
                               join t in _appContext.TimeTypes on p.TIMETYPE_ID equals t.ID into tmp1
                               from t1 in tmp1.DefaultIfEmpty()
                               where a.ID == id
                               orderby a.CREATED_DATE descending
                               select new
                               {
                                   workingDay = p.DATE_START,
                                   FromDate = p.DATE_START,
                                   ToDate = p.DATE_END,
                                   Name = t1.NAME,
                                   Note = p.NOTE,
                                   TimeStart = p.TIME_START,
                                   TimeEnd = p.TIME_END,
                                   TimeEarly = p.TIME_EARLY,
                                   TimeLate = p.TIME_LATE,
                                   Type = p.TYPE_ID,
                                   Status = a.STATUS_ID,
                                   AppDate = a.APPROVE_DAY,
                                   AppName = a.APPROVE_NAME,
                                   AppPos = a.APPROVE_POS,
                                   AppNote = a.APPROVE_NOTE,
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        private async Task<ResultWithError> Register(RegisterOffInputDTO param, int type, long? empId)
        {
            try
            {
                var data = new AT_REGISTER_OFF();
                if (type == Consts.REGISTER_OFF)
                {


                    var chk1 = await QueryData.ExecuteList("PKG_TIMESHEET.CHECK_DAYOFF",
                             new
                             {
                                 P_EMP_ID = empId,
                                 P_TIME_TYPE = param.TimeTypeId,
                                 P_FROM = param.DateStart,
                                 P_TODATE = param.DateEnd,
                                 P_CUR = QueryData.OUT_CURSOR
                             }, true);
                    var chkDay = chk1.Select(c => (int?)((dynamic)c).DAY_OF).FirstOrDefault();
                    if (chkDay == 0)
                    {
                        return new ResultWithError("DAYOFF_NOT_ENOUGH");
                    }

                    data = Map(param, new AT_REGISTER_OFF());
                }
                else if (type == Consts.REGISTER_OT)
                {
                    DateTime a = DateTime.ParseExact(param.WorkingDay, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    data.WORKING_DAY = a;
                    data.DATE_START = a;
                    data.DATE_END = a;
                    data.TIME_START = param.TimeStart;
                    data.TIME_END = param.TimeEnd;
                    data.NOTE = param.Note;
                }
                else
                {
                    data = Map(param, new AT_REGISTER_OFF());
                }
                var dataChk = await QueryData.ExecuteList("PKG_TIMESHEET.CHECK_REGISTER",
                         new
                         {

                             P_EMP_ID = empId,
                             P_TYPE = type,
                             P_DATE_START = param.DateStart,
                             P_DATE_END = param.DateEnd,
                             P_TIME_START = param.TimeStart,
                             P_TIME_END = param.TimeEnd,
                             P_TIME_EARLY = param.TimeEarly,
                             P_TIME_LATE = param.TimeLate,
                             P_TIME_TYPE = param.TimeTypeId,
                             P_CUR = QueryData.OUT_CURSOR
                         }, false);

                var chk = dataChk.Select(c => (int?)((dynamic)c).TOTAL).FirstOrDefault();
                if (chk > 0)
                {
                    return new ResultWithError("DATA_EXISTS");
                }

                data.STATUS_ID = OtherConfig.STATUS_WAITING;
                data.TYPE_ID = type;
                data.EMPLOYEE_ID = empId.Value;
                var result = await _appContext.RegisterOffs.AddAsync(data);
                // insert record to Table Approved
                var app = new AT_APPROVED();
                app.REGISTER_ID = data.ID;
                app.EMP_RES_ID = data.EMPLOYEE_ID;
                app.STATUS_ID = OtherConfig.STATUS_WAITING;
                app.APPROVE_DAY = DateTime.Now;
                app.APPROVE_NOTE = param.Note;
                app.IS_READ = 0;
                app.TYPE_ID = type;
                await _appContext.Approveds.AddAsync(app);
                await _appContext.SaveChangesAsync();
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
                    if (Mng.mngId == app.EMP_RES_ID) // người phê duyệt là người đăng ký Up lên 1 lvl
                    {
                        var lstEmpId = await _appContext.Organizations.Where(x => x.ID == Mng.orgParentId).Select(v => v.MNG_ID).FirstOrDefaultAsync();
                        if (lstEmpId != null)
                        {
                            var lstToken = await QueryData.ExecuteList("connectionString",
                        "PKG_NOTIFI.GET_FMC_TOKEN", new
                        {

                            P_EMP_ID = lstEmpId.ToString(),
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
                                no.ACTION = Consts.ACTION_CREATE;
                                no.NOTIFI_ID = data.ID;
                                no.EMP_CREATE_ID = app.EMP_RES_ID;
                                no.FMC_TOKEN = string.Join(";", lstDevice);
                                //no.IS_READ = string.Join(";", IsRead);
                                //no.EMP_NOTIFY_ID = lstEmpId.ToString();
                                
                                no.CREATED_DATE = DateTime.Now;
                                // sau khi lưu gửi thông báo
                                var FullName = _appContext.Employees.Where(x => x.ID == data.EMPLOYEE_ID).Select(e => e.Profile.FULL_NAME).FirstOrDefault();
                                var Title = "";
                                var Body = "";
                                if (type == Consts.REGISTER_OFF)
                                {
                                    no.TYPE = 1;
                                    Title = FullName + " đăng ký nghỉ";
                                    if (param.DateStart.ToString("dd/MM/yyyy") == param.DateEnd.ToString("dd/MM/yyyy"))
                                    {
                                        Body = "Từ ngày " + param.DateStart.ToString("dd/MM/yyyy") + " đến ngày " + param.DateEnd.ToString("dd/MM/yyyy");
                                    }
                                    else
                                    {
                                        Body = "Ngày " + param.DateStart.ToString("dd/MM/yyyy");
                                    }
                                }
                                else if (type == Consts.REGISTER_LATE)
                                {
                                    no.TYPE = 2;
                                    Title = FullName + " đăng ký đi muộn về sớm";
                                    if (param.DateStart.ToString("dd/MM/yyyy") == param.DateEnd.ToString("dd/MM/yyyy"))
                                    {
                                        Body = "Từ ngày " + param.DateStart.ToString("dd/MM/yyyy") + " đến ngày " + param.DateEnd.ToString("dd/MM/yyyy");
                                    }
                                    else
                                    {
                                        Body = "Ngày " + param.DateStart.ToString("dd/MM/yyyy");
                                    }
                                }
                                else
                                {
                                    no.TYPE = 3;
                                    Title = FullName + " đăng ký OT ngày " + param.WorkingDay;
                                    if (param.DateStart.ToString("dd/MM/yyyy") == param.DateEnd.ToString("dd/MM/yyyy"))
                                    {
                                        Body = "Từ ngày " + param.DateStart.ToString("dd/MM/yyyy") + " đến ngày " + param.DateEnd.ToString("dd/MM/yyyy");
                                    }
                                    else
                                    {
                                        Body = "Ngày " + param.WorkingDay;
                                    }
                                }

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
                    else
                    {

                        var lstToken = await QueryData.ExecuteList("connectionString",
                    "PKG_NOTIFI.GET_FMC_TOKEN", new
                    {

                        P_EMP_ID = Mng.mngId.ToString(),
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
                            if (type == Consts.REGISTER_OFF)
                            {
                                no.TYPE = 1;
                                Title = FullName + " đăng ký nghỉ";
                                if (param.DateStart.ToString("dd/MM/yyyy") != param.DateEnd.ToString("dd/MM/yyyy"))
                                {
                                    Body = "Từ " + param.DateStart.ToString("dd/MM/yyyy") + " - " + param.DateEnd.ToString("dd/MM/yyyy");
                                }
                                else
                                {
                                    Body = "Ngày " + param.DateStart.ToString("dd/MM/yyyy");
                                }
                            }
                            else if (type == Consts.REGISTER_LATE)
                            {
                                no.TYPE = 2;
                                Title = FullName + " đăng ký đi muộn về sớm";
                                if (param.DateStart.ToString("dd/MM/yyyy") != param.DateEnd.ToString("dd/MM/yyyy"))
                                {
                                    Body = "Từ " + param.DateStart.ToString("dd/MM/yyyy") + " - " + param.DateEnd.ToString("dd/MM/yyyy");
                                }
                                else
                                {
                                    Body = "Ngày " + param.DateStart.ToString("dd/MM/yyyy");
                                }
                            }
                            else
                            {
                                no.TYPE = 3;
                                Title = FullName + " đăng ký OT ngày " + param.WorkingDay;
                                if (param.DateStart.ToString("dd/MM/yyyy") != param.DateEnd.ToString("dd/MM/yyyy"))
                                {
                                    Body = "Từ " + param.DateStart.ToString("dd/MM/yyyy") + " - " + param.DateEnd.ToString("dd/MM/yyyy");
                                }
                                else
                                {
                                    Body = "Ngày " + param.WorkingDay;
                                }
                            }

                            await _appContext.Notifications.AddAsync(no);
                            await _appContext.SaveChangesAsync();
                            await SendNotification(new NotificationModel
                            {
                                Devices = lstDevice,
                                Title = Title,
                                Body = Body
                            });
                        }

                    }
                }
                return new ResultWithError(200);

            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// Version 2
        /// <summary>
        /// Lấy danh sách lịch sử đăng ký 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<HistoryRegView>> PortalListRegister(DateSearchParam param)
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
                return await QueryData.ExecuteStorePage<HistoryRegView>("PKG_PORTAL.HISTORY_REGISTER",
                    new
                    {

                        P_EMP_ID = _appContext.EmpId,
                        P_DATE_START = param.DateStart,
                        P_DATE_END = param.DateEnd,
                        p_page_no = param.PageNo,
                        p_page_size = param.PageSize,
                        P_CUR = QueryData.OUT_CURSOR,
                        P_CUR_PAGE = QueryData.OUT_CURSOR
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        ///  Lịch sử đăng ký by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">Loại đăng ký</param>
        /// <returns></returns>       
        public async Task<ResultWithError> PortalHistoryBy(int id)
        {
            try
            {
                var r = await (from p in _appContext.RegisterOffs
                               join t in _appContext.TimeTypes on p.TIMETYPE_ID equals t.ID into tmp1
                               from t1 in tmp1.DefaultIfEmpty()
                               where p.ID == id && p.EMPLOYEE_ID == _appContext.EmpId
                               select new
                               {
                                   FromDate = p.DATE_START,
                                   ToDate = p.DATE_END,
                                   Name = t1.NAME,
                                   TimeStart = p.TIME_START,
                                   TimeEnd = p.TIME_END,
                                   TimeEarly = p.TIME_EARLY,
                                   TimeLate = p.TIME_LATE,
                                   Note = p.NOTE,
                                   Status = p.STATUS_ID,
                                   AppDate = p.APPROVE_DAY,
                                   AppName = p.APPROVE_NAME,
                                   AppPos = p.APPROVE_POS,
                                   AppNote = p.APPROVE_NOTE
                               }).FirstOrDefaultAsync();

                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        ///  Hủy đăng ký
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">Loại đăng ký</param>
        /// <returns></returns>       
        public async Task<ResultWithError> PortalCancel(long id)
        {
            try
            {
                var r = await _appContext.RegisterOffs.Where(x => x.ID == id).FirstOrDefaultAsync();
                if (r.STATUS_ID == OtherConfig.STATUS_APPROVE)
                {
                    return new ResultWithError("RECORD_IS_APPROVED");
                }
                _appContext.RegisterOffs.Remove(r);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> test(List<string> code)
        {
            try
            {
                await SendNotification(new NotificationModel
                {
                    Devices = code,
                    Title = "test",
                    Body = "test"
                });
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// PORTAL GET OT INFOR THIS MONTH
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> PortalOTGet()
        {
            try
            {
                var r = await QueryData.ExecuteStorePortal<decimal>("PKG_PORTAL.CURRENT_OT",
                new
                {
                    P_CUR = QueryData.OUT_CURSOR,
                });
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// PORTAL GET OT INFOR THIS MONTH
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> PortalDMVSGet()
        {
            try
            {
                var r = await QueryData.ExecuteStorePortal<PortalDMVSParam>("PKG_PORTAL.CURRENT_DMVS",
                new
                {
                    P_CUR = QueryData.OUT_CURSOR,
                });
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

    }
}
