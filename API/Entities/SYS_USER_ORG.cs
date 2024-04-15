using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_USER_ORG")]
public partial class SYS_USER_ORG: BASE_ENTITY
{
    public required string USER_ID { get; set; }
    public long ORG_ID { get; set; }
}
