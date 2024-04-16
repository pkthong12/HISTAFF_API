using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("PA_SAL_IMPORT")]
    public class PA_SAL_IMPORT
    {
        public long ID { get; set; }
        public long? PERIOD_ID { get; set; }
        public long? EMPLOYEE_ID { get; set; }
        public long? ORG_ID { get; set; }
        public long? TITLE_ID { get; set; }
        public long? OBJ_SALARY_ID { get; set; }
        public long? WORKING_ID { get; set; }
        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string CREATED_LOG { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public string UPDATED_LOG { get; set; }
        public DateTime? JOIN_DATE { get; set; }
        public long? JOBPOSITION_ID { get; set; }
        public bool? IS_CONGDOAN { get; set; }
        public bool? IS_KHOAN { get; set; }
        public double? LUONG_BHXH { get; set; }
        public double? LUONG_BHTN { get; set; }
        public double? LUONG_CO_BAN { get; set; }
        public double? CL11 { get; set; }
        public double? CL29 { get; set; }
        public double? CL31 { get; set; }
        public double? CL33 { get; set; }
        public double? CLCHINH8 { get; set; }
        public double? CSUM1 { get; set; }
        public double? CSUM2 { get; set; }
        public double? DEDUCT3 { get; set; }
        public double? TAX54 { get; set; }

        public string? NOTE { get; set; }

    }

}
