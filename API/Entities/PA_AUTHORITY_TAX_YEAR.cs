using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PA_AUTHORITY_TAX_YEAR")]

public class PA_AUTHORITY_TAX_YEAR : BASE_ENTITY
{
    public long? EMPLOYEE_ID { get; set; }
    public int? YEAR { get; set; }
    public bool? IS_EMP_REGISTER { get; set; }
    public bool? IS_COM_APPROVE { get; set; }
    public string? REASON_REJECT { get; set; }
    public string? NOTE { get; set; }
}
