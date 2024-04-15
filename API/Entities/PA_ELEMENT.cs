using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PA_ELEMENT")]
public partial class PA_ELEMENT
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public long GROUP_ID { get; set; }

    public bool IS_SYSTEM { get; set; }

    public bool IS_ACTIVE { get; set; }

    public int ORDERS { get; set; }

    public long DATA_TYPE { get; set; }

    public string? NOTE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public virtual PA_ELEMENT_GROUP GROUP { get; set; } = null!;

    //public virtual ICollection<PA_SALARY_PAYCHECK> PA_SALARY_PAYCHECKs { get; set; } = new List<PA_SALARY_PAYCHECK>();

    //public virtual ICollection<PA_SALARY_STRUCTURE> PA_SALARY_STRUCTUREs { get; set; } = new List<PA_SALARY_STRUCTURE>();

    //public virtual ICollection<PA_SAL_IMPORT> PA_SAL_IMPORTs { get; set; } = new List<PA_SAL_IMPORT>();
}
