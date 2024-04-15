using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PA_SALARY_STRUCTURE")]
public partial class PA_SALARY_STRUCTURE
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public int SALARY_TYPE_ID { get; set; }

    public long ELEMENT_ID { get; set; }

    public bool? IS_VISIBLE { get; set; }

    public bool? IS_CALCULATE { get; set; }

    public bool? IS_IMPORT { get; set; }

    public bool? IS_SUM { get; set; }

    public bool? IS_CHANGE { get; set; }

    public int? ORDERS { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public virtual PA_ELEMENT ELEMENT { get; set; } = null!;

    public virtual HU_SALARY_TYPE SALARY_TYPE { get; set; } = null!;
}
