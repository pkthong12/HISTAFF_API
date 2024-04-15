namespace API.DTO
{
    public class SysSettingMapDTO
    {
        public long? Id { get; set; }
        public long? OrgId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Lat { get; set; }
        public string? Lng { get; set; }
        public string? Ip { get; set; }
        public decimal? Radius { get; set; }
        public int? Zoom { get; set; }
        public string? Center { get; set; }
        public string? Bssid { get; set; }
        public string? Qrcode { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
