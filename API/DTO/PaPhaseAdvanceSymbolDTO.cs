using API.Main;

namespace API.DTO
{
    public class PaPhaseAdvanceSymbolDTO : BaseDTO
    {
        public long? PhaseAdvanceId { get; set; }
        public long? SymbolId { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }

    }
}
