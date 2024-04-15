using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_WELFARE_CONTRACT")]
public partial class HU_WELFARE_CONTRACT
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long WELFARE_ID { get; set; }

    public long CONTRACT_TYPE_ID { get; set; }
}
