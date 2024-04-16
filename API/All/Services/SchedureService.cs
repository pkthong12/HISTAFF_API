using API.All.DbContexts;
using Common.DataAccess;
using Common.Extensions;
using Common.Interfaces;
using RegisterServicesWithReflection.Services.Base;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace API.All.Services
{
    [ScopedRegistration]
    public class SchedureService: ISchedureService
    {

        private FullDbContext _fullDbContext;
        protected AbsQueryDataTemplate QueryData;


        public SchedureService(FullDbContext fullDbContext)
        {
            _fullDbContext = fullDbContext;
            QueryData = new SqlQueryDataTemplate(fullDbContext);
        }

        public async Task<object> AddTestingRecordAt5AmEveryMorning()
        {
            var newRecord = new DEMO_HANGFIRE_RECORD()
            {
                TEXT = "This is auto added by Hangfire Recuring Job at 5AM every morning",
                CREATED_DATE = DateTime.Now,
            };

            await _fullDbContext.DemoHangfireRecords.AddAsync(newRecord);
            await _fullDbContext.SaveChangesAsync();

            return newRecord;

        }

        public async Task<object> TestNhanh()
        {
            Console.WriteLine("OK. Da thuc thi Nhiem vu");
            return await Task.Run(() => true);
        }

        public string DeveloperRooting()
        {
            try
            {
                /* Đã golive. Xin thông cảm. Không thay đổi dưới mọi hình thức */
                var developers = new List<string> {
                "ROOT",
                "TANNV",
                };
                /* Thông cảm. Cấm thay đổi dưới mọi hình thức */

                var users = _fullDbContext.SysUsers.ToList();
                users.ForEach(user =>
                {
                    if (developers.Contains(user.USERNAME!.ToUpper()))
                    {
                        user.IS_ROOT = true;
                    }
                    else
                    {
                        user.IS_ROOT = false;
                    }
                });
                _fullDbContext.SysUsers.UpdateRange(users);
                _fullDbContext.SaveChanges();
                return "DEVELOPER_ROOTING_SUCCESS";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CalculateTimesheetDailyByDate()
        {
            var datetimeNow = DateTime.Now;
            var currentPeriod = (from e in _fullDbContext.AtSalaryPeriods
                                 where e.START_DATE.Date <= datetimeNow.Date && e.END_DATE.Date >= datetimeNow.Date
                                 select new
                                 {
                                     Id = e.ID,
                                     StartDate = e.START_DATE,
                                     EndDate = e.END_DATE,
                                 }).FirstOrDefault();
            if (currentPeriod != null)
            {
                var x = new
                {
                    P_USER_ID = "46cc736a-ac6c-43fd-8ecc-2d84d9f9fc76",
                    P_PERIOD_ID = currentPeriod.Id,
                    P_ORG_ID = 1,
                    P_ISDISSOLVE = -1,
                    P_EMPLOYEE_ID = 0
                };
                var data = QueryData.ExecuteStoreToTable(Procedures.PKG_ATTENDANCE_CALCULATE_CAL_TIME_TIMESHEET_MACHINE,
                    x, false);
            }
            return "DEVELOPER_ROOTING_SUCCESS";
        }
        public void ChangePositionPoliticalByDate()
        {
            var getListConccurent = (from c in _fullDbContext.HuConcurrentlys
                                     join  ee in _fullDbContext.HuEmployees on c.EMPLOYEE_ID equals ee.ID
                                     join cv in _fullDbContext.HuEmployeeCvs on ee.PROFILE_ID equals cv.ID
                                     join sys in _fullDbContext.SysOtherLists on c.POSITION_POLITICAL_ID equals sys.ID
                                     select new
                                     {
                                         concurrently = c,
                                         employee = ee,
                                         employeeCv = cv,
                                         sysOtherList = sys
                                     }).ToList();
            getListConccurent.ForEach(item =>
            {


                if (item.concurrently.EFFECTIVE_DATE >= DateTime.Now)
                {
                    if (item.sysOtherList.CODE == "00290")
                    {
                        item.employeeCv.IS_UNIONIST = true;
                    }
                    if (item.sysOtherList.CODE == "00291")
                    {
                        item.employeeCv.IS_JOIN_YOUTH_GROUP = true;
                    }
                    if (item.sysOtherList.CODE == "00292")
                    {
                        item.employeeCv.IS_MEMBER = true;
                    }
                }
                if (item.concurrently.EXPIRATION_DATE != null && item.concurrently.EXPIRATION_DATE <= DateTime.Now)
                {
                    if (item.sysOtherList.CODE == "00290")
                    {
                        item.employeeCv.IS_UNIONIST = false;
                    }
                    if (item.sysOtherList.CODE == "00291")
                    {
                        item.employeeCv.IS_JOIN_YOUTH_GROUP = false;
                    }
                    if (item.sysOtherList.CODE == "00292")
                    {
                        item.employeeCv.IS_MEMBER = false;
                    }
                }
            });
            _fullDbContext.SaveChanges();
        }

        public void SendEmailPortal()
        {
            var list = _fullDbContext.SeMails.AsNoTracking().Where(x => x.ACTFLG == "I").ToList();
            var config =  _fullDbContext.SeConfigs.FirstOrDefault();

            foreach (var item in list)
            {
                MailMessage msg = new();
                msg.To.Add(item.MAIL_TO!);
                msg.From = new MailAddress(item.MAIL_FROM!);
                msg.Subject = item.SUBJECT;
                msg.Body = item.MAIL_CONTENT;
                msg.IsBodyHtml = true;
                msg.BodyEncoding = Encoding.UTF8;
                using SmtpClient smtp = new();
                var credential = new NetworkCredential
                {
                    UserName = config!.ACCOUNT,
                    Password = config!.PASSWORD
                };
                smtp.Credentials = credential;
                smtp.Host = config.NAME!;
                smtp.Port = config.MODULE!.Value;
                smtp.EnableSsl = config.IS_AUTH_SSL!.Value;
                item.ACTFLG = "S";
                _fullDbContext.Update(item);
                _fullDbContext.SaveChanges();
                smtp.Send(msg);
            }
        }
    }


}
