using API.All.DbContexts;
using ProfileDAL.Repositories;

namespace AttendanceDAL.Repositories
{
    public interface IAttendanceUnitOfWork : IDisposable
    {

        AttendanceDbContext DataContext { get; }

        // IOtherListRepository SysOtherLists { get; }  
        ITimeTypeRepository TimeTypeRepository { get; }
        ISalaryPeriodRepository SalaryPeriodRepository { get; }
        //ITimeSheetDailyRepository TimeSheetDailyRepository { get; }
        ITimeLateEarlyRepository TimeLateEarlyRepository { get; }
        IRegisterOffRepository RegisterOffRepository { get; }
        ITimeSheetMonthlyRepository TimeSheetMonthlyRepository { get; }
        IDayOffYearRepository DayOffYearRepository { get; }
        IOverTimeRepository OverTimeRepository { get; }
        IEntitlementEditRepository EntitlementEditRepository { get; }
        IReportRepository ReportRepository { get; }
        IConfigRepository ConfigRepository { get; }
        IEntitlementRepository EntitlementRepository { get; }
    }
}
