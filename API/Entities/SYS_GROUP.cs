using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_GROUP")]
public partial class SYS_GROUP
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string NAME { get; set; } = null!;

    public string? NOTE { get; set; }

    public string? CODE { get; set; }

    public bool? IS_ADMIN { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
    public bool? IS_SYSTEM { get; set; }


    //public virtual ICollection<SYS_GROUP_PERMISSION> SYS_GROUP_PERMISSIONs { get; set; } = new List<SYS_GROUP_PERMISSION>();

    //public virtual ICollection<SYS_USER> SYS_USERs { get; set; } = new List<SYS_USER>();
}
