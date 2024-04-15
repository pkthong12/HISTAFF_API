using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("AD_BOOKING")]
public partial class AD_BOOKING
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long EMP_ID { get; set; }

    public long ROOM_ID { get; set; }

    public DateTime BOOKING_DAY { get; set; }

    public DateTime HOUR_FROM { get; set; }

    public DateTime HOUR_TO { get; set; }

    public string? NOTE { get; set; }

    public long STATUS_ID { get; set; }

    public long? APPROVED_ID { get; set; }

    public DateTime? APPROVED_DATE { get; set; }

    public string? APPROVED_NOTE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public virtual AD_ROOM ROOM { get; set; } = null!;
}
