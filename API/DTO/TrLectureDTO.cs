using System.Drawing;
using API.Main;
using CORE.Services.File;

namespace API.DTO
{
    public class TrLectureDTO : BaseDTO
    {
        public long? TrCenterId { get; set; }
        public string? TeacherCode { get; set; }
        public string? TeacherName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? AddressContact { get; set; }
        public string? SupplierCode { get; set; }
        public string? SupplierName { get; set; }
        public string? Website { get; set; }
        public string? TypeOfService { get; set; }
        public string? NameOfFile { get; set; }
        public AttachmentDTO? AttachmentBuffer { get; set; }
        public bool? IsInternalTeacher { get; set; }
        public string? Note { get; set; }
        public bool? IsApply { get; set; }
        public string? TrCenterName { get; set; }
    }
}