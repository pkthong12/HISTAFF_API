using API.Main;

namespace API.DTO
{
    public class HuComClassificationDTO : BaseDTO
    {
        public long? CommunistId { get; set; }
        public long? CommunistOrgId { get; set; }
        public long? CommunistTitleId { get; set; }
        public long? DecisionNumber { get; set; }
        public DateTime? DecisionDate { get; set; }
        public int? Year { get; set; }
        public long? ClassificationId { get; set; }
        public string? RewardName { get; set; }
        public string? Note { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
        public long? OrgId { get; set; }
        public long? EmployeeId { get; set; }
    }
}
