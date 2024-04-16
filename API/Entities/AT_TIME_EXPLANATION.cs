using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("AT_TIME_EXPLANATION")]
    public class AT_TIME_EXPLANATION:BASE_ENTITY
    {
        public long? EMPLOYEE_ID { get; set; }
        public DateTime? EXPLANATION_DAY { get; set; }
        public long? SHIFT_ID { get; set; }
        public DateTime? SWIPE_IN { get; set; }
        public DateTime? SWIPE_OUT { get; set; }
        public long? ACTUAL_WORKING_HOURS { get; set; }
        public string? EXPLANATION_CODE { get; set; }
        public string? REASON { get; set; }
        public long? REASON_ID { get; set; }
        public long? TYPE_REGISTER_ID { get; set; }
    }
}
