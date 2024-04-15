using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("HU_CONCURRENTLY")]
    public partial class HU_CONCURRENTLY : BASE_ENTITY
    {
        public DateTime? EFFECTIVE_DATE { get; set; }
        public DateTime? EXPIRATION_DATE { get; set; }
        public DateTime? SIGNING_DATE { get; set; }
        public bool? IS_ACTIVE { get; set; }
        public string? DECISION_NUMBER { get; set; }
        public long? ORG_ID { get; set; }
        public long? POSITION_ID { get; set; }
        public long? EMPLOYEE_ID { get; set; }
        public long? SIGNING_EMPLOYEE_ID { get; set; }
        public string? SIGNING_POSITION_NAME { get; set; }
        public string? NOTE { get; set; }
        public long? STATUS_ID { get; set; }
        public bool? IS_FROM_WORKING { get; set; }
        public long? POSITION_POLITICAL_ID { get; set; }
        public long? WORKING_ID { get; set; }
    }
}
