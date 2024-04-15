using API.Main;

namespace API.DTO
{
    public class SeReminderSeenDTO : BaseDTO
    {
        public long? RefId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Avatar { get; set; }
    }
}
