using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SE_APP_TEMPLATE_DTL")]
public partial class SE_APP_TEMPLATE_DTL
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long? TEMPLATE_ID { get; set; }

    public int? APP_LEVEL { get; set; }

    public int? APP_TYPE { get; set; }

    public long? APP_ID { get; set; }

    public int? INFORM_DATE { get; set; }

    public string? INFORM_EMAIL { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? CREATED_LOG { get; set; }

    public DateTime? MODIFIED_DATE { get; set; }

    public string? MODIFIED_BY { get; set; }

    public string? MODIFIED_LOG { get; set; }

    public long? TITLE_ID { get; set; }

    public string? NODE_VIEW { get; set; }
}
