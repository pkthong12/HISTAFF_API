namespace API.Main
{
    public class SysMutationLogBeforeAfterRequest
    {
        public required string SysFunctionCode { get; set; }
        public required string SysActionCode { get; set; }
        public required string Before { get; set; }
        public required string After { get; set; }
        public required string Username { get; set; }
    }
}
