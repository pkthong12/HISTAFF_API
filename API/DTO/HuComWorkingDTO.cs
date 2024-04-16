using API.Main;

namespace API.DTO
{
    public class HuComWorkingDTO:BaseDTO
    {
        public long? ComEmployeeId { get; set; }
        public long? CommunistPositonId { get; set; }
        public long? CommunistOrgId { get; set; }
        public long? TranferTypeId { get; set; }
        public DateTime? EffectDate { get; set; }
        public string? DecisionNo { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? DateOfPayment { get; set; }
        public long? StatusId { get; set; }
        public string? Note { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
        public string? CommunistOrgName { get; set; }
        public string? CommunistTitleName { get; set; }
        public long? CommunistOrgIdOld { get; set; }
        public long? CommunistTitleIdOld { get; set; }
        public long? CommunistTitleIdMax { get; set; }
        public string? SignName { get; set; }
        public string? SignPositionName { get; set; }
        public string? NotePd { get; set; }
        public DateTime? SignDate { get; set; }
        public long? HuComEmployeeid { get; set; }
        public string? CommunistTitleNameMax { get; set; }
        public string? AttachedFile { get; set; }
    }
}
