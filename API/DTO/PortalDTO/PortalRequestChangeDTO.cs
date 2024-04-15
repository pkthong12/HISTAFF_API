using API.Main;
using CORE.Services.File;

namespace API.DTO.PortalDTO
{
    public class PortalRequestChangeDTO : BaseDTO
    {
        public long? EmployeeId { get; set; }
        public long? IdChange { get; set; }
        public int? JobOrderNum { get; set; }
        public string? SysOtherCode { get; set; }
        public string? ContentChange { get; set; }
        public string? ReasonChange { get; set; }
        public long? IsApprove { get; set; }
        public string? IsApproveName { get; set; }
        public decimal?  SalInsu { get; set; }
        public string? FileName { get; set; }
        public AttachmentDTO? AttachmentBuffer { get; set; }

        //HU_WORKINGBEFORE
        public long? EmployeeStatus { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? PositionName { get; set; }
        public string? OrgName { get; set; }
        public string? CompanyName { get; set; }
        public string? TitleName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? MainDuty { get; set; }
        public string? TerReason { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public DateTime? UpdatedDate { get; set; }
        //public string? CreatedBy { get; set; }
        //public string? UpdatedBy { get; set; }
        public string? Seniority { get; set; }
        public long? OrgId { get; set; }


        // HU_WORKING
        public string? StatusName { get; set; }
        public string? TypeDecision { get; set; }
        public string? DecisionNo { get; set; }
        public string? EffectDateStr { get; set; }
        public string? ExpireDateStr { get; set; }
        public string? CeasePositionDateStr { get; set; }
        public string? AddressWorking { get; set; }
        public string? EmployeeObjectName { get; set; }
        public string? WorkingTimeStr { get; set; }
        public string? ReasonDiscard { get; set; }
        public string? FunctionName { get; set; }
    }
}
