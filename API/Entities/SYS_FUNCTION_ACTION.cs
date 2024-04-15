using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_FUNCTION_ACTION")]
public partial class SYS_FUNCTION_ACTION: BASE_ENTITY
{
    public long FUNCTION_ID { get; set; }

    public long ACTION_ID { get; set; }

}
