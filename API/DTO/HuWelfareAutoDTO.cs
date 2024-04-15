using API.Main;

namespace API.DTO
{
    public class HuWelfareAutoDTO: BaseDTO
    {
        public long? EmployeeID { get; set; }
        public long? WelfareId { get; set; }
        public long? PeriodId  { get; set; }
        public long? EmployeeId { get; set; }
        public long? Money  { get; set; }
        public long? MoneyApproved  { get; set; }
        public long? OrgId  { get; set; }
        public string? Remark { get; set; }
        public long? GenderId { get; set; }
        public int? Seniority { get; set; }
        public int? CountChild { get; set; }
        public string? ContactTypeName   { get; set; }
        public int? MoneyAdjust  { get; set; }
        public bool? IsPay  { get; set; }
        public int? ChildAge { get; set; }
        public DateTime? PayDate { get; set; }
        public DateTime? CalculateDate { get; set; }
        public bool? IsDocumentOff { get; set; }
        public int? NumberManual  { get; set; }
        public int? Is50 { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public long? BenefitId { get; set; }
        public string? BenefitName { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? PositionName { get; set; }
        public string? OrgName { get; set; }
        public long? SalaryPeriodId { get; set; }
        public string? SalaryPeriodName { get; set; }
        public string?  GenderName { get; set; }
        public int? Year { get; set; }
        public int? JobOrderNum { get; set; }
    }
}
