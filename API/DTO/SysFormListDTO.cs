namespace API.DTO
{
    public class SysFormListDTO
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public long? IdMap { get; set; }
        public long? ParentId { get; set; }
        public long? TypeId { get; set; }
        public long? IdOrigin { get; set; }
        public string? Text { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public long? CreatedDate { get; set; }
        public long? UpdatedDate { get; set; }
    }
}
