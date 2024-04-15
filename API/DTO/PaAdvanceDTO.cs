namespace API.DTO
{
    public class PaAdvanceDTO
    {
        public long? Id { get; set; }
        public long? EmployeeId { get; set; }
        public long? PeriodId { get; set; }
        public long? StatusId { get; set; }
        public long? SignId { get; set; }
        public int? Year { get; set; }
        public int? Money { get; set; }
        public DateTime? AdvanceDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? SignDate { get; set; }

        public string? SignerName { get; set; }
        public string? SignerPosition { get; set; }
        public string? Note { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
