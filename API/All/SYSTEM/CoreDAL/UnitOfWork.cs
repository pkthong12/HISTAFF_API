using API.All.DbContexts;
using API.Entities;
using CoreDAL.Models;
using CoreDAL.Repositories;
using CoreDAL.Utilities;
using Microsoft.AspNetCore.Identity;
using RegisterServicesWithReflection.Services.Base;

namespace CoreDAL
{
    [TransientRegistration]
    public class UnitOfWork : IUnitOfWork
    {
        readonly CoreDbContext _context;
        public CoreDbContext DataContext { get { return _context; } }
        readonly RefreshTokenContext _refreshTokenContext;
        readonly UserManager<SysUser> _userManager;
        readonly IRefreshTokenService _refreshTokenService;
        ISysGroupFunctionRepository _SysGroupFunctions;
        //ISysFunctionRepository _SysFunctions;
        ISysPermissionRepository _SysPermissions;
        ISysGroupPermissionRepository _SysGroupPermissions;
        ISysGroupUserRepository _SysGroupUsers;
        ISysUserRepository _SysUsers;
        ISysUserPermissionRepository _SysUserPermissionRepositorys;
        ISysOtherListRepository _SysOtherList;
        ISysModuleRepository _SysModules;
        //ITenantRepository _Tenants;
        //ITenantGroupRepository _TenantGroups;
        //ITenantGroupPermissionRepository _TenantGroupPermissions;
        //ITenantUserPermissionRepository _TenantUserPermission;
        //ITenantUserRepository _TenantUserRepository;
        IApproveProcessRepository _ApproveProcess;
        IApproveTemplateRepository _ApproveTemplates;

        public UnitOfWork(CoreDbContext context, RefreshTokenContext refreshTokenContext, UserManager<SysUser> userManager, IRefreshTokenService refreshTokenService)
        {
            _context = context;
            _refreshTokenContext = refreshTokenContext;
            _userManager = userManager;
            _refreshTokenService = refreshTokenService;
        }
        //public ITenantUserRepository TenantUserRepository
        //{
        //    get
        //    {
        //        if (_TenantUserRepository == null)
        //            _TenantUserRepository = new TenantUserRepository(_context);

        //        return _TenantUserRepository;
        //    }
        //}
        //public ITenantUserPermissionRepository TenantUserPermission
        //{
        //    get
        //    {
        //        if (_TenantUserPermission == null)
        //            _TenantUserPermission = new TenantUserPermissionRepository(_context);

        //        return _TenantUserPermission;
        //    }
        //}


        public ISysGroupUserRepository SysGroupUsers
        {
            get
            {
                if (_SysGroupUsers == null)
                    _SysGroupUsers = new SysGroupUserRepository(_context);

                return _SysGroupUsers;
            }
        }
        public ISysGroupFunctionRepository SysGroupFunctions
        {
            get
            {
                if (_SysGroupFunctions == null)
                    _SysGroupFunctions = new AspGroupFunctionRepository(_context);

                return _SysGroupFunctions;
            }
        }

        /*
        public ISysFunctionRepository SysFunctions
        {
            get
            {
                if (_SysFunctions == null)
                    _SysFunctions = new SysFunctionRepository(_context);

                return _SysFunctions;
            }
        }
        */
        public ISysPermissionRepository SysPermissions
        {
            get
            {
                if (_SysPermissions == null)
                    _SysPermissions = new SysPermissionRepository(_context);

                return _SysPermissions;
            }
        }
        public ISysGroupPermissionRepository SysGroupPermissions
        {
            get
            {
                if (_SysGroupPermissions == null)
                    _SysGroupPermissions = new SysGroupPermissionRepository(_context);

                return _SysGroupPermissions;
            }
        }
        public ISysUserRepository SysUsers
        {
            get
            {
                if (_SysUsers == null)
                    _SysUsers = new SysUserRepository(_context, _userManager, _refreshTokenService);

                return _SysUsers;
            }
        }

        public ISysUserPermissionRepository SysUserPermissions
        {
            get
            {
                if (_SysUserPermissionRepositorys == null)
                    _SysUserPermissionRepositorys = new SysUserPermissionRepository(_context);

                return _SysUserPermissionRepositorys;
            }
        }


        public ISysOtherListRepository SysOtherLists
        {
            get
            {
                if (_SysOtherList == null)
                    _SysOtherList = new SysOtherListRepository(_context);

                return _SysOtherList;
            }
        }
        //public ITenantRepository Tenants
        //{
        //    get
        //    {
        //        if (_Tenants == null)
        //            _Tenants = new TenantRepository(_context, _refreshTokenService);

        //        return _Tenants;
        //    }
        //}
        
      
        public ISysModuleRepository SysModules
        {
            get
            {
                if (_SysModules == null)
                    _SysModules = new SysModuleRepository(_context);

                return _SysModules;
            }
        }
        //public ITenantGroupRepository TenantGroups
        //{
        //    get
        //    {
        //        if (_TenantGroups == null)
        //            _TenantGroups = new TenantGroupRepository(_context);

        //        return _TenantGroups;
        //    }
        //}

        //public ITenantGroupPermissionRepository TenantGroupPermissions
        //{
        //    get
        //    {
        //        if (_TenantGroupPermissions == null)
        //            _TenantGroupPermissions = new TenantGroupPermissionRepository(_context);

        //        return _TenantGroupPermissions;
        //    }
        //}

        public IApproveProcessRepository ApproveProcess
        {
            get
            {
                if (_ApproveProcess == null)
                    _ApproveProcess = new ApproveProcessRepository(_context);

                return _ApproveProcess;
            }
        }

        public IApproveTemplateRepository ApproveTemplates
        {
            get
            {
                if (_ApproveTemplates == null)
                    _ApproveTemplates = new ApproveTemplateRepository(_context);

                return _ApproveTemplates;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
