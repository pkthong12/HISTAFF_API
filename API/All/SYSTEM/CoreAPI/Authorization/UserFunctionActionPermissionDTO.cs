namespace API.All.SYSTEM.CoreAPI.Authorization
{
    public class FunctionActionPermissionDTO
    {
        public long FunctionId { get; set; }
        public required List<long> AllowedActionIds { get; set; }
        public required string ModuleCode { get; set; }
        public required string FunctionCode { get; set; }
        public required string FunctionUrl { get; set; }
        public required List<string> AllowedActionCodes { get; set; }
    }
}
