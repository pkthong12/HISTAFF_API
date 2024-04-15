using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PA_KPI_SALARY_DETAIL_TMP")]
public partial class PA_KPI_SALARY_DETAIL_TMP
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long? PERIOD_ID { get; set; }

    public long EMPLOYEE_ID { get; set; }

    public long KPI_TARGET_ID { get; set; }

    public decimal? REAL_VALUE { get; set; }

    public decimal? START_VALUE { get; set; }

    public decimal? EQUAL_VALUE { get; set; }

    public virtual AT_SALARY_PERIOD? PERIOD { get; set; }
}
