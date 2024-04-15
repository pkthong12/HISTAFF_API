using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_CONTRACT")]
public partial class HU_CONTRACT
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public long?  PROFILE_ID { get; set; }

    public long? EMPLOYEE_ID { get; set; }

    public string CONTRACT_NO { get; set; } = null!;

    public long? CONTRACT_TYPE_ID { get; set; }

    public DateTime? START_DATE { get; set; }

    public DateTime? EXPIRE_DATE { get; set; }
    public long? ORG_ID { get; set; }
    public long? POSITION_ID { get; set; }

    public long? SIGN_PROFILE_ID { get; set; }

    public long? SIGN_ID { get; set; }

    public string? SIGNER_NAME { get; set; }

    public string? SIGNER_POSITION { get; set; }

    public DateTime? SIGN_DATE { get; set; }

    public decimal SAL_BASIC { get; set; }

    public decimal SAL_PERCENT { get; set; }

    public string? NOTE { get; set; }

    public long? STATUS_ID { get; set; }

    public long? WORKING_ID { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
    public string? UPLOAD_FILE { get; set; }

    public virtual HU_CONTRACT_TYPE CONTRACT_TYPE_ { get; set; } = null!;

    public virtual HU_EMPLOYEE? EMPLOYEE_ { get; set; } = null!;

    //public virtual ICollection<HU_EMPLOYEE_EDIT> HU_EMPLOYEE_EDITs { get; set; } = new List<HU_EMPLOYEE_EDIT>();

    //public virtual ICollection<HU_EMPLOYEE> HU_EMPLOYEEs { get; set; } = new List<HU_EMPLOYEE>();

    public virtual HU_EMPLOYEE? SIGN_ { get; set; } = null!;

    public virtual HU_WORKING? WORKING_ { get; set; } = null!;
    public bool? IS_RECEIVE { get; set; }

    public DateTime? LIQUIDATION_DATE { get; set; }
    public string? LIQUIDATION_REASON { get; set; }
}
