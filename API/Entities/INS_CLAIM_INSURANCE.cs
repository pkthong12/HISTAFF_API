using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("INS_CLAIM_INSURANCE")]

public class INS_CLAIM_INSURANCE : BASE_ENTITY
{
    public long? INS_HEALTH_ID { get; set; }
    public long? SUN_CARE_ID { get; set; }
    public DateTime? EXAMINE_DATE { get; set; }
    public string? DISEASE_NAME { get; set; }
    public decimal? AMOUNT_OF_CLAIMS { get; set; }
    public decimal? AMOUNT_OF_COMPENSATION { get; set; }
    public DateTime? COMPENSATION_DATE { get; set; }
    public string? NOTE { get; set; }

}
