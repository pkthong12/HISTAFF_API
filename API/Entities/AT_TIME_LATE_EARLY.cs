using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_TIME_LATE_EARLY")]
public partial class AT_TIME_LATE_EARLY
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public DateTime? DATE_START { get; set; }

    public DateTime? DATE_END { get; set; }

    public int? TIME_LATE { get; set; }

    public int? TIME_EARLY { get; set; }

    public long? EMPLOYEE_ID { get; set; }

    public int STATUS_ID { get; set; }

    public string? NOTE { get; set; }

    public bool IS_ACTIVE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public virtual HU_EMPLOYEE? EMPLOYEE { get; set; }
}
