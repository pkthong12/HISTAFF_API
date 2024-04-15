using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SE_PROCESS_APPROVE_POS")]

public class SE_PROCESS_APPROVE_POS : BASE_ENTITY
{

    public long? PROCESS_APPROVE_ID { get; set; }
    public long? POS_ID { get; set; }
    public bool? IS_MNG_AFFILIATED_DEPARTMENTS { get; set; }
    public bool? IS_DIRECT_MNG_OF_DIRECT_MNG { get; set; }
    public bool? IS_MNG_SUPERIOR_DEPARTMENTS { get; set; }
    public bool? IS_DIRECT_MANAGER { get; set; }

}
