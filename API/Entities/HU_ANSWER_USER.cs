using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_ANSWER_USER")]
public partial class HU_ANSWER_USER
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long ANSWER_ID { get; set; }

    public long EMP_ID { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public virtual HU_ANSWER ANSWER { get; set; } = null!;

    public virtual HU_EMPLOYEE EMP { get; set; } = null!;
}
