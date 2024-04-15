namespace API.DTO
{
    public class AtTimesheetMachineEditDTO
    {
        public long? Id { get; set; }
        public long? TenantId { get; set; }
        public long? PeriodId { get; set; }
        public long? EmployeeId { get; set; }
        public DateTime? Workingday { get; set; }
        public string? TimePoint1 { get; set; }
        public string? TimePoint4 { get; set; }
        public bool? IsEditIn { get; set; }
        public bool? IsEditOut { get; set; }
        public string? Note { get; set; }
    }
}
