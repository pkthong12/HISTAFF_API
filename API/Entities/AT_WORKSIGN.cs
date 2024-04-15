using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_WORKSIGN")]
public partial class AT_WORKSIGN : BASE_ENTITY
{
    public long? EMPLOYEE_ID { get; set; }

    public long? PERIOD_ID { get; set; }

    public DateTime? WORKINGDAY { get; set; }

    public long? SHIFT_ID { get; set; }

}
