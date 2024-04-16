using API.Main;

namespace API.All.SYSTEM.CoreAPI.SysUser
{
    public class SysUserCreateUpdateRequest
    {
        public string? Id { get; set; }
        public required string UserName { get; set; }
        public required string Fullname { get; set; }
        public long GroupId { get; set; }
        public string? Password { get; set; }
        public string? PasswordConfirm { get; set; }
        public bool IsPortal { get; set; }
        public bool IsWebapp { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public long? EmpId { get; set; }
        public long? EmployeeId { get; set; }
        public string? Avatar { get; set; }
        public string? AvatarFileData { get; set; }
        public string? AvatarFileName { get; set; }
        public string? AvatarFileType { get; set; }
        public long? TestOrgId { get; set; }
        public SysMutationLogBeforeAfterRequest? SysMutationLogBeforeAfterRequest { get; set; }
        public List<string>? ActualFormDeclaredFields { get; set; }

    }

}
