using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_NATION")]
public partial class HU_NATION : BASE_ENTITY
{
    public string? CODE { get; set; }
    public string? NAME { get; set; }
    public string? NOTE { get; set; }
    public bool? IS_ACTIVE { get; set; }

}
