using API.Main;

namespace API.DTO
{
    public class SysUserGroupOrgDTO: BaseDTO
    {
        public long? GroupId { get; set; }
        public long? OrgId { get; set; }
    }
}
