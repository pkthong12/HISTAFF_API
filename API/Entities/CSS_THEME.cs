using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("CSS_THEME")]
public partial class CSS_THEME: BASE_ENTITY
{
    public string CODE { get; set; } = null!;

    public string? DESCRIPTION { get; set; }

}
