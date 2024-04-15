using API.Main;

namespace API.DTO
{
    public class PaPayrollFundDTO:BaseDTO
    {
        public int? Year { get; set; }
        public long? Amount { get; set; }
        public string? Note { get; set; }
        public long? CompanyId { get; set; }
        public long? ListFundId { get; set; }
        public long? ListFundSourceId { get; set; }
        public long? SalaryPeriodId { get; set; }
        public int? Month { get; set; }
        public string? CompanyName { get; set; }
        public string? ListFundName { get; set; }
        public string? ListFundSourceName { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
        public long[]? OrgIds { get; set; }
    }
}
