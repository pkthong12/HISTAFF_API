namespace API.DTO
{
    public class AtTimeTypeDTO
    {
        public long? Id { get; set; }
        public string? Code { get; set; }
        public long? MorningId { get; set; }
        public long? AfternoonId { get; set; }
        public bool? IsOff { get; set; }
        public bool? IsFullday { get; set; }
        public int? Orders { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
