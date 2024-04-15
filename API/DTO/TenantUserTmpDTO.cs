namespace API.DTO
{
    public class TenantUserTmpDTO
    {
        public long? Id { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? GroupName { get; set; }
        public bool? IsPortal { get; set; }
        public bool? IsWebapp { get; set; }
        public bool? IsExists { get; set; }
        public string? EmpCode { get; set; }
        public string? PasswordHash { get; set; }
        public string? Salt { get; set; }
    }
}
