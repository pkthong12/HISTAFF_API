using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_SWIPE_DATA")]
public partial class AT_SWIPE_DATA : BASE_ENTITY
{

    public DateTime? VALTIME { get; set; }
    public string? TIME_ONLY { get; set; }
    public long? TERMINAL_ID { get; set; }
    public string? CREATED_LOG { get;set; }
    public string? UPDATED_LOG { get;set; }
    public long? ORG_ID { get; set; }
    public DateTime? WORKING_DAY { get; set; }
    public int? EVT { get; set; }
    public int? IS_GPS { get;set; }
    public int? ADDRESS_ID { get; set; }
    public string? LATATUIDE_LONGTATUIDE { get;set; }
    public long? SHIFT_ID { get; set; }
    public long? EMP_ID { get; set; }
    public string? ITIME_ID { get; set; }

    //public virtual HU_EMPLOYEE EMP { get; set; } = null!;
}
