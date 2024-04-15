using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_REPORT")]
public partial class HU_REPORT
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string NAME { get; set; } = null!;

    public string? CODE { get; set; }

    public long? PARENT_ID { get; set; }

    public string? NOTE { get; set; }

    //public virtual ICollection<HU_REPORT> InversePARENT { get; set; } = new List<HU_REPORT>();

    public virtual HU_REPORT? Parent { get; set; }
    public List<HU_REPORT> Childs { get; set; }
}
