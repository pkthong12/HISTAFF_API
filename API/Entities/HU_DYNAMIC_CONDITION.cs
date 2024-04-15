using API.All.HRM.DynamicReport;
using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("HU_DYNAMIC_CONDITION")]
    public partial class HU_DYNAMIC_CONDITION
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string? CONDITION { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }
        public string? CREATED_BY { get; set; }
        public long? VIEW_ID { get; set; }
        public long? REPORT_DETAIL_ID { get; set; }
        public int? CONDITION_INDEX { get; set; }
        public string? VALUE_COMPARE { get; set; }
        public long? PARENT_ID { get; set; }
        public EnumConditionFilterDetail? CONDITION_FILTER_DETAIL { get; set; }
        public EnumConditionFilter? CONDITION_FILTER { get; set; }
    }
}
