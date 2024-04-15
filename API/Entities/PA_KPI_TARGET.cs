using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PA_KPI_TARGET")]
public partial class PA_KPI_TARGET
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public long KPI_GROUP_ID { get; set; }

    public string? UNIT { get; set; }

    public int? COL_ID { get; set; }

    public string? COL_NAME { get; set; }

    public int? MAX_VALUE { get; set; }

    public bool? IS_REAL_VALUE { get; set; }

    public bool? IS_PAY_SALARY { get; set; }

    public bool? IS_IMPORT_KPI { get; set; }

    public bool IS_ACTIVE { get; set; }

    public bool? IS_SYSTEM { get; set; }

    public long? TYPE_ID { get; set; }

    public int ORDERS { get; set; }

    public string? NOTE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public virtual PA_KPI_GROUP KPI_GROUP { get; set; } = null!;

    //public virtual ICollection<PA_KPI_POSITION> PA_KPI_POSITIONs { get; set; } = new List<PA_KPI_POSITION>();

    //public virtual ICollection<PA_KPI_SALARY_DETAIL> PA_KPI_SALARY_DETAILs { get; set; } = new List<PA_KPI_SALARY_DETAIL>();
}
