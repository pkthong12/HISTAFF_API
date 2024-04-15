namespace API.DTO
{
    public class AtTimesheetMachineDTO
    {
        public long? Id { get; set; }
        public long? TenantId { get; set; }
        public long? PeriodId { get; set; }
        public long? EmployeeId { get; set; }
        public long? TimetypeId { get; set; }
        public DateTime? Workingday { get; set; }
        public string? TimePoint1 { get; set; }
        public string? TimePoint4 { get; set; }
        public string? TimePointOt { get; set; }
        public string? OtStart { get; set; }
        public string? OtEnd { get; set; }
        public int? OtLateIn { get; set; }
        public int? OtEarlyOut { get; set; }
        public int? OtTime { get; set; }
        public int? OtTimeNight { get; set; }
        public bool? IsRegisterOff { get; set; }
        public bool? IsRegisterLateEarly { get; set; }
        public bool? IsHoliday { get; set; }
        public int? LateIn { get; set; }
        public int? EarlyOut { get; set; }
        public string? HoursStart { get; set; }
        public string? HoursStop { get; set; }
        public bool? IsEditIn { get; set; }
        public bool? IsEditOut { get; set; }
        public string? Note { get; set; }
        public long? MorningId { get; set; }
        public long? AfternoonId { get; set; }
    }
}
