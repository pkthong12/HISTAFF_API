using API.Main;
using CORE.Services.File;

namespace API.DTO
{
    public class TrPlanDTO:BaseDTO
    {
        public string? Name { get; set; }
        public string? Year { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        public DateTime? StartDatePlan { get; set; }
        public DateTime? EndDatePlan { get; set; }
        public DateTime? StartDateReal { get; set; }
        public DateTime? EndDateReal { get; set; }
        public int? PersonNumReal { get; set; }
        public int? PersonNumPlan { get; set; }

        public long? ExpectedCost { get; set; }
        public long? ActualCost { get; set; }
        public long? CourseId { get; set; }
        public string? CourseName { get; set; }
        public string? Content { get; set; }
        public string? FormTraining { get; set; }
        public string? AddressTraining { get; set; }
        public long? CenterId { get; set; }
        public string? CenterName { get; set; }
        public string? Note { get; set; }
        public string? Filename { get; set; }
        public AttachmentDTO? AttachmentBuffer { get; set; }
        public string? Attachment { get; set; }
        public string? Code { get; set; }


    }
}
