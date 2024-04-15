using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PA_ADVANCE")]
public partial class PA_ADVANCE
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long EMPLOYEE_ID { get; set; }

    public int YEAR { get; set; }

    public int PERIOD_ID { get; set; }

    public int MONEY { get; set; }

    public DateTime? ADVANCE_DATE { get; set; }

    public long? SIGN_ID { get; set; }

    public string? SIGNER_NAME { get; set; }

    public string? SIGNER_POSITION { get; set; }

    public DateTime? SIGN_DATE { get; set; }

    public string? NOTE { get; set; }

    public int STATUS_ID { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public virtual HU_EMPLOYEE EMPLOYEE { get; set; } = null!;

    public virtual HU_EMPLOYEE? SIGN { get; set; }
}
