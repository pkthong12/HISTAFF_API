using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PA_PAYROLL_TAX_YEAR")]

public class PA_PAYROLL_TAX_YEAR : BASE_ENTITY
{
    public long? EMPLOYEE_ID { get; set; }
    public long? ORG_ID { get; set; }
    public long? YEAR { get; set; }
    public long? OBJ_SALARY_ID { get; set; }
    public int? WORKING_ID { get; set; }
    public DateTime? FROM_DATE { get; set; }
    public DateTime? TO_DATE { get; set; }
    public int? IS_LOCK { get; set; }
}
