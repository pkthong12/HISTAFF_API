using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities.PORTAL
{
    [Table("PORTAL_REGISTER_OFF")]
    public partial class PORTAL_REGISTER_OFF : BASE_ENTITY
    {
        public long EMPLOYEE_ID { get; set; }
        public DateTime? DATE_START { get; set; }
        public DateTime? DATE_END { get; set; }
        public DateTime? TIME_START { get; set; }
        public DateTime? TIME_END { get; set; }
        public DateTime? WORKING_DAY { get; set; }
        public int? TIME_LATE { get; set; }
        public int? TIME_EARLY { get; set; }
        public long? TIME_TYPE_ID { get; set; }
        public long? TYPE_ID { get; set; }
        public long? RECEIVE_WORKER_ID { get; set; }
        public long? STATUS_ID { get; set; }
        public long? APPROVE_ID { get; set; }
        public string? NOTE { get; set; }
        public string? APPROVE_NAME { get; set; }
        public string? APPROVE_POS { get; set; }
        public string? APPROVE_NOTE { get; set; }
        public DateTime? APPROVE_DAY { get; set; }
        public bool? IS_ACTIVE { get; set; }
        public string? TYPE_CODE { get; set; }
        public int? TOTAL_DAY { get; set; }
        public decimal? TOTAL_OT { get; set; }
        public string? ID_REGGROUP { get; set; }
        public bool? IS_EACH_DAY { get; set; }
        public long? EXPLAIN_REASON { get; set; }
        public bool? IS_REGISTER { get; set; }
    }
}
