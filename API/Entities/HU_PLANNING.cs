using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("HU_PLANNING")]
public class HU_PLANNING : BASE_ENTITY
{
    public long? PLANNING_PERIOD_ID { get; set; }
    public long? FROM_YEAR_ID { get; set; }
    public long? TO_YEAR_ID { get; set; }
    public string? DECISION_NO { get; set; }
    public DateTime? EFFECT_DATE { get; set; }
    public DateTime? EXPIRE_DATE { get; set; }
    public long? APP_LEVEL { get; set; }
    public long? TOTAL_PERSONNEL { get; set; }
    public string? ATTACHMENT { get; set; }
    public DateTime? SIGN_DATE { get; set; }
    public long? SIGNER_ID { get; set; }
    public string? NOTE { get; set; }
    public string? CREATED_LOG { get; set; }
    public string? UPDATED_LOG { get; set; }
    //public string? EVALUATE { get; set; }

}
