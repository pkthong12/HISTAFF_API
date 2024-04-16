using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_DISCIPLINE_EMP")]
public partial class HU_DISCIPLINE_EMP:BASE_ENTITY
{

    public long? DISCIPLINE_ID { get; set; }
    public long? EMPLOYEE_ID { get; set; }
}
