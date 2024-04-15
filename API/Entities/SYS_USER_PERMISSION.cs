using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_USER_PERMISSION")]
public partial class SYS_USER_PERMISSION
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string? USER_ID { get; set; }

    public long FUNCTION_ID { get; set; }

    public string? PERMISSION_STRING { get; set; }

    // public virtual SYS_FUNCTION FUNCTION { get; set; } = null!;

    // public virtual SYS_USER? USER { get; set; }
}
