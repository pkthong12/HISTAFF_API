using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_WELFARE")]
public partial class HU_WELFARE
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public decimal? MONNEY { get; set; }

    public int? SENIORITY { get; set; }

    public DateTime? DATE_START { get; set; }

    public DateTime? DATE_END { get; set; }

    public bool? IS_ACTIVE { get; set; }

    public string? NOTE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public int? SENIORITY_ABOVE { get; set; }

    public bool? IS_AUTO_ACTIVE { get; set; }

    public bool? IS_CAL_TAX { get; set; }

    public DateTime? PAYMENT_DATE { get; set; }

    public int? PERCENTAGE { get; set; }

    public long? GENDER_ID { get; set; }

    public int? AGE_FROM { get; set; }

    public int? AGE_TO { get; set; }

    public int? WORK_LEAVE_NOPAY_FROM { get; set; }

    public int? WORK_LEAVE_NOPAY_TO { get; set; }

    public int? MONTHS_PEND_FROM { get; set; }

    public int? MONTHS_PEND_TO { get; set; }

    public int? MONTHS_WORK_IN_YEAR { get; set; }
}
