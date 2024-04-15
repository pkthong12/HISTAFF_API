namespace API.DTO
{
    public class SysModuleDTO
    {
        public long? Id { get; set; }
        public long? ApplicationId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Note { get; set; }

        public int? Orders { get; set; }
        public decimal? Price { get; set; }
        public bool? IsActive { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
