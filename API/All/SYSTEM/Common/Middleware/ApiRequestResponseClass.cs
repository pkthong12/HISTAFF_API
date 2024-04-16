using CORE.DTO;

namespace API.All.SYSTEM.Common.Middleware
{
    public class ApiRequestResponseClass
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public object? Request { get; set; }
        public FormatedResponse? Response { get; set; }
    }
}
