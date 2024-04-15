using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_FILECONTRACT_IMPORT")]
[PrimaryKey(nameof(XLSX_USER_ID), nameof(XLSX_EX_CODE), nameof(XLSX_SESSION), nameof(XLSX_ROW))]
public partial class HU_FILECONTRACT_IMPORT : BASE_IMPORT
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long? ID { get; set; }
    public long? PROFILE_ID { get; set; }
    public long? EMPLOYEE_ID { get; set; }
    public long? ID_CONTRACT { get; set; }
    public long? APPEND_TYPEID { get; set; }

    public string CONTRACT_NO { get; set; } = null!;

    public DateTime? START_DATE { get; set; }

    public DateTime? EXPIRE_DATE { get; set; }

    public long? SIGN_ID { get; set; }

    public string? SIGNER_NAME { get; set; }

    public string? SIGNER_POSITION { get; set; }

    public DateTime? SIGN_DATE { get; set; }

    public string? NOTE { get; set; }

    public long? STATUS_ID { get; set; }

    public long? WORKING_ID { get; set; }

    public long? ORG_ID { get; set; }
    public long? POSITION_ID { get; set; }

}
