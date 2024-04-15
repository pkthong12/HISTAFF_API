using Common.Paging;
namespace CoreDAL.ViewModels
{
    public class SysUserPermissionDTO : Pagings
    {
        public string UserId { get; set; }
        public string UseName { get; set; }
        public long FunctionId { get; set; }
        public string FunctionName { get; set; }
        public long ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string PermissionStr { get; set; }
    }
    public class UserPermissionInputDTO
    {
        public string UserId { get; set; }
        public long FunctionId { get; set; }
        public string PermissionString { get; set; }
    }
}
