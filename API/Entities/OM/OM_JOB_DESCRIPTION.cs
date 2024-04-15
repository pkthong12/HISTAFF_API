using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities.OM;
[Table("OM_JOB_DESCRIPTION")]
public partial class OM_JOB_DESCRIPTION
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string? JOB_TARGET_1 { get; set; }

    public string? JOB_TARGET_2 { get; set; }

    public string? JOB_TARGET_3 { get; set; }

    public string? JOB_TARGET_4 { get; set; }

    public string? JOB_TARGET_5 { get; set; }

    public string? JOB_TARGET_6 { get; set; }

    public int? TDCM { get; set; }

    public string? MAJOR { get; set; }

    public string? WORK_EXP { get; set; }

    public int? LANGUAGE { get; set; }

    public int? COMPUTER { get; set; }

    public string? SUPPORT_SKILL { get; set; }

    public string? INTERNAL_1 { get; set; }

    public string? INTERNAL_2 { get; set; }

    public string? INTERNAL_3 { get; set; }

    public string? OUTSIDE_1 { get; set; }

    public string? OUTSIDE_2 { get; set; }

    public string? OUTSIDE_3 { get; set; }

    public string? RESPONSIBILITY_1 { get; set; }

    public string? RESPONSIBILITY_2 { get; set; }

    public string? RESPONSIBILITY_3 { get; set; }

    public string? RESPONSIBILITY_4 { get; set; }

    public string? RESPONSIBILITY_5 { get; set; }

    public string? DETAIL_RESPONSIBILITY_1 { get; set; }

    public string? DETAIL_RESPONSIBILITY_2 { get; set; }

    public string? DETAIL_RESPONSIBILITY_3 { get; set; }

    public string? DETAIL_RESPONSIBILITY_4 { get; set; }

    public string? DETAIL_RESPONSIBILITY_5 { get; set; }

    public string? OUT_RESULT_1 { get; set; }

    public string? OUT_RESULT_2 { get; set; }

    public string? OUT_RESULT_3 { get; set; }

    public string? OUT_RESULT_4 { get; set; }

    public string? OUT_RESULT_5 { get; set; }

    public string? PERMISSION_1 { get; set; }

    public string? PERMISSION_2 { get; set; }

    public string? PERMISSION_3 { get; set; }

    public string? PERMISSION_4 { get; set; }

    public string? PERMISSION_5 { get; set; }

    public string? PERMISSION_6 { get; set; }

    public string? FILE_NAME { get; set; }

    public long? POSITION_ID { get; set; }

    public string? UPLOAD_FILE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public virtual HU_POSITION? POSITION { get; set; }
}
