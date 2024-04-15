using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_JOB_FUNCTION")]
public partial class HU_JOB_FUNCTION
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string NAME { get; set; } = null!;

    public string? NAME_EN { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? CREATED_LOG { get; set; }

    public string? MODIFIED_BY { get; set; }

    public DateTime? MODIFIED_DATE { get; set; }

    public string? MODIFIED_LOG { get; set; }

    public long? PARENT_ID { get; set; }

    public long? JOB_ID { get; set; }

    public long? FUNCTION_ID { get; set; }

    //public virtual ICollection<HU_JOB_FUNCTION> InversePARENT { get; set; } = new List<HU_JOB_FUNCTION>();

    public virtual HU_JOB? JOB { get; set; }

    public virtual HU_JOB_FUNCTION? PARENT { get; set; }
}
