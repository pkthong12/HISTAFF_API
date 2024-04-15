using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PA_KPI_SALARY_DETAIL")]
public partial class PA_KPI_SALARY_DETAIL
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long? PERIOD_ID { get; set; }

    public long EMPLOYEE_ID { get; set; }

    public long KPI_TARGET_ID { get; set; }

    public decimal? REAL_VALUE { get; set; }

    public decimal? START_VALUE { get; set; }

    public decimal? EQUAL_VALUE { get; set; }

    public decimal? KPI_SALARY { get; set; }

    public bool? IS_PAY_SALARY { get; set; }

    public string? NOTE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public virtual HU_EMPLOYEE EMPLOYEE { get; set; } = null!;

    public virtual PA_KPI_TARGET KPI_TARGET { get; set; } = null!;

    public virtual AT_SALARY_PERIOD? PERIOD { get; set; }
}
