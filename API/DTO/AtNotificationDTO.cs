namespace API.DTO
{
    public class AtNotificationDTO
    {
        public long? Id { get; set; }
        public long? Type { get; set; }
        public long? Action { get; set; }
        public long? NotifiId { get; set; }
        public long? EmpCreateId { get; set; }
        public string? FmcToken { get; set; }
        public bool? IsRead { get; set; }
        public long? TenantId { get; set; }
        public string? Title { get; set; }
        public long? EmpNotifyId { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
