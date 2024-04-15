namespace API.All.SYSTEM.CoreAPI.SysUser
{
    public class SysUserFunctionActionItemDTO
    {
        public long FunctionId { get; set; }
        public required List<long> Actions { get; set; }
    }
}
