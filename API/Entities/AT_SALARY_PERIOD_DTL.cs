using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_SALARY_PERIOD_DTL")]
public partial class AT_SALARY_PERIOD_DTL
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long PERIOD_ID { get; set; }

    public long? ORG_ID { get; set; }

    public long? EMP_ID { get; set; }

    public decimal WORKING_STANDARD { get; set; }

    public decimal? STANDARD_TIME { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public virtual AT_SALARY_PERIOD PERIOD { get; set; } = null!;
}
