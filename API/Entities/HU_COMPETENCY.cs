using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_COMPETENCY")]

public class HU_COMPETENCY : BASE_ENTITY
{
    public long? COMPETENCY_GROUP_ID { get; set; }
    public string? CODE { get; set; }
    public string? NAME { get; set; }
    public string? NOTE { get; set; }
    public DateTime? EFFECT_DATE { get; set; }
    public DateTime? EXPIRE_DATE { get; set; }
    public bool? IS_ACTIVE { get; set; }
}
