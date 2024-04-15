using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.Main;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using static System.Runtime.InteropServices.JavaScript.JSType;

// bảng quản lý cấu hình chung

namespace API.Entities;
[Table("SYS_CONFIGURATION_COMMON")]

public partial class SYS_CONFIGURATION_COMMON : BASE_ENTITY
{
    // trường 0:
    // ID được kế thừa


    // trường 1:
    // số lần login tối đa
    public int? YOUR_MAXIMUM_TURN { get; set; }


    // trường 2:
    // cổng thông tin
    public int? PORTAL_PORT { get; set; }


    // trường 3:
    // cổng ứng dụng
    public int? APPLICATION_PORT { get; set; }


    // trường 4:
    // độ dài tối thiểu trong password
    public int? MINIMUM_LENGTH { get; set; }


    // trường 5:
    // ký tự chữ hoa
    public bool? IS_UPPERCASE { get; set; }


    // trường 6:
    // ký tự số
    public bool? IS_NUMBER { get; set; }


    // trường 7:
    // chữ thường
    public bool? IS_LOWERCASE { get; set; }


    // trường 8:
    // ký tự đặc biệt
    public bool? IS_SPECIAL_CHAR { get; set; }


    // trường 9:
    // ngày tạo
    // được kế thừa
    // public DateTime? CREATED_DATE { get; set; }


    // trường 10:
    // người tạo (được tạo bởi)
    // được kế thừa
    // public string? CREATED_BY { get; set; }


    // trường 11:
    // nhật ký tạo (log tạo)
    public string? CREATED_LOG { get; set; }


    // trường 12:
    // ngày cập nhật
    // được kế thừa
    // public DateTime? UPDATED_DATE { get; set; }


    // trường 13:
    // người cập nhật (được cập nhật bởi)
    // được kế thừa
    // public string? UPDATED_BY { get; set; }


    // trường 14:
    // nhật ký cập nhật (log cập nhật)
    public string? UPDATED_LOG { get; set; }
}