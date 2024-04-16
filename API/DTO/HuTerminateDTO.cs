namespace API.DTO
{
    public class HuTerminateDTO
    {
        public long? Id { get; set; }
        public long? EmployeeId { get; set; }
        public long? SignId { get; set; }
        public long? StatusId { get; set; }
        public long? ReasonId { get; set; }
        public DateTime? EffectDate { get; set; }
        public DateTime? SendDate { get; set; }
        public DateTime? LastDate { get; set; }
        public DateTime? SignDate { get; set; }
        public string TerReason { get; set; }
        public string SignerName { get; set; }
        public string SignerPosition { get; set; }
        public string DecisionNo { get; set; }
        public decimal? AmountViolations { get; set; }
        public decimal? TrainingCosts { get; set; }
        public decimal? OtherCompensation { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsBlackList { get; set; }
    }
}
