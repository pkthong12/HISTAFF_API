using Microsoft.Extensions.Options;
using API.Entities;

namespace API.All.DbContexts
{

    public class ProfileDbContext : DbContextBase
    {
        public ProfileDbContext(IConfiguration config, DbContextOptions<ProfileDbContext> options, IHttpContextAccessor accessor, IOptions<AppSettings> appSettings)
            : base(config, options, accessor, appSettings)
        {

        }

        //List of DB Models - Add your DB models here
        //--------------- System --------------------
        //----------- Security ----------------------
        public DbSet<HU_COMPANY> CompanyInfos { get; set; }
        public DbSet<HU_ORGANIZATION> Organizations { get; set; }
        public DbSet<HUV_ORGANIZATION> ViewOrganizations { get; set; }
        public DbSet<HU_POSITION_GROUP> GroupPositions { get; set; }
        public DbSet<HU_POSITION> Positions { get; set; }
        public DbSet<HU_POSITION_ORG_MAP_BUFFER> HuPositionOrgMapBuffers { get; set; }
        public DbSet<HU_FAMILY> Famylies { get; set; }
        public DbSet<HU_JOB_DESCRIPTION> PositionDesriptions { get; set; }
        public DbSet<SYS_OTHER_LIST_FIX> OtherListFixs { get; set; }
        public DbSet<SYS_OTHER_LIST> OtherLists { get; set; }
        public DbSet<SYS_OTHER_LIST_TYPE> OtherListTypes { get; set; }
        public DbSet<HU_CONTRACT_TYPE> ContractTypes { get; set; }
        public DbSet<HU_ALLOWANCE> Allowances { get; set; }
        public DbSet<HU_WELFARE> Welfares { get; set; }
        public DbSet<HU_WELFARE_CONTRACT> WelfareContracts { get; set; }
        public DbSet<HU_WELFARE_MNG> WelfareMng { get; set; }
        public DbSet<HU_ALLOWANCE_EMP> AllowanceEmps { get; set; }

        public DbSet<HU_SALARY_SCALE> SalaryScales { get; set; }
        public DbSet<HU_SALARY_RANK> SalaryRanks { get; set; }
        public DbSet<HU_SALARY_LEVEL> SalaryLevels { get; set; }
        public DbSet<HU_CLASSIFICATION> Classifications { get; set; }
        public DbSet<SYS_USER_ORG> UserOrganis { get; set; }
        public DbSet<SYS_USER_GROUP_ORG> UserGroupOrganis { get; set; }
        // Danh mục
        public DbSet<HU_SALARY_TYPE> SalaryTypes { get; set; }
        public DbSet<HU_JOB_BAND> HUJobBands { get; set; }
        public DbSet<HU_JOB> HUJobs { get; set; }
        public DbSet<HU_JOB_FUNCTION> HUJobFunction { get; set; }
        public DbSet<AT_SALARY_PERIOD> SalaryPeriods { get; set; }
        public DbSet<SYS_OTHER_LIST> OtOtherLists { get; set; }

        public DbSet<TR_PLAN> TrPlans { get; set; }

        public DbSet<TR_CENTER> TrCenters { get; set; }
        public DbSet<TR_COURSE> TrCourses { get; set; }
        // Dùng chung All Tenant
        public DbSet<HU_PROVINCE> Provinces { get; set; }
        public DbSet<HU_DISTRICT> Districts { get; set; }
        public DbSet<HU_WARD> Wards { get; set; }
        public DbSet<INS_TYPE> InsuranceTypes { get; set; }
        public DbSet<INS_REGION> InsRegions { get; set; }
        public DbSet<INS_WHEREHEALTH> InsWhereHealThs { get; set; }
        public DbSet<THEME_BLOG> ThemeBlogs { get; set; }
        public DbSet<SYS_USER> SysUers { get; set; }
        public DbSet<SYS_POSITION_GROUP> GroupPositionSyses { get; set; }
        public DbSet<SYS_POSITION> PositionSyses { get; set; }
        public DbSet<SYS_CONTRACT_TYPE> ContractTypeSyses { get; set; }
        public DbSet<SYS_SHIFT> ShiftSyses { get; set; }
        public DbSet<SYS_PA_ELEMENT> SalaryElementSyses { get; set; }
        public DbSet<SYS_SALARY_TYPE> SalaryTypeSyses { get; set; }
        public DbSet<SYS_SALARY_STRUCTURE> SalaryStructSyses { get; set; }
        public DbSet<SYS_PA_FORMULA> FormulaSyses { get; set; }
        public DbSet<SYS_FORM_LIST> FormListSyses { get; set; }
        public DbSet<SYS_KPI> KPISyss { get; set; }
        public DbSet<SYS_KPI_GROUP> KPIGroupSyss { get; set; }


        // Nghiep vu
        public DbSet<HU_EMPLOYEE> Employees { get; set; }
        public DbSet<HU_EMPLOYEE_CV> EmployeeCvs { get; set; }
        public DbSet<HU_EMPLOYEE_EDIT> EmployeeEdits { get; set; }
        public DbSet<HU_FAMILY_EDIT> SituationEdits { get; set; }
        public DbSet<HU_EMPLOYEE_TMP> EmployeeTmps { get; set; }
        public DbSet<HU_EMPLOYEE_PAPERS> EmployeePaperses { get; set; }

        public DbSet<HU_FAMILY> Families { get; set; }
        public DbSet<HU_WORKING> Workings { get; set; }
        public DbSet<HU_FILECONTRACT> HuFileContracts { get; set; }
        public DbSet<HU_FILECONTRACT_TYPE> HuFileContractTypes { get; set; }
        public DbSet<HU_WORKING_ALLOW> WorkingAllowances { get; set; }
        public DbSet<HU_WORKING_BEFORE> WorkingBefores { get; set; }
        public DbSet<HU_CONTRACT> Contracts { get; set; }
        public DbSet<HU_CONTRACT_IMPORT> ContractImports { get; set; }
        public DbSet<HU_COMMEND> Commends { get; set; }
        public DbSet<HU_COMMEND_EMP> CommendEmps { get; set; } // bảng cũ k dùng
        public DbSet<HU_COMMEND_EMPLOYEE> CommendEmployees { get; set; }
        public DbSet<HU_DISCIPLINE> Disciplines { get; set; }
        public DbSet<HU_DISCIPLINE_EMP> DisciplineEmps { get; set; }
        public DbSet<HU_TERMINATE> Terminates { get; set; }
        public DbSet<HU_CONCURRENTLY> HuConcurrentlies { get; set; }
        public DbSet<INS_INFORMATION> InsInformations { get; set; }
        public DbSet<INS_CHANGE> InsChanges { get; set; }
        public DbSet<SE_DOCUMENT> SeDocuments { get; set; }
        public DbSet<SE_DOCUMENT_INFO> SeDocumentInfos { get; set; }


        public DbSet<RC_CANDIDATE_SCANCV> CandidateScanCVs { get; set; }

        //Report
        public DbSet<HU_FORM_LIST> FormLists { get; set; }
        public DbSet<HU_FORM_ELEMENT> FormElements { get; set; }
        public DbSet<HU_SETTING_REMIND> SettingReminds { get; set; }
        public DbSet<PT_BLOG_INTERNAL> BlogInternals { get; set; }
        public DbSet<HU_QUESTION> Questions { get; set; }
        public DbSet<HU_ANSWER> Answers { get; set; }
        public DbSet<HU_ANSWER_USER> AnswerUsers { get; set; }
        public DbSet<SYS_SETTING_MAP> SettingMaps { get; set; }
        public DbSet<HU_REPORT> Reports { get; set; }
        public DbSet<SY_AUDITLOG> AuditLogs { get; set; }
        //share 
        public DbSet<HU_BANK> Banks { get; set; }
        public DbSet<HU_BANK_BRANCH> BankBranchs { get; set; }
        // Administration
        public DbSet<AD_ROOM> Rooms { get; set; }
        public DbSet<AD_BOOKING> Bookings { get; set; }
        public DbSet<AT_NOTIFICATION> Notifications { get; set; }
        public DbSet<HU_POS_PAPER> PostionPaperses { get; set; }
        // temp 
        public DbSet<TMP_HU_WORKING> WorkingTmps { get; set; }
        public DbSet<TMP_HU_CONTRACT> ContractTmps { get; set; }
        public DbSet<TMP_INS_CHANGE> InsChangeTmps { get; set; }

        public DbSet<HU_CERTIFICATE> Certificates { get; set; }
        // CSS
        public DbSet<CSS_VAR> CssVars { get; set; }
        // INS
        public DbSet<INS_REGION> Regions { get; set; }


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
        */
        //public override void 

    }
}
