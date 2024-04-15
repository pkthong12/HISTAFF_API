using API.Main;

namespace API.DTO
{
    public class InsRegionDTO : BaseDTO
    {
        public string? RegionCode { get; set; }
        public long? AreaId { get; set; }
        public string? AreaName { get; set; }
        public string? OtherListCode { get; set; }
        public string? Note { get; set; }
        public string? Actflg { get; set; }
        public decimal? Money { get; set; }
        public DateTime? EffectDate { get; set; }
        public DateTime? ExprivedDate { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
        public decimal? CeilingUi { get; set; }
    }
}
