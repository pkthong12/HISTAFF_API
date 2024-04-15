using API.Main;

namespace API.DTO
{
    public class SysUserOrgDTO: BaseDTO
    {
        public string? UserId { get; set; }
        public long? OrgId { get; set; }
    }
}
