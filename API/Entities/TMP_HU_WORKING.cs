using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("TMP_HU_WORKING")]
public partial class TMP_HU_WORKING
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string? REF_CODE { get; set; }

    public string? CODE { get; set; }

    public long? EMPLOYEE_ID { get; set; }

    public string? POS_NAME { get; set; }

    public long? POSITION_ID { get; set; }

    public long ORG_ID { get; set; }

    public DateTime? EFFECT_DATE { get; set; }

    public DateTime? EXPIRE_DATE { get; set; }

    public string? DECISION_NO { get; set; }

    public string? TYPE_NAME { get; set; }

    public long? TYPE_ID { get; set; }

    public string? SALARY_TYPE_NAME { get; set; }

    public long? SALARY_TYPE_ID { get; set; }

    public long? SALARY_LEVEL_ID { get; set; }

    public decimal? SAL_BASIC { get; set; }

    public decimal? COEFFICIENT { get; set; }

    public decimal? SAL_TOTAL { get; set; }

    public decimal? SAL_PERCENT { get; set; }

    public string? STATUS_NAME { get; set; }

    public long? STATUS_ID { get; set; }

    public DateTime? SIGN_DATE { get; set; }

    public string? SIGNER_NAME { get; set; }

    public string? SIGNER_POSITION { get; set; }
}
