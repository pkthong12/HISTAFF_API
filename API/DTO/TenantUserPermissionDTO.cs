namespace API.DTO
{
    public class TenantUserPermissionDTO
    {
        public long? Id { get; set; }
        public long? FunctionId { get; set; }
        public long? ApplicationId { get; set; }
        public string? UserId { get; set; }

        public string? PermissionString { get; set; }
        public long? FuncActionId { get; set; }

    }
}
