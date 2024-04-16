using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace API.DTO
{
    public class RcCandidateDTO : BaseDTO
    {
        public string? Avatar { get; set; }
        public string? CandidateCode { get; set; }
        public long? GenderId { get; set; }
        public string? GenderName { get; set; }
        public string? FullnameVn { get; set; }
        public long? OrgId { get; set; }
        public long? TitleId { get; set; }
        public DateTime? JoinDate { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
        public DateTime? EffectDate { get; set; }
        public string? TitleName { get; set; }
        public string? FileName { get; set; }
        public bool? IsWorkPermit { get; set; }
        public string? WorkPermitNo { get; set; }
        public DateTime? PermitStartDate { get; set; }
        public DateTime? PermitEndDate { get; set; }
        public long? RcProgramId { get; set; }
        public bool? IsPontential { get; set; }
        public bool? IsBlacklist { get; set; }
        public bool? IsRehire { get; set; }
        public string? StatusId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? CareTitleName { get; set; }
        public string? RecruimentWebsite { get; set; }
        public long? RcSourceRecId { get; set; }//nguon ung vien
        public string? RcSourceRecName { get; set; }//nguon ung vien
        public long? WantedLocation1 { get; set; }
        public string? WantedLocation1Name { get; set; }
        public long? WantedLocation2 { get; set; }
        public string? WantedLocation2Name { get; set; }
        public decimal? LevelSalaryWish { get; set; }
        public long? ProfileId { get; set; }

    }
}
