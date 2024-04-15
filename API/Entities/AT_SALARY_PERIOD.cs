using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_SALARY_PERIOD")]
public class AT_SALARY_PERIOD:BASE_ENTITY
{
    public string NAME { get; set; } = null!;

    public int YEAR { get; set; }

    public DateTime START_DATE { get; set; }

    public DateTime END_DATE { get; set; }

    public int STANDARD_WORKING { get; set; }

    public string? NOTE { get; set; }

    public bool? IS_ACTIVE { get; set; }
    public int? MONTH { get; set; }
    //public decimal? STANDARD_TIME { get; set; }

    //public virtual ICollection<AT_OVERTIME> AT_OVERTIMEs { get; set; } = new List<AT_OVERTIME>();

    //public virtual ICollection<AT_SALARY_PERIOD_DTL> AT_SALARY_PERIOD_DTLs { get; set; } = new List<AT_SALARY_PERIOD_DTL>();

    //public virtual ICollection<AT_TIMESHEET_DAILY> AT_TIMESHEET_DAILies { get; set; } = new List<AT_TIMESHEET_DAILY>();

    //public virtual ICollection<AT_TIMESHEET_LOCK> AT_TIMESHEET_LOCKs { get; set; } = new List<AT_TIMESHEET_LOCK>();

    //public virtual ICollection<AT_TIMESHEET_MACHINE_EDIT> AT_TIMESHEET_MACHINE_EDITs { get; set; } = new List<AT_TIMESHEET_MACHINE_EDIT>();

    //public virtual ICollection<AT_TIMESHEET_MONTHLY_DTL> AT_TIMESHEET_MONTHLY_DTLs { get; set; } = new List<AT_TIMESHEET_MONTHLY_DTL>();

    //public virtual ICollection<AT_TIMESHEET_MONTHLY> AT_TIMESHEET_MONTHLies { get; set; } = new List<AT_TIMESHEET_MONTHLY>();

    //public virtual ICollection<AT_WORKSIGN> AT_WORKSIGNs { get; set; } = new List<AT_WORKSIGN>();

    //public virtual ICollection<HU_COMMEND> HU_COMMENDs { get; set; } = new List<HU_COMMEND>();

    //public virtual ICollection<PA_CALCULATE_LOCK> PA_CALCULATE_LOCKs { get; set; } = new List<PA_CALCULATE_LOCK>();

    //public virtual ICollection<PA_KPI_LOCK> PA_KPI_LOCKs { get; set; } = new List<PA_KPI_LOCK>();

    //public virtual ICollection<PA_KPI_SALARY_DETAIL_TMP> PA_KPI_SALARY_DETAIL_TMPs { get; set; } = new List<PA_KPI_SALARY_DETAIL_TMP>();

    //public virtual ICollection<PA_KPI_SALARY_DETAIL> PA_KPI_SALARY_DETAILs { get; set; } = new List<PA_KPI_SALARY_DETAIL>();

    //public virtual ICollection<PA_SAL_IMPORT> PA_SAL_IMPORTs { get; set; } = new List<PA_SAL_IMPORT>();
}
