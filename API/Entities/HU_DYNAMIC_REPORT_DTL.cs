using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("HU_DYNAMIC_REPORT_DTL")]
    public partial class HU_DYNAMIC_REPORT_DTL
    {
        public long? ID { get; set; }
        public long? VIEW_ID { get; set; }
        public string? COLUMN_NAME{ get; set; }
        public string? COLUMN_TYPE{ get; set; }
        public string? TRANSLATE{ get; set; }
        public int? COLUMN_ORDER{ get; set; }
    }
}
