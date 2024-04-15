using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_GROUP_PERMISSION")]
public partial class SYS_GROUP_PERMISSION
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long GROUP_ID { get; set; }

    public long FUNCTION_ID { get; set; }

    public string? PERMISSION_STRING { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public virtual SYS_FUNCTION FUNCTION { get; set; } = null!;

    public virtual SYS_GROUP GROUP { get; set; } = null!;
}
