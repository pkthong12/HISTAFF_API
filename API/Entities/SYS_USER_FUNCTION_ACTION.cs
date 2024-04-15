using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_USER_FUNCTION_ACTION")]
public partial class SYS_USER_FUNCTION_ACTION: BASE_ENTITY
{
    public string USER_ID { get; set; } = null!;

    public long FUNCTION_ID { get; set; }

    public long ACTION_ID { get; set; }

}
