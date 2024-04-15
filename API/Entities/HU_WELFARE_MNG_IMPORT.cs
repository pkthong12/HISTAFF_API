using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("HU_WELFARE_MNG_IMPORT")]
    [PrimaryKey(nameof(XLSX_USER_ID), nameof(XLSX_EX_CODE), nameof(XLSX_SESSION), nameof(XLSX_ROW))]
    public class HU_WELFARE_MNG_IMPORT : BASE_IMPORT
    {
        public long? ID { get; set; }
        public long? WELFARE_ID { get; set; }
        public long? EMPLOYEE_ID { get; set; }
        public long? PROFILE_ID { get; set; }
        public long? ORG_ID { get; set; }
        public long? POSITION_ID { get; set; }
        public DateTime? EFFECT_DATE { get; set; }
        public DateTime? EXPIRE_DATE { get; set; }
        public string? NOTE { get; set; }
        public decimal? MONEY { get; set; }
        public long? PERIOD_ID { get; set; }
        public DateTime? PAY_DATE { get; set; }
        public string? DECISION_CODE { get; set; }
        public long? IS_TRANSFER { get; set; }
        public long? IS_CASH { get; set; }
    }
}

