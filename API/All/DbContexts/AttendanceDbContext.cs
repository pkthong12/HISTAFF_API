using API.Entities;
using Microsoft.Extensions.Options;

namespace API.All.DbContexts
{
    public class AttendanceDbContext : DbContextBase
    {
        public AttendanceDbContext(IConfiguration config, DbContextOptions<AttendanceDbContext> options, IHttpContextAccessor accessor, IOptions<AppSettings> appSettings)
            : base(config, options, accessor, appSettings)
        {

        }

        // Sytem
        public DbSet<SYS_OTHER_LIST> OtherLists { get; set; }
        //

        public DbSet<AT_SYMBOL> Symbols { get; set; }
        public DbSet<AT_NOTIFICATION> Notifications { get; set; }
        public DbSet<AT_TIME_TYPE> TimeTypes { get; set; }
        public DbSet<AT_SHIFT> Shifts { get; set; }
        public DbSet<AT_HOLIDAY> Holidays { get; set; }
        public DbSet<AT_SALARY_PERIOD> SalaryPeriods { get; set; }
        public DbSet<AT_SALARY_PERIOD_DTL> SalaryPeriodDtls { get; set; }
        public DbSet<AT_TIMESHEET_DAILY> TimeSheetDailys { get; set; }
        public DbSet<AT_TIME_TIMESHEET_DAILY> TimeTimeSheetDailys { get; set; }
        public DbSet<AT_WORKSIGN> WorkSigns { get; set; }
        public DbSet<AT_TIMESHEET_MONTHLY> TimeSheetMonthlies { get; set; }
        public DbSet<AT_TIMESHEET_MONTHLY_DTL> TimeSheetMonthlyDtls { get; set; }
        public DbSet<AT_TIME_LATE_EARLY> TimeLateEarlys { get; set; }
        public DbSet<AT_REGISTER_OFF> RegisterOffs { get; set; }
        public DbSet<AT_APPROVED> Approveds { get; set; }
        public DbSet<AT_SWIPE_DATA> SwipeDatas { get; set; }
        public DbSet<AT_SWIPE_DATA_TMP> SwipeDataTmps { get; set; }
        public DbSet<AT_SWIPE_DATA_SYNC> SwipeDataSyncs { get; set; }
        public DbSet<AT_SWIPE_DATA_WRONG> SwipeDataWrongs { get; set; }
        public DbSet<AT_TIMESHEET_LOCK> TimeSheetLocks { get; set; }
        public DbSet<AT_TIMESHEET_FORMULA> TimeSheetFomulas { get; set; }
        public DbSet<AT_WORKSIGN_TMP> WorkSignTmps { get; set; }
        public DbSet<HU_EMPLOYEE> Employees { get; set; }
        public DbSet<HU_ORGANIZATION> Organizations { get; set; }
        public DbSet<HU_POSITION> Positions { get; set; }
        public DbSet<HU_JOB> Jobs { get; set; }
        public DbSet<AT_TIMESHEET_MACHINE> TimesheetMachines { get; set; }
        public DbSet<AT_TIMESHEET_MACHINE_EDIT> TimesheetMachineEdits { get; set; }
        public DbSet<AT_DAYOFFYEAR> DayOffYears { get; set; }
        public DbSet<AT_DAYOFFYEAR_CONFIG> DayOffYearConfigs { get; set; }
        public DbSet<AT_CONFIG> AttendanceConfigs { get; set; }
        public DbSet<AT_OVERTIME> OverTimes { get; set; }
        public DbSet<AT_OVERTIME_CONFIG> OverTimeConfigs { get; set; }
        public DbSet<SYS_SETTING_MAP> SettingMaps { get; set; }
        public DbSet<AT_WORKSIGN_DUTY> WorkSignDutys { get; set; }
        public DbSet<AT_ENTITLEMENT_EDIT> EntitlementEdits { get; set; }
        public DbSet<AT_ENTITLEMENT> Entitlements { get; set; }
        public DbSet<AT_PERIOD_STANDARD> PeriodStandards { get; set; }
        public DbSet<HU_WORKING> Workings { get; set; }
        public DbSet<AT_ORG_PERIOD> OrgPeriods { get; set; }

        protected override void ConfigureConventions(
            ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>()
                .HavePrecision(18, 9);
        }

        /*
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        */
    }
}
