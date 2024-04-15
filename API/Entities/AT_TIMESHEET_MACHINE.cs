using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_TIMESHEET_MACHINE")]
public partial class AT_TIMESHEET_MACHINE
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long? TENANT_ID { get; set; }

    public long? PERIOD_ID { get; set; }

    public long? EMPLOYEE_ID { get; set; }

    public DateTime? WORKINGDAY { get; set; }

    public long? TIMETYPE_ID { get; set; }

    public string? TIME_POINT1 { get; set; }

    public string? TIME_POINT4 { get; set; }

    public string? TIME_POINT_OT { get; set; }

    public string? OT_START { get; set; }

    public string? OT_END { get; set; }

    public int? OT_LATE_IN { get; set; }

    public int? OT_EARLY_OUT { get; set; }

    public int? OT_TIME { get; set; }

    public int? OT_TIME_NIGHT { get; set; }

    public bool? IS_REGISTER_OFF { get; set; }

    public bool? IS_REGISTER_LATE_EARLY { get; set; }

    public bool? IS_HOLIDAY { get; set; }

    public int? LATE_IN { get; set; }

    public int? EARLY_OUT { get; set; }

    public string? HOURS_START { get; set; }

    public string? HOURS_STOP { get; set; }

    public bool? IS_EDIT_IN { get; set; }

    public bool? IS_EDIT_OUT { get; set; }

    public string? NOTE { get; set; }

    public long? MORNING_ID { get; set; }

    public long? AFTERNOON_ID { get; set; }

    public virtual HU_EMPLOYEE? EMPLOYEE { get; set; }
}
