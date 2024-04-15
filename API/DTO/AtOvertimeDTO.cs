using CORE.Services.File;

namespace API.DTO
{
    public class AtOvertimeDTO
    {
        public long? Id { get; set; }
        public long? EmployeeId { get; set; }
        public long? PeriodId { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public int? JobOrderNum { get; set; }
        public string? TimeStartStr { get; set; }
        public string? TimeEndStr { get; set; }
        public long? StatusID { get; set; }
        public bool? IsActive { get; set; }
        public string? Note { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Reason { get; set; }
        public string? FileName { get; set; }
        public long? OrgId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? OrgName { get; set; }
        public string? PositionName { get; set; }

        public long[]? EmployeeIds { get; set; }

        public AttachmentDTO? FirstAttachmentBuffer { get; set; }

    }
}
