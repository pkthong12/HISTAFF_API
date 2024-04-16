using System.Drawing;
using API.Main;

namespace API.DTO
{
    public class TrTranningRecordDTO : BaseDTO
    {
        public long? EmployeeId { get; set; }
        public long? TrCourseId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? Content { get; set; }
        public string? TargetText { get; set; }
        public string? TrainingPlace { get; set; }
        public string? TrainingCenter { get; set; }
        public string? Result { get; set; }
        public decimal? Scores { get; set; }
        public string? Rating { get; set; }
        public string? EvaluateDue1 { get; set; }
        public string? EvaluateDue2 { get; set; }
        public string? EvaluateDue3 { get; set; }
        public string? CertificateText { get; set; }
        public DateTime? CertificateIssuanceDate { get; set; }
        public string? CommitmentNumber { get; set; }
        public decimal? CommitmentAmount { get; set; }
        public int? MonthCommitment { get; set; }
        public DateTime? CommitmentStartDate { get; set; }
        public DateTime? CommitmentEndDate { get; set; }



        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? PositionName { get; set; }
        public string? OrgName { get; set; }
        public string? CourseName { get; set; }
        public long? WorkStatusId { get; set; }
    }
}