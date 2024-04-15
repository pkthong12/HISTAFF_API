using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("HU_COMMEND_EMPLOYEE_IMPORT")]
    [PrimaryKey(nameof(XLSX_USER_ID), nameof(XLSX_EX_CODE), nameof(XLSX_SESSION), nameof(XLSX_ROW))]
    public class HU_COMMEND_EMPLOYEE_IMPORT : BASE_IMPORT
    {
        public long? ID { get; set; }
        public long? PROFILE_ID { get; set; }
        public long? EMPLOYEE_ID { get; set; }
        public long? COMMEND_ID { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string? CREATED_BY { get; set; }
        public string? UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public bool? IS_ACTIVE { get; set; }
        public long? STATUS_ID { get; set; }
    }
}
