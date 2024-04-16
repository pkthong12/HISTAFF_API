using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_COM_COMMEND")]
public partial class HU_COM_COMMEND:BASE_ENTITY
{
    public string? NO { get; set; }
    public long? POSITION_DECISION { get; set; }
    public long? COMMEND_OBJ { get; set; }
    public long? REWARD_ID { get; set; }
    public int? YEAR { get; set; }
    public string? CONTENT { get; set; }
    public string? NOTE { get; set; }
    public string? ATTACHMENT { get; set; }
    public DateTime? SIGN_DATE { get; set; }
}

