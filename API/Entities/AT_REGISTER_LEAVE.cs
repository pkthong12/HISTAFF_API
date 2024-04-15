using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_REGISTER_LEAVE")]
public partial class AT_REGISTER_LEAVE
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long? EMPLOYEE_ID { get; set; }

    public DateTime? DATE_START { get; set; }

    public DateTime? DATE_END { get; set; }

    public long? TYPE_ID { get; set; }
    public long? TYPE_OFF { get; set; }
    public bool? IS_EACH_DAY { get; set; }

    public string? FILE_NAME { get; set; }

    public string? REASON { get; set; }

    public string? NOTE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

}
