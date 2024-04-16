using API.Main;

namespace API.DTO
{
    public class TrSettingCriDetailDTO : BaseDTO
    {
        public long? CriteriaId { get; set; }
        public string? CriteriaName { get; set; }
        public double? Ratio { get; set; }
        public double? PointMax { get; set; }
        public long? CourseId { get; set; }
    }
}
