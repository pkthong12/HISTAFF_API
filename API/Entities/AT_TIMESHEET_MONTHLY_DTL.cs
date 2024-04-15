using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_TIMESHEET_MONTHLY_DTL")]
public partial class AT_TIMESHEET_MONTHLY_DTL
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long EMPLOYEE_ID { get; set; }

    public long PERIOD_ID { get; set; }

    public int FULLDAY { get; set; }

    public DateTime? WORKING_DAY { get; set; }

    public long? ORG_ID { get; set; }

    public decimal WORKING_CD { get; set; }

    public decimal WORKING_P { get; set; }

    public decimal WORKING_KL { get; set; }

    public decimal WORKING_NB { get; set; }

    public decimal WORKING_L { get; set; }

    public decimal WORKING_X { get; set; }

    public decimal WORKING_CT { get; set; }

    public decimal WORKING_VR { get; set; }

    public decimal WORKING_TS { get; set; }

    public decimal WORKING_H { get; set; }

    public decimal WORKING_XL { get; set; }

    public decimal WORKING_OFF { get; set; }

    public decimal WORKING__ { get; set; }

    public decimal WORKING_LPAY { get; set; }

    public decimal WORKING_PAY { get; set; }

    public decimal WORKING_N { get; set; }

    public decimal? WORKING_O { get; set; }

    public decimal OT_NL { get; set; }

    public decimal OT_DNL { get; set; }

    public decimal OT_NN { get; set; }

    public decimal OT_DNN { get; set; }

    public decimal OT_NT { get; set; }

    public decimal OT_DNT { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public virtual HU_EMPLOYEE EMPLOYEE { get; set; } = null!;

    public virtual HU_ORGANIZATION? ORG { get; set; }

    public virtual AT_SALARY_PERIOD PERIOD { get; set; } = null!;
}
