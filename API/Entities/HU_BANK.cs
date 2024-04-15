using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_BANK")]
public partial class HU_BANK : BASE_ENTITY
{
    public string? CODE { get; set; }

    public string? NAME { get; set; }

    public string? SHORT_NAME { get; set; }

    public string? NOTE { get; set; }

    public int? ORDER { get; set; }

    public bool? IS_ACTIVE { get; set; }

}
