using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("INS_TYPE")]
public partial class INS_TYPE:BASE_ENTITY
{
    public long? TYPE_ID { get; set; }

    public string? NAME { get; set; } = null!;

    public string? CODE { get; set; }

    public string? NOTE { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public bool? STATUS { get; set; }
}
