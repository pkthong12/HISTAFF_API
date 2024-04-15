using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_OTHER_LIST")]
public partial class SYS_OTHER_LIST
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long? ID { get; set; }

    public string? CODE { get; set; } = null!;

    public string? NAME { get; set; } = null!;

    public long? TYPE_ID { get; set; }

    public int? ORDERS { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public string? NOTE { get; set; }
    public DateTime? EFFECTIVE_DATE { get; set; }
    public DateTime? EXPIRATION_DATE { get; set; }

}
