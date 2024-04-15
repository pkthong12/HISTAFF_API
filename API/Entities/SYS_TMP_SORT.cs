using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SYS_TMP_SORT")]
public partial class SYS_TMP_SORT
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long TMP_ID { get; set; }

    public int? ORDERS { get; set; }

    public string? REF_CODE { get; set; }
}
