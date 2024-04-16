namespace API.All.SYSTEM.Common.Middleware
{
    public class ApiCustomRequestResponseClass
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string? ResponseCode { get; set; }
        public string? OldRequestBody { get; set; }
        public string? ResponseMessage { get; set; }
    }
}
