using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_SETTING_REMIND")]
public partial class HU_SETTING_REMIND
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string? NAME { get; set; }

    public string? CODE { get; set; }

    public int? DAY { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
}
