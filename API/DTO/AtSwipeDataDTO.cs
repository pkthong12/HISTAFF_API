using API.Main;

namespace API.DTO
{
    public class AtSwipeDataDTO : BaseDTO
    {
        public string? ItimeId { get;set; }
        public DateTime? ValTime { get;set; }
        public string? TimeOnly { get; set; }
        public int? TerminalId { get;set; }
        public string? CreateLog { get; set; }
        public string? UpdateLog { get; set; }
        public long? OrgId { get; set; }
        public DateTime? WorkingDay { get; set; }
        public int? Evt { get; set; }
        public int? IsGps { get; set; }
        public int? AddressId { get; set; }
        public string? LatatuideLongtatuide { get;set; }
        public string? EmplName { get; set; }
        public string? EmplCode { get; set; }
        public string? PosName { get;set; }
        public int? ShiftId { get;set; }
        public string? ShiftCode { get; set; }
        public string? TerminalCode { get; set; }
        public string? AddressPlace { get;set; }
        //
        public long? EmpId { get; set; }
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
        public bool? IsPortal { get; set; }
        public int? JobOrderNum { get; set; }

    }
}
