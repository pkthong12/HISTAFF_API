using Common.Paging;
using AttendanceDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace AttendanceDAL.Repositories
{
    public class TimeLateEarlyRepository : RepositoryBase<AT_TIME_LATE_EARLY>, ITimeLateEarlyRepository
    {
        private AttendanceDbContext _appContext => (AttendanceDbContext)_context;
        public TimeLateEarlyRepository(AttendanceDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<RegisterOffDTO>> GetAll(RegisterOffDTO param)
        {
            var queryable = from p in _appContext.RegisterOffs
                            join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                            join o in _appContext.Organizations on e.ORG_ID equals o.ID
                            join po in _appContext.Positions on e.POSITION_ID equals po.ID
                            
                            orderby p.ID descending
                            select new RegisterOffDTO
                            {
                                Id = p.ID,
                                DateStart = p.DATE_START,
                                DateEnd = p.DATE_END,
                                TimeLate = p.TIME_LATE,
                                TimeEarly = p.TIME_EARLY,
                                EmployeeId = p.EMPLOYEE_ID,
                                EmployeeName = e.Profile.FULL_NAME,
                                EmployeeCode = e.CODE,
                                OrgId = e.ORG_ID,
                                OrgName = o.NAME,
                                PositionName = po.NAME,
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

            if (!string.IsNullOrWhiteSpace(param.Note))
            {
                queryable = queryable.Where(p => p.Note.ToUpper().Contains(param.Note.ToUpper()));
            }
            return await PagingList(queryable, param);
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
                               join e in _appContext.Employees on p.EMPLOYEE_ID equals e.ID
                               join o in _appContext.Organizations on e.ORG_ID equals o.ID
                               join po in _appContext.Positions on e.POSITION_ID equals po.ID
                               where p.ID == id
                               select new
                               {
                                   Id = p.ID,
                                   DateStart = p.DATE_START,
                                   DateEnd = p.DATE_END,
                                   TimeLate = p.TIME_LATE,
                                   TimeEarly = p.TIME_EARLY,
                                   EmployeeId = p.EMPLOYEE_ID,
                                   EmployeeName = e.Profile.FULL_NAME,
                                   EmployeeCode = e.CODE,
                                   OrgName = o.NAME,
                                   PositionName = po.NAME,
                                   Note = p.NOTE,
                                   IsActive = p.IS_ACTIVE,
                                   CreateBy = p.CREATED_BY,
                                   UpdatedBy = p.UPDATED_BY,
                                   CreateDate = p.CREATED_DATE,
                                   UpdatedDate = p.UPDATED_DATE
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Create Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateAsync(TimeLateEarlyInputDTO param)
        {

            try
            {
                var data = Map(param, new AT_REGISTER_OFF());
                data.STATUS_ID = OtherConfig.STATUS_WAITING;
                data.IS_ACTIVE = true;
                var result = await _appContext.RegisterOffs.AddAsync(data);
                // insert record to Table Approved
                var app = new AT_APPROVED();
                app.REGISTER_ID = data.ID;
                app.EMP_RES_ID = data.EMPLOYEE_ID;
                app.STATUS_ID = OtherConfig.STATUS_WAITING;
                app.APPROVE_DAY = DateTime.Now;
                app.APPROVE_NOTE = param.Note;
                app.IS_READ = 0;
                app.TYPE_ID = Consts.REGISTER_LATE;
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
                                no.TYPE = Consts.REGISTER_LATE;
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

                                Title = FullName + " đăng ký đi muộn về sớm";
                                Body = FullName + " từ ngày " + param.DateStart.ToString("MM/dd/yyyy") + " đến ngày " + param.DateEnd.ToString("MM/dd/yyyy");

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
                            no.TYPE = Consts.REGISTER_LATE;
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
                            Title = FullName + " đăng ký đi muộn về sớm";
                            Body = FullName + " từ ngày " + param.DateStart.ToString("MM/dd/yyyy") + " đến ngày " + param.DateEnd.ToString("MM/dd/yyyy");
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
                return new ResultWithError(param);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }

        }
        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(TimeLateEarlyInputDTO param)
        {
            try
            {
                // check code

                var r = _appContext.RegisterOffs.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
               

                var data = Map(param, r);
                data.STATUS_ID = OtherConfig.STATUS_WAITING;
                var result = _appContext.RegisterOffs.Update(data);
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
