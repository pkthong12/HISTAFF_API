using API.Main;
using CORE.Services.File;

namespace API.DTO
{
    public class TrRequestDTO : BaseDTO
    {
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        public DateTime? RequestDate { get; set; }
        public int? Year { get; set; }
        public long? TrPlanId { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public decimal? ExpectedCost { get; set; }
        public long? TrCurrencyId { get; set; }
        public long? StatusId { get; set; }
        public string? Status { get; set; }
        public long? RequestSenderId { get; set; }
        public string? SenderName { get; set; }
        public string? SenderPosition { get; set; }
        public string? SenderEmail { get; set; }
        public string? SenderPhoneNumber { get; set; }
        public bool? IsIrregularly { get; set; }
        public long? TrCourseId { get; set; }
        public string? TrTrainFeild { get; set; }
        public string? TrCourseName { get; set; }
        public long? TrainFormId { get; set; }
        public string? TrainFormName { get; set; }
        public long? PropertiesNeedId { get; set; }
        public string? PropertiesNeedName { get; set; }
        public long? TrUnitId { get; set; }
        public string? Venue { get; set; }
        public AttachmentDTO? AttachedFileBuffer { get; set; }
        public string? AttachFile { get; set; }
        public string? RejectReason { get; set; }
        public DateTime? ExpectDateTo { get; set; }
        public long? SenderTitleId { get; set; }
        public string? OtherCourse { get; set; }
        public int? TrainerNumber { get; set; }
        public bool? TrCommit { get; set; }
        public bool? Certificate { get; set; }
        public string? TrPlace { get; set; }
        public string? TeachersId { get; set; }
        public List<long>? ListTeachersId { get; set; }
        public string? CentersId { get; set; }
        public List<long>? ListCentersId { get; set; }
        public bool? IsPortal { get; set; }
        public bool? IsApprove { get; set; }
        public string? ReasonPortal { get; set; }
        public string? RequestCode { get; set; }
        public string? Content { get; set; }
        public string? Remark { get; set; }
        public string? TargetTrain { get; set; }
        public List<long>? EmployeeIds { get; set; }
    }
}
