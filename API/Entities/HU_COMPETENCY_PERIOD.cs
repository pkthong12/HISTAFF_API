using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_COMPETENCY_PERIOD")]

public class HU_COMPETENCY_PERIOD : BASE_ENTITY
{
    public int? YEAR { get; set; }
    public string? NAME { get; set; }
    public string? CREATED_LOG { get; set; }
    public string? UPDATED_LOG { get; set; }
    public long? QUARTER_ID { get; set; }
    public string? CODE { get; set; }
    public DateTime? EFFECTED_DATE {get; set;}
    public DateTime? EXPRIED_DATE { get; set; }
    public bool? IS_ACTIVE { get; set; }
}
