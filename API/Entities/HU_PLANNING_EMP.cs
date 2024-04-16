using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("HU_PLANNING_EMP")]
    public class HU_PLANNING_EMP : BASE_ENTITY
    {
        public long? PLANNING_ID { get; set; }
        public long? EMPLOYEE_ID { get; set; }
        public long? PLANNING_TITLE_ID { get; set; }
        public long? PLANNING_TYPE_ID { get; set; }
        public long? EVALUATE_ID { get; set; }
    }
}
