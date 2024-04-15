using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("HU_ALLOWANCE_EMP_IMPORT")]
    [PrimaryKey(nameof(XLSX_USER_ID), nameof(XLSX_EX_CODE), nameof(XLSX_SESSION), nameof(XLSX_ROW))]
    public class HU_ALLOWANCE_EMP_IMPORT : BASE_IMPORT
    {
        public long? ID { get; set; }
        public long? EMPLOYEE_ID { get; set; }

        public long? ALLOWANCE_ID { get; set; }

        public decimal? MONNEY { get; set; }

        public decimal? COEFFICIENT { get; set; }

        public DateTime? DATE_START { get; set; }

        public DateTime? DATE_END { get; set; }

        public bool? IS_ACTIVE { get; set; }

        public string? NOTE { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string? CREATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public string? UPDATED_BY { get; set; }
    }
}
