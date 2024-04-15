namespace API.DTO
{
    public class AtApprovedDTO
    {
        public long? Id { get; set; }
        public long? RegisterId { get; set; }
        public long? EmpResId { get; set; }
        public long? ApproveId { get; set; }
        public string? ApproveName { get; set; }
        public string? ApprovePos { get; set; }
        public DateTime? ApproveDay { get; set; }
        public string? ApproveNote { get; set; }
        public long? TypeId { get; set; }
        public bool? IsReg { get; set; }
        public long? StatusId { get; set; }
        public bool? IsRead { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? EmployeeId { get; set; }
        public long? TimeTypeId { get; set; }
    }
}
