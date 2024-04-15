using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_SALARY_RANK")]
public partial class HU_SALARY_RANK : BASE_ENTITY
{
    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public int? ORDERS { get; set; }

    public int? LEVEL_START { get; set; }

    public long? SALARY_SCALE_ID { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? NOTE { get; set; }

    public DateTime? EXPIRATION_DATE { get; set; }

    public DateTime? EFFECTIVE_DATE { get; set; }

    //public virtual ICollection<HU_SALARY_LEVEL> HU_SALARY_LEVELs { get; set; } = new List<HU_SALARY_LEVEL>();

    //public virtual ICollection<HU_WORKING> HU_WORKINGs { get; set; } = new List<HU_WORKING>();

    // public virtual HU_SALARY_SCALE? SALARY_SCALE { get; set; }
}
