using API.Main;
using CORE.Services.File;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.DTO
{
    public class TrPlanDTO : BaseDTO
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
        public long? FormTrainingId { get; set; }
        public string? FormTrainingName { get; set; }
        public string? AddressTraining { get; set; }
        public long? CenterId { get; set; }
        public string? CenterName { get; set; }
        public string? Note { get; set; }
        public string? Filename { get; set; }
        public AttachmentDTO? AttachmentBuffer { get; set; }
        public string? Attachment { get; set; }
        public string? Code { get; set; }
        public long? PropertiesNeedId { get; set; }
        public string? PropertiesNeedName { get; set; }
        public string? ExpectClass { get; set; }
        public bool? IsCommitTrain { get; set; }
        public bool? IsCertificate { get; set; }
        public string? CertificateName { get; set; }
        public long? TypeTrainingId { get; set; }
        public string? TypeTrainingName { get; set; }
        public bool? IsPostTrain { get; set; }
        public string? JobFamilyIds { get; set;}
        public string? JobFamilyName {get; set;}
        public string? JobIds { get; set; }
        public string? JobName { get; set; }
        public DateTime? EvaluationDueDate1 { get; set; }
        public DateTime? EvaluationDueDate2 { get; set; }
        public DateTime? EvaluationDueDate3 { get; set; }
        public long? UnitMoneyId { get; set; }
        public string? UnitMoneyName { get; set; }
        public string? TrTrainFeildName { get; set; }

        public List<long?>? ListJobFamilyIds { get; set; }
        public List<long?>? ListJobFamilyId { get; set; }
        public List<long?>? ListJobIds { get; set; }
        public List<long?>? ListJobId { get; set; }
    }
}
