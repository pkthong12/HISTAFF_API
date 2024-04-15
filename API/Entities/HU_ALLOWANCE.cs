using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_ALLOWANCE")]
public partial class HU_ALLOWANCE
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string CODE { get; set; } = null!;

    public string? COL_NAME { get; set; }

    public string NAME { get; set; } = null!;

    public long? TYPE_ID { get; set; }

    public bool? IS_INSURANCE { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? NOTE { get; set; }

    public bool? IS_FULLDAY { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
    public long? SALARY_RANK_ID { get; set; }

    public bool? IS_COEFFICIENT { get; set; }
    public bool? IS_SAL { get; set; }

    //public virtual ICollection<HU_ALLOWANCE_EMP> HU_ALLOWANCE_EMPs { get; set; } = new List<HU_ALLOWANCE_EMP>();
}
