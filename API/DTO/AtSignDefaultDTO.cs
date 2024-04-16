using API.Main;

namespace API.DTO
{
    public class AtSignDefaultDTO : BaseDTO
    {
        public long? EmployeeId { get; set; }

        public long? OrgId { get; set; }

        public string? EmployeeCode { get; set; }

        public string? EmployeeName { get; set; }

        public string? PositionName { get; set; }

        public string? OrgName { get; set; }

        public DateTime? EffectDateFrom { get; set; }

        public DateTime? EffectDateTo { get; set; }

        public long? SignDefault { get; set; }

        public string? SignDefaultName { get; set; }

        public bool? IsActive { get; set; }

        public string? Note { get; set; }

        public string? Actflg { get; set; }

        public string? CreatedLog { get; set; }

        public string? UpdatedLog { get; set; }

        public string? Status { get; set; }

        public int? JobOrderNum { get; set; }
    }
}
