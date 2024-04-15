using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SE_HR_PROCESS")]
public partial class SE_HR_PROCESS
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string NAME { get; set; } = null!;

    public string CODE { get; set; } = null!;

    public string? CREATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public string? MODIFIED_BY { get; set; }

    public DateTime? MODIFIED_DATE { get; set; }

    public string? MODIFIED_LOG { get; set; }

    public string? START_BY { get; set; }

    public string? ORDER_BY { get; set; }

    public string? ICON_KEY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
}
