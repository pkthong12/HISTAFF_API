using API.Main;

namespace API.DTO
{
    public class InsRegimesMngDTO: BaseDTO
    {
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public int? JobOrderNum { get; set; }
        public long? OrgId { get; set; }
        public string? PositionName { get; set; }
        public decimal? SalaryBasic { get; set; }
        public string? OrgName { get; set; }
        public string? BhxhNo { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? BirthPlace { get; set; }
        public long? InsGroupId { get; set; }
        public long? RegimeId { get; set; }
        public string? RegimeName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long? DayCalculator { get; set; }
        public long? AccumulateDay { get; set; }
        public long? ChildrenNo { get; set; }
        public long? AverageSalSixMonth { get; set; }
        public long? BhxhSalary { get; set; }
        public long? RegimeSalary { get; set; }
        public long? SubsidyAmountChange { get; set; }
        public long? SubsidyMoneyAdvance { get; set; }
        public DateTime? DeclareDate { get; set; }
        public DateTime? DateCalculator { get; set; }
        public long? InsPayAmount { get; set; }
        public DateTime? PayApproveDate { get; set; }
        public long? ApprovDayNum { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
    }
}
