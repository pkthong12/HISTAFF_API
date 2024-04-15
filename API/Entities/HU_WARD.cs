using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_WARD")]
public partial class HU_WARD: BASE_ENTITY
{

    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public bool IS_ACTIVE { get; set; }

    public string? NOTE { get; set; }

    public long DISTRICT_ID { get; set; }

}
