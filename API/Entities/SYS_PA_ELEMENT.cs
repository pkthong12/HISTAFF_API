using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_PA_ELEMENT")]
public partial class SYS_PA_ELEMENT
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public long GROUP_ID { get; set; }

    public long AREA_ID { get; set; }

    public bool IS_SYSTEM { get; set; }

    public bool IS_ACTIVE { get; set; }

    public int ORDERS { get; set; }

    public long DATA_TYPE { get; set; }

    public string? NOTE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    //public virtual ICollection<SYS_SALARY_STRUCTURE> SYS_SALARY_STRUCTUREs { get; set; } = new List<SYS_SALARY_STRUCTURE>();
}
