using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;
using API.Main;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Entities
{
    [Table("PA_SAL_IMPORT_BACKDATE")]
    public partial class PA_SAL_IMPORT_BACKDATE : BASE_ENTITY
    {
        public long? EMPLOYEE_ID { get; set; }
        public long? EMPLOYEE_ID_CV { get; set; }
        public string? NOTE { get; set; }
        public string? CREATED_LOG { get; set; }
        public string? UPDATED_LOG { get; set; }
        public long? PERIOD_ID { get; set; }
        public long? PERIOD_ADD_ID { get; set; }
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
        public float? LUONG_BHXH { get; set; }
        public float? LUONG_BHTN { get; set; }
        public float? LUONG_CO_BAN { get; set; }
        public long? TEST2 { get; set; }
        public long? AL1 { get; set; }
        public long? AL2 { get; set; }
        public long? AMNL002 { get; set; }
        public double? CLCHINH8 { get; set; }
        public double? DEDUCT5 { get; set; }

        public double? CL1 { get; set; }
        public double? CL27 { get; set; }

    }
}