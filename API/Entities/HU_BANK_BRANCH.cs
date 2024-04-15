using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_BANK_BRANCH")]
public partial class HU_BANK_BRANCH
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long? BANK_ID { get; set; }

    public string? CODE { get; set; }

    public string? NAME { get; set; }

    public string? NOTE { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    //public virtual HU_BANK BANK { get; set; } = null!;
}
