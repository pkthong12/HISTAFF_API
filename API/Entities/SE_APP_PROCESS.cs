using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SE_APP_PROCESS")]
public partial class SE_APP_PROCESS
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string? NAME { get; set; }

    public string? ACTFLG { get; set; }

    public string? PROCESS_CODE { get; set; }

    public int? NUMREQUEST { get; set; }

    public string? EMAIL { get; set; }

    public bool? IS_SEND_EMAIL { get; set; }
}
