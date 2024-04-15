using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_SALARY_STRUCTURE")]
public partial class SYS_SALARY_STRUCTURE
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long AREA_ID { get; set; }

    public long SALARY_TYPE_ID { get; set; }

    public long ELEMENT_ID { get; set; }

    public bool IS_VISIBLE { get; set; }

    public bool IS_CALCULATE { get; set; }

    public bool IS_IMPORT { get; set; }

    public int? ORDERS { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public virtual SYS_PA_ELEMENT ELEMENT { get; set; } = null!;

    public virtual SYS_SALARY_TYPE SALARY_TYPE { get; set; } = null!;
}
