using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PA_PAYROLLSHEET_SUM_SUB")]

public class PA_PAYROLLSHEET_SUM_SUB : BASE_ENTITY
{
    public long? PERIOD_ID { get; set; }
    public long? EMPLOYEE_ID { get; set; }
    public long? ORG_ID { get; set; }
    public long? TITLE_ID { get; set; }
    public long? OBJ_SALARY_ID { get; set; }
    public int? WORKING_ID { get; set; }
    public DateTime? FROM_DATE { get; set; }
    public DateTime? TO_DATE { get; set; }
    public DateTime? JOIN_DATE { get; set; }
    public long? JOBPOSITION_ID { get; set; }
    public long? FUND_ID { get; set; }
}

