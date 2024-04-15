using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("AT_APPROVED")]
public partial class AT_APPROVED
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long REGISTER_ID { get; set; }

    public long EMP_RES_ID { get; set; }

    public long APPROVE_ID { get; set; }

    public string? APPROVE_NAME { get; set; }

    public string? APPROVE_POS { get; set; }

    public DateTime? APPROVE_DAY { get; set; }

    public string? APPROVE_NOTE { get; set; }

    public int TYPE_ID { get; set; }

    public int IS_REG { get; set; }

    public int STATUS_ID { get; set; }

    public int IS_READ { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public long? EMPLOYEE_ID { get; set; }

    public long? TIME_TYPE_ID { get; set; }

    public virtual AT_REGISTER_OFF REGISTER { get; set; } = null!;

    public virtual AT_TIME_TYPE? TimeType { get; set; }

    public virtual HU_EMPLOYEE? employee { get; set; }
}
