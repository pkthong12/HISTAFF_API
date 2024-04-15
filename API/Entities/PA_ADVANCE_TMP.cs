using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PA_ADVANCE_TMP")]
public partial class PA_ADVANCE_TMP
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string? CODE_REF { get; set; }

    public string? EMPLOYEE_CODE { get; set; }

    public long? EMPLOYEE_ID { get; set; }

    public int MONEY { get; set; }

    public DateTime? ADVANCE_DATE { get; set; }

    public string? SIGNER_NAME { get; set; }

    public string? SIGNER_POSITION { get; set; }

    public DateTime? SIGN_DATE { get; set; }

    public string? NOTE { get; set; }

    public string? STATUS_NAME { get; set; }

    public long? STATUS_ID { get; set; }
}
