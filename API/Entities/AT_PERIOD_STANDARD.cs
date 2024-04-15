using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_PERIOD_STANDARD")]
public class AT_PERIOD_STANDARD : BASE_ENTITY
{
    public int? YEAR { get; set; }

    public long? PERIOD_ID { get; set; }

    public long? OBJECT_ID { get; set; }

    public int? PERIOD_STANDARD { get; set; }

    public int? PERIOD_STANDARD_NIGHT { get; set; }

    public string? NOTE { get; set; }

    public bool? IS_ACTIVE { get; set; }
}
