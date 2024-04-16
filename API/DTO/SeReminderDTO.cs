using API.Main;
using Common.Extensions;

namespace API.DTO
{
    public class SeReminderDTO:BaseDTO
    {
        public long? SysOtherlistId { get; set; }
        public int? Value { get; set; }

        // Extra
        public string? Code { get; set; }
        public bool? IsActive { get; set; }
    }
    public class SeReminderPushDTO
    {
         public string? Name { get; set; }
        public string? Code { get; set; }
        public int? Day { get; set; }
        public int Count { get; set; }
        public string? State { get; set; }
        public List<ReminderParam> Value { get; set; }
    }
}
