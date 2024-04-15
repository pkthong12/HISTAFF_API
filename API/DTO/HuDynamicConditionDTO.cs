using API.Main;

namespace API.DTO
{
    public class HuDynamicConditionDTO : BaseDTO
    {
        public int? ViewId { get; set; }
        public string? ReportName { get; set; }
        public int? ConditionFilter{ get; set; }
        public int? ConditionFilterDetail{ get; set; }
        public long? ParentId { get; set; }
        public string? ReportDetailName { get; set; }
        public long? ReportDetailId { get; set; }
        public int? ConditionIndex { get; set; }
    }
}
