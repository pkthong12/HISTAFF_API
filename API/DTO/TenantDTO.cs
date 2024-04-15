namespace API.DTO
{
    public class TenantDTO
    {
        public long? Id { get; set; }
        public string? Code { get; set; }
        public string? TenancyName { get; set; }
        public string? OwnerName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Qrcode { get; set; }
        public string? Email { get; set; }
        public string? Logo { get; set; }
        public string? ConnectionString { get; set; }
        public string? UserRef { get; set; }
        public string? CodeEmp { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DateExpire { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? AreaId { get; set; }
        public bool? IsActive { get; set; }
    }
}
