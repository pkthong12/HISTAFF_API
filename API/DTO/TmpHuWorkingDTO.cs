namespace API.DTO
{
    public class TmpHuWorkingDTO
    {
        public long? Id { get; set; }
        public string? RefCode { get; set; }
        public string? Code { get; set; }
        public string? PosName { get; set; }

        public long? EmployeeId { get; set; }
        public long? PositionId { get; set; }
        public long? OrgId { get; set; }
        public long? TypeId { get; set; }

        public DateTime? EffectDate { get; set; }
        public DateTime? ExpireDate { get; set; }

        public string? DecisionNo { get; set; }
        public string? TypeName { get; set; }
        public string? SalaryTypeName { get; set; }
        public long? SalaryTypeId { get; set; }
        public long? SalaryLevelId { get; set; }
        public long? StatusId { get; set; }

        public decimal? SalBasic { get; set; }
        public decimal? Coefficient { get; set; }
        public decimal? SalTotal { get; set; }
        public decimal? SalPercent { get; set; }

        public string? StatusName { get; set; }
        public string? SignerName { get; set; }
        public string? SignerPosition { get; set; }
        public DateTime? SignDate { get; set; }
    }
}
