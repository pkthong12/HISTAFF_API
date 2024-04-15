namespace API.DTO
{
    public class HuWorkingAllowDTO
    {
        public long? Id { get; set; }
        public long? WorkingId { get; set; }
        public long? AllowanceId { get; set; }
        public string? AllowanceName { get; set; }
        public decimal? Coefficient { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Effectdate { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}