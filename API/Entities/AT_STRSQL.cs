using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_STRSQL")]
public partial class AT_STRSQL
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public decimal id { get; set; }

    public string? stringsql { get; set; }
}
