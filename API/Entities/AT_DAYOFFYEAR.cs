using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_DAYOFFYEAR")]
public partial class AT_DAYOFFYEAR
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long EMPLOYEE_ID { get; set; }

    public int? YEAR_ID { get; set; }

    public int? DAY_OFF { get; set; }

    public int? MONTH_1 { get; set; }

    public int? MONTH_2 { get; set; }

    public int? MONTH_3 { get; set; }

    public int? MONTH_4 { get; set; }

    public int? MONTH_5 { get; set; }

    public int? MONTH_6 { get; set; }

    public int? MONTH_7 { get; set; }

    public int? MONTH_8 { get; set; }

    public int? MONTH_9 { get; set; }

    public int? MONTH_10 { get; set; }

    public int? MONTH_11 { get; set; }

    public int? MONTH_12 { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
}
