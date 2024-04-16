using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PA_PHASE_ADVANCE")]
public class PA_PHASE_ADVANCE : BASE_ENTITY
{
    public long? PERIOD_ID { get; set; }
    public string? ACTFLG { get; set; }
    public string? NOTE { get; set; }
    public int? PHASE_NAME_ID { get; set; }
    public double? MONTH_LBS { get; set; }
    public double? SENIORITY { get; set; }
    public long? SYMBOL_ID { get; set; }
    //public string? SYMBOL_IDS { get; set; }
    public string? SYMBOL { get; set; }
    public bool? IS_ACTIVE { get; set; }
    public string? NAME_VN { get; set; }
    public int? YEAR { get; set; }
    public DateTime? PHASE_DAY { get; set; }
    public DateTime? FROM_DATE { get; set; }
    public DateTime? TO_DATE { get; set; }
    public long? ORG_ID { get; set; }
}

