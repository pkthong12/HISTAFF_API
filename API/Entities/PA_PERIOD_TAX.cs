using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PA_PERIOD_TAX")]

public class PA_PERIOD_TAX : BASE_ENTITY
{
    public long? PERIOD_ID { get; set; }
    public int? YEAR { get; set; }
    public long? MONTHLY_TAX_CALCULATION { get; set; }
    public DateTime? TAX_DATE { get; set;}
    public DateTime? CALCULATE_TAX_FROM_DATE { get; set; }
    public DateTime? CALCULATE_TAX_TO_DATE { get; set; }
    public string? CREATED_LOG { get;set; }
    public string? UPDATED_LOG { get; set; }
    public bool? IS_ACTIVE { get; set; }
    public string? NOTE { get; set; }
}
