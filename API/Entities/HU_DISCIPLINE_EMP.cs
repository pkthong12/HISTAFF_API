using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_DISCIPLINE_EMP")]
public partial class HU_DISCIPLINE_EMP
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long? DISCIPLINE_ID { get; set; }

    public long? EMPLOYEE_ID { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
}
