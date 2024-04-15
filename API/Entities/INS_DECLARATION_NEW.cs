using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.Main;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;

namespace API.Entities;
[Table("INS_DECLARATION_NEW")]

public partial class INS_DECLARATION_NEW : BASE_ENTITY
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


    // trường 1:
    // ID được kế thừa


    // trường 2:
    // mã nhân viên
    public string? EMPLOYEE_CODE { get; set; }


    // trường 3:
    // họ và tên
    public string? FULL_NAME { get; set; }


    // trường 4:
    // id của phòng ban (bảng tham chiếu HU_ORGANIZATION)
    public long? ID_ORG { get; set; }


    // trường 5:
    // id của chức danh (bảng tham chiếu HU_POSITION)
    public long? ID_POSITION { get; set; }


    // trường 6:
    // id của đơn vị bảo hiểm (bảng tham chiếu SYS_OTHER_LIST)
    public long? ID_UNIT_INSURANCE { get; set; }


    // trường 7:
    // id của loại biến động (bảng tham chiếu SYS_OTHER_LIST_TYPE)
    public long? ID_TYPE_BDBH { get; set; }


    // trường 8:
    // id của nhân viên (bảng tham chiếu HU_EMPLOYEE)
    public long? ID_EMPLOYEE { get; set; }


    // trường 9:
    // ngày hiệu lực
    public DateTime? EFFECTIVE_DATE { get; set; }


    // trường 10:
    // lương BHXH - BHYT cũ
    public long? SALARY_BHXH_BHYT_OLD { get; set; }


    // trường 11:
    // lương bảo hiểm thất nghiệp cũ
    public long? SALARY_BHTN_OLD { get; set; }


    // trường 12:
    // lương BHXH - BHYT mới
    public long? SALARY_BHXH_BHYT_NEW { get; set; }


    // trường 13:
    // lương bảo hiểm thất nghiệp mới
    public long? SALARY_BHTN_NEW { get; set; }


    // trường 14:
    // BHXH (checklist)
    public bool? CHECKLIST_BHXH { get; set; }


    // trường 15:
    // BHYT (checklist)
    public bool? CHECKLIST_BHYT { get; set; }


    // trường 16:
    // BHTN (checklist)
    // bảo hiểm thất nghiệp
    public bool? CHECKLIST_BHTN { get; set; }


    // trường 17:
    // bảo hiểm tai nạn lao động - bệnh nghề nghiệp (checklist)
    public bool? CHECKLIST_BHTNLD_BNN { get; set; }


    // trường 18:
    // ghi chú
    public string? NOTE { get; set; }


    // trường 19:
    // ngày tạo (kế thừa)


    // trường 20:
    // người tạo (kế thừa)


    // trường 21:
    // nhật ký tạo (log tạo)
    public string? CREATED_LOG { get; set; }


    // trường 22:
    // ngày cập nhật (kế thừa)


    // trường 23:
    // người cập nhật (kế thừa)


    // trường 24:
    // nhật ký cập nhật (log cập nhật)
    public string? UPDATED_LOG { get; set; }
}