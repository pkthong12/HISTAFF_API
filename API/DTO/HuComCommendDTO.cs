using API.Main;
using CORE.Services.File;

namespace API.DTO
{
    public class HuComCommendDTO:BaseDTO
    {
        public string? No { get; set; }
        public long? PositionDecision { get; set; }
        public long? CommendObj { get; set; }
        public long? RewardId { get; set; }
        public int? Year { get; set; }
        public string? Content { get; set; }
        public string? Note { get; set; }
        public string? Attachment { get; set; }
        public DateTime? SignDate { get; set; }
        public AttachmentDTO? AttachmentBuffer { get; set; }
        public List<long>? EmployeeIds { get; set; }
    }
}
