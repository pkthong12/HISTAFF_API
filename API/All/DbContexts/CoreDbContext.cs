using CoreDAL.Models;
using Microsoft.Extensions.Options;
using API.Entities;

namespace API.All.DbContexts
{
    public class CoreDbContext : DbContextBase
    {
        public CoreDbContext(IConfiguration config, DbContextOptions<CoreDbContext> options, IHttpContextAccessor accessor, IOptions<AppSettings> appSettings)
            : base(config, options, accessor, appSettings)
        {
        }

        public DbSet<CSS_THEME> CssThemes { get; set; }

        public DbSet<CSS_THEME_VAR> CssThemeVars { get; set; }

        public DbSet<CSS_VAR> CssVars { get; set; }

        public DbSet<HRM_OBJECT> HrmObjects { get; set; }

        public DbSet<HRM_INFOTYPE> HrmInfotypes { get; set; }

        public DbSet<SYS_ERROR> SysErrors { get; set; }
        public DbSet<SYS_MENU> SysMenus { get; set; }
        public DbSet<SYS_LANGUAGE> SysLanguages { get; set; }
        public DbSet<SYS_FUNCTION_GROUP> SysGroupFunctions { get; set; }
        public DbSet<SYS_FUNCTION> SysFunctions { get; set; }
        public DbSet<SYS_ACTION> SysActions { get; set; }
        public DbSet<SYS_FUNCTION_ACTION> SysFunctionAction { get; set; }
        public DbSet<SYS_PERMISSION> SysPermissions { get; set; }
        public DbSet<SYS_GROUP_PERMISSION> SysGroupPermissions { get; set; }
        public DbSet<SYS_GROUP> SysGroupUsers { get; set; }
        public DbSet<SYS_USER> SysUsers { get; set; }
        public DbSet<SYS_USER_ORG>SysUserOrgs { get; set; }
        public DbSet<SYS_USER_PERMISSION> AspUserPermissions { get; set; }

        //------------ OtherList ---------------------
        public DbSet<SYS_OTHER_LIST_TYPE> SysOtherListTypes { get; set; }
        public DbSet<SYS_OTHER_LIST> SysOtherLists { get; set; }

        public DbSet<SYS_CONFIG> SysConfigs { get; set; }

        //------------ Package Seller ---------------------
        public DbSet<SYS_MODULE> SysModules { get; set; }
        public DbSet<SY_AUDITLOG> AuditLogs { get; set; }
        public DbSet<SE_APP_PROCESS> ApproveProcess { get; set; }
        public DbSet<SE_APP_TEMPLATE> ApproveTemplates { get; set; }
        public DbSet<SE_APP_TEMPLATE_DTL> ApproveTemplateDetails { get; set; }
        public DbSet<HU_EMPLOYEE> Employees { get; set; }
        public DbSet<HU_EMPLOYEE_CV> HuEmployeeCvs { get; set; }
        public DbSet<HU_POSITION> Positions { get; set; }
        public DbSet<HU_WORKING> Workings { get; set; }
        public DbSet<HU_WORKING_ALLOW> WorkingAllowances { get; set; }
        public DbSet<HU_ORGANIZATION> Organizations { get; set; }
        public DbSet<HU_DISCIPLINE> Disciplines { get; set; }
        public DbSet<HU_DISCIPLINE_EMP> DisciplineEmps { get; set; }
        public DbSet<SYS_OTHER_LIST_FIX> OtherListFixs { get; set; }
        public DbSet<SE_PROCESS_APPROVE> SeProcessApproves { get; set; }
        public DbSet<HU_COMMEND> HuCommends { get; set; }
        public DbSet<HU_COMMEND_EMPLOYEE> HuCommendEmployees { get; set; }
        public DbSet<HU_JOB> HuJobs { get; set; }

        //------------ Profile ---------------------
        //------------ Business -----------------------
        public DbSet<HU_WORKING_BEFORE> WorkingBefores { get; set; }
        public DbSet<HU_CONTRACT> Contracts { get; set; }
        public DbSet<HU_FAMILY> Families { get; set; }
        public DbSet<HU_PROVINCE> Provinces { get; set; }
        public DbSet<HU_DISTRICT> Districts { get; set; }
        public DbSet<HU_WARD> Wards { get; set; }
        public DbSet<HU_TERMINATE> Terminates { get; set; }

        //------------ List ---------------------
        public DbSet<HU_CONTRACT_TYPE> Contracttypes { get; set; }
        public DbSet<SYS_CONTRACT_TYPE> SysContracttypes { get; set; }
        public DbSet<HU_WELFARE> Welfares { get; set; }
        public DbSet<HU_SALARY_TYPE> SalaryTypes { get; set; }
        public DbSet<HU_SALARY_RANK> SalaryRanks { get; set; }
        public DbSet<HU_SALARY_SCALE> SalaryScales { get; set; }
        public DbSet<HU_SALARY_LEVEL> SalaryLevels { get; set; }
        public DbSet<HU_NATION> Nations { get; set; }
        public DbSet<HU_COMPANY> Companies { get; set; }
        //------------ Attendance ---------------------
        //------------ List ---------------------
        public DbSet<AT_TIME_TYPE> TimeTypes { get; set; }
        public DbSet<AT_SYMBOL> Symbols { get; set; }
        public DbSet<AT_SALARY_PERIOD> Periods { get; set; }
        public DbSet<ApiAuditLog> ApiAuditLogs {get;set;}

        //------------ Insurance ---------------------
        //------------ Business ---------------------
        public DbSet<INS_ARISING> Arisings { get; set; }
        public DbSet<INS_CHANGE> Changes { get; set; }
        public DbSet<INS_SPECIFIED_OBJECTS> SpecifiedObjects { get; set; }
        public DbSet<INS_INFORMATION> Informations { get; set; }
        public DbSet<INS_REGION> Regions { get; set; }
        public DbSet<INS_TYPE> Types { get; set; }
        //------------ Payroll ---------------------
        //------------ List ---------------------
        public DbSet<PA_KPI_GROUP> KpiGroups { get; set; }
        public DbSet<PA_PAYROLLSHEET_SUM> PaPayrollsheetSums { get; set; }

        public DbSet<PA_PAYROLLSHEET_SUM_BACKDATE> PaPayrollsheetSumBackdates { get; set; }


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
