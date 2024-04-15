using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("HU_WELFARE_AUTO")]
    public class HU_WELFARE_AUTO: BASE_ENTITY
    {
        public long? EMPLOYEE_ID { get; set; }
        public long? WELFARE_ID { get; set; }
        public long? MONEY { get; set; }
        public long? MONEY_APPROVED { get; set; }
        public long? ORG_ID { get; set; }
        public string? REMARK { get; set; }
        public long?  GENDER_ID { get; set; }
        public long? SALARY_PERIOD_ID { get; set; }
        public int? SENIORITY { get; set; }
        public int? COUNT_CHILD { get; set; }
        public string? CONTRACT_TYPE_NAME { get; set; }
        public int? MONEY_ADJUST { get; set; }
        public bool? IS_PAY { get; set; }
        public int? CHILD_AGE { get; set; }
        public DateTime? PAY_DATE { get; set; }
        public DateTime? CALCULATE_DATE { get; set; }
        public bool? IS_DOCUMENT_OFF { get; set; }
        public int? NUMBER_MANUAL { get; set; } = 0;
        public int? IS50 { get; set; }
        public DateTime? BIRTH_DATE { get; set; }
        public DateTime? EFFECTIVE_DATE { get; set; }
        public DateTime? EXPIRATION_DATE { get; set; }
        public int? YEAR { get; set; }

    }
}
