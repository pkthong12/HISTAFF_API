using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_TIME_TYPE")]
public partial class AT_TIME_TYPE
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public long MORNING_ID { get; set; }

    public long AFTERNOON_ID { get; set; }

    public int ORDERS { get; set; }

    public bool? IS_OFF { get; set; }

    public string? NOTE { get; set; }

    public bool IS_ACTIVE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    //public virtual AT_SYMBOL AFTERNOON { get; set; } = null!;

    //public virtual ICollection<AT_APPROVED> AT_APPROVEDs { get; set; } = new List<AT_APPROVED>();

    //public virtual ICollection<AT_REGISTER_OFF> AT_REGISTER_OFFs { get; set; } = new List<AT_REGISTER_OFF>();

    //public virtual ICollection<AT_SHIFT> AT_SHIFTs { get; set; } = new List<AT_SHIFT>();

    //public virtual ICollection<AT_TIMESHEET_DAILY> AT_TIMESHEET_DAILies { get; set; } = new List<AT_TIMESHEET_DAILY>();
}
