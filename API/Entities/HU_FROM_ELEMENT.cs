using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_FROM_ELEMENT")]
public partial class HU_FROM_ELEMENT
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public double ID { get; set; }

    public double? TYPE_ID { get; set; }

    public string? NAME { get; set; }

    public string? CODE { get; set; }

    public double? ORDERS { get; set; }
}
