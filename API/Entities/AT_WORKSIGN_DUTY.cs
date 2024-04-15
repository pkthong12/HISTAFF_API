using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_WORKSIGN_DUTY")]
public partial class AT_WORKSIGN_DUTY
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long? EMPLOYEE_ID { get; set; }

    public long? PERIOD_ID { get; set; }

    public DateTime WORKINGDAY { get; set; }

    public long? SHIFT_ID { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public virtual AT_SHIFT? SHIFT { get; set; }
}
