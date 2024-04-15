using API.Main;

namespace API.DTO
{
    public class PaAuthorityTaxYearDTO : BaseDTO
    {
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? PositionName { get; set; }
        public long? OrgId { get; set; }
        public string? DepartmentName { get; set; }
        public int? Year { get; set; }
        public bool? IsEmpRegister { get; set; }
        public bool? IsComApprove { get; set; }
        public string? ReasonReject { get; set; }
        public string? Note { get; set; }
        public long[]? EmployeeIds { get; set; }
        public int? JobOrderNum { get; set; }
    }
}
