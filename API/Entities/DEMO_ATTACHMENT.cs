using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("DEMO_ATTACHMENT")]
public partial class DEMO_ATTACHMENT: BASE_ENTITY
{
    public string NAME { get; set; } = null!;

    public string? FIRST_ATTACHMENT { get; set; }

    public string? SECOND_ATTACHMENT { get; set; }

    public long? STATUS_ID { get; set; }

    public DateTime? EFFECT_DATE { get; set; }

}
