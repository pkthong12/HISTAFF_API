using API.Main;

namespace API.DTO
{
    public class AtTerminalDTO : BaseDTO
    {
        public string? TerminalCode { get; set; }

        public string? TerminalName { get; set; }

        public string? AddressPlace { get; set; }

        public string? TerminalPort { get; set; }

        public string? TerminalIp { get; set; }

        public string? Pass { get; set; }

        public bool? IsActive { get; set; }

        public string? Status { get; set; }

        public string? Note { get; set; }

        public DateTime? LastTimeStatus { get; set; }
        public string? TerminalStatus { get; set; }
        public long? TerminalRow { get; set; }
        public DateTime? LastTimeUpdate { get; set; }

    }
}
