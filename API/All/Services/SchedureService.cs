using API.All.DbContexts;
using RegisterServicesWithReflection.Services.Base;

namespace API.All.Services
{
    [ScopedRegistration]
    public class SchedureService : ISchedureService
    {

        private FullDbContext _fullDbContext;

        public SchedureService(FullDbContext fullDbContext)
        {
            _fullDbContext = fullDbContext;
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

        public void ChangeIsActivePosition()
        {
            var getListPosition = _fullDbContext.HuPositions.Where(x => x.MASTER != null || x.MASTER != 0).ToList();
            getListPosition.ForEach(item =>
            {
                item.IS_ACTIVE = true;
            });
            _fullDbContext.HuPositions.UpdateRange(getListPosition);
            _fullDbContext.SaveChanges();
        }

        public async Task<object> TheSystemChecksTheTerminate()
        {
            var approveId = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "DD")!.ID;

            // The system retrieves all records
            // with the condition "EFFECT_DATE" = "DateTime.Now.Date"
            var getAll = from item in _fullDbContext.HuTerminates
                         where item.EFFECT_DATE!.Value.Date == DateTime.Now.Date
                               && item.STATUS_ID == approveId
                         select item;

            var hasRetiredId = _fullDbContext.SysOtherLists.FirstOrDefault(x => x.CODE == "ESQ")!.ID;

            foreach (var item in getAll)
            {
                // get employee
                var employee = _fullDbContext.HuEmployees.FirstOrDefault(x => x.ID == item.EMPLOYEE_ID);

                employee!.WORK_STATUS_ID = hasRetiredId;
                employee.STATUS_DETAIL_ID = item.TYPE_ID;
                
                _fullDbContext.SaveChanges();
            }

            return await Task.Run(() => true);
        }
    }
}
