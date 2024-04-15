using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("CSS_VAR")]
public partial class CSS_VAR: BASE_ENTITY
{
    public string NAME { get; set; } = null!;

    public string? DESCRIPTION { get; set; }

}
