using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PA_KPI_LOCK")]
public partial class PA_KPI_LOCK
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long PERIOD_ID { get; set; }

    public long? ORG_ID { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public virtual HU_ORGANIZATION? ORG { get; set; }

    public virtual AT_SALARY_PERIOD PERIOD { get; set; } = null!;
}
