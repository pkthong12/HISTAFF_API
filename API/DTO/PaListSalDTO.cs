using System.Drawing;
using API.Main;

namespace API.DTO
{
    public class PaListSalDTO : BaseDTO
    {
        public string? CodeListsal { get; set; }
        public string? NameVn { get; set; }
        public string? NameEn { get; set; }
        public long? DataTypeId { get; set; }
        public long? ListKyhieuId { get; set; }
        public int? ThuTu { get; set; }
        public bool IsActive { get; set; }
        public string? Note { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdateLog { get; set; }


        // cái này để load ra view chữ "Áp dụng"
        public string? IsActiveStr { get; set; }


        // cái này để load ra view trường "Tên kiểu dữ liệu"
        public string? DataTypeName { get; set; }


        // cái này để load ra view trường "Tên ký hiệu"
        public string? ListKyHieuName { get; set; }


        public int? Order { get; set; }
    }
}
