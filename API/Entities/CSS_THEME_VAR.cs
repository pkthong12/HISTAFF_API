using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;


[Table("CSS_THEME_VAR")]
public partial class CSS_THEME_VAR: BASE_ENTITY
{
    public long CSS_THEME_ID { get; set; }

    public long CSS_VAR_ID { get; set; }

    public string VALUE { get; set; } = null!;

    public virtual CSS_THEME CSS_THEME { get; set; } = null!;

    public virtual CSS_VAR CSS_VAR { get; set; } = null!;
}
