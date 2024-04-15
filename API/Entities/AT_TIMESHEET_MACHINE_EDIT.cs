using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_TIMESHEET_MACHINE_EDIT")]
public partial class AT_TIMESHEET_MACHINE_EDIT
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public int? TENANT_ID { get; set; }

    public long? PERIOD_ID { get; set; }

    public long? EMPLOYEE_ID { get; set; }

    public DateTime? WORKINGDAY { get; set; }

    public string? TIME_POINT1 { get; set; }

    public string? TIME_POINT4 { get; set; }

    public bool? IS_EDIT_IN { get; set; }

    public bool? IS_EDIT_OUT { get; set; }

    public string? NOTE { get; set; }

    public virtual HU_EMPLOYEE? EMPLOYEE { get; set; }

    public virtual AT_SALARY_PERIOD? PERIOD { get; set; }
}
