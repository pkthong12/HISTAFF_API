using API.Main;

namespace API.DTO
{
    public class SysFunctionActionDTO: BaseDTO
    {
        public long? FunctionId { get; set; }
        public long? ActionId { get; set; }

        // More
        public string? ActionCode { get; set; }
        public string? FunctionCode { get; set; }
    }
}
