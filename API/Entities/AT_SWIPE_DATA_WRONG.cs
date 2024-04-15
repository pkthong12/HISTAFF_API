using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_SWIPE_DATA_WRONG")]
public partial class AT_SWIPE_DATA_WRONG
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public int? TENANT_ID { get; set; }

    public long? EMP_ID { get; set; }

    public string? ITIME_ID { get; set; }

    public DateTime? TIME_POINT { get; set; }

    public string? LATITUDE { get; set; }

    public string? LONGITUDE { get; set; }

    public string? MODEL { get; set; }

    public string? IMAGE { get; set; }

    public string? MAC { get; set; }

    public string? OPERATING_SYSTEM { get; set; }

    public string? OPERATING_VERSION { get; set; }

    public string? WIFI_IP { get; set; }

    public string? BSS_ID { get; set; }
}
