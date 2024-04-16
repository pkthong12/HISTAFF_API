using API.Main;
using Common.Paging;
using CORE.Services.File;

namespace API.DTO
{
    public class HuCertificateEditDTO : BaseDTO
    {
        public long? EmployeeId { get; set; }
        public int? JobOrderNum { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeFullName { get; set; }
        public string? OrgName { get; set; }
        public string? TypeCertificateName { get; set; }

        public string? TitleName { get; set; }
        public string? LevelTrainName { get; set; }
        public string? TypeTrainName { get; set; }
        public string? PositionName { get; set; }

        public long? OrgId { get; set; }
        public bool? IsPrime { get; set; }
        public long? TypeCertificate { get; set; }
        public string? Name { get; set; }
        public DateTime? EffectFrom { get; set; }
        public DateTime? EffectTo { get; set; }
        
        public DateTime? TrainFromDate { get; set; }
        public string? TrainFromDateStr { get; set; }
        
        public DateTime? TrainToDate { get; set; }
        public string? TrainToDateStr { get; set; }

        public string? Major { get; set; }
        public long? LevelTrain { get; set; }
        public string? ContentTrain { get; set; }
        public long? SchoolId { get; set; }
        public string? SchoolName { get; set; }
        public int? Year { get; set; }
        public decimal? Mark { get; set; }
        public long? TypeTrain { get; set; }
        public string? CodeCertificate { get; set; }
        public string? Classification { get; set; }
        public string? FileName { get; set; }
        public string? Remark { get; set; }
        public string? CreatedLog { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedLog { get; set; }

        public string? Level { get; set; }
        public long? LevelId { get; set; }
        public string? LevelName { get; set; }
        public string? IsPrimeStr { get; set; }
        public AttachmentDTO? FirstAttachmentBuffer { get; set; }


        public bool? IsSendPortal { get; set; }
        public bool? IsApprovePortal { get; set; }
        public long? StatusId { get; set; }


        // ModelChange để lưu
        // các trường bị người dùng sửa
        // biết được trường người dùng sửa
        // thì mới điền dữ liệu vào bảng chính
        // sau khi phê duyệt
        public string? ModelChange {  get; set; }


        // IdHuCertificate
        // để lập trình
        // kiểm tra bản ghi này
        // là sửa hay thêm mới
        public long? IdHuCertificate { get; set; }



        // trường này để lưu trạng thái của bản ghi
        public string? StatusRecord { get; set; }



        // trạng thái
        // id: 993, code: CD, name: Chờ phê duyệt, type id: 65
        // id: 994, code: DD, name: Đã phê duyệt, type id: 65
        // id: 995, code: TC, name: Không phê duyệt, type id: 65
        public long? IdSysOtherListApprove { get; set; }


        // tên của trạng thái
        public string? StatusName { get; set; }


        // bản ghi đã lưu trong Portal
        public bool? IsSavePortal { get; set; }


        // lý do
        public string? Reason { get; set; }
    }
}
