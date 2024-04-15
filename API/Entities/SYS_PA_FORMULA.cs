using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_PA_FORMULA")]
public partial class SYS_PA_FORMULA : BASE_ENTITY
{
    public long? AREA_ID { get; set; }

    public string? COL_NAME { get; set; }

    public long? SALARY_TYPE_ID { get; set; }

    public string? FORMULA { get; set; }

    public string? FORMULA_NAME { get; set; }

    public int? ORDERS { get; set; }

    public bool? IS_ACTIVE { get; set; }

    //public virtual SYS_SALARY_TYPE SALARY_TYPE { get; set; } = null!;
}
