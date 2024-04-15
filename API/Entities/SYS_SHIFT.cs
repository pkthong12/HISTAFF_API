using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_SHIFT")]
public partial class SYS_SHIFT
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public long AREA_ID { get; set; }

    public DateTime HOURS_START { get; set; }

    public DateTime HOURS_STOP { get; set; }

    public DateTime BREAKS_FROM { get; set; }

    public DateTime BREAKS_TO { get; set; }

    public int? TIME_LATE { get; set; }

    public int? TIME_EARLY { get; set; }

    public long TIME_TYPE_ID { get; set; }

    public bool? IS_NOON { get; set; }

    public bool? IS_BREAK { get; set; }

    public string? NOTE { get; set; }

    public bool IS_ACTIVE { get; set; }

    public long? MON_ID { get; set; }

    public long? TUE_ID { get; set; }

    public long? WED_ID { get; set; }

    public long? THU_ID { get; set; }

    public long? FRI_ID { get; set; }

    public long? SAT_ID { get; set; }

    public long? SUN_ID { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
}
