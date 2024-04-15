namespace API.DTO
{
    public class HuClassificationDTO
    {
        public long? Id { get; set; }
        public string? Code { get; set; }
        public string? StatusName { get; set; }
        public long? ClassificationType { get; set; }
        public string? ClassificationTypeName { get; set; }
        public long? ClassificationLevel { get; set; }
        public long? PointFrom { get; set; }
        public long? PointTo { get; set; }
        public string? ClassificationLevelName { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
