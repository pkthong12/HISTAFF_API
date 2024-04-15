using CORE.Services.File;
using ProfileDAL.ViewModels;

namespace API.DTO
{
    public class AtRegisterLeaveDTO
    {
        public long? Id { get; set; }
        public long? EmployeeId { get; set; }
        public long? OrgId { get; set; }
        public string? EmployeeCode { get; set; }

        public string? EmployeeName { get; set; }
        public int? JobOrderNum { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public long? TypeId { get; set; }
        public long? TypeOff { get; set; }
        public bool? IsEachDay { get; set; }
        public string? TypeName { get; set; }
        public string? TypeOffName { get; set; }
        public string? FileName { get; set; }

        public string? OrgName { get; set; }
        public string? PositionName { get; set; }
        public string? Reason { get; set; }
        public string? Note { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? PeriodId { get; set; }

        public long[]? EmployeeIds { get; set; }

        public AttachmentDTO? FirstAttachmentBuffer { get; set; }
    }
}
