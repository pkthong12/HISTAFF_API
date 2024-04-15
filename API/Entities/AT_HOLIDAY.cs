using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_HOLIDAY")]
public partial class AT_HOLIDAY
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public DateTime START_DAYOFF { get; set; }

    public DateTime END_DAYOFF { get; set; }

    public DateTime? WORKINGDAY { get; set; }

    public string? NOTE { get; set; }

    public bool IS_ACTIVE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
}
