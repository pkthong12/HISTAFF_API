using API.All.SYSTEM.CoreAPI.Authorization;
using API.Entities;

namespace CoreDAL.Models
{
    public class AuthResponse: LoginOutput
    {
        public SYS_REFRESH_TOKEN RefreshToken { get; set; }
        public bool? IsFirstLogin { get; set; }
        public bool? IsLock { get; set; }
        public bool? IsWebapp { get; set; }
        public bool? IsPortal { get; set; }
        public bool? IsMobile { get; set; }
        public bool? IsLdap { get; set; }
        public long? EmployeeId { get; set; }
    }
    public class LoginParam
    {
        public SYS_REFRESH_TOKEN RefreshToken { get; set; }

        public string Id { get; set; }
        public long TenantId { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string Avatar { get; set; }
        public long EmpId { get; set; }
        public bool IsFirstLogin { get; set; }

        public string Token { get; set; }
        public bool? IsAdmin { get; set; }       
        public List<PermissionParam> PermissionParams { get; set; }

    }

    public class LoginOutput
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string Token { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsRoot { get; set; }
        public dynamic OrgIds { get; set; }
        /* PermissionParams depricated, use PermissionActions instead */
        //public List<PermissionParam>? PermissionParams { get; set; }
        public List<FunctionActionPermissionDTO>? PermissionActions { get; set; }
    }

    public class PermissionParam
    {
        public string ModuleCode { get; set; }
        public string GroupFuncCode { get; set; }
        public string FunctionCode { get; set; }
        public string PermissionString { get; set; }
        public string Url { get; set; }
    }
    public class OrgTreeView
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? pid { get; set; }
        public string pName { get; set; }
        public string orgManager { get; set; }
        public bool hasChild { get; set; }
    }
}
