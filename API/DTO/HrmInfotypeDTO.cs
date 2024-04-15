namespace API.DTO
{
    public class HrmInfotypeDTO
    {
        public long? Id { get; set; }
        public string? Code { get; set; }
        public string? NameCode { get; set; }
        public string? NameEn { get; set; }
        public string? NameVn { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
