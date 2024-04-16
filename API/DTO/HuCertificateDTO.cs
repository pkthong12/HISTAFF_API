using API.Main;
using Common.Paging;
using CORE.Services.File;

namespace API.DTO
{
    public class HuCertificateDTO : BaseDTO
	{
        public long? EmployeeId { get; set; }

		public string? EmployeeCode { get; set; }
		public string? EmployeeFullName { get; set; }
		public string? OrgName { get; set; }
		public string? TypeCertificateName { get; set; }

		public string? TitleName { get; set; }
		public string? LevelTrainName { get; set; }
		public string? TypeTrainName { get; set; }


		public long? OrgId { get; set; }
		public bool? IsPrime { get; set; }
        public long? TypeCertificate { get; set; }
        public string? Name { get; set; }
        public DateTime? EffectFrom { get; set; }
        public DateTime? EffectTo { get; set; }
        public DateTime? TrainFromDate { get; set; }
        public DateTime? TrainToDate { get; set; }
        public string? Major { get; set; }
        public long? LevelTrain { get; set; }
        public string? ContentTrain { get; set; }
		public long? SchoolId { get; set; }
		public string? SchoolName { get; set; }
        public int? Year { get; set; }
        public string? YearStr { get; set; }
        public decimal? Mark { get; set; }
        public string? MarkStr { get; set; }
        public long? TypeTrain { get; set; }
        public string? CodeCertificate { get; set; }
        public string? Classification { get; set; }
        public string? FileName { get; set; }
        public string? Remark { get; set; }
        public string? CreatedLog { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedLog { get; set; }
        public bool? IsLicense { get; set; }
        public string? Level { get; set; }
		public long? LevelId { get; set; }
		public string? LevelName { get; set; }
		public string? IsPrimeStr { get; set; }
		public AttachmentDTO? FirstAttachmentBuffer { get; set; }

        public long? WorkStatusId { get; set; }


        // trường này
        // để load ra giao diện portal
        // là "đã phê duyệt"
        // nếu Admin thêm bằng tay
        // thì nó sẽ là null
        public string? StatusRecord { get; set; }
        public int? JobOderNum { get; set; } 
    }
}
