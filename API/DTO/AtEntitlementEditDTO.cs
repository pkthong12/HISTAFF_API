namespace API.DTO
{
    public class AtEntitlementEditDTO
    {
        public long? Id { get; set; }
        public long? EmployeeId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public decimal? NumberChange { get; set; }
        public string? Note { get; set; }
        public string? Code { get; set; }
        public string? CodeRef { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
