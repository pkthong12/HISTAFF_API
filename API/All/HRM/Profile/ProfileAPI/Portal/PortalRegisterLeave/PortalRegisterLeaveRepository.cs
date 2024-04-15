using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using CoreDAL.Repositories;
using Microsoft.Extensions.Options;
using CORE.Services.File;
using API.All.HRM.Profile.ProfileAPI.HuOrganization;
using API.All.Services;

namespace API.Controllers.PortalRegisterLeave
{
    public class PortalRegisterLeaveRepository : IPortalRegisterLeaveRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;

        private readonly API.Controllers.SysUser.SysUserRepository _sysUserRepository;


        public PortalRegisterLeaveRepository(FullDbContext context, GenericUnitOfWork uow, IWebHostEnvironment env, IOptions<AppSettings> options, IFileService fileService, IEmailService emailService)
        {
            _dbContext = context;
            _uow = uow;
            _sysUserRepository = new(context, uow, env, options, fileService, emailService);
        }

        public async Task<FormatedResponse> WillLeaveInNextSevenDay(string sid)
        {

            try
            {

                var user = _dbContext.SysUsers.Single(x => x.ID == sid);

                var orgPermission = ((List<HuOrganizationMinimumDTO>)_sysUserRepository.QueryOrgPermissionList(user).Result.InnerBody!).AsQueryable()
                    .Where(x => x.Protected != true).Select(x => new { x.Id }).ToList();

                var list = await (from l in _dbContext.AtRegisterLeaves.AsNoTracking()
                            from e in _dbContext.HuEmployees.AsNoTracking().Where(e => e.ID == l.EMPLOYEE_ID).DefaultIfEmpty()
                            from cv in _dbContext.HuEmployeeCvs.AsNoTracking().Where(c => c.ID == e.PROFILE_ID).DefaultIfEmpty()
                            from p in _dbContext.HuPositions.AsNoTracking().Where(p => p.ID == e.POSITION_ID).DefaultIfEmpty()
                            where l.DATE_START!.Value.AddDays(-7) <= DateTime.UtcNow && l.DATE_START >= DateTime.UtcNow && cv.FULL_NAME != null

                            select new
                            {
                                EmployeeId = l.ID,
                                OrgId = p.ORG_ID,
                                FullName = cv.FULL_NAME,
                                Avatar = cv.AVATAR,
                                ComingDate = l.DATE_START,
                                ProfileId = cv.ID
                            }).ToListAsync();
                var listInRange = list.Where(l => orgPermission.Any(x => x.Id == l.OrgId));

                return new() { InnerBody = listInRange };

            } catch (Exception ex)
            {

                return new() { ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message, StatusCode = EnumStatusCode.StatusCode500 };

            }

        }

    }
}

