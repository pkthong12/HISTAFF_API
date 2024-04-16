using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("TR_CLASS_RESULT")]
public class TR_CLASS_RESULT : BASE_ENTITY
{
    public long? TR_CLASS_ID { get; set; }
    public long? EMPLOYEE_ID { get; set; }
    public long? GRADE { get; set; }
    public string? REMARK { get; set; }
    public string? CREATED_LOG { get; set; }
    public string? UPDATED_LOG { get; set; }
}
