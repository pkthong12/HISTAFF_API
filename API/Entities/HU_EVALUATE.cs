
using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("HU_EVALUATE")]
    public class HU_EVALUATE: BASE_ENTITY
    {
        public long? EVALUATE_TYPE { get; set; }
        public long? EMPLOYEE_ID { get; set; }
        public string? EMPLOYEE_CODE { get; set; }
        public string? EMPLOYEE_NAME { get; set; }
        public long? ORG_ID { get; set; }
        public long? POSITION_ID { get; set; }
        public int? YEAR { get; set; }
        public long? CLASSIFICATION_ID { get; set; }
        public int? POINT { get; set; }
        public string? NOTE { get; set; }
        public string? POSITION_NAME { get; set; }
        public string? ORG_NAME { get; set; }
        public string? POSITION_CONCURRENT_NAME { get; set; }
        public string? ORG_CONCURRENT_NAME { get; set; }
        public long? POSITION_CONCURRENT_ID { get; set; }
        public long? EMPLOYEE_CONCURRENT_ID { get; set; }
        public string? EMPLOYEE_CONCURRENT_NAME { get; set; }
        public long? ORG_CONCURRENT_ID { get; set; }
        public long? PROFILE_ID { get; set; }


    }
}
