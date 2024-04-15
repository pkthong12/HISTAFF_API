using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PA_KPI_POSITION")]
public partial class PA_KPI_POSITION
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long KPI_TARGET_ID { get; set; }

    public long? POSITION_ID { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public virtual PA_KPI_TARGET KPI_TARGET { get; set; } = null!;

    public virtual HU_POSITION? POSITION { get; set; }
}
