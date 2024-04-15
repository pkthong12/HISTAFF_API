using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_POS_PAPER")]
public partial class HU_POS_PAPER
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long? POS_ID { get; set; }

    public int PAPER_ID { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public virtual SYS_OTHER_LIST PAPER { get; set; } = null!;

    public virtual HU_POSITION? POS { get; set; }
}
