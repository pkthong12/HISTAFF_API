using API.Main;

namespace API.DTO
{
    public class SysMutationLogDTO : BaseDTO
    {
        public string? SysFunctionCode { get; set; }
        public string? SysActionCode { get; set; }
        public string? Before { get; set; }
        public string? Before1 { get; set; }
        public string? Before2 { get; set; }
        public string? Before3 { get; set; }
        public string? After { get; set; }
        public string? After1 { get; set; }
        public string? After2 { get; set; }
        public string? After3 { get; set; }
        public string? Username { get; set; }
    }
}
