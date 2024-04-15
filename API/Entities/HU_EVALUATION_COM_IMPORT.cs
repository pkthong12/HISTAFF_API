using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("HU_EVALUATION_COM_IMPORT")]
    [PrimaryKey(nameof(XLSX_USER_ID), nameof(XLSX_EX_CODE), nameof(XLSX_SESSION), nameof(XLSX_ROW))]
    public partial class HU_EVALUATION_COM_IMPORT: BASE_IMPORT
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long? ID { get; set; }
        public long? EMPLOYEE_ID { get; set; }
        public int? YEAR_EVALUATION { get; set; }
        public long? CLASSIFICATION_ID { get; set; }
        public int? POINT_EVALUATION { get; set; }
        public string? NOTE { get; set; }
        public long? PROFILE_ID { get; set; }
    }
}
