using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("INS_REGIMES_MNG")]
    public class INS_REGIMES_MNG:BASE_ENTITY
    {
        public long? EMPLOYEE_ID { get; set; }
        public long? REGIME_ID { get; set; }
        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }
        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        public long? DAY_CALCULATOR { get; set; }
        public long? ACCUMULATE_DAY { get; set; }
        public string? CHILDREN_NO { get; set; }
        public long? AVERAGE_SAL_SIX_MONTH { get; set; }
        public long? BHXH_SALARY { get; set; }
        public long? REGIME_SALARY { get; set; }
        public long? SUBSIDY_AMOUNT_CHANGE { get; set; }
        public long? SUBSIDY_MONEY_ADVANCE { get; set; }
        public DateTime? DECLARE_DATE { get; set; }
        public DateTime? DATE_CALCULATOR { get; set; }
        public long? INS_PAY_AMOUNT { get; set; }
        public DateTime? PAY_APPROVE_DATE { get; set; }
        public long? APPROV_DAY_NUM { get; set; }
        public bool? IS_ACTIVE { get; set; }
        public string? STATUS { get; set; }
        public string? NOTE { get; set; }
        public string? CREATED_LOG { get; set; }
        public string? UPDATED_LOG { get; set; }
    }
}
