using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_CONFIG")]
public partial class AT_CONFIG
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public decimal? ADVANCE_NUMBER { get; set; }

    public DateTime? DATE_CLEAR { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
}
