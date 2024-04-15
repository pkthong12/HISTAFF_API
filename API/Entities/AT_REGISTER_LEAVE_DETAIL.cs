using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_REGISTER_LEAVE_DETAIL")]
public partial class AT_REGISTER_LEAVE_DETAIL
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }
    public DateTime LEAVE_DATE { get; set; }
    public long REGISTER_ID { get; set; }
    public long? MANUAL_ID { get; set; }
    public long? TYPE_OFF { get; set; }
    public decimal? NUMBER_DAY { get; set; }

}
