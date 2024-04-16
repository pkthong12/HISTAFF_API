using API.Main;

namespace API.DTO
{
    public class HuCompetencyPeriodDTO : BaseDTO
    {
        public int? Year { get; set; }
        public string? Name { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
        public string? Code { get; set; }
        public long? QuarterId { get; set; }
        public string? QuarterName { get; set; }
        public DateTime? EffectedDate { get; set; }
        public DateTime? ExpriedDate { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
    }
}
