using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SE_APP_TEMPLATE")]
public partial class SE_APP_TEMPLATE
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string? TEMPLATE_NAME { get; set; }

    public long? TEMPLATE_TYPE { get; set; }

    public int? TEMPLATE_ORDER { get; set; }

    public string? ACTFLG { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? CREATED_LOG { get; set; }

    public DateTime? MODIFIED_DATE { get; set; }

    public string? MODIFIED_BY { get; set; }

    public string? MODIFIED_LOG { get; set; }

    public string? TEMPLATE_CODE { get; set; }
}
