using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_ALLOWANCE_EMP")]
public partial class HU_ALLOWANCE_EMP : BASE_ENTITY
{
    public long EMPLOYEE_ID { get; set; }
    public long? PROFILE_ID { get; set; }

    public long? ALLOWANCE_ID { get; set; }

    public decimal? MONNEY { get; set; }

    public decimal? COEFFICIENT { get; set; }

    public DateTime? DATE_START { get; set; }

    public DateTime? DATE_END { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? NOTE { get; set; }

//  public virtual HU_ALLOWANCE? ALLOWANCE_ { get; set; }

//  public virtual HU_EMPLOYEE EMPLOYEE_ { get; set; } = null!;
}
