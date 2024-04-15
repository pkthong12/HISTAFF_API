using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("SYS_FUNCTION_IGNORE")]
public partial class SYS_FUNCTION_IGNORE: BASE_ENTITY
{
    public string PATH { get; set; } = null!;

}
