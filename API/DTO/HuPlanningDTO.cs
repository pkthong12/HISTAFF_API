using API.Main;
using CORE.Services.File;
using ProfileDAL.ViewModels;
using System.Dynamic;

namespace API.DTO
{
    public class HuPlanningDTO : BaseDTO
    {
        public long? PlanningPeriodId { get; set; }
        public string? PlanningPeriodName { get; set; }
        public long? FromYearId { get; set; }
        public long? ToYearId { get; set; }
        public string? DecisionNo { get; set; }
        public DateTime? EffectDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public long? AppLevel { get; set; }
        public long? TotalPersonnel { get; set; }
        public string? Attachment { get; set; }
        public DateTime? SignDate { get; set; }
        public long? SignerId { get; set; }
        public string? Note { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }

        public AttachmentDTO? AttachmentBuffer { get; set; }
        public List<long>? EmployeeIds { get; set; }
        public string? Evaluate { get; set; }

        public long? PlanningId { get; set; }
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? PositionName { get; set; }
        public string? PlanningTitleName { get; set; }//chuc danh quy hoach
        public string? PlanningTypeName { get; set; }//loai quy hoach
        public long? PlanningTitleId { get; set; }//chuc danh quy hoach
        public long? PlanningTypeId { get; set; }//loai quy hoach

        public List<DynamicDTO>? EmployeeList { get; set; }

        public long? WorkStatusId { get; set; }
        public DateTime? StartDate { get; set; }

        public string? GenderName { get; set; }//gt
        public string? NativeName { get; set; }//dan toc
        public DateTime? BirthDate { get; set; }//ngay sinh
        public string? PlaceName { get; set; }//que quan
        public DateTime? MemberDate { get; set; }//ngay vao dang
        public string? LevelTrainName { get; set; }//trinh do hoc van
        public string? LevelName { get; set; }//trinh do chuyen mon
        public string? PoliticalTheoryLevel { get; set; }//trinh do ly luan trinh tri
    }

    public class HuPlanningInputDTO:BaseDTO
    {
        public long? AppLevelId { get; set; }
        public long? PlanningPeriodId { get; set; }
        public string? PlanningPeriodName { get; set; }
        public string? SignerName { get; set; }
        public long? FromYearId { get; set; }
        public long? ToYearId { get; set; }
        public string? DecisionNo { get; set; }
        public DateTime? EffectDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public long? AppLevel { get; set; }
        public long? TotalPersonnel { get; set; }
        public string? Attachment { get; set; }
        public DateTime? SignDate { get; set; }
        public long? SignerId { get; set; }
        public string? Note { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }

        public string? AttachmentBuffer { get; set; }
        public List<long?>? EmployeeIds { get; set; }
        //public long? EvaluateId { get; set; }

        public long? PlanningId { get; set; }
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? PositionName { get; set; }
        public string? PlanningTitleName { get; set; }//chuc danh quy hoach
        public string? PlanningTypeName { get; set; }//loai quy hoach
        public long? PlanningTitleId { get; set; }//chuc danh quy hoach
        public long? PlanningTypeId { get; set; }//loai quy hoach

        public List<EmployeeDTO>? EmployeeList { get; set; }

        public long? WorkStatusId { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
