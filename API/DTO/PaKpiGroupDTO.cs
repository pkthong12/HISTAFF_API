namespace API.DTO
{
    public class PaKpiGroupDTO
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public long? Status { get; set; }
        public bool? IsActive { get; set; }
        public int? Orders { get; set; }
        public string? Note { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
