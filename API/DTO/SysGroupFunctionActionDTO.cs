using API.Main;

namespace API.DTO
{
    public class SysGroupFunctionActionDTO: BaseDTO
    {
        public long? GroupId { get; set; }
        public long? FunctionId { get; set; }
        public long? ActionId { get; set; }
        // More
        public string? ModuleCode { get; set; }
        public string? FunctionCode { get; set; }
        public string? FunctionUrl { get; set; }
        public string? ActionCode { get; set; }
    }
}
