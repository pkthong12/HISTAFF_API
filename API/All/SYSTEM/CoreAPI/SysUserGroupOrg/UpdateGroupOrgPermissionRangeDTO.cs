using API.DTO;

namespace API.All.SYSTEM.CoreAPI.SysUserGroupOrg
{
    public class UpdateGroupOrgPermissionRangeDTO
    {
        public long GroupId { get; set; }
        public required List<SysUserGroupOrgDTO> Range { get; set; }

    }
}
