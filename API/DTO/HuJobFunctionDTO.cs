namespace API.DTO
{
    public class HuJobFunctionDTO
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public string? NameEn { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedLog { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedLog { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public long? ParentId { get; set; }
        public long? JobId { get; set; }
        public long? FunctionId { get; set; }
    }
}
