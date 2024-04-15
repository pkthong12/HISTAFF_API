namespace API.DTO
{
    public class PaKpiPositionDTO
    {
        public long? Id { get; set; }
        public long? KpiTargetId { get; set; }
        public long? PositionId { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
