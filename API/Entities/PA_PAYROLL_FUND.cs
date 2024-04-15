using API.Main;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("PA_PAYROLL_FUND")]
    public class PA_PAYROLL_FUND:BASE_ENTITY
    {
        public int? YEAR { get; set; }
        public long? AMOUNT { get; set; }
        public string? NOTE { get; set; }
        public long? COMPANY_ID { get; set; }
        public long? SALARY_PERIOD_ID { get; set; }
        public long? LIST_FUND_ID { get; set; }
        public long? LIST_FUND_SOURCE_ID { get; set; }
        public DateTime? APPROVAL_DATE { get; set; }
        public string? CREATED_LOG { get; set; }
        public string? UPDATED_LOG { get; set; }
    }
}
