using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.Main;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace API.Entities;
[Table("PA_LISTSAL")]

public partial class PA_LISTSAL : BASE_ENTITY
{
    // xóa cái này đi, [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    // mã danh mục lương
    public string? CODE_LISTSAL { get; set; }


    // tên tiếng Việt
    public string? NAME_VN { get; set; }


    // tên tiếng Anh
    public string? NAME_EN { get; set; }


    // id của kiểu dữ liệu (bảng tham chiếu SYS_OTHER_LIST)
    public long? DATA_TYPE_ID { get; set; }


    // id của danh sách ký hiệu (bảng tham chiếu SYS_OTHER_LIST)
    public long? LIST_KYHIEU_ID { get; set; }


    // thứ tự
    public int? THU_TU { get; set; }


    // trạng thái
    public bool IS_ACTIVE { get; set; }


    // ghi chú
    public string? NOTE { get; set; }


    // ngày tạo (được kế thừa)
    // public DateTime? CREATED_DATE { get; set; }


    // người tạo (được kế thừa)
    // public string? CREATED_BY { get; set; }


    // log tạo
    public string? CREATED_LOG { get; set; }


    // ngày cập nhật (được kế thừa)
    // public DateTime? UPDATED_DATE { get; set; }


    // người cập nhật (được kế thừa)
    // public string? UPDATED_BY { get; set; }


    // log cập nhật
    public string? UPDATE_LOG { get; set; }


    // trường order
    // dùng để sắp xếp
    public int? ORDER { get; set; }
}