using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("SYS_REFRESH_TOKEN")]
public partial class SYS_REFRESH_TOKEN
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string USER { get; set; } = null!;

    public string TOKEN { get; set; } = null!;

    public DateTime EXPIRES { get; set; }

    public DateTime CREATED { get; set; }

    public string CREATED_BY_IP { get; set; } = null!;

    public DateTime? REVOKED { get; set; }

    public string? REVOKED_BY_IP { get; set; }

    public string? REPLACED_BY_TOKEN { get; set; }

    public string? REASON_REVOKED { get; set; }

    [NotMapped]
    public bool IS_EXPIRED => DateTime.UtcNow >= EXPIRES;
    [NotMapped]
    public bool IS_REVOKED => REVOKED != null;
    [NotMapped]
    public bool IS_ACTIVE => !IS_REVOKED && !IS_EXPIRED;
}
