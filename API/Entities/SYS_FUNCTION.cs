using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_FUNCTION")]
public partial class SYS_FUNCTION: BASE_ENTITY
{

    public long MODULE_ID { get; set; }

    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public long? GROUP_ID { get; set; }
    
    public string PATH { get; set; } = null!;

    public bool? PATH_FULL_MATCH { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public bool? ROOT_ONLY { get; set; }

    // public virtual SYS_FUNCTION_GROUP? GROUP { get; set; }

    // public virtual SYS_MODULE MODULE { get; set; } = null!;

    //public virtual ICollection<SYS_GROUP_PERMISSION> SYS_GROUP_PERMISSIONs { get; set; } = new List<SYS_GROUP_PERMISSION>();

    //public virtual ICollection<SYS_USER_PERMISSION> SYS_USER_PERMISSIONs { get; set; } = new List<SYS_USER_PERMISSION>();

    //public virtual ICollection<TENANT_FUNCTION> TENANT_FUNCTIONs { get; set; } = new List<TENANT_FUNCTION>();
}
