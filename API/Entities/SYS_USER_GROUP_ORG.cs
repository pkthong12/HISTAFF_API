using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_USER_GROUP_ORG")]
public partial class SYS_USER_GROUP_ORG: BASE_ENTITY
{
    public long GROUP_ID { get; set; }

    public long ORG_ID { get; set; }
}
