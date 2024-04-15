using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PA_FORMULA")]
public partial class PA_FORMULA
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string COL_NAME { get; set; } = null!;

    public int SALARY_TYPE_ID { get; set; }

    public string FORMULA { get; set; } = null!;

    public string? FORMULA_NAME { get; set; }

    public int? ORDERS { get; set; }

    public int? IS_DAILY { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public virtual HU_SALARY_TYPE SALARY_TYPE { get; set; } = null!;
}
