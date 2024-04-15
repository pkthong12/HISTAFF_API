using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_OTHER_LIST_TYPE")]
public partial class SYS_OTHER_LIST_TYPE
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public int? ORDERS { get; set; }
    public string? NOTE { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
    public bool IS_SYSTEM { get; set; }

    //public virtual ICollection<SYS_OTHER_LIST> SYS_OTHER_LISTs { get; set; } = new List<SYS_OTHER_LIST>();
}
