using API.Main;

namespace API.DTO
{
    public class HuCompetencyAspectDTO : BaseDTO
    {
        public long? CompetencyId { get; set; }
        public string? CompetencyName { get; set; }
        public string? CompetencyGroupName { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
    }
}
