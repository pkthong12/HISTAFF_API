using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("TR_REIMBURSEMENT")]
public class TR_REIMBURSEMENT : BASE_ENTITY
{
    public int? YEAR { get; set; }
    public long? TR_PROGRAM_ID { get; set; }
    public long? EMPLOYEE_ID { get; set; }
    public DateTime? START_DATE { get; set; }
    public string? CREATED_LOG { get; set; }
    public string? UPDATED_LOG { get; set; }
    public decimal? COST_REIMBURSE { get; set; }
    public bool? IS_RESERVES { get; set; }
    public string? REMARK { get; set; }
    public DateTime? DATE_FROM_COMMITMENT { get; set; }
    public DateTime? END_DATE_TO_COMMITMENT { get; set; }
    public DateTime? END_DATE_FROM_COMMITMENT { get; set; }
    public DateTime? DATE_TO_COMMITMENT { get; set; }
    public bool? IS_LAVE_DATE {get;set;}
    public DateTime? FINAL_DATE  {get;set;}
    public DateTime? MONTH_REIMBURSEMENT { get; set; }

}
