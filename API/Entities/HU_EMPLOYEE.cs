using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_EMPLOYEE")]
public partial class HU_EMPLOYEE: BASE_ENTITY
{
    public string CODE { get; set; } = null!;
    public string? ITIME_ID { get; set; }
    public long? INSURENCE_AREA_ID { get; set; }


    public long? ORG_ID { get; set; }

    public long? CONTRACT_ID { get; set; }

    public DateTime? CONTRACT_EXPIRED { get; set; }

    public long? CONTRACT_TYPE_ID { get; set; }

    public long? POSITION_ID { get; set; }

    public long? RESIDENT_ID { get; set; }

    public long? LAST_WORKING_ID { get; set; }
    public long? DIRECT_MANAGER_ID { get; set; }

    public long? EMPLOYEE_OBJECT_ID { get; set; }
    public long? LABOR_OBJECT_ID { get; set; }

    public DateTime? EFFECT_DATE { get; set; }

    public DateTime? TER_EFFECT_DATE { get; set; }

    public DateTime? TER_LAST_DATE { get; set; }

    public long? SALARY_TYPE_ID { get; set; }
    public string? PROFILE_CODE { get; set; }

    public int? SENIORITY { get; set; }

    

    public decimal? SAL_TOTAL { get; set; }

    public decimal? SAL_BASIC { get; set; }

    public decimal? SAL_RATE { get; set; }

    public decimal? DAY_OF { get; set; }

    public long? WORK_STATUS_ID { get; set; }
    public long? STATUS_DETAIL_ID { get; set; }

    public DateTime? WORK_DATE { get; set; }
    public DateTime? JOIN_DATE { get; set; }
    public DateTime? JOIN_DATE_STATE { get; set; }
    

    public long? STAFF_RANK_ID { get; set; }
    public long? PROFILE_ID { get; set; }
    public long? INSURENCE_AREA { get; set; }
    public bool? IS_NOT_CONTRACT { get; set; }

    [ForeignKey("PROFILE_ID")]
    public virtual HU_EMPLOYEE_CV? Profile { get; set; }

    
}
