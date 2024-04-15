using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_SALARY_LEVEL")]
public partial class HU_SALARY_LEVEL : BASE_ENTITY
{

    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public int? ORDERS { get; set; }

    public long? SALARY_RANK_ID { get; set; }
    public long? SALARY_SCALE_ID { get; set; }
    public decimal? COEFFICIENT { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? NOTE { get; set; }

    public decimal? PERFORM_BONUS { get; set; }
    public decimal? OTHER_BONUS { get; set; }
    public DateTime? EXPIRATION_DATE { get; set; }
    public DateTime? EFFECTIVE_DATE { get; set; }
    public DateTime? HOLDING_TIME { get; set; }
    public int? HOLDING_MONTH { get; set; }
    public long? REGION_ID { get; set; }

    //public virtual ICollection<HU_WORKING> HU_WORKINGs { get; set; } = new List<HU_WORKING>();

    //public virtual HU_SALARY_RANK? SALARY_RANK { get; set; }
}
