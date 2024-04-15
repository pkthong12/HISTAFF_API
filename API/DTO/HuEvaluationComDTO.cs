using System.Drawing;
using API.Main;

namespace API.DTO
{
    public class HuEvaluationComDTO : BaseDTO
    {
        // trường 1:
        // ID được kế thừa
        // public long? ID { get; set; }


        // trường đặc biệt:
        // đó là EMPLOYEE_ID
        public long? EmployeeId { get; set; }


        // trường 2:
        // mã nhân viên
        // dùng EMPLOYEE_ID để tham chiếu đến HU_EMPLOYEE
        // rồi lấy ra mã nhân viên
        public string? EmployeeCode { get; set; }

        // trường 3:
        // họ và tên
        // dùng EMPLOYEE_ID để tham chiếu đến HU_EMPLOYEE
        // rồi lấy ra họ và tên
        public string? FullName { get; set; }


        // trường 4:
        // năm đánh giá
        public int? YearEvaluation { get; set; }
        public string? YearEvaluationStr { get; set; }


        // trường đặc biệt
        // đó là EMPLOYEE_CV_ID
        // trường này sai cách join bảng nên cmt lại
        // public long? EmployeeCvId { get; set; }


        // trường 5:
        // chức vụ đảng
        // lấy từ bảng HU_EMPLOYEE_CV
        // Position là vị trí, chức vụ
        // Communist là người theo cộng sản
        // PositionCommunist là chức vụ của người theo cộng sản
        // public string? PositionCommunist { get; set; }


        // Thắng làm chức vụ đảng là MemberPosition
        public string? MemberPosition { get; set; }


        // trường 6:
        // chi bộ sinh hoạt
        // hiển thị theo nhân viên
        // dùng EMPLOYEE_CV_ID để tham chiếu đến EMPLOYEE_CV
        // LivingArea là khu vực sống, khu vực sinh hoạt
        //public string? LivingArea { get; set; }


        // Thắng làm chi bộ sinh hoạt là LIVING_CELL
        public string? LivingCell { get; set; }


        // trường đặc biệt
        // dùng để hiển thị ra xếp loại đánh giá
        // CLASSIFICATION_ID bigint,
        public long? ClassificationId { get; set; }


        // trường 7:
        // xếp loại đánh giá
        // danh mục xếp loại đánh giá(HU_CLASSIFICATION)
        // Category là xếp loại, phân loại
        // Evaluation là sự đánh giá
        // EvaluationCategory là xếp loại đánh giá
        public string? EvaluationCategory { get; set; }


        // trường 8:
        // điểm đánh giá
        // nhập số
        public int? PointEvaluation { get; set; }
        public string? PointEvaluationStr { get; set; }


        // trường 9:
        // ghi chú
        public string? Note { get; set; }


        // trường 10:
        // ngày tạo
        // được kế thừa
        // public DateTime? CreatedDate { get; set; }


        // trường 11:
        // người tạo (được tạo bởi)
        // được kế thừa
        // public string? CreatedBy { get; set; }


        // trường 12:
        // nhật ký tạo (log tạo)
        public string? CreatedLog { get; set; }


        // trường 13:
        // ngày cập nhật
        // được kế thừa
        // public DateTime? UpdatedDate { get; set; }


        // trường 14:
        // người cập nhật (được cập nhật bởi)
        // được kế thừa
        // public string? UpdatedBy { get; set; }


        // trường 15:
        // nhật ký cập nhật (log cập nhật)
        public string? UpdatedLog { get; set; }


        // thêm org ID
        // để load ra cái bảng bên trái ở ngoài giao diện
        // nó như kiểu lọc các nhân viên theo tổ chức, phòng ban ấy
        public long? OrgId { get; set; }


        // thêm trường name
        // để load cho cái Drop Down List
        // xếp loại đánh giá
        public string? Name { get; set; }


        // kiểm tra đảng viên
        public bool? IsMember { get; set; }


        // lấy ra điểm từ "POINT_FROM"
        public long? PointFrom { get; set; }


        // lấy ra điểm đến "POINT_TO"
        public long? PointTo { get; set; }


        // có thể cái này
        // giúp fix bug không tìm được nhân viên theo trạng thái
        // kiểu trạng thái đang làm việc hay đã nghỉ việc ấy
        public long? WorkStatusId { get; set; }
        public int? JobOrderNum { get; set; }
    }
}