using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_COMMEND_EMP")]
public partial class HU_COMMEND_EMP
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long TENANT_ID { get; set; }

    public long EMPLOYEE_ID { get; set; }

    public long COMMEND_ID { get; set; }

    public long? POS_ID { get; set; }

    public long? ORG_ID { get; set; }

    //public virtual HU_COMMEND COMMEND { get; set; } = null!;

    //public virtual HU_EMPLOYEE EMPLOYEE { get; set; } = null!;

    //public virtual HU_ORGANIZATION? ORG { get; set; }

    //public virtual HU_POSITION? POS { get; set; }
}
