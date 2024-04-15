namespace API.DTO
{
    public class SysUserPermissionDTO
    {
        public long? Id { get; set; }
        public string? UserId { get; set; }
        public long? FunctionId { get; set; }
        public string? PermissionString { get; set; }

    }
}
