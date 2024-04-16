namespace API.DTO
{
    public class HuDisciplineDTO
    {
        public long? Id { get; set; }
        public long? TenantId { get; set; }
        public long? EmployeeId { get; set; }
        public long? OrgId { get; set; }
        public long? PositionId { get; set; }
        public long? DisciplineObjId { get; set; }
        public long? SignId { get; set; }
        public long? PeriodId { get; set; }
        public long? StatusId { get; set; }
        public DateTime? EffectDate { get; set; }
        public DateTime? SignDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? SignerName { get; set; }
        public string? No { get; set; }
        public string? SignerPosition { get; set; }
        public string? DisciplineType { get; set; }
        public string? Reason { get; set; }
        public float? Money { get; set; }
        public bool? IsSalary { get; set; }
        public int? Year { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
