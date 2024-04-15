namespace API.DTO
{
    public class HuAnswerUserDTO
    {
        public long? Id { get; set; }
        public long? AnswerId { get; set; }
        public long? EmpId { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
