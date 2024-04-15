using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_SWIPE_DATA_SYNC")]
public partial class AT_SWIPE_DATA_SYNC
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }
    public long? EMP_ID { get; set; }
    public string? ITIME_ID { get; set; }
    public DateTime TIME_POINT { get; set; }
    public string? REF_CODE { get; set; }
    public string? TIME_POINT_STR { get; set; }

}
