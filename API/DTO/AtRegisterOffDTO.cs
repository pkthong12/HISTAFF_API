namespace API.DTO
{
    public class AtRegisterOffDTO
    {
        public long? Id { get; set; }
        public long? EmployeeId { get; set; }

        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public DateTime? WorkingDay { get; set; }
        public int? TimeLate { get; set; }
        public int? TimeEarly { get; set; }
        public long? TimetypeId { get; set; }
        public string? Note { get; set; }
        public long? TypeId { get; set; }
        public long? StatusId { get; set; }
        public long? ApproveId { get; set; }
        public string? ApproveName { get; set; }
        public string? ApprovePos { get; set; }
        public DateTime? ApproveDay { get; set; }
        public string? ApproveNote { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
