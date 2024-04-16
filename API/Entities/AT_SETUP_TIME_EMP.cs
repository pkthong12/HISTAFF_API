using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_SETUP_TIME_EMP")]
public class AT_SETUP_TIME_EMP : BASE_ENTITY
{
    public long? EMPLOYEE_ID { get; set; }

    public int? NUMBER_SWIPECARD { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? NOTE { get; set; }

    public string? ACTFLG { get; set; }
}
