using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("PA_TAX_ANNUAL_IMPORT")]
    public class PA_TAX_ANNUAL_IMPORT : BASE_ENTITY
    {
        public long? YEAR { get; set; }
        public long? EMPLOYEE_ID { get; set; }
        public long? ORG_ID { get; set; }
        public long? TITLE_ID { get; set; }
        public long? OBJ_SALARY_ID { get; set; }
        public long? WORKING_ID { get; set; }
        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }
        public DateTime? JOIN_DATE { get; set; }
        public long? JOBPOSITION_ID { get; set; }
        public bool? IS_CONGDOAN { get; set; }
        public bool? IS_KHOAN { get; set; }
        public double? LUONG_BHXH { get; set; }
        public double? LUONG_BHTN { get; set; }
        public double? LUONG_CO_BAN { get; set; }
        public double? AL2 { get; set; }
        public double? AMNL002 { get; set; }
        public string? KC001 { get; set; }
        public string? NOTE { get; set; }
        public double? AAA { get; set; }
        public double? PPPP { get; set; }
        public double? TTTTT { get; set; }
        public double? YYYYYY { get; set; }
        public double? DMLK01 { get; set; }
        public double? A002 { get; set; }
        public double? DEDUCT5 { get; set; }
        public double? CLCHINH3 { get; set; }
        public double? CLCHINH4 { get; set; }
        public double? CL1 { get; set; }
        public double? CL27 { get; set; }
        public double? TAX18 { get; set; }
        public double? TAX26 { get; set; }
    }

}
