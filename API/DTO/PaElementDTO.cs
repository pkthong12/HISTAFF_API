namespace API.DTO
{
    public class PaElementDTO
    {
        public long? Id { get; set; }
        public long? GroupId { get; set; }
        public long? DataType { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsSystem { get; set; }
        public bool? IsActive { get; set; }

        public int? Orders { get; set; }

        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
