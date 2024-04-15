using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_DAYOFFYEAR_CONFIG")]
public partial class AT_DAYOFFYEAR_CONFIG
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public decimal ADVANCE_NUMBER { get; set; }

    public bool? IS_INTERN { get; set; }

    public bool? IS_ACCUMULATION { get; set; }

    public int? MONTH_ID { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
}
