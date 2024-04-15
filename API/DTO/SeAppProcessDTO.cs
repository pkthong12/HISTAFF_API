namespace API.DTO
{
    public class SeAppProcessDTO
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public string? Actflg { get; set; }
        public string? ProcessCode { get; set; }
        public string? Email { get; set; }
        public bool? IsSendEmail { get; set; }
        public int? Numrequest { get; set; }
    }
}
