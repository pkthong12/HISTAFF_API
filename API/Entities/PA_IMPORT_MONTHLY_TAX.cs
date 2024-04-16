using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("PA_IMPORT_MONTHLY_TAX")]
    public class PA_IMPORT_MONTHLY_TAX : BASE_ENTITY
    {
        public long? PERIOD_ID {  get; set; }
        public long? EMPLOYEE_ID {  get; set; }
        public long? ORG_ID {  get; set; }
        public long? TITLE_ID {  get; set; }
        public long? OBJ_SALARY_ID {  get; set; }
        public long? WORKING_ID {  get; set; }
        public DateTime? FROM_DATE {  get; set; }
        public DateTime? TO_DATE {  get; set; }
        public DateTime? DATE_CALCULATE {  get; set; }
        public long? DATE_CALCULATE_ID {  get; set; }
        public string? CREATED_LOG { get; set; }
        public string? UPDATED_LOG {get; set;}
        public DateTime? JOIN_DATE {  get; set; }
        public long? JOBPOSITION_ID {  get; set; }
        public bool? IS_CONGDOAN {  get; set; }
        public bool? IS_KHOAN { get; set; }
        public double? CLCHINH8 { get; set; }
        public double? TAX20 { get; set; }
        public double? TAX21 { get; set; }
        public double? TAX23 { get; set; }
        public double? TAX24 { get; set; }
        public double? TAX25 { get; set; }
        public double? TAX26 { get; set; }
        public double? TAX27 { get; set; }
        public double? TAX28 { get; set; }
        public double? TAX29 { get; set; }
        public double? TAX30 { get; set; }
        public double? TAX31 { get; set; }
        public double? TAX32 { get; set; }
        public double? TAX33 { get; set; }
        public double? TAX34 { get; set; }
        public double? TAX35 { get; set; }
        public double? TAX36 { get; set; }
        public double? TAX37 { get; set; }
        public double? TAX38 { get; set; }
        public double? TAX41 { get; set; }
        public double? TAX42 { get; set; }
        public double? TAX43 { get; set; }
        public double? TAX44 { get; set; }
        public double? DEDUCT5 { get; set; }
        public string? NOTE { get; set; }

    }
}
