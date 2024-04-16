using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("RC_HR_PLANING_DETAIL")]
    public class RC_HR_PLANING_DETAIL: BASE_ENTITY
    {
        public long? ORG_ID { get; set; }
        public long? POSITION_ID { get; set; }
        public long? MONTH_1 { get; set; }
        public long? MONTH_2 { get; set; }
        public long? MONTH_3 { get; set; }
        public long? MONTH_4 { get; set; }
        public long? MONTH_5 { get; set; }
        public long? MONTH_6 { get; set; }
        public long? MONTH_7 { get; set; }
        public long? MONTH_8 { get; set; }
        public long? MONTH_9 { get; set; }
        public long? MONTH_10 { get; set; }
        public long? MONTH_11 { get; set; }
        public long? MONTH_12 { get; set; }
        public long? RANK_SAL { get; set; }
        public long? YEAR_PLAN_ID { get; set; }
        public string? CREATED_LOG { get; set; }
        public string? UPDATED_LOG { get; set; }
        public long? INDIRECT_1 { get; set; }
        public long? INDIRECT_2 { get; set; }
        public long? INDIRECT_3 { get; set; }
        public long? INDIRECT_4 { get; set; }
        public long? INDIRECT_5 { get; set; }
        public long? INDIRECT_6 { get; set; }
        public long? INDIRECT_7 { get; set; }
        public long? INDIRECT_8 { get; set; }
        public long? INDIRECT_9 { get; set; }
        public long? INDIRECT_10 { get; set; }
        public long? INDIRECT_11 { get; set; }
        public long? INDIRECT_12 { get; set; }
        public long? DB_DIRECT_1 { get; set; }
        public long? DB_DIRECT_2 { get; set; }
        public long? DB_DIRECT_3 { get; set; }
        public long? DB_DIRECT_4 { get; set; }
        public long? DB_DIRECT_5 { get; set; }
        public long? DB_DIRECT_6 { get; set; }
        public long? DB_DIRECT_7 { get; set; }
        public long? DB_DIRECT_8 { get; set; }
        public long? DB_DIRECT_9 { get; set; }
        public long? DB_DIRECT_10 { get; set; }
        public long? DB_DIRECT_11 { get; set; }
        public long? DB_DIRECT_12 { get; set; }
        public long? DB_INDIRECT_1 { get; set; }
        public long? DB_INDIRECT_2 { get; set; }
        public long? DB_INDIRECT_3 { get; set; }
        public long? DB_INDIRECT_4 { get; set; }
        public long? DB_INDIRECT_5 { get; set; }
        public long? DB_INDIRECT_6 { get; set; }
        public long? DB_INDIRECT_7 { get; set; }
        public long? DB_INDIRECT_8 { get; set; }
        public long? DB_INDIRECT_9 { get; set; }
        public long? DB_INDIRECT_10 { get; set; }
        public long? DB_INDIRECT_11 { get; set; }
        public long? DB_INDIRECT_12 { get; set; }
        public long? COST_CENTER { get; set; }
    }
}
