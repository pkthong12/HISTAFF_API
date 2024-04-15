using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PA_KPI_GROUP")]
public partial class PA_KPI_GROUP
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string NAME { get; set; } = null!;

    public long? STATUS { get; set; }

    public bool IS_ACTIVE { get; set; }

    public int ORDERS { get; set; }

    public string? NOTE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

}
