namespace API.DTO
{
    public class AtTimeLateEarlyDTO
    {
        public long? Id { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int? TimeLate { get; set; }
        public int? TimeEarly { get; set; }
        public long? EmployeeId { get; set; }
        public long? StatusId { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
