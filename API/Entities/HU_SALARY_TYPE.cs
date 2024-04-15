using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_SALARY_TYPE")]
public partial class HU_SALARY_TYPE
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public int? ORDERS { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? DESCRIPTION { get; set; }

    public long? SALARY_TYPE_GROUP { get; set; }

    public string? NOTE { get; set; }

    public DateTime? EFFECT_DATE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    //public virtual ICollection<HU_WORKING> HU_WORKINGs { get; set; } = new List<HU_WORKING>();

    //public virtual ICollection<PA_FORMULA> PA_FORMULAs { get; set; } = new List<PA_FORMULA>();

    //public virtual ICollection<PA_SALARY_PAYCHECK> PA_SALARY_PAYCHECKs { get; set; } = new List<PA_SALARY_PAYCHECK>();

    //public virtual ICollection<PA_SALARY_STRUCTURE> PA_SALARY_STRUCTUREs { get; set; } = new List<PA_SALARY_STRUCTURE>();
}
