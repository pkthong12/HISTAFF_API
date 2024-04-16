using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTO;
[Table("TR_CLASSIFICATION")]

public class TR_CLASSIFICATION : BASE_ENTITY
{
    public string? NAME { get; set; }
    public long? DESC_ID { get; set; }
    public double? SCORE_FROM { get; set; }
    public double? SCORE_TO { get; set; }
    public DateTime? EFFECT_DATE { get; set; }
    public bool? IS_ACTIVE { get; set; }
}
