using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_FILECONTRACT")]
public partial class HU_FILECONTRACT
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long? ID { get; set; }

    public long? EMPLOYEE_ID { get; set; }
    public long? PROFILE_ID { get; set; }
    public long? ID_CONTRACT { get; set; }
    public long? APPEND_TYPEID { get; set; }

    public string? CONTRACT_NO { get; set; } = null!;

    public DateTime? START_DATE { get; set; }

    public DateTime? EXPIRE_DATE { get; set; }

    public long? SIGN_ID { get; set; }
    public long? SIGN_PROFILE_ID { get; set; }

    public string? SIGNER_NAME { get; set; }

    public string? SIGNER_POSITION { get; set; }

    public DateTime? SIGN_DATE { get; set; }

    public decimal? SAL_BASIC { get; set; }

    public decimal? SAL_PERCENT { get; set; }

    public string? NOTE { get; set; }

    public long? STATUS_ID { get; set; }

    public long? WORKING_ID { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

     public long? ORG_ID { get; set; }
     public long? POSITION_ID { get; set; }
    public string? UPLOAD_FILE { get; set; }

}
