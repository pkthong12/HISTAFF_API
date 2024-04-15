using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_TIMESHEET_DAILY")]
public partial class AT_TIMESHEET_DAILY
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long EMPLOYEE_ID { get; set; }

    public long PERIOD_ID { get; set; }

    public DateTime WORKINGDAY { get; set; }

    public long TIMETYPE_ID { get; set; }

    public int? OT_TIME { get; set; }

    public int? OT_TIME_NIGHT { get; set; }

    public bool? IS_REGISTER_OFF { get; set; }

    public bool? IS_REGISTER_LATE_EARLY { get; set; }

    public bool? IS_HOLIDAY { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public virtual HU_EMPLOYEE EMPLOYEE { get; set; } = null!;

    public virtual AT_SALARY_PERIOD PERIOD { get; set; } = null!;

    public virtual AT_TIME_TYPE TIMETYPE { get; set; } = null!;
}
