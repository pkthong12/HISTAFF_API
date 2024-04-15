using Common.Paging;

namespace CoreDAL.ViewModels
{
    public class SysGroupPermissionDTO :  Pagings
    {
        public long UserGroupId { get; set; }
        public string UseGroupName { get; set; }
        public long FunctionId { get; set; }
        public string FunctionName { get; set; }
        public long ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string PermissionStr { get; set; }
    }
    public class GroupPermissionInputDTO
    {
        public int GroupId { get; set; }
        public long FunctionId { get; set; }
        public string PermissionString { get; set; }
    }
    public class GridFunctionOutput
    {
        public long? UserGroupId { get; set; }
        public string UserId { get; set; }
        public long?  FunctionId { get; set; }
        public string FunctionCode { get; set; }
        public string FunctionName { get; set; }
        public long? GroupFuntionId { get; set; }
        public string GroupFunctionName { get; set; }
        public string ModuleName { get; set; }
        public bool? IsView { get; set; }
        public bool? IsAll { get; set; }
        public bool? IsAdd { get; set; }
        public bool? IsEdit { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsLock { get; set; }
        public bool? IsSum { get; set; }
        public bool? IsCal { get; set; }
        public bool? IsImport { get; set; }
        public bool? IsExport { get; set; }

    }
    public class GridFunctionInput : Pagings
    {
        public long? UserGroupId { get; set; }
        public string UserId { get; set; }
        public long? FunctionId { get; set; }
        public string FunctionCode { get; set; }
        public string FunctionName { get; set; }
        public long? GroupFuntionId { get; set; }
        public string GroupFunctionName { get; set; }
        public string ModuleName { get; set; }
        public bool? IsAdd { get; set; }
        public bool? IsEdit { get; set; }
        public bool? IsDelete { get; set; }

      
    }
}
