using API.Main;

namespace API.DTO
{
    public class TrClassificationDTO : BaseDTO
    {
        public string? Name { get; set; }
        public long? DescId { get; set; }
        public string? DescName { get; set; }
        public double? ScoreFrom { get; set; }
        public double? ScoreTo { get; set; }
        public DateTime? EffectDate { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }

    }
}
