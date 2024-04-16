using API.Main;

namespace API.DTO
{
    public class HuCompetencyDictDTO : BaseDTO
    {
        public long? CompetencyGroupId { get; set; }
        public string? CompetencyGroupName { get; set; }
        public long? CompetencyId { get; set; }
        public string? CompetencyName { get; set; }
        public long? LevelNumber { get; set; }
        public string? LevelNumberName { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
        public long? CompetencyAspectId { get; set; }
        public string? CompetencyAspectName { get; set; }
    }
}
