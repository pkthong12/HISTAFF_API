using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_CERTIFICATE")]
public partial class HU_CERTIFICATE
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }
    public long? EMPLOYEE_ID { get; set; }
    public long? PROFILE_ID { get; set; }
    public bool? IS_PRIME { get; set; }
    public long? TYPE_CERTIFICATE { get; set; }
    public string? NAME { get; set; }
    public DateTime? EFFECT_FROM { get; set; }

    public DateTime? EFFECT_TO { get; set; }
    public DateTime? TRAIN_FROM_DATE { get; set; }
    public DateTime? TRAIN_TO_DATE { get; set; }
	public string? LEVEL { get; set; }
	public long? LEVEL_ID { get; set; }
	public string? MAJOR { get; set; }
    public long? LEVEL_TRAIN { get; set; }
    public string? CONTENT_TRAIN { get; set; }
    public long? SCHOOL_ID { get; set; }
    public int? YEAR { get; set; }
    public decimal? MARK { get; set; }
    public long? TYPE_TRAIN { get; set; }
    public string? CODE_CETIFICATE { get; set; }
    public string? CLASSIFICATION { get; set; }
    public string? FILE_NAME { get; set; }
    public string? REMARK { get; set; }
    public DateTime? CREATED_DATE { get; set; }
    public string? CREATED_BY { get; set; }
    public string? CREATED_LOG { get; set; }
    public DateTime? MODIFIED_DATE { get; set; }
    public string? MODIFIED_BY { get; set; }
    public string? MODIFIED_LOG { get; set; }



    // trường này
    // để load ra giao diện portal
    // là "đã phê duyệt"
    // nếu Admin thêm bằng tay
    // thì nó sẽ là null
    public string? STATUS_RECORD { get; set; }
}
