using System.Drawing;
using API.Main;

namespace API.DTO
{
    public class SysConfigurationCommonDTO : BaseDTO
    {
        // trường 0:
        // ID được kế thừa
        // public long? ID { get; set; }


        // trường 1:
        // số lần login tối đa
        public int? YourMaximumTurn { get; set; }
        

        // trường 2:
        // cổng thông tin
        public int? PortalPort { get; set; }
        

        // trường 3:
        // cổng ứng dụng
        public int? ApplicationPort { get; set; }
        

        // trường 4:
        // độ dài tối thiểu trong password
        public int? MinimumLength { get; set; }
        

        // trường 5:
        // ký tự chữ hoa
        public bool? IsUppercase { get; set; }
        

        // trường 6:
        // ký tự số
        public bool? IsNumber { get; set; }
        

        // trường 7:
        // chữ thường
        public bool? IsLowercase { get; set; }
        

        // trường 8:
        // ký tự đặc biệt
        public bool? IsSpecialChar { get; set; }


        // trường 9:
        // ngày tạo
        // được kế thừa
        // public DateTime? CreatedDate { get; set; }


        // trường 10:
        // người tạo (được tạo bởi)
        // được kế thừa
        // public string? CreatedBy { get; set; }


        // trường 11:
        // nhật ký tạo (log tạo)
        public string? CreatedLog { get; set; }


        // trường 12:
        // ngày cập nhật
        // được kế thừa
        // public DateTime? UpdatedDate { get; set; }


        // trường 13:
        // người cập nhật (được cập nhật bởi)
        // được kế thừa
        // public string? UpdatedBy { get; set; }


        // trường 14:
        // nhật ký cập nhật (log cập nhật)
        public string? UpdatedLog { get; set; }
    }
}
