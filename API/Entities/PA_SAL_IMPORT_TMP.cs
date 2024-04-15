using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PA_SAL_IMPORT_TMP")]
public partial class PA_SAL_IMPORT_TMP
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string? REF_CODE { get; set; }

    public long EMPLOYEE_ID { get; set; }

    public string? EMPLOYEE_CODE { get; set; }

    public long ELEMENT_ID { get; set; }

    public int PERIOD_ID { get; set; }

    public string? VALUES { get; set; }

    public int TYPE_SAL { get; set; }
}
