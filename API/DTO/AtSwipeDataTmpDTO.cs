namespace API.DTO
{
    public class AtSwipeDataTmpDTO
    {
        public long? Id { get; set; }
        public long? TenantId { get; set; }
        public string? RefCode { get; set; }
        public long? EmpId { get; set; }
        public string? ItimeId { get; set; }
        public DateTime? TimePoint { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? Model { get; set; }
        public string? Image { get; set; }
        public string? Mac { get; set; }
        public string? OperatingSystem { get; set; }
        public string? OperatingVersion { get; set; }
        public string? WifiIp { get; set; }
        public string? BssId { get; set; }
    }
}
