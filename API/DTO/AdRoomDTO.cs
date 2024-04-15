namespace API.DTO
{
    public class AdRoomDTO
    {
        public long? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }
        public int? Orders { get; set; }
        public bool? IsActive { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
