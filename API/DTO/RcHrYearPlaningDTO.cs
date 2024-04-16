using API.Main;

namespace API.DTO
{
    public class RcHrYearPlaningDTO :BaseDTO
    {
        public int? Year { get; set; }
        public DateTime? EffectDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? Version { get; set; }
        public string? Note { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
        public long? OrgId { get; set; }
        public long? CopiedId { get; set; }
    }
}
