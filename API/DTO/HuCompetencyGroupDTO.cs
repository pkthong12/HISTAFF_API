using API.Main;

namespace API.DTO
{
    public class HuCompetencyGroupDTO : BaseDTO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
    }
}
