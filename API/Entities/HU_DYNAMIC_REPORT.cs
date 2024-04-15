using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("HU_DYNAMIC_REPORT")]
    public partial class HU_DYNAMIC_REPORT
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        //public string? REPORT_NAME_DETAIL { get; set; }
        public string? JSON { get; set; }
        //public int? ORDERS { get; set; }
        public string? EXPRESSION { get; set; }
        //public int? CONDITION_FILTER_DETAIL { get; set; }
        public string? REPORT_NAME { get; set; }
        public string? FORM { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string? CREATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public string? UPDATED_BY { get; set; }
        public string? VIEW_NAME { get; set; }
        public string? SELECTED_COLUMNS { get; set; }
        public string? PREFIX_TRANS { get; set; }
    }
}
