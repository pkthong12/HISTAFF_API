using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_POSITION")]
public partial class HU_POSITION
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long ID { get; set; }
	public long? GROUP_ID { get; set; }
	public string? CODE { get; set; }
	public string? NAME { get; set; }
	public string? NOTE { get; set; }
	public string? JOB_DESC { get; set; }
	public long? TENANT_ID { get; set; }
	public bool? IS_ACTIVE { get; set; }
	public string? CREATED_BY { get; set; }
	public string? UPDATED_BY { get; set; }
	public DateTime? CREATED_DATE { get; set; }
	public DateTime? UPDATED_DATE { get; set; }
	public long? ORG_ID { get; set; }
	public long? JOB_ID { get; set; }
	public int? LM { get; set; }
	public bool? ISOWNER { get; set; }
	public long? CSM { get; set; }
	public bool? IS_NONPHYSICAL { get; set; }
	public long? MASTER { get; set; }
	public int? CONCURRENT { get; set; }
	public bool? IS_PLAN { get; set; }
	public long? INTERIM { get; set; }
	public DateTime? EFFECTIVE_DATE { get; set; }
	public string? TYPE_ACTIVITIES { get; set; }
	public string? FILENAME { get; set; }
	public string? UPLOADFILE { get; set; }
	public int? WORKING_TIME { get; set; }
	public string? NAME_EN { get; set; }
	public long? WORK_LOCATION { get; set; }
	public long? HIRING_STATUS { get; set; }
    public bool? IS_TDV { get; set; }
    public bool? IS_NOTOT { get; set; }

    //public virtual HU_POSITION_GROUP? GROUP { get; set; }

    //public virtual ICollection<HU_COMMEND_EMP> HU_COMMEND_EMPs { get; set; } = new List<HU_COMMEND_EMP>();

    //public virtual ICollection<HU_DISCIPLINE> HU_DISCIPLINEs { get; set; } = new List<HU_DISCIPLINE>();

    //public virtual ICollection<HU_EMPLOYEE_EDIT> HU_EMPLOYEE_EDITs { get; set; } = new List<HU_EMPLOYEE_EDIT>();

    //public virtual ICollection<HU_EMPLOYEE> HU_EMPLOYEEs { get; set; } = new List<HU_EMPLOYEE>();

    //public virtual ICollection<HU_JOB_DESCRIPTION> HU_JOB_DESCRIPTIONs { get; set; } = new List<HU_JOB_DESCRIPTION>();

    //public virtual ICollection<HU_POS_PAPER> HU_POS_PAPERs { get; set; } = new List<HU_POS_PAPER>();

    //public virtual ICollection<HU_WORKING> HU_WORKINGs { get; set; } = new List<HU_WORKING>();

    //public virtual HU_EMPLOYEE? INTERIMNavigation { get; set; }

    //public virtual HU_JOB? JOB { get; set; }

    //public virtual HU_EMPLOYEE? MASTERNavigation { get; set; }

    //public virtual HU_ORGANIZATION? ORG { get; set; }

    //public virtual ICollection<PA_KPI_POSITION> PA_KPI_POSITIONs { get; set; } = new List<PA_KPI_POSITION>();
}
