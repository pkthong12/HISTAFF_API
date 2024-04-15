using API.DTO;

namespace API.All.SYSTEM.CoreAPI.SysGroupFunctionAction
{
    public class UpdateGroupFunctionActionPermissionRangeDTO
    {
        public long GroupId { get; set; }
        public required List<SysGroupFunctionActionDTO> Range { get; set; }

    }
}
