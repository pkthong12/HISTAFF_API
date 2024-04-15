using System.Drawing;
using API.Main;

namespace API.DTO
{
    public class InsDeclarationNewDTO : BaseDTO
    {
        // trường 1:
        // ID được kế thừa


        // trường 2:
        // mã nhân viên
        public string? EmplyeeCode { get; set; }


        // trường 3:
        // họ và tên
        public string? FullName { get; set; }


        // trường 4:
        // id của phòng ban (bảng tham chiếu HU_ORGANIZATION)
        public long? IdOrg { get; set; }


        // trường 4.5:
        // dùng để in ra tên phòng ban
        public string? OrgName { get; set; }


        // trường 5:
        // id của chức danh (bảng tham chiếu HU_POSITION)
        public long? IdPosition { get; set; }


        // trường 5.5:
        // dùng để in ra chức danh
        public string? PositionName { get; set; }


        // trường 6:
        // id của đơn vị bảo hiểm (bảng tham chiếu SYS_OTHER_LIST)
        public long? IdUnitInsurance { get; set; }


        // trường 6.5:
        // dùng để in ra tên đơn vị bảo hiểm
        public string? UnitInsuranceName { get; set; }


        // trường 7:
        // id của loại biến động (bảng tham chiếu SYS_OTHER_LIST)
        public long? IdTypeBdbh { get; set; }


        // trường 7.5:
        // dùng để in ra tên của cái loại biến động bảo hiểm
        public string? TypeBdbhName { get; set; }


        // trường 8:
        // id của nhân viên (bảng tham chiếu HU_EMPLOYEE)
        public long? IdEmployee { get; set; }


        // trường 8.5:
        // dùng để in ra số của cái sổ bảo hiểm
        public string? InsuranceNumber { get; set; }


        // trường 9:
        // ngày hiệu lực
        public DateTime? EffectiveDate { get; set; }


        // trường 10:
        // lương BHXH - BHYT cũ
        public long? SalaryBhxhBhytOld { get; set; }


        // trường 11:
        // lương bảo hiểm thất nghiệp cũ
        public long? SalaryBhtnOld { get; set; }


        // trường 12:
        // lương BHXH - BHYT mới
        public long? SalaryBhxhBhytNew { get; set; }


        // trường 13:
        // lương bảo hiểm thất nghiệp mới
        public long? SalaryBhtnNew { get; set; }


        // trường 14:
        // BHXH (checklist)
        public bool? ChecklistBhxh { get; set; }


        // trường 15:
        // BHYT (checklist)
        public bool? ChecklistBhyt { get; set; }


        // trường 16:
        // BHTN (checklist)
        // bảo hiểm thất nghiệp
        public bool? ChecklistBhtn { get; set; }


        // trường 17:
        // bảo hiểm tai nạn lao động - bệnh nghề nghiệp (checklist)
        public bool? ChecklistBhtnldBnn { get; set; }


        // trường 18:
        // ghi chú
        public string? Note { get; set; }


        // trường 19:
        // ngày tạo (kế thừa)


        // trường 20:
        // người tạo (kế thừa)


        // trường 21:
        // nhật ký tạo (log tạo)
        public string? CreatedLog { get; set; }


        // trường 22:
        // ngày cập nhật (kế thừa)


        // trường 23:
        // người cập nhật (kế thừa)


        // trường 24:
        // nhật ký cập nhật (log cập nhật)
        public string? UpdatedLog { get; set; }
    }
}