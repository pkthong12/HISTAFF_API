using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("PA_SAL_IMPORT_ADD")]
    public class PA_SAL_IMPORT_ADD : BASE_ENTITY
    {
        public long? PERIOD_ID { get; set; }
        public long? EMPLOYEE_ID { get; set; }
        public long? ORG_ID { get; set; }
        public long? PHASE_ID { get; set; }
        public long? TITLE_ID { get; set; }
        public long? OBJ_SALARY_ID { get; set; }
        public long? WORKING_ID { get; set; }
        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }
        public string? CREATED_LOG { get; set; }
        public string? UPDATED_LOG { get; set; }
        public DateTime? JOIN_DATE { get; set; }
        public long? JOBPOSITION_ID { get; set; }
        public bool? IS_CONGDOAN { get; set; }
        public bool? IS_KHOAN { get; set; }
        public double? CLCHINH8 { get; set; }
        public double? CLCHINH3 { get; set; }
        public double? CLCHINH4 { get; set; }
        public string? NOTE { get; set; }
        public double? LBS9 { get; set; }
        public double? LBS23 { get; set; }
        public double? LBS25 { get; set; }

    }
}
