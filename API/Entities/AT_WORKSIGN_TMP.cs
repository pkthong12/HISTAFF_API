using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_WORKSIGN_TMP")]
public partial class AT_WORKSIGN_TMP
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string? CODE { get; set; }

    public string? REF_CODE { get; set; }

    public long EMPLOYEE_ID { get; set; }

    public long PERIOD_ID { get; set; }

    public string? DAY_1 { get; set; }

    public string? DAY_2 { get; set; }

    public string? DAY_3 { get; set; }

    public string? DAY_4 { get; set; }

    public string? DAY_5 { get; set; }

    public string? DAY_6 { get; set; }

    public string? DAY_7 { get; set; }

    public string? DAY_8 { get; set; }

    public string? DAY_9 { get; set; }

    public string? DAY_10 { get; set; }

    public string? DAY_11 { get; set; }

    public string? DAY_12 { get; set; }

    public string? DAY_13 { get; set; }

    public string? DAY_14 { get; set; }

    public string? DAY_15 { get; set; }

    public string? DAY_16 { get; set; }

    public string? DAY_17 { get; set; }

    public string? DAY_18 { get; set; }

    public string? DAY_19 { get; set; }

    public string? DAY_20 { get; set; }

    public string? DAY_21 { get; set; }

    public string? DAY_22 { get; set; }

    public string? DAY_23 { get; set; }

    public string? DAY_24 { get; set; }

    public string? DAY_25 { get; set; }

    public string? DAY_26 { get; set; }

    public string? DAY_27 { get; set; }

    public string? DAY_28 { get; set; }

    public string? DAY_29 { get; set; }

    public string? DAY_30 { get; set; }

    public string? DAY_31 { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
}
