using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_REGISTER_OFF")]
public partial class AT_REGISTER_OFF
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long EMPLOYEE_ID { get; set; }

    public DateTime DATE_START { get; set; }

    public DateTime DATE_END { get; set; }

    public DateTime? TIME_START { get; set; }

    public DateTime? TIME_END { get; set; }

    public DateTime? WORKING_DAY { get; set; }

    public int? TIME_LATE { get; set; }

    public int? TIME_EARLY { get; set; }

    public long? TIMETYPE_ID { get; set; }

    public string? NOTE { get; set; }

    public int TYPE_ID { get; set; }

    public int STATUS_ID { get; set; }

    public long? APPROVE_ID { get; set; }

    public string? APPROVE_NAME { get; set; }

    public string? APPROVE_POS { get; set; }

    public DateTime? APPROVE_DAY { get; set; }

    public string? APPROVE_NOTE { get; set; }

    public bool IS_ACTIVE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    //public virtual ICollection<AT_APPROVED> AT_APPROVEDs { get; set; } = new List<AT_APPROVED>();

    public virtual HU_EMPLOYEE EMPLOYEE { get; set; } = null!;

    public virtual AT_TIME_TYPE? TIMETYPE { get; set; }
}
