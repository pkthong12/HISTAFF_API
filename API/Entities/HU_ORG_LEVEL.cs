using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("HU_ORG_LEVEL")]
public partial class HU_ORG_LEVEL: BASE_ENTITY
{

    public string NAME { get; set; } = null!;
    public string? CODE { get; set; }
    public int? ORDER_NUM { get; set; }
    public string? NOTE { get; set; }
    public bool? IS_ACTIVE { get; set; }

}
