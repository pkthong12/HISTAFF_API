using Common.Paging;
using CORE.GenericUOW;
using CORE.Services.File;
using InsuranceDAL.Repositories;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.ComponentModel.DataAnnotations;

namespace ProfileDAL.ViewModels
{
    public class TerminateDTO : Pagings
    {
        public long Id { get; set; }
        public long? TenantID { get; set; }
        public long? EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string PositionName { get; set; }
        public string ContractNo { get; set; }
        public string ContractTypeName { get; set; }
        public long? OrgId { get; set; }
        public string OrgName { get; set; }
        public string TerReason { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime? EffectDate { get; set; }
        public DateTime? SendDate { get; set; }
        public DateTime? LastDate { get; set; }
        public long? SignId { get; set; }
        public string SignerName { get; set; }
        public string SignerPosition { get; set; }
        public DateTime? SignDate { get; set; }
        public long? StatusId { get; set; }
        public string DecisionNo { get; set; }
        public string StatusName { get; set; }
        public long? ReasonId { get; set; }
        public long? TypeId { get; set; }
        public bool? IsCalSeveranceAllowance { get; set; }
        public decimal? AvgSalSixMo { get; set; }
        public decimal? SeveranceAllowance { get; set; }
        public decimal? PaymentRemainingDay { get; set; }
        public string? NoticeNo { get; set; }
        public string? Attachment { get; set; }
        public string? Seniority { get; set; }
        public AttachmentDTO? AttachmentBuffer { get; set; }
        public DateTime? JoinDate { get; set; }
        public string? FileName { get; set; }
        public AttachmentDTO? FileBuffer { get; set; }
        public int? JobOrderNum { get; set; }
    }

    public class TerminateView
    {
        public long? Id { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string PositionName { get; set; }
        public string ContractNo { get; set; }
        public long? OrgId { get; set; }
        public string OrgName { get; set; }
        public string TerReason { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime? EffectDate { get; set; }
        public string SignerName { get; set; }
        public string SignerPosition { get; set; }
        public DateTime? SignDate { get; set; }
        public long? StatusId { get; set; }
        public string DecisionNo { get; set; }
        public string StatusName { get; set; }
        public long? ReasonId { get; set; }
    }
    public class TerminateInputDTO
    {
        public long? Id { get; set; }
      
        public long? TenantID { get; set; }
        public long? EmployeeId { get; set; }
        public DateTime? EffectDate { get; set; }
        public DateTime? SendDate { get; set; }
        public DateTime? LastDate { get; set; }
        public string? TerReason { get; set; }
        public long? SignId { get; set; }
        public string? SignerName { get; set; }
        public string? SignerPosition { get; set; }
        public DateTime? SignDate { get; set; }
        public long? StatusId { get; set; }
        public string? DecisionNo { get; set; }
        public long? ReasonId { get; set; }
        public long? TypeId { get; set; }
        public bool? IsCalSeveranceAllowance { get; set; }
        public decimal? AvgSalSixMo { get; set; }
        public decimal? SeveranceAllowance { get; set; }
        public decimal? PaymentRemainingDay { get; set; }
        public string? NoticeNo { get; set; }
        public string? Attachment { get; set; }
        public AttachmentDTO? AttachmentBuffer { get; set; }
        public List<long?>? Ids { get; set; }
        public bool? ValueToBind { get; set; }
        public string? FileName { get; set; }
        public AttachmentDTO? FileBuffer { get; set; }
    }

}
