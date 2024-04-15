namespace API.DTO
{
    public class SeAppTemplateDtlDTO
    {
        public long? Id { get; set; }
        public long? TemplateId { get; set; }
        public long? AppId { get; set; }
        public long? TitleId { get; set; }

        public int? AppType { get; set; }
        public int? AppLevel { get; set; }
        public int? InformDate { get; set; }
        public string? InformEmail { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedLog { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedLog { get; set; }
        public string? NodeView { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
