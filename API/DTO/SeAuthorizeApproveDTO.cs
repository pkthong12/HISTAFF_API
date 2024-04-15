using API.Main;

namespace API.DTO
{
    public class SeAuthorizeApproveDTO:BaseDTO
    {
        public long? ProcessId { get; set; }
        public long? OrgId { get; set; }
        public string? ProcessName { get; set; }
        public long? LevelOrderId { get; set; }
        public string? LevelOrderName { get; set; }
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public long? EmployeeAuthId { get; set; }
        public string? EmployeeAuthCode { get; set; }
        public string? EmployeeAuthName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsPerReplace { get; set; }
    }
}
