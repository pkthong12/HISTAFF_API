using Common.Paging;
using CORE.Services.File;
using System.ComponentModel.DataAnnotations;

namespace ProfileDAL.ViewModels
{
    public class DisciplineDTO
    {
        public long? Id { get; set; }
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        public string? PositionName { get; set; }
        public long? DecisionType { get; set; }
        public string? DecisionTypeName { get; set; }
        public string? DecisionNo { get; set; }
        public DateTime? EffectDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public long? EmpStatusId { get; set; }
        public long? StatusId { get; set; }
        public string? StatusName { get; set; }
        public DateTime? IssuedDate { get; set; }
        public DateTime? ViolatedDate { get; set; }
        public string? BasedOn { get; set; }
        public DateTime? DocumentSignDate { get; set; }
        public long? SignId { get; set; }
        public string? SignName { get; set; }
        public string? SignPosition { get; set; }
        public DateTime? SignDate { get; set; }
        public long? DisciplineObj { get; set; }
        public string? DisciplineObjName { get; set; }
        public long? DisciplineType { get; set; }
        public string? DisciplineTypeName { get; set; }
        public string? Reason { get; set; }
        public long? ExtendSalTime { get; set; }
        public long[]? EmployeeIds { get; set; }
        
        public string? Attachment { get; set; }
        public string? Note { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public AttachmentDTO? AttachmentBuffer { get; set; }
        public int? JobOrderNum { get; set; }
    }
}
