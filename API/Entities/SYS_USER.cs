using System.ComponentModel.DataAnnotations.Schema;
using API.Main;

namespace API.Entities;
[Table("SYS_USER")]
public partial class SYS_USER
{
    public string ID { get; set; } = null!;

    public string? DISCRIMINATOR { get; set; }
    //[ForeignKey("SysGroupUser")]
    public long? GROUP_ID { get; set; }

    public string? FULLNAME { get; set; }

    public bool IS_ADMIN { get; set; }

    //ROOT user can modify the App Configuration such as ButtonGroup for each Screen, ect.
    public bool? IS_ROOT { get; set; }

    public string? AVATAR { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public string? USERNAME { get; set; }

    public string? NORMALIZEDUSERNAME { get; set; }

    public string? EMAIL { get; set; }

    public string? NORMALIZEDEMAIL { get; set; }

    public bool EMAILCONFIRMED { get; set; }
    public bool IS_LOCK { get; set; }
    public string? PASSWORDHASH { get; set; }

    public string? SECURITYSTAMP { get; set; }

    public string? CONCURRENCYSTAMP { get; set; }

    public string? PHONENUMBER { get; set; }

    public bool PHONENUMBERCONFIRMED { get; set; }

    public bool TWOFACTORENABLED { get; set; }

    public DateTimeOffset? LOCKOUTEND { get; set; }

    public bool LOCKOUTENABLED { get; set; }

    public int ACCESSFAILEDCOUNT { get; set; }
    public string? SHORT_LIVED_TOKEN { get; set; }
    public bool IS_FIRST_LOGIN { get; set; }
    public bool? IS_PORTAL { get; set; }

    public bool? IS_WEBAPP { get; set; }
    public bool? IS_LDAP { get; set; }
    public long? EMPLOYEE_ID { get; set; }

    //public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    //public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    //public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; } = new List<AspNetUserToken>();

    //public virtual SYS_GROUP? GROUP { get; set; }

    //public virtual ICollection<SYS_USER_PERMISSION> SYS_USER_PERMISSIONs { get; set; } = new List<SYS_USER_PERMISSION>();

    //public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();
}
