using API.DTO;

namespace API.All.SYSTEM.CoreAPI.SysUserFunctionAction
{
    public class UpdateUserFunctionActionPermissionRangeDTO
    {
        public required string UserId {  get; set; }
        public required List<SysUserFunctionActionDTO> Range { get; set; }

    }
}
