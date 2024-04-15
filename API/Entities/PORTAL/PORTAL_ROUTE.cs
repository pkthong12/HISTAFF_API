using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities.PORTAL;

[Table("PORTAL_ROUTE")]
public partial class PORTAL_ROUTE: BASE_ENTITY
{
    public string PATH { get; set; } = null!;

    public string VI { get; set; } = null!;

    public string EN { get; set; } = null!;

}
