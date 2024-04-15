using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_KPI")]
public partial class SYS_KPI
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public long KPI_GROUP_ID { get; set; }

    public string? UNIT { get; set; }

    public bool IS_REAL_VALUE { get; set; }

    public bool IS_PAY_SALARY { get; set; }

    public bool IS_IMPORT_KPI { get; set; }

    public bool? IS_SYSTEM { get; set; }

    public bool IS_ACTIVE { get; set; }

    public int ORDERS { get; set; }

    public string? NOTE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
}
