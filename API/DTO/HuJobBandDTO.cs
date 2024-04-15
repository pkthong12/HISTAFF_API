namespace API.DTO
{
    public class HuJobBandDTO
    {
        public long? Id { get; set; }
        public long? TitleGroupId { get; set; }
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string LevelFrom { get; set; }
        public string LevelTo { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedLog { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedLog { get; set; }
        public int? Other { get; set; }

    }
}
