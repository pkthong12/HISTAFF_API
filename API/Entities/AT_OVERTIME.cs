using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_OVERTIME")]
public partial class AT_OVERTIME
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long? EMPLOYEE_ID { get; set; }
    public long? PERIOD_ID { get; set; }

    public DateTime? TIME_START { get; set; }

    public DateTime? TIME_END { get; set; }

    public long? STATUS_ID { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? NOTE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public DateTime? START_DATE { get; set; }

    public DateTime? END_DATE { get; set; }

    public string? REASON { get; set; }
    public string? FILE_NAME { get; set; }


}
