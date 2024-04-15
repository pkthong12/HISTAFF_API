using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_JOB_BAND")]
public partial class HU_JOB_BAND
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string NAME_VN { get; set; } = null!;

    public string? NAME_EN { get; set; }

    public string? LEVEL_FROM { get; set; }

    public string? LEVEL_TO { get; set; }

    public bool? STATUS { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? CREATED_LOG { get; set; }

    public string? MODIFIED_BY { get; set; }

    public DateTime? MODIFIED_DATE { get; set; }

    public string? MODIFIED_LOG { get; set; }

    public int? TITLE_GROUP_ID { get; set; }

    public int? OTHER { get; set; }

    //public virtual ICollection<HU_JOB> HU_JOBs { get; set; } = new List<HU_JOB>();
}
