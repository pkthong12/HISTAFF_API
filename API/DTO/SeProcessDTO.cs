using API.Main;

namespace API.DTO
{
    public class SeProcessDTO:BaseDTO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public long? ProcessTypeId { get; set; }
        public string? ProcessTypeName { get; set; }
        public string? ApprovedContent { get; set; }
        public string? ApprovedSucessContent { get; set; }
        public string? NotApprovedContent { get; set; }
        public bool? IsNotiApprove { get; set; }
        public bool? IsNotiApproveSuccess { get; set; }
        public bool? IsNotiNotApprove { get; set; }
        public string? ProDescription { get; set; }
        public string? Approve { get; set; }
        public string? Refuse { get; set; }
        public string? AdjustmentParam { get; set; }
    }
}
