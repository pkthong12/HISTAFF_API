namespace API.DTO
{
    public class HuWorkingBeforeDTO
    {
        public long? Id { get; set; }
        public long? EmployeeId { get; set; }
        public long? EmployeeStatus { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? PositionName { get; set; }
        public string? OrgName { get; set; }
        public string? CompanyName { get; set; }
        public string? TitleName { get; set; }
        public DateTime? FromDate { get; set; }
        public string? FromDateStr { get; set; }
        public DateTime? EndDate { get; set; }
        public string? EndDateStr { get; set; }
        public string? MainDuty { get; set; }
        public string? TerReason { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? Seniority { get; set; }
        public long? OrgId { get; set; }
        public int? JobOrderNum { get; set; }
    }
}
