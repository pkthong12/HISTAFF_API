using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("HU_EVALUATE_IMPORT")]
    [PrimaryKey(nameof(XLSX_USER_ID), nameof(XLSX_EX_CODE), nameof(XLSX_SESSION), nameof(XLSX_ROW))]
    public partial class HU_EVALUATE_IMPORT:BASE_IMPORT
    {
        public long? ID { get; set; }
        public long? EVALUATE_TYPE { get; set; }
        public long? EMPLOYEE_ID { get; set; }
        public long? ORG_ID { get; set; }
        public long? POSITION_ID { get; set; }
        public int? YEAR { get; set; }
        public long? CLASSIFICATION_ID { get; set; }
        public int? POINT { get; set; }
        public string? NOTE { get; set; }
        public long? POSITION_CONCURRENT_ID { get; set; }
        public long? EMPLOYEE_CONCURRENT_ID { get; set; }
        public long? ORG_CONCURRENT_ID { get; set; }
        public long? PROFILE_ID { get; set; }
    }
}
