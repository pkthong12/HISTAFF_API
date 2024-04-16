using API.Entities.PORTAL;
using Microsoft.Extensions.Options;

namespace API.All.DbContexts
{
    public class FullDbContext : DbContextBase
    {

        public FullDbContext(IConfiguration config, DbContextOptions<FullDbContext> options, IHttpContextAccessor accessor, IOptions<AppSettings> appSettings)
            : base(config, options, accessor, appSettings) { }

        public DbSet<AD_BOOKING> AdBookings { get; set; }

        public DbSet<AD_PROGRAMS> AdProgramss { get; set; }
        public DbSet<AD_ROOM> AdRooms { get; set; }
        //public DbSet<HU_DYNAMIC_CONDITION> HuDynamicConditions { get; set; }
        public DbSet<HU_DYNAMIC_REPORT> HuDynamicReports { get; set; }
        //public DbSet<HU_DYNAMIC_REPORT_DTL> HuDynamicReportDtls { get; set; }
        //public DbSet<HU_DYNAMIC_REPORT_DTL_TOTAL> HuDynamicReportDtlTotals { get; set; }

        public DbSet<AD_REQUEST> AdRequests { get; set; }

        public DbSet<AT_APPROVED> AtApproveds { get; set; }

        public DbSet<AT_CONFIG> AtConfigs { get; set; }

        public DbSet<AT_DAYOFFYEAR> AtDayoffyears { get; set; }

        public DbSet<AT_DAYOFFYEAR_CONFIG> AtDayoffyearConfigs { get; set; }

        public DbSet<AT_ENTITLEMENT> AtEntitlements { get; set; }

        public DbSet<AT_ENTITLEMENT_EDIT> AtEntitlementEdits { get; set; }

        public DbSet<AT_HOLIDAY> AtHolidays { get; set; }

        public DbSet<AT_NOTIFICATION> AtNotifications { get; set; }

        public DbSet<AT_OVERTIME> AtOvertimes { get; set; }

        public DbSet<AT_OVERTIME_CONFIG> AtOvertimeConfigs { get; set; }

        public DbSet<AT_PERIOD_STANDARD> AtPeriodStandards { get; set; }

        public DbSet<AT_REGISTER_OFF> AtRegisterOffs { get; set; }

        public DbSet<AT_SALARY_PERIOD> AtSalaryPeriods { get; set; }

        public DbSet<AT_SALARY_PERIOD_DTL> AtSalaryPeriodDtls { get; set; }

        public DbSet<AT_SETUP_TIME_EMP> AtSetupTimeEmps { get; set; }

        public DbSet<AT_SHIFT> AtShifts { get; set; }

        public DbSet<AT_SIGN_DEFAULT> AtSignDefaults { get; set; }
        public DbSet<AT_SIGN_DEFAULT_IMPORT> AtSignDefaultImports { get; set; }

        public DbSet<AT_SWIPE_DATA> AtSwipeDatas { get; set; }

        public DbSet<AT_SWIPE_DATA_IMPORT> AtSwipeDataImports { get; set; }

        public DbSet<AT_SWIPE_DATA_TMP> AtSwipeDataTmps { get; set; }
        public DbSet<AT_SWIPE_DATA_SYNC> SwipeDataSyncs { get; set; }
        public DbSet<AT_SWIPE_DATA_WRONG> AtSwipeDataWrongs { get; set; }

        public DbSet<AT_SYMBOL> AtSymbols { get; set; }

        public DbSet<AT_TERMINAL> AtTerminals { get; set; }

        public DbSet<AT_TIME_LATE_EARLY> AtTimeLateEarlys { get; set; }

        public DbSet<AT_TIME_EXPLANATION> AtTimeExplanations { get; set; }

        public DbSet<AT_TIME_TIMESHEET_DAILY> AtTimeTimesheetDailys { get; set; }

        public DbSet<AT_TIME_TYPE> AtTimeTypes { get; set; }

        public DbSet<AT_TIMESHEET_DAILY> AtTimesheetDailys { get; set; }

        public DbSet<AT_TIMESHEET_FORMULA> AtTimesheetFormulas { get; set; }

        public DbSet<AT_TIMESHEET_LOCK> AtTimesheetLocks { get; set; }

        public DbSet<AT_TIMESHEET_MACHINE> AtTimesheetMachines { get; set; }

        public DbSet<AT_TIMESHEET_MACHINE_EDIT> AtTimesheetMachineEdits { get; set; }

        public DbSet<AT_TIMESHEET_MONTHLY> AtTimesheetMonthlys { get; set; }

        public DbSet<AT_TIMESHEET_MONTHLY_DTL> AtTimesheetMonthlyDtls { get; set; }

        public DbSet<AT_WORKSIGN> AtWorksigns { get; set; }

        public DbSet<AT_WORKSIGN_DUTY> AtWorksignDutys { get; set; }

        public DbSet<AT_WORKSIGN_TMP> AtWorksignTmps { get; set; }

        public DbSet<CSS_THEME> CssThemes { get; set; }

        public DbSet<CSS_THEME_VAR> CssThemeVars { get; set; }

        public DbSet<CSS_VAR> CssVars { get; set; }

        public DbSet<DEMO_ATTACHMENT> DemoAttachments { get; set; }

        public DbSet<DEMO_HANGFIRE_RECORD> DemoHangfireRecords { get; set; }

        public DbSet<HRM_INFOTYPE> HrmInfotypes { get; set; }

        public DbSet<HRM_OBJECT> HrmObjects { get; set; }

        public DbSet<HU_ALLOWANCE> HuAllowances { get; set; }

        public DbSet<HU_ALLOWANCE_EMP> HuAllowanceEmps { get; set; }
        public DbSet<HU_ALLOWANCE_EMP_IMPORT> HuAllowanceEmpImports { get; set; }
        public DbSet<HU_ANSWER> HuAnswers { get; set; }
        public DbSet<HU_CONCURRENTLY> HuConcurrentlys { get; set; }

        public DbSet<HU_ANSWER_USER> HuAnswerUsers { get; set; }

        public DbSet<HU_BANK> HuBanks { get; set; }

        public DbSet<HU_BANK_BRANCH> HuBankBranchs { get; set; }

        public DbSet<HU_CERTIFICATE> HuCertificates { get; set; }
        public DbSet<HU_CERTIFICATE_IMPORT> HuCertificateImports { get; set; }

        public DbSet<HU_COMMEND> HuCommends { get; set; }
        public DbSet<HU_COMMEND_IMPORT> HuCommendImports { get; set; }

        public DbSet<HU_WORKING_ALLOW> WorkingAllowances { get; set; }

        public DbSet<HU_WORKING_BEFORE> HuWorkingBefores { get; set; }

        public DbSet<HU_COMMEND_EMP> HuCommendEmps { get; set; }

        public DbSet<HU_COMPANY> HuCompanys { get; set; }

        public DbSet<HU_CONTRACT> HuContracts { get; set; }
        public DbSet<HU_CONTRACT_IMPORT> HuContractImports { get; set; }
        
        public DbSet<HU_FILECONTRACT> HuFileContracts { get; set; }
        public DbSet<HU_FILECONTRACT_IMPORT> HuFilecontractImports { get; set; }

        public DbSet<HU_CONTRACT_TYPE> HuContractTypes { get; set; }

        public DbSet<HU_DISCIPLINE> HuDisciplines { get; set; }

        public DbSet<HU_DISTRICT> HuDistricts { get; set; }

        public DbSet<HU_EMPLOYEE> HuEmployees { get; set; }

        public DbSet<HU_EMPLOYEE_IMPORT> HuEmployeeImports { get; set; }

        public DbSet<HU_EMPLOYEE_CV> HuEmployeeCvs { get; set; }

        public DbSet<HU_EMPLOYEE_CV_IMPORT> HuEmployeeCvImports { get; set; }

        public DbSet<HU_EMPLOYEE_CV_EDIT> HuEmployeeCvEdits { get; set; }

        public DbSet<HU_EMPLOYEE_EDIT> HuEmployeeEdits { get; set; }

        public DbSet<HU_EMPLOYEE_PAPERS> HuEmployeePaperss { get; set; }

        public DbSet<HU_EMPLOYEE_TMP> HuEmployeeTmps { get; set; }

        public DbSet<HU_FAMILY> HuFamilys { get; set; }

        public DbSet<HU_FAMILY_EDIT> HuFamilyEdits { get; set; }

        public DbSet<HU_FORM_ELEMENT> HuFormElements { get; set; }

        public DbSet<HU_FORM_LIST> HuFormLists { get; set; }

        public DbSet<HU_FROM_ELEMENT> HuFromElements { get; set; }

        public DbSet<HU_JOB> HuJobs { get; set; }

        public DbSet<HU_JOB_BAND> HuJobBands { get; set; }

        public DbSet<HU_JOB_DESCRIPTION> HuJobDescriptions { get; set; }

        public DbSet<HU_JOB_FUNCTION> HuJobFunctions { get; set; }

        public DbSet<HU_ORG_LEVEL> HuOrgLevels { get; set; }

        public DbSet<HU_ORGANIZATION> HuOrganizations { get; set; }

        public DbSet<HU_POS_PAPER> HuPosPapers { get; set; }

        public DbSet<HU_POSITION> HuPositions { get; set; }

        public DbSet<HU_POSITION_GROUP> HuPositionGroups { get; set; }

        public DbSet<HU_PROVINCE> HuProvinces { get; set; }

        public DbSet<HU_NATION> HuNations { get; set; }


        public DbSet<HU_QUESTION> HuQuestions { get; set; }

        public DbSet<HU_REPORT> HuReports { get; set; }

        public DbSet<HU_SALARY_LEVEL> HuSalaryLevels { get; set; }

        public DbSet<HU_SALARY_RANK> HuSalaryRanks { get; set; }

        public DbSet<HU_SALARY_SCALE> HuSalaryScales { get; set; }

        public DbSet<HU_SALARY_TYPE> HuSalaryTypes { get; set; }

        public DbSet<HU_SETTING_REMIND> HuSettingReminds { get; set; }

        public DbSet<HU_TERMINATE> HuTerminates { get; set; }

        public DbSet<HU_TITLE_GROUP> HuTitleGroups { get; set; }

        public DbSet<HU_WARD> HuWards { get; set; }

        public DbSet<HU_WELFARE> HuWelfares { get; set; }

        public DbSet<HU_WELFARE_CONTRACT> HuWelfareContracts { get; set; }

        public DbSet<HU_WELFARE_MNG> HuWelfareMngs { get; set; }
        public DbSet<HU_WELFARE_MNG_IMPORT> HuWelfareMngImports { get; set; }

        public DbSet<HU_WORK_LOCATION> HuWorkLocations { get; set; }

        public DbSet<HU_WORKING> HuWorkings { get; set; }
        public DbSet<HU_WORKING_IMPORT> HuWorkingImports { get; set; }
        public DbSet<HU_WORKING_ALLOW> HuWorkingAllows { get; set; }

        public DbSet<HUV_WORKING_MAX_BYTYPE> HuVWorkingMaxByTypes { get; set; }

        public DbSet<HUV_WAGE_MAX> HuVWageMax { get; set; }

        public DbSet<INS_CHANGE> InsChanges { get; set; }

        public DbSet<INS_INFORMATION> InsInformations { get; set; }

        public DbSet<INS_SPECIFIED_OBJECTS> InsSpecifiedObjectss { get; set; }

        public DbSet<INS_WHEREHEALTH> InsWhereHealThs { get; set; }

        public DbSet<INS_REGIMES> InsRegimess { get; set; }

        public DbSet<INS_TYPE> InsTypes { get; set; }

        public DbSet<INS_REGION> InsRegions { get; set; }

        public DbSet<INS_ARISING> InsArisings { get; set; }

        public DbSet<INS_GROUP> InsGroups { get; set; }

        public DbSet<INS_REGIMES_MNG> InsRegimesMngs { get; set; }

        public DbSet<PA_ADVANCE> PaAdvances { get; set; }

        public DbSet<PA_ADVANCE_TMP> PaAdvanceTmps { get; set; }
        public DbSet<PA_AUTHORITY_TAX_YEAR> PaAuthorityTaxYears { get; set; }
        public DbSet<PA_CALCULATE_LOCK> PaCalculateLocks { get; set; }

        public DbSet<PA_ELEMENT> PaElements { get; set; }

        public DbSet<PA_ELEMENT_GROUP> PaElementGroups { get; set; }

        public DbSet<PA_FORMULA> PaFormulas { get; set; }

        public DbSet<PA_KPI_FORMULA> PaKpiFormulas { get; set; }

        public DbSet<PA_KPI_GROUP> PaKpiGroups { get; set; }

        public DbSet<PA_KPI_LOCK> PaKpiLocks { get; set; }

        public DbSet<PA_KPI_POSITION> PaKpiPositions { get; set; }

        public DbSet<PA_KPI_SALARY_DETAIL> PaKpiSalaryDetails { get; set; }

        public DbSet<PA_KPI_SALARY_DETAIL_TMP> PaKpiSalaryDetailTmps { get; set; }

        public DbSet<PA_KPI_TARGET> PaKpiTargets { get; set; }

        public DbSet<PA_SAL_IMPORT> PaSalImports { get; set; }

        public DbSet<PA_SAL_IMPORT_TMP> PaSalImportTmps { get; set; }

        public DbSet<PA_SALARY_PAYCHECK> PaSalaryPaychecks { get; set; }

        public DbSet<PA_SALARY_STRUCTURE> PaSalaryStructures { get; set; }

        public DbSet<PA_LISTFUND> PaListfunds { get; set; }

        public DbSet<PA_LIST_FUND_SOURCE> PaListFundSources { get; set; }

        public DbSet<PA_LISTSALARIES> PaListsalariess { get; set; }

        public DbSet<PA_PAYROLL_FUND> PaPayrollFunds { get; set; }

        public DbSet<PA_PAYROLLSHEET_SUM> PaPayrollsheetSums { get; set; }

        public DbSet<PA_PAYROLLSHEET_SUM_BACKDATE> PaPayrollsheetSumBackdates { get; set; }

        public DbSet<PA_PAYROLLSHEET_SUM_SUB> PaPayrollsheetSumSubs { get; set; }

        public DbSet<PA_PAYROLL_TAX_YEAR> PaPayrollTaxYears { get; set; }

        public DbSet<PA_PHASE_ADVANCE> PaPhaseAdvances { get; set; }
        public DbSet<PA_PHASE_ADVANCE_SYMBOL> PaPhaseAdvanceSymbols { get; set; }

        public DbSet<PA_PERIOD_TAX> PaPeriodTaxs { get; set; }

        public DbSet<PA_PAYROLLSHEET_TAX> PaPayrollsheetTaxs { get; set; }


        public DbSet<PA_SAL_IMPORT_ADD> PaSalImportAdds { get; set; }

        public DbSet<PA_IMPORT_MONTHLY_TAX> PaImportMonthlyTaxs { get; set; }

        public DbSet<PA_TAX_ANNUAL_IMPORT> PaTaxAnnualImports { get; set; }
        
        public DbSet<PORTAL_REQUEST_CHANGE> PortalRequestChanges { get; set; }

        public DbSet<PORTAL_ROUTE> PortalRoutes { get; set; }

        public DbSet<PT_BLOG_INTERNAL> PtBlogInternals { get; set; }

        public DbSet<RC_CANDIDATE_SCANCV> RcCandidateScancvs { get; set; }

        public DbSet<SE_APP_PROCESS> SeAppProcesss { get; set; }

        public DbSet<SE_APP_TEMPLATE> SeAppTemplates { get; set; }

        public DbSet<SE_APP_TEMPLATE_DTL> SeAppTemplateDtls { get; set; }
        public DbSet<SE_CONFIG> SeConfigs { get; set; }
        public DbSet<SE_MAIL> SeMails { get; set; }

        public DbSet<SE_HR_PROCESS> SeHrProcesss { get; set; }

        public DbSet<SE_HR_PROCESS_DATA_MODEL> SeHrProcessDataModels { get; set; }

        public DbSet<SE_HR_PROCESS_INSTANCE> SeHrProcessInstances { get; set; }

        public DbSet<SE_HR_PROCESS_NODE> SeHrProcessNodes { get; set; }

        public DbSet<SE_LDAP> SeLdaps { get; set; }

        public DbSet<SE_PROCESS> SeProcesss { get; set; }
        public DbSet<SE_AUTHORIZE_APPROVE> SeAuthorizeApproves { get; set; }

        public DbSet<SE_HR_PROCESS_TYPE> SeHrProcessTypes { get; set; }

        public DbSet<SE_PROCESS_APPROVE> SeProcessApproves { get; set; }

        public DbSet<SE_PROCESS_APPROVE_POS> SeProcessApprovePos { get; set; }
        public DbSet<SE_DOCUMENT> SeDocuments { get; set; }
        public DbSet<SE_DOCUMENT_INFO> SeDocumentInfos { get; set; }

        public DbSet<SY_AUDITLOG> SyAuditlogs { get; set; }

        public DbSet<SYS_ACTION> SysActions { get; set; }

        public DbSet<SYS_CONFIG> SysConfigs { get; set; }

        public DbSet<SYS_CONTRACT_TYPE> SysContractTypes { get; set; }

        public DbSet<SYS_FORM_LIST> SysFormLists { get; set; }

        public DbSet<SYS_FUNCTION> SysFunctions { get; set; }

        public DbSet<SYS_FUNCTION_ACTION> SysFunctionActions { get; set; }

        public DbSet<SYS_FUNCTION_GROUP> SysFunctionGroups { get; set; }

        public DbSet<SYS_FUNCTION_IGNORE> SysFunctionIgnores { get; set; }

        public DbSet<SYS_GROUP> SysGroups { get; set; }

        public DbSet<SYS_GROUP_FUNCTION_ACTION> SysGroupFunctionActions { get; set; }

        public DbSet<SYS_GROUP_PERMISSION> SysGroupPermissions { get; set; }

        public DbSet<SYS_KPI> SysKpis { get; set; }

        public DbSet<SYS_KPI_GROUP> SysKpiGroups { get; set; }

        public DbSet<SYS_LANGUAGE> SysLanguages { get; set; }

        public DbSet<SYS_MENU> SysMenus { get; set; }

        public DbSet<SYS_MODULE> SysModules { get; set; }

        public DbSet<SYS_MUTATION_LOG> SysMutationLogs { get; set; }

        public DbSet<SYS_OTHER_LIST> SysOtherLists { get; set; }
         
        public DbSet<SYS_OTHER_LIST_FIX> SysOtherListFixs { get; set; }

        public DbSet<SYS_OTHER_LIST_TYPE> SysOtherListTypes { get; set; }

        public DbSet<SYS_PA_ELEMENT> SysPaElements { get; set; }

        public DbSet<SYS_PA_FORMULA> SysPaFormulas { get; set; }
         
        public DbSet<SYS_PERMISSION> SysPermissions { get; set; }

        public DbSet<SYS_REFRESH_TOKEN> SysRefreshTokens { get; set; }

        public DbSet<SYS_SALARY_STRUCTURE> SysSalaryStructures { get; set; }

        public DbSet<SYS_SALARY_TYPE> SysSalaryTypes { get; set; }

        public DbSet<SYS_SETTING_MAP> SysSettingMaps { get; set; }   

        public DbSet<SYS_TMP_SORT> SysTmpSorts { get; set; }

        public DbSet<SYS_USER> SysUsers { get; set; }

        public DbSet<SYS_USER_FUNCTION_ACTION> SysUserFunctionActions { get; set; }

        public DbSet<SYS_USER_GROUP_ORG> SysUserGroupOrgs { get; set; }

        public DbSet<SYS_USER_ORG> SysUserOrgs { get; set; }

        public DbSet<SYS_USER_PERMISSION> SysUserPermissions { get; set; }

        public DbSet<THEME_BLOG> ThemeBlogs { get; set; }

        public DbSet<TMP_HU_CONTRACT> TmpHuContracts { get; set; }

        public DbSet<TMP_HU_WORKING> TmpHuWorkings { get; set; }

        public DbSet<TMP_INS_CHANGE> TmpInsChanges { get; set; }

        public DbSet<TR_CENTER> TrCenters { get; set; }

        public DbSet<TR_COURSE> TrCourses { get; set; }
        public DbSet<TR_PLAN> TrPlans { get; set; }
        public DbSet<HU_WELFARE_AUTO> HuWelfareAutos { get; set; }
        public DbSet<AT_ORG_PERIOD> AtOrgPeriods { get; set; }

        public DbSet<AT_REGISTER_LEAVE> AtRegisterLeaves { get; set; }
        public DbSet<AT_REGISTER_LEAVE_DETAIL> AtRegisterLeaveDetails { get; set; }
        public DbSet<AT_DECLARE_SENIORITY> AtDeclareSenioritys { get; set; }

        public DbSet<PA_LISTSAL> PaListSals { get; set; }
        public DbSet<SE_REMINDER> SeReminders { get; set; }
        public DbSet<SE_REMINDER_SEEN> SeReminderSeens { get; set; }
        public DbSet<HU_COMMEND_EMPLOYEE> HuCommendEmployees { get; set; }
        public DbSet<HU_COMMEND_EMPLOYEE_IMPORT> HuCommendEmployeeImports { get; set; }
        public DbSet<HU_EVALUATE> HuEvaluates { get; set; }
        public DbSet<HU_EVALUATE_IMPORT> HuEvaluateImports { get; set; }
        public DbSet<AT_OTHER_LIST> AtOtherLists { get; set; }


        // thêm DbSet
        public DbSet<INS_DECLARATION_NEW> InsDeclarationNews { get; set; }
        public DbSet<HU_EVALUATION_COM> HuEvaluationComs { get; set; }
        public DbSet<HU_EVALUATION_COM_IMPORT> HuEvaluationComImports { get; set; }
        public DbSet<HU_EVALUATE_CONCURRENT_IMPORT> HuEvaluateConcurrentImports { get; set; }
        public DbSet<HU_CLASSIFICATION> HuClassifications { get; set; }
        public DbSet<PA_SAL_IMPORT_BACKDATE> PaSalImportBackdates { get; set; }
        public DbSet<HU_CERTIFICATE_EDIT> HuCertificateEdits { get; set; }
        public DbSet<HU_WORKING_HSL_PC_IMPORT> HuWorkingHslPcImports { get; set; }
        public DbSet<HU_WORKING_ALLOW_IMPORT> HuWorkingAllowImports { get; set; }
        public DbSet<HU_FAMILY_IMPORT> HuFamilyImports { get; set; }


        public DbSet<SYS_CONFIGURATION_COMMON> SysConfigurationCommons { get; set; }
        public DbSet<PORTAL_REGISTER_OFF> PortalRegisterOffs { get; set; }
        public DbSet<PORTAL_REGISTER_OFF_DETAIL> PortalRegisterOffDetails { get; set; }
        public DbSet<PORTAL_CERTIFICATE> PortalCertificates { get; set; }
        public DbSet<SE_PROCESS_APPROVE> ProcessApproves { get; set; }
        public DbSet<SE_PROCESS_APPROVE_POS> ProcessApprovePoses { get; set; }
        public DbSet<SE_PROCESS_APPROVE_STATUS> ProcessApproveStatuses { get; set; }
        public DbSet<HU_WORKING_BEFORE_IMPORT> HuWorkingBeforeImports { get; set; }
        public DbSet<INS_INFORMATION_IMPORT> InsInformationImports { get; set; }
        public DbSet<SYS_MAIL_TEMPLATE> SysMailTemplates { get; set; }

        /* CÁC DbSet dưới đây lấy từ Views để làm nguồn báo cáo */
        public DbSet<REPORT_DATA_STAFF_PROFILE> ReportDataStaffProfiles { get; set; }

        protected override void ConfigureConventions(
    ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>()
                .HavePrecision(18, 9);
        }
    }
}

