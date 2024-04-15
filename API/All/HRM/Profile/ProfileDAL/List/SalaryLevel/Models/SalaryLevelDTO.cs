using Common.Paging;

namespace ProfileDAL.ViewModels
{
    public class SalaryLevelDTO : Pagings
    {
        public long? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? Orders { get; set; }
        public long? SalaryRankId { get; set; }
        public string? SalaryRankName { get; set; }
        public string? SalaryScaleName { get; set; }
        public long? SalaryScaleId { get; set; }
        public decimal? PerformBonus { get; set; }
        public decimal? OtherBonus { get; set; }
        public decimal? TotalSalary { get; set; }
        public decimal? Monney { get; set; }
        public bool? IsActive { get; set; }
        public string? Note { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public int? HoldingMonth { get; set; }
        public DateTime? HoldingTime { get; set; }
        public decimal? Coefficient { get; set; }
        public long? RegionId { get; set; }
        public string? RegionName { get; set; }
    }

    public class SalaryLevelInputDTO
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Orders { get; set; }
        public long SalaryRankId { get; set; }
        public long SalaryScaleId { get; set; }
        public decimal? Monney { get; set; }
        public decimal? Coefficient { get; set; }
        public bool? IsActive { get; set; }
        public string? Note { get; set; }
        public decimal? TotalSalary { get; set; }
        public decimal? OtherBonus { get; set; }
        public decimal? PerformBonus { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public int? HoldingMonth { get; set; }
        public DateTime? HoldingTime { get; set; }
        public long? RegionId { get; set; }


    }
}
