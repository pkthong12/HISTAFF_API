using API.Main;

namespace API.DTO
{
    public class TrReimbursementDTO : BaseDTO
    {
        public int? Year { get; set; }
        public long? TrProgramId { get; set; }
        public string? TrProgramName { get; set; }
        public DateTime? TrStartDate { get; set; }
        public DateTime? TrEndDate { get; set; }
        public long? EmployeeId { get; set; }
        public DateTime? StartDate { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
        public decimal? CostReimburse { get; set; }
        public bool? IsReserves { get; set; }
        public string? Remark { get; set; }
        public long? OrgId { get; set; }
        public DateTime? DateFromCommitment { get; set; }
        public DateTime? EndDateToCommitment { get; set; }
        public DateTime? EndDateFromCommitment { get; set; }
        public DateTime? DateToCommitment { get; set; }
        public DateTime? MonthReimbursement { get; set; }
        public DateTime? FinalDate { get; set; }
        public bool? IsLaveDate { get; set; }
        public string? EmpCode { get; set; }
        public string? EmpName { get; set; }
        public string? PosName { get; set; }
        public string? OrgName { get; set;}
        public DateTime? LeaveJobDate { get; set;}
        public decimal? NumCommitment { get; set;}
        public decimal? CostStudent { get; set;}
        public decimal? NumDayRemainingCommit { get; set;}
    }
}
