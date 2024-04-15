using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
// Not used
[Table("SYS_FUNCTION_GROUP")]
public partial class SYS_FUNCTION_GROUP
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string NAME { get; set; } = null!;

    public string? CODE { get; set; }

    public long APPLICATION_ID { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    // public virtual SYS_CONFIG APPLICATION { get; set; } = null!;

    //public virtual ICollection<SYS_FUNCTION> SYS_FUNCTIONs { get; set; } = new List<SYS_FUNCTION>();
}
