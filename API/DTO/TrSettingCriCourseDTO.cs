using API.Main;

namespace API.DTO
{
    public class TrSettingCriCourseDTO : BaseDTO
    {
        public long? TrCourseId { get; set; }
        public string? TrCourseName { get; set; }
        public long? TrCriteriaGroupId { get; set; }
        public string? TrCriteriaGroupName { get; set; }
        public DateTime? EffectFrom { get; set; }
        public DateTime? EffectTo { get; set; }
        public string? Remark { get; set; }
        public double? ScalePoint { get; set; }
        public List<TrSettingCriDetailDTO>? TrSettingCriDetails { get; set; }
    }
}
