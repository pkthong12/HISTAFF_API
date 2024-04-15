using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("DEMO_HANGFIRE_RECORD")]
public partial class DEMO_HANGFIRE_RECORD
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string TEXT { get; set; } = null!;

    public DateTime CREATED_DATE { get; set; }
}
