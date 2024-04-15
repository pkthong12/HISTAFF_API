using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.Main;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using static System.Runtime.InteropServices.JavaScript.JSType;

// bảng quản lý đánh giá Đảng viên hàng năm

namespace API.Entities;
[Table("HU_EVALUATION_COM")]


// chú ý:
// khi select
// thì phải where là "đảng viên" IS NOT NULL
// tức là bản ghi nào bị null đảng viên thì không cho hiển thị


public partial class HU_EVALUATION_COM : BASE_ENTITY
{
    // phải bỏ dòng code này
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    /*
        vì đã kế thừa BASE_ENTITY
        nên viết [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        thì sẽ gây ra lỗi toàn hệ thống

        cách khắc phục:
        xóa code đó đi

        
        cái dòng code
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        có tác dụng khi ánh xạ sang CSDL
        thì nó sẽ tự động tăng
        nó như kiểu
        ID int identity(1, 1) đấy
    */

    // trường 1:
    // ID được kế thừa
    // public long? ID { get; set; }


    // trường đặc biệt:
    // đó là EMPLOYEE_ID
    public long? EMPLOYEE_ID { get; set; }


    // trường 2:
    // mã nhân viên
    // dùng EMPLOYEE_ID để tham chiếu đến HU_EMPLOYEE
    // rồi lấy ra mã nhân viên


    // trường 3:
    // họ và tên
    // dùng EMPLOYEE_ID để tham chiếu đến ID của HU_EMPLOYEE
    // sau đó dùng PROFILE_ID để tham chiếu đến ID của HU_EMPLOYEE_CV
    // rồi lấy ra họ và tên


    // trường 4:
    // năm đánh giá
    public int? YEAR_EVALUATION { get; set; }


    // trường đặc biệt
    // đó là EMPLOYEE_CV_ID
    // trường này sai cách join bảng nên cmt lại
    // public long? EMPLOYEE_CV_ID { get; set; }


    // trường 5:
    // chức vụ đảng
    // dùng EMPLOYEE_ID để tham chiếu đến ID của HU_EMPLOYEE
    // sau đó dùng PROFILE_ID (của bảng HU_EMPLOYEE) để tham chiếu đến ID của HU_EMPLOYEE_CV
    // rồi lấy ra MEMBER_POSITION (của bảng HU_EMPLOYEE_CV)


    // trường 6:
    // chi bộ sinh hoạt
    // hiển thị theo nhân viên
    // dùng EMPLOYEE_ID để tham chiếu đến ID của HU_EMPLOYEE
    // sau đó dùng PROFILE_ID (của bảng HU_EMPLOYEE) để tham chiếu đến ID của HU_EMPLOYEE_CV
    // rồi lấy ra LIVING_CELL (của bảng HU_EMPLOYEE_CV)


    // trường đặc biệt
    // dùng để hiển thị ra xếp loại đánh giá
    // CLASSIFICATION_ID bigint,
    public long? CLASSIFICATION_ID { get; set; }


    // trường 7:
    // xếp loại đánh giá
    // danh mục xếp loại đánh giá(HU_CLASSIFICATION)


    // trường 8:
    // điểm đánh giá
    // nhập số
    public int? POINT_EVALUATION { get; set; }


    // trường 9:
    // ghi chú
    public string? NOTE { get; set; }


    // trường 10:
	// ngày tạo
    // được kế thừa
    // public DateTime? CREATED_DATE { get; set; }


	// trường 11:
	// người tạo (được tạo bởi)
    // được kế thừa
    // public string? CREATED_BY { get; set; }


	// trường 12:
	// nhật ký tạo (log tạo)
    public string? CREATED_LOG { get; set; }


    // trường 13:
    // ngày cập nhật
    // được kế thừa
    // public DateTime? UPDATED_DATE { get; set; }


	// trường 14:
	// người cập nhật (được cập nhật bởi)
    // được kế thừa
    // public string? UPDATED_BY { get; set; }


	// trường 15:
	// nhật ký cập nhật (log cập nhật)
    public string? UPDATED_LOG { get; set; }
    public long? PROFILE_ID  { get; set; }
}