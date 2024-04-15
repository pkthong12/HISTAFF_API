namespace API.DTO
{
    public class HuAnswerDTO
    {
        public long? Id { get; set; }
        public string? Answer { get; set; }
        public long? QuestionId { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
