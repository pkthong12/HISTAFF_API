using API.Main;

namespace API.DTO
{
    public class AtSetupTimeEmpDTO : BaseDTO
    {
        public long? EmployeeId { get; set; }
        public long? OrgId { get; set; }

        public string? EmployeeName { get; set; }

        public string? EmployeeCode { get; set; }

        public string? PositionName { get; set; }

        public int? NumberSwipecard { get; set; }

        public bool? IsActive { get; set; }

        public string? Status { get; set; }

        public string? Note { get; set; }

        public string? Actflg { get; set; }

        public int? JobOrderNum { get; set; }
    }
}
