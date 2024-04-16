using API.Main;

namespace API.DTO
{
    public class TrProgramResultDTO:BaseDTO
    {
        public long? TrProgramId { get; set; }
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? OrgName { get; set; }
        public string? PositionName { get; set; }
        public long? OrgId { get; set; }
        public long? PositionId { get; set; }
        public long? Duration { get; set; }
        public bool? IsExams { get; set; }
        public bool? IsEnd { get; set; }
        public bool? IsReach { get; set; }
        public bool? IsCertificate { get; set; }
        public DateTime? CertificateDate { get; set; }
        public string? CertificateNo { get; set; }
        public long? TrRankId { get; set; }
        public string? TrRankName { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
        public long? ToiecBenchmark { get; set; }
        public long? ToiecScoreIn { get; set; }
        public long? ToiecScoreOut { get; set; }
        public long? IncrementScore { get; set; }
        public long? AbsentReason { get; set; }
        public long? AbsentUnreason { get; set; }
        public DateTime? CerReceiveDate { get; set; }
        public DateTime? CommitStartdate { get; set; }
        public DateTime? CommitEnddate { get; set; }
        public long? CommitWorkmonth { get; set; }
        public bool? IsRefundFee { get; set; }
        public bool? IsReserves { get; set; }
        public long? FinalScore { get; set; }
        public long? RtestScore { get; set; }
        public long? RetestRankId { get; set; }
        public string? AttachedFile { get; set; }
        public string? Note { get; set; }
        public string? Retestremark { get; set; }
        public long? RetestFee { get; set; }
        public long? ReservesPeriod { get; set; }
        public string? Comment1 { get; set; }
        public string? Comment2 { get; set; }
        public string? Comment3 { get; set; }
        public bool? InsertHsnv { get; set; }
        public DateTime? CertDate { get; set; }
    }
}
