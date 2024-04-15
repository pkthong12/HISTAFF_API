using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("INS_REGION")]

public partial class INS_REGION : BASE_ENTITY
{

    public string? REGION_CODE { get; set; }
    public long? AREA_ID { get; set; }
    public string? OTHER_LIST_CODE { get; set; }
    public string? NOTE { get; set; }
    public string? ACTFLG { get; set; }
    public decimal? MONEY { get; set; }
    public DateTime? EFFECT_DATE { get; set; }
    public DateTime? EXPRIVED_DATE { get; set; }
    public bool? IS_ACTIVE { get; set; }
    public decimal? CEILING_UI { get; set; }
}

