using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities.PORTAL;
[Table("PORTAL_CERTIFICATE")]
public class PORTAL_CERTIFICATE : BASE_ENTITY
{
    public long? EMPLOYEE_ID { get; set; }
    public bool? IS_PRIME { get; set; }
    public long? TYPE_CERTIFICATE { get; set; }
    public string? NAME { get; set; }
    public DateTime? EFFECT_FROM { get; set; }
    public DateTime? TRAIN_FROM_DATE { get; set; }
    public DateTime? TRAIN_TO_DATE { get; set; }
    public string? MAJOR { get; set; }
    public long? LEVEL_TRAIN { get; set; }
    public string? CONTENT_TRAIN { get; set; }
    public int? YEAR { get; set; }
    public decimal? MARK { get; set; }
    public long? TYPE_TRAIN { get; set; }
    public string? CODE_CETIFICATE { get; set; }
    public string? CLASSIFICATION { get; set; }
    public string? FILE_NAME { get; set; }
    public string? REMARK { get; set; }
    public string? CREATED_LOG { get; set; }
    public string? UPDATED_LOG { get; set; }
    public long? SCHOOL_ID { get; set; }
    public long? LEVEL_ID { get; set; }
    public string? LEVEL { get; set; }
    public DateTime? EFFECT_TO { get; set; }
    public bool? IS_SAVE { get; set; }

}
