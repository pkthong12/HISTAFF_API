using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_CERTIFICATE_EDIT")]
public partial class HU_CERTIFICATE_EDIT
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }
    public long? EMPLOYEE_ID { get; set; }
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


    public bool? IS_SEND_PORTAL { get; set;}
    public bool? IS_APPROVE_PORTAL { get; set; }
    public string? MODEL_CHANGE { get; set; }
    public long? ID_HU_CERTIFICATE { get; set; }
    public long? STATUS_ID { get; set; }


    // trường này để lưu trạng thái của bản ghi
    public string? STATUS_RECORD { get; set; }



    // trạng thái
    // id: 993, code: CD, name: Chờ phê duyệt, type id: 65
    // id: 994, code: DD, name: Đã phê duyệt, type id: 65
    // id: 995, code: TC, name: Không phê duyệt, type id: 65
    public long? ID_SYS_OTHER_LIST_APPROVE { get; set; }



    // bản ghi đã lưu trong Portal
    public bool? IS_SAVE_PORTAL { get; set;}


    // lý do
    public string? REASON {  get; set; }
}
