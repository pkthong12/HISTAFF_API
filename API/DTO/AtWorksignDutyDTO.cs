namespace API.DTO
{
    public class AtWorksignDutyDTO
    {
        public long? Id { get; set; }
        public long? EmployeeId { get; set; }
        public long? PeriodId { get; set; }
        public long? ShiftId { get; set; }
        public DateTime? Workingday { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
