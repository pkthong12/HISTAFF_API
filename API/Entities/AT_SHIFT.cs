using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_SHIFT")]
public partial class AT_SHIFT
{

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }
    public string CODE { get; set; }
    public string NAME { get; set; }
    public DateTime HOURS_START { get; set; }
    public DateTime HOURS_STOP { get; set; }
    public DateTime? BREAKS_FROM { get; set; }
    public DateTime? BREAKS_TO { get; set; }
    public int? TIME_LATE { get; set; }
    public int? TIME_EARLY { get; set; }
    public long TIME_TYPE_ID { get; set; }
    public bool? IS_BREAK { get; set; }
    public bool? IS_BOQUACC { get; set; }
    public string? NOTE { get; set; }
    public bool IS_ACTIVE { get; set; }
    public string? TIME_START { get; set; }
    public string? TIME_STOP { get; set; }
    public string? CREATED_BY { get; set; }
    public string? UPDATED_BY { get; set; }
    public DateTime? CREATED_DATE { get; set; }
    public DateTime? UPDATED_DATE { get; set; }
    public int? MIN_HOURS_WORK { get; set; }
    public bool? IS_NIGHT { get; set; }
    public bool? IS_SUNDAY { get; set; }
    public long? SUNDAY { get; set; }
    public long? SATURDAY { get; set; }
}
