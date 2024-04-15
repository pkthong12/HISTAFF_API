using API.All.SYSTEM.CoreAPI.Authorization.SysFunction;

namespace API.All.SYSTEM.CoreAPI.SysFunctionAction
{
    public class UpdateSysFunctionActionDTO
    {
        public long FunctionId { get; set; }
        public List<string> Codes { get; set; } = null!;
    }
}
