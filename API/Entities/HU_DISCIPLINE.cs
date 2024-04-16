using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_DISCIPLINE")]
public partial class HU_DISCIPLINE: BASE_ENTITY
{

    public long? EMPLOYEE_ID { get; set; }

    public long? DECISION_TYPE { get; set; }

    public string? DECISION_NO { get; set; }

    public DateTime? EFFECT_DATE { get; set; }

    public DateTime? EXPIRE_DATE { get; set; }

    public long? STATUS_ID { get; set; }

    public DateTime? ISSUED_DATE { get; set; }

    public DateTime? VIOLATED_DATE { get; set; }

    public string? BASED_ON { get; set; }

    public DateTime? DOCUMENT_SIGN_DATE { get; set; }

    public long? SIGN_ID { get; set; }

    public DateTime? SIGN_DATE { get; set; }

    public long? DISCIPLINE_OBJ { get; set; }

    public long? DISCIPLINE_TYPE { get; set; }

    public string? REASON { get; set; }

    public long? EXTEND_SAL_TIME { get; set; }

    public string? ATTACHMENT { get; set; }

    public string? NOTE { get; set; }
}
