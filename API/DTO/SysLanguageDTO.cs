namespace API.DTO
{
    public class SysLanguageDTO
    {
        public long? Id { get; set; }
        public string? Key { get; set; }
        public string? Vi { get; set; }
        public string? En { get; set; }
        public string? Note { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
