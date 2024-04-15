using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("AT_OTHER_LIST")]
    public class AT_OTHER_LIST: BASE_ENTITY
    {
        public DateTime? EFFECT_DATE { get; set; }
        public DateTime? EXPIRATION_DATE { get; set; }
        public bool? IS_ENTIRE_YEAR { get; set; }
        public int? MAX_WORKING_MONTH { get; set; }
        public int? MAX_WORKING_YEAR { get; set; }
        public double? OVERTIME_DAY_WEEKDAY { get; set; }
        public double? OVERTIME_DAY_HOLIDAY { get; set; }
        public double? OVERTIME_DAY_OFF { get; set; }
        public double? OVERTIME_NIGHT_WEEKDAY { get; set; }
        public double? OVERTIME_NIGHT_HOLIDAY { get; set; }
        public double? OVERTIME_NIGHT_OFF { get; set; }
        public bool? IS_ACTIVE { get; set; }
        public string? NOTE { get; set; }
        public decimal? PERSONAL_DEDUCTION_AMOUNT { get; set; }
        public decimal? SELF_DEDUCTION_AMOUNT { get; set; }
        public decimal? BASE_SALARY { get; set; }
        public decimal? WORKDAY_UNIT_PRICE { get; set; }

    }
}
