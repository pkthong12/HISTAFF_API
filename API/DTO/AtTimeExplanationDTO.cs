using API.Main;

namespace API.DTO
{
    public class AtTimeExplanationDTO:BaseDTO
    {
        public long? EmployeeId { get; set; }
        public long? OrgId { get; set; }
        public long? PeriodId { get; set; }
        public long? Year { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? PositionName { get; set; }
        public string? OrgName { get; set; }
        public string? ShiftYear { get; set; }
        public DateTime? ExplanationDay { get; set; }
        public long? ShiftId { get; set; }
        public string? ShiftName { get; set; }
        public DateTime? SwipeIn { get; set; }
        public DateTime? SwipeOut { get; set; }
        public long? ActualWorkingHours { get; set; }
        public string? ExplanationCode { get; set; }
        public string? Reason { get; set; }
        public int? JobOrderNum { get; set; }
    }
}
