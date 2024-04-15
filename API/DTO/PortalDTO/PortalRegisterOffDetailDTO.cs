namespace API.DTO.PortalDTO
{
    public class PortalRegisterOffDetailDTO
    {
        public long Id { get; set; }
        public string? IdReggroup { get; set; }
        public long? ShiftId { get; set; }
        public DateTime? LeaveDate { get; set; }
        public long? RegisterId { get; set; }
        public long? ManualId { get; set; }
        public long? TypeOff { get; set; }
        public decimal? NumberDay { get; set; }
    }
}
