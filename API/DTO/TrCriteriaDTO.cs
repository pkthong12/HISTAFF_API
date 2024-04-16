using API.Main;

namespace API.DTO
{
    public class TrCriteriaDTO : BaseDTO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? MaxScore { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
    }
}
