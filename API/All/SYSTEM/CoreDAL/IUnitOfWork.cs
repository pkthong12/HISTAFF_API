using API.All.DbContexts;

namespace CoreDAL.Repositories
{
    public interface IUnitOfWork
    {
        CoreDbContext DataContext { get; }
        Task<int> SaveChangesAsync();
        ISysGroupUserRepository SysGroupUsers { get; }
        ISysGroupFunctionRepository SysGroupFunctions { get; }
        //ISysFunctionRepository SysFunctions { get; }
        ISysPermissionRepository SysPermissions { get; }
        ISysGroupPermissionRepository SysGroupPermissions { get; }
        ISysUserRepository SysUsers { get; }
        ISysUserPermissionRepository SysUserPermissions { get; }
        ISysOtherListRepository SysOtherLists { get; }
        ISysModuleRepository SysModules { get; }
        //ITenantRepository Tenants { get; }
        //ITenantGroupRepository TenantGroups { get; }
        //ITenantGroupPermissionRepository TenantGroupPermissions { get; }
        //ITenantUserPermissionRepository TenantUserPermission { get; }
        //ITenantUserRepository TenantUserRepository { get; }
        IApproveProcessRepository ApproveProcess { get; }
        IApproveTemplateRepository ApproveTemplates { get; }
    }
}
