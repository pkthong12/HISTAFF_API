using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_POSITION_GROUP")]
public partial class HU_POSITION_GROUP
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public string? NOTE { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    //public virtual ICollection<HU_POSITION> HU_POSITIONs { get; set; } = new List<HU_POSITION>();
}
