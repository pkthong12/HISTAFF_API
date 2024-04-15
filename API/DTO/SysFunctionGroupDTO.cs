namespace API.DTO
{
    public class SysFunctionGroupDTO
    {
        public long? Id { get; set; }
        public long? ApplicationId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
