namespace API.DTO
{
    public class HuPosPaperDTO
    {
        public long? Id { get; set; }
        public long? PosId { get; set; }
        public long? PaperId { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
