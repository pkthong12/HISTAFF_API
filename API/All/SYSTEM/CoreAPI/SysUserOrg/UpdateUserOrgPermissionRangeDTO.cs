using API.DTO;

namespace API.All.SYSTEM.CoreAPI.SysUserOrg
{
    public class UpdateUserOrgPermissionRangeDTO
    {
        public required string UserId { get; set; }
        public required List<SysUserOrgDTO> Range { get; set; }

    }
}
