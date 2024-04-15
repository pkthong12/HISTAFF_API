using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_FORM_ELEMENT")]
public partial class HU_FORM_ELEMENT
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string? NAME { get; set; }

    public string? CODE { get; set; }

    public int? TYPE_ID { get; set; }

    public decimal? ORDERS { get; set; }
}
