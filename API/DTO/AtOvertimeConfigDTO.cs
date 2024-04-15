namespace API.DTO
{
    public class AtOvertimeConfigDTO
    {
        public long? Id { get; set; }
        public int? HourMin { get; set; }
        public int? HourMax { get; set; }
        public decimal? FactorNt { get; set; }
        public decimal? FactorNn { get; set; }
        public decimal? FactorNl { get; set; }
        public decimal? FactorDnt { get; set; }
        public decimal? FactorDnn { get; set; }
        public decimal? FactorDnl { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedDate { get; set; }
        public string? UpdatedDate { get; set; }
    }
}
