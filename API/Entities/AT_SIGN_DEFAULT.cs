using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_SIGN_DEFAULT")]

public class AT_SIGN_DEFAULT : BASE_ENTITY
{
    public long? EMPLOYEE_ID { get; set; }

    public long? ORG_ID { get; set; }

    public DateTime? EFFECT_DATE_FROM { get; set; }

    public DateTime? EFFECT_DATE_TO { get; set; }

    public long? SIGN_DEFAULT { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? NOTE { get; set; }

    public string? ACTFLG { get; set; }

    public string? CREATED_LOG { get; set; }

    public string? UPDATED_LOG { get; set; }
}
