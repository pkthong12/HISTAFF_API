using System.Drawing;
using API.Main;

namespace API.DTO
{
    public class InsArisingDTO : BaseDTO
    {
        public long[]? Ids { get; set; }

        public long? InsGroupType { get; set; }

        public string? InsGroupTypeName { get; set; }

        public long? InsTypeId { get; set; }

        public long? InsTypeChooseId { get; set; }

        public long? PkeyRef { get; set; }

        public string? TableRef { get; set; }

        public string? InsNo { get; set; }

        public DateTime? EffectDate { get; set; }

        public long? EmployeeId { get; set; }

        public string? EmployeeCode { get; set; }

        public string? EmployeeName { get; set; }

        public string? PositionName { get; set; }

        public long? OrgId { get; set; }

        public string? OrgName { get; set; }

        public long? OldOrgId { get; set; }

        public long? NewOrgId { get; set; }

        public double? OldSal { get; set; }

        public double? NewSal { get; set; }

        public long? OldPositionId { get; set; }

        public long? NewPositionId { get; set; }

        public bool? Si { get; set; }

        public bool? Hi { get; set; }

        public bool? Ui { get; set; }

        public bool? Ai { get; set; }

        public string? Reasons { get; set; }

        public long? Status { get; set; }

        public long? InsOrgId { get; set; }

        public string? InsOrgName { get; set; }

        public DateTime? DeclaredDate { get; set; }

        public DateTime? DeclaredMonth { get; set; }

        public bool? IsDeleted { get; set; }
        public long? InsInformationId { get; set; }
        public float? OldInsSal { get; set; }
        public float? NewInsSal { get; set; }

        public long? InsSpecifiedId { get; set; }
        public int? JobOrderNum { get; set; }
    }
}
