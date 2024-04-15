namespace API.DTO
{
    public class PaAdvanceTmpDTO
    {
        public long? Id { get; set; }
        public long? EmployeeId { get; set; }
        public long? StatusId { get; set; }
        public string? CodeRef { get; set; }
        public string? EmployeeCode { get; set; }
        public string? SignerName { get; set; }
        public string? SignerPosition { get; set; }
        public string? Note { get; set; }
        public string? StatusName { get; set; }
        public decimal? Money { get; set; }
        public DateTime? AdvanceDate { get; set; }
        public DateTime? SignDate { get; set; }

    }
}
