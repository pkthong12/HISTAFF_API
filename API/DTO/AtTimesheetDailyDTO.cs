namespace API.DTO
{
    public class AtTimesheetDailyDTO
    {
        public long? Id { get; set; }
        public long? EmployeeId { get; set; }
        public long? PeriodId { get; set; }
        public long? TimetypeId { get; set; }
        public DateTime? Workingday { get; set; }
        public int? OtTime { get; set; }
        public int? OtTimeNight { get; set; }
        public bool? IsRegisterOff { get; set; }
        public bool? IsRegisterLateEarly { get; set; }
        public bool? IsHoliday { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
