using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("SE_AUTHORIZE_APPROVE")]
    public class SE_AUTHORIZE_APPROVE : BASE_ENTITY
    {
        public long? PROCESS_ID { get; set; }
        public long? LEVEL_ORDER_ID { get; set; }
        public long? EMPLOYEE_ID { get; set; }
        public long? EMPLOYEE_AUTH_ID { get; set; }
        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }
        public bool? IS_PER_REPLACE { get; set; }

    }
}
