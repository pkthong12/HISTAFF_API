using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities.OM;
[Table("OM_JOB")]
public partial class OM_JOB
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string CODE { get; set; } = null!;

    public string NAME_VN { get; set; } = null!;

    public string? NAME_EN { get; set; }

    public string? ACTFLG { get; set; }

    public string? REQUEST { get; set; }

    public string? PURPOSE { get; set; }

    public string? NOTE { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? CREATED_LOG { get; set; }

    public string? MODIFIED_BY { get; set; }

    public DateTime? MODIFIED_DATE { get; set; }

    public string? MODIFIED_LOG { get; set; }

    public long? PHAN_LOAI_ID { get; set; }

    public long? JOB_BAND_ID { get; set; }

    public long? JOB_FAMILY_ID { get; set; }

    public long? LEVEL_ID { get; set; }
}
