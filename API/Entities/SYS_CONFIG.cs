using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

// Not used
public partial class SYS_CONFIG
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public string TYPE { get; set; } = null!;

    public bool? IS_ACTIVE { get; set; }

    //public virtual ICollection<SYS_FUNCTION_GROUP> SYS_FUNCTION_GROUPs { get; set; } = new List<SYS_FUNCTION_GROUP>();

    //public virtual ICollection<SYS_MODULE> SYS_MODULEs { get; set; } = new List<SYS_MODULE>();

    //public virtual ICollection<TENANT_GROUP_PERMISSION> TENANT_GROUP_PERMISSIONs { get; set; } = new List<TENANT_GROUP_PERMISSION>();

    //public virtual ICollection<TENANT_USER_PERMISSION> TENANT_USER_PERMISSIONs { get; set; } = new List<TENANT_USER_PERMISSION>();
}
