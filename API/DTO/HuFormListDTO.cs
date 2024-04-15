namespace API.DTO
{
    public class HuFormListDTO
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public int? IdMap { get; set; }
        public long? ParentId { get; set; }
        public long? TypeId { get; set; }
        public long? TenantId { get; set; }
        public long? IdOrigin { get; set; }
        public string? Text { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
