using API.All.HRM.DynamicReport;
using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("HU_DYNAMIC_REPORT_DTL_TOTAL")]
    public partial class HU_DYNAMIC_REPORT_DTL_TOTAL
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        //public string? REPORT_NAME_DETAIL { get; set; }
        public string? JSON { get; set; }
        public int? ORDERS { get; set; }
        public string? EXPRESSION { get; set; }
        //public int? CONDITION_FILTER_DETAIL { get; set; }
        public string? REPORT_NAME { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string? CREATED_BY { get; set; }
    }
}
