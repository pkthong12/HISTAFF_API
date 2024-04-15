namespace API.DTO
{
    public class HuAllowanceDTO
    {
        public long? Id { get; set; }
        public string? Code { get; set; }
        public string? ColName { get; set; }
        public string? Name { get; set; }
        public long? TypeId { get; set; }
        public bool? IsInsurance { get; set; }
        public bool? IsActive { get; set; }
        public string? Note { get; set; }
        public bool? IsFullday { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? SalaryRankId { get; set; }
    }
}
