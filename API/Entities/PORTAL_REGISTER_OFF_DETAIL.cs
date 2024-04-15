using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("PORTAL_REGISTER_OFF_DETAIL")] 

public class PORTAL_REGISTER_OFF_DETAIL
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }
    public string? ID_REGGROUP { get; set; }
    public DateTime? LEAVE_DATE { get; set; }
    public long? REGISTER_ID { get; set; }
    public long? MANUAL_ID { get; set; }
    public long? TYPE_OFF { get; set; }
    public decimal? NUMBER_DAY { get; set; }
    public long? SHIFT_ID { get; set; }
}
