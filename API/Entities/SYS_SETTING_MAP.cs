using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_SETTING_MAP")]
public partial class SYS_SETTING_MAP
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string? NAME { get; set; }

    public string? ADDRESS { get; set; }

    public decimal? RADIUS { get; set; }

    public string? LAT { get; set; }

    public string? LNG { get; set; }

    public int? ZOOM { get; set; }

    public string? CENTER { get; set; }

    public long? ORG_ID { get; set; }

    public string? IP { get; set; }

    public string? BSSID { get; set; }

    public string? QRCODE { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
}
