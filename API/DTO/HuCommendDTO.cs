using API.Main;
using CORE.Services.File;

namespace API.DTO
{
    public class HuCommendDTO:BaseDTO
    {
        public long? SignId { get; set; }
        public long? OrgId { get; set; }
        public long? CommendObjId { get; set; }
        public long? SourceCostId { get; set; }
        public DateTime? EffectDate { get; set; }
        public DateTime? SignDate { get; set; }
        public string? No { get; set; }
        public string? SignerName { get; set; }
        public string? SignerPosition { get; set; }
        public string? CommendType { get; set; }
        public string? Reason { get; set; }
        public long? StatusId { get; set; }
        public double? Money { get; set; }
        public bool? IsTax { get; set; }
        public long? PeriodId { get; set; }
        public int? Year { get; set; }
        public string? Content { get; set; }
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }

        public string? NumRewardPay { get; set; }
        public string? OrgName { get; set; }
        public string? PositionName { get; set; }
        public string? CommendObjName { get; set; }
        public string? StatusName { get; set; }
        public string? SourceCostName { get; set; }
        public int? SalaryIncreaseTime { get; set; }
        public long? OrgLevelId { get; set; }
        public long? AwardTitleId { get; set; }
        public string? Note { get; set; }
        public DateTime? SignPaymentDate { get; set; }

        public string? PaymentNo { get; set; }
        public long? StatusPaymentId { get; set; }
        public string? StatusPaymentName { get; set; }
        public long? SignPaymentId { get; set; }
        public string? PositionPaymentName { get; set; }
        public long? FundSourceId { get; set; }
        public string? FundSourceName { get; set; }
        public long? RewardId { get; set; }
        public string? RewardName { get; set; }
        public long? RewardLevelId { get; set; }
        public string? RewardLevelName { get; set; }
        public int? MonthTax { get; set; }
        public string? Attachment { get; set; }
        public AttachmentDTO? AttachmentBuffer { get; set; }
        public List<long>? EmployeeIds { get; set; }
        public string? OrgLevelName { get; set; }
        public long? CommendId { get; set; }
        public string? SignPaymentName { get; set; }
        public string? PaymentContent { get; set; }
        public string? PaymentAttachment { get; set; }
        public string? PaymentNote { get; set; }
        public AttachmentDTO? PaymentAttachmentBuffer { get; set; }
        public string? AwardTitleName { get; set; }
        public string? ListRewardLevelId { get; set; }
        public int? JobOrderNum { get; set; }



        // tạo checklist cấp chi thưởng
        public List<int>? CheckListRewardLevel { get; set; }
    }
}
