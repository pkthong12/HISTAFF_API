using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("SYS_GROUP_FUNCTION_ACTION")]
public partial class SYS_GROUP_FUNCTION_ACTION: BASE_ENTITY
{
    public long GROUP_ID { get; set; }

    public long FUNCTION_ID { get; set; }

    public long ACTION_ID { get; set; }

}
