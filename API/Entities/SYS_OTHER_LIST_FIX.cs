using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_OTHER_LIST_FIX")]
public partial class SYS_OTHER_LIST_FIX
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string TYPE { get; set; } = null!;

    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public int ORDERS { get; set; }
}
