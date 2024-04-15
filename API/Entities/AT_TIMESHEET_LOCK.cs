using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_TIMESHEET_LOCK")]
public partial class AT_TIMESHEET_LOCK
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long PERIOD_ID { get; set; }

    public int ORG_ID { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public virtual AT_SALARY_PERIOD PERIOD { get; set; } = null!;
}
