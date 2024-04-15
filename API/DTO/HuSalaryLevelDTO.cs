namespace API.DTO
{
    public class HuSalaryLevelDTO
    {
        public long? Id { get; set; }
        public long? SalaryRankId { get; set; }
        public long? SalaryScaleId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? Orders { get; set; }
        public decimal? Monney { get; set; }
        public decimal? Coefficient { get; set; }
        public bool? IsActive { get; set; }
        public string? Note { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public decimal? PerformBonus { get; set; }
        public decimal? OtherBonus { get; set; }
        public decimal? TotalSalary { get; set; }
    }
}
