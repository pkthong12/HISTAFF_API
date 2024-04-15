using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_SALARY_TYPE")]
public partial class SYS_SALARY_TYPE
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public int? ORDERS { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? NOTE { get; set; }

    public long AREA_ID { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    //public virtual ICollection<SYS_PA_FORMULA> SYS_PA_FORMULAs { get; set; } = new List<SYS_PA_FORMULA>();

    //public virtual ICollection<SYS_SALARY_STRUCTURE> SYS_SALARY_STRUCTUREs { get; set; } = new List<SYS_SALARY_STRUCTURE>();
}
