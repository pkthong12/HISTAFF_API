using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_SALARY_SCALE")]
public partial class HU_SALARY_SCALE : BASE_ENTITY
{
    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public int? ORDERS { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? NOTE { get; set; }

    public DateTime? EXPIRATION_DATE { get; set; }

    public DateTime? EFFECTIVE_DATE { get; set; }
    public bool? IS_TABLE_SCORE { get; set; }

    //public virtual ICollection<HU_SALARY_RANK> HU_SALARY_RANKs { get; set; } = new List<HU_SALARY_RANK>();

    //public virtual ICollection<HU_WORKING> HU_WORKINGs { get; set; } = new List<HU_WORKING>();
}
