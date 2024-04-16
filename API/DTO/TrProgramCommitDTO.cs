using System.Drawing;
using API.Main;

namespace API.DTO
{
    public class TrProgramCommitDTO : BaseDTO
    {
        public long? EmployeeId { get; set; }
        public decimal? CalculateReimbursement { get; set; }
        public string? CommitNo { get; set; }
        public DateTime? SignDate { get; set; }
        public long? TrainingCosts { get; set; }
        public long? MoneyCommit { get; set; }
        public int? TimeCommit { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? DayQuantity { get; set; }



        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? OrgName { get; set; }
    }
}