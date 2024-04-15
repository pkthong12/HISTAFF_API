using API.Main;

namespace API.DTO
{
    public class AtWorksignDTO : BaseDTO
    {
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? PositionName { get; set; }
        public string? DepartmentName { get; set; }
        public long? PeriodId { get; set; }
        public long? ShiftId { get; set; }
        public long? OrgId { get; set; }
        public DateTime? Workingday { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long[]? ListOrgIds { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
        public long[]? EmployeeIds { get; set; }
    }
}
