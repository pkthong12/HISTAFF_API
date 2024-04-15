using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using API.Entities;

namespace API.All.DbContexts
{
    public class PayrollDbContext : DbContextBase
    {

        private readonly AppSettings _appSettings;
        public PayrollDbContext(IConfiguration config, DbContextOptions<PayrollDbContext> options, IHttpContextAccessor accessor, IOptions<AppSettings> appSettings)
            : base(config, options, accessor, appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public DbSet<HU_EMPLOYEE> Employees { get; set; }
        public DbSet<AT_SALARY_PERIOD> SalaryPeriods { get; set; }
        public DbSet<PA_ELEMENT_GROUP> ElementGroups { get; set; }
        public DbSet<PA_ELEMENT> SalaryElements { get; set; }
        public DbSet<HU_SALARY_TYPE> SalaryTypes { get; set; }
        public DbSet<PA_SALARY_STRUCTURE> SalaryStructures { get; set; }
        public DbSet<PA_SALARY_PAYCHECK> Paychecks { get; set; }
        public DbSet<SYS_TMP_SORT> SysTempSorts { get; set; }

        public DbSet<PA_FORMULA> Formulas { get; set; }
        public DbSet<PA_KPI_GROUP> KpiGroups { get; set; }
        public DbSet<PA_KPI_TARGET> KpiTargets { get; set; }
        public DbSet<PA_KPI_FORMULA> KpiFormulas { get; set; }
        public DbSet<PA_KPI_POSITION> KpiPositions { get; set; }
        public DbSet<PA_KPI_SALARY_DETAIL_TMP> KpiEmployeeTmps { get; set; }

        public DbSet<HU_POSITION> Positions { get; set; }
        public DbSet<PA_KPI_SALARY_DETAIL> KpiEmployees { get; set; }
        public DbSet<HU_ORGANIZATION> Organizations { get; set; }
        public DbSet<HU_WORKING> Workings { get; set; }
        public DbSet<PA_ADVANCE> Advances { get; set; }
        public DbSet<PA_ADVANCE_TMP> AdvanceTmps { get; set; }
        public DbSet<PA_CALCULATE_LOCK> CalculateLocks { get; set; }
        public DbSet<PA_KPI_LOCK> KpiLocks { get; set; }
        public DbSet<PA_SAL_IMPORT> SalaryImports { get; set; }
        public DbSet<PA_SAL_IMPORT_TMP> SalaryImportTmps { get; set; }
        public DbSet<SYS_OTHER_LIST> OtherLists { get; set; }
        public DbSet<SYS_OTHER_LIST_FIX> OtherListFixs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<IdentityRole>();
            modelBuilder.Ignore<IdentityUser>();
            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Ignore<IdentityUserRole<string>>();
            modelBuilder.Ignore<IdentityUserLogin<string>>();
            modelBuilder.Ignore<IdentityUserClaim<string>>();
            modelBuilder.Ignore<IdentityRoleClaim<string>>();

        }

        protected override void ConfigureConventions(
    ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>()
                .HavePrecision(18, 9);
        }

        /*
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        */
    }
}
