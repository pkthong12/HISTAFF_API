using API.Main;

namespace API.DTO
{
    public class SysUserFunctionActionDTO: BaseDTO
    {
        public string? UserId { get; set; }
        public long? FunctionId { get; set; }
        public long? ActionId { get; set; }

        // More
        public string? ModuleCode { get; set; }
        public string? FunctionCode { get; set; }
        public string? FunctionPath { get; set; }
        public string? ActionCode { get; set; }
    }
}
