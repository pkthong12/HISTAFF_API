using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_NOTIFICATION")]
public partial class AT_NOTIFICATION
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long? TYPE { get; set; }

    public long? ACTION { get; set; }

    public long? NOTIFI_ID { get; set; }

    public long? EMP_CREATE_ID { get; set; }

    public string? FMC_TOKEN { get; set; } = null!;

    public bool? IS_READ { get; set; }

    public long? TENANT_ID { get; set; }
    public long? REF_ID { get; set; }

    public string? TITLE { get; set; } = null!;
    public string? REASON { get; set; }

    public string? EMP_NOTIFY_ID { get; set; }
    public long? STATUS_NOTIFY { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
    public string? MODEL_CHANGE { get; set; }
}
