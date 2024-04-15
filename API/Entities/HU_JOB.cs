using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_JOB")]
public partial class HU_JOB
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

    public decimal? ORDERNUM { get; set; }

    //public virtual ICollection<HU_JOB_FUNCTION> HU_JOB_FUNCTIONs { get; set; } = new List<HU_JOB_FUNCTION>();

    //public virtual ICollection<HU_POSITION> HU_POSITIONs { get; set; } = new List<HU_POSITION>();

    //public virtual HU_JOB_BAND? JOB_BAND { get; set; }
}
