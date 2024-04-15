using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_SYMBOL")]
public partial class AT_SYMBOL
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string? CODE { get; set; } = null!;

    public string? NAME { get; set; } = null!;

    public string? COL_NAME { get; set; }

    public string? NOTE { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public bool? IS_OFF { get; set; }

    public bool? IS_HOLIDAY_CAL { get; set; }

    public bool? IS_INS_ARISING { get; set; }

    public bool? IS_PORTAL { get; set; }

    public bool? IS_REGISTER { get; set; }

    public bool? IS_HAVE_SAL { get; set; }

    public long? WORKING_HOUR { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
}
