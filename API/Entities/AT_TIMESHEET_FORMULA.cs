using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_TIMESHEET_FORMULA")]
public partial class AT_TIMESHEET_FORMULA
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string? NAME { get; set; }

    public string? COL_NAME { get; set; }

    public string? FORMULA { get; set; }

    public string? FORMULA_NAME { get; set; }

    public int ORDERS { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
}
