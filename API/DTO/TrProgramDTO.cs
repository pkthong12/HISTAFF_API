using API.Main;

namespace API.DTO
{
    public class TrProgramDTO:BaseDTO
    {
        public string? Name { get; set; }
        public long? OrgId { get; set; }
        public int? Year { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long? TrCourseId { get; set; }
        public string? TrCourseName { get; set; }
        public long? Duration { get; set; }
        public long? TrDurationUnitId { get; set; }
        public long? DurationStudyId { get; set; }
        public long? DurationHc { get; set; }
        public long? DurationOt { get; set; }
        public bool? IsReimburse { get; set; }
        public long? TrLanguageId { get; set; }
        public string? Venue { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
        public string? Titles { get; set; }
        public string? Departments { get; set; }
        public string? Centers { get; set; }
        public string? Lectures { get; set; }
        public List<long>? ListCenter { get; set; }
        public List<long>? ListLecture { get; set; }
        public long? TrRequestId { get; set; }
        public long? TrPlanId { get; set; }
        public long? StudentNumber { get; set; }
        public long? CostStudent { get; set; }
        public long? TrUnitId { get; set; }
        public bool? IsRetest { get; set; }
        public long? CostTotalUs { get; set; }
        public long? CostStudentUs { get; set; }
        public long? CostTotal { get; set; }
        public long? TrainFormId { get; set; }
        public long? PropertiesNeedId { get; set; }
        public string? PropertiesNeedName { get; set; }
        public string? TrProgramCode { get; set; }
        public long? TrTypeId { get; set; }
        public string? TrTypeName { get; set; }
        public bool? IsPlan { get; set; }
        public bool? IsPlanDx { get; set; }
        public bool? IsPlanNc { get; set; }
        public long? TrTrainField { get; set; }
        public string? PlanName { get; set; }
        public bool? TrCommit { get; set; }
        public bool? Certificate { get; set; }
        public string? CertificateName { get; set; }
        public string? AttachedFile { get; set; }
        public bool? IsPublic { get; set; }
        public long? ExpectClass { get; set; }
        public long? TrCurrencyId { get; set; }
        public bool? TrAfterTrain { get; set; }
        public DateTime? DayReview1 { get; set; }
        public DateTime? DayReview2 { get; set; }
        public DateTime? DayReview3 { get; set; }
        public long? PublicStatus { get; set; }
        public DateTime? PortalRegistFrom { get; set; }
        public DateTime? PortalRegistTo { get; set; }
        public long? AssEmp1Id { get; set; }
        public long? AssEmp2Id { get; set; }
        public long? AssEmp3Id { get; set; }
        public DateTime? AssDate { get; set; }
        public string? Content { get; set; }
        public string? TargetTrain { get; set; }
        public string? Note { get; set; }
    }
}
