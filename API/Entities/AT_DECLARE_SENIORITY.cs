using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_DECLARE_SENIORITY")]
public partial class AT_DECLARE_SENIORITY
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }
    public long EMPLOYEE_ID { get; set; }
    public long? YEAR_DECLARE { get; set; }
    public long? MONTH_ADJUST { get; set; }
    public long? MONTH_ADJUST_NUMBER { get; set; }
    public string? REASON_ADJUST { get; set; }
    public long? MONTH_DAY_OFF { get; set; }
    public double? NUMBER_DAY_OFF { get; set; }
    public string? REASON_ADJUST_DAY_OFF { get; set; }
    public double? TOTAL { get; set; }
    public string? CREATED_BY { get; set; }
    public string? UPDATED_BY { get; set; }
    public DateTime? CREATED_DATE { get; set; }
    public DateTime? UPDATED_DATE { get; set; }

}
