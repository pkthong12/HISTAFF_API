using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_TERMINATE")]
public partial class HU_TERMINATE
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long EMPLOYEE_ID { get; set; }

    public DateTime? EFFECT_DATE { get; set; }

    public DateTime? SEND_DATE { get; set; }

    public DateTime? LAST_DATE { get; set; }

    public string? TER_REASON { get; set; }

    public long? SIGN_ID { get; set; }

    public string? SIGNER_NAME { get; set; }

    public string? SIGNER_POSITION { get; set; }

    public DateTime? SIGN_DATE { get; set; }

    public long? STATUS_ID { get; set; }

    public string? DECISION_NO { get; set; } = null!;

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public long? REASON_ID { get; set; }

    public long? TYPE_ID { get; set; }

    public bool? IS_CAL_SEVERANCE_ALLOWANCE { get; set; }

    public decimal? AVG_SAL_SIX_MO { get; set; }

    public decimal? SEVERANCE_ALLOWANCE { get; set; }

    public decimal? PAYMENT_REMAINING_DAY { get; set; }

    public string? NOTICE_NO { get; set; }

    public string? ATTACHMENT { get; set; }
    public string? FILE_NAME { get; set; }
}
