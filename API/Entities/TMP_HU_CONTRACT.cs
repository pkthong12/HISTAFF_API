using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("TMP_HU_CONTRACT")]
public partial class TMP_HU_CONTRACT
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string? REF_CODE { get; set; }

    public string? CODE { get; set; }

    public long? EMPLOYEE_ID { get; set; }

    public DateTime? DATE_START { get; set; }

    public DateTime? DATE_END { get; set; }

    public string? CONTRACT_NO { get; set; }

    public string? CONTRACT_TYPE_NAME { get; set; }

    public int? CONTRACT_TYPE_ID { get; set; }

    public decimal? SAL_BASIC { get; set; }

    public decimal? SAL_TOTAL { get; set; }

    public decimal? SAL_PERCENT { get; set; }

    public string? STATUS_NAME { get; set; }

    public long? STATUS_ID { get; set; }

    public DateTime? SIGN_DATE { get; set; }

    public string? SIGNER_NAME { get; set; }

    public string? SIGNER_POSITION { get; set; }
}
