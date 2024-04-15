using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_ORGANIZATION")]
public partial class HU_ORGANIZATION: BASE_ENTITY
{
    public long? TENANT_ID { get; set; }

    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public long? PARENT_ID { get; set; }
    
    public long? COMPANY_ID { get; set; }

    public long? ORG_LEVEL_ID { get; set; }

    public long? HEAD_POS_ID { get; set; }
    

    public int? ORDER_NUM { get; set; }

    public long? MNG_ID { get; set; }

    public DateTime? FOUNDATION_DATE { get; set; }

    public DateTime? DISSOLVE_DATE { get; set; }

    public string? PHONE { get; set; }

    public string? FAX { get; set; }

    public string? ADDRESS { get; set; }

    public string? BUSINESS_NUMBER { get; set; }

    public DateTime? BUSINESS_DATE { get; set; }

    public string? TAX_CODE { get; set; }

    public string? NOTE { get; set; }

    public bool? IS_ACTIVE { get; set; }

    /// <summary>
    /// TEN VIET TAT
    /// </summary>
    public string? SHORT_NAME { get; set; }

    public string? NAME_EN { get; set; }

    public double? UY_BAN { get; set; }
    
    public int? LEVEL_ORG { get; set; }

    public double? GROUPPROJECT { get; set; }
    public string? ATTACHED_FILE { get; set; }

    //public virtual ICollection<AT_TIMESHEET_MONTHLY_DTL> AT_TIMESHEET_MONTHLY_DTLs { get; set; } = new List<AT_TIMESHEET_MONTHLY_DTL>();

    //public virtual ICollection<AT_TIMESHEET_MONTHLY> AT_TIMESHEET_MONTHLies { get; set; } = new List<AT_TIMESHEET_MONTHLY>();

    //public virtual ICollection<HU_COMMEND_EMP> HU_COMMEND_EMPs { get; set; } = new List<HU_COMMEND_EMP>();

    //public virtual ICollection<HU_COMMEND> HU_COMMENDs { get; set; } = new List<HU_COMMEND>();

    //public virtual ICollection<HU_DISCIPLINE> HU_DISCIPLINEs { get; set; } = new List<HU_DISCIPLINE>();

    //public virtual ICollection<HU_EMPLOYEE_EDIT> HU_EMPLOYEE_EDITs { get; set; } = new List<HU_EMPLOYEE_EDIT>();

    //public virtual ICollection<HU_EMPLOYEE> HU_EMPLOYEEs { get; set; } = new List<HU_EMPLOYEE>();

    //public virtual ICollection<HU_POSITION> HU_POSITIONs { get; set; } = new List<HU_POSITION>();

    //public virtual ICollection<HU_WORKING> HU_WORKINGs { get; set; } = new List<HU_WORKING>();

    //public virtual ICollection<HU_ORGANIZATION> InversePARENT { get; set; } = new List<HU_ORGANIZATION>();

    //public virtual HU_ORGANIZATION? PARENT { get; set; }
    //public virtual ICollection<HU_ORGANIZATION>? HU_ORGANIZATIONs { get; set; }


    //public virtual ICollection<PA_CALCULATE_LOCK> PA_CALCULATE_LOCKs { get; set; } = new List<PA_CALCULATE_LOCK>();

    //public virtual ICollection<PA_KPI_LOCK> PA_KPI_LOCKs { get; set; } = new List<PA_KPI_LOCK>();
}
