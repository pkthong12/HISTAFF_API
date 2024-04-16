using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("TR_CLASS")]

public class TR_CLASS :  BASE_ENTITY
{
    public long? TR_PROGRAM_ID { get; set; }
    public string? Name { get; set; }
    public DateTime? START_DATE { get; set; }
    public DateTime? END_DATE { get; set; }
    public string? ADDRESS { get; set; }
    public long? DISTRICT_ID { get; set; }
    public long? PROVINCE_ID { get; set; }
    public DateTime? TIME_FROM { get; set; }
    public DateTime? TIME_TO { get; set; }
    public int? TOTAL_DAY { get; set; }
    public long? TOTAL_TIME { get; set; }
    public string? NOTE { get; set; }
    public string? EMAIL_CONTENT { get; set; }
    public double? RATIO { get; set; }
}
