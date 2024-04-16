using System.Drawing;
using API.Main;
using CORE.Services.File;

namespace API.DTO
{
    public class RcRequestDTO : BaseDTO
    {
        // This is "General information" for "View Edit"
        public string? Code { get; set; }
        public string? ResterName { get; set; }
        public bool? IsInBoundary { get; set; }
        public bool? IsOutBoundary { get; set; }
        public long? OrgId { get; set; }
        public long? PositionId { get; set; }
        public string? PositionGroupOfRecruitment { get; set; }
        public long? PetitionerId { get; set; }
        public DateTime? DateVote { get; set; }
        public DateTime? DateNeedResponse { get; set; }
        public long? RecruitmentForm { get; set; }
        public long? Workplace { get; set; }
        public long? SalaryLevel { get; set; }
        public int? QuantityAvailable { get; set; }
        public int? BoundaryQuantity { get; set; }
        public int? PlanLeave { get; set; }
        public long? RecruitmentReason { get; set; }
        public int? NumberNeed { get; set; }
        public bool? IsBeyondBoundary { get; set; }
        public bool? IsRequireComputer { get; set; }
        public string? DetailExplanation { get; set; }
        public int? ListRadio {  get; set; }
        public string? OrgName { get; set; }
        public string? PositionName { get; set; }
        public string? PetitionerName { get; set; }
        public string? RecruitmentReasonStr { get; set; }



        // This is "Details of recruitment requirements" for "View Edit"
        public long? EducationLevelId { get; set; }
        public long? SpecializeLevelId { get; set; }
        public string? OtherProfessionalQualifications { get; set; }
        public int? AgeFrom { get; set; }
        public int? AgeTo { get; set; }
        public long? LanguageId { get; set; }
        public long? LanguageLevelId { get; set; }
        public int? LanguagePoint { get; set; }
        public string? ForeignLanguageAbility { get; set; }
        public int? MinimumYearExp { get; set; }
        public long? GenderPriorityId { get; set; }
        public string? ComputerLevel { get; set; }
        public string? JobDescription { get; set; }
        public string? LevelPriority { get; set; }
        public string? NameOfFile { get; set; }
        public AttachmentDTO? AttachmentBuffer { get; set; }
        public string? OtherRequire { get; set; }
        public bool? IsApprove { get; set; }
        public long? PersonInCharge { get; set; }
        public string? PersonInChargeStr { get; set; }
        public long? StatusId { get; set; }
        public string? StatusName { get; set; }
    }
}