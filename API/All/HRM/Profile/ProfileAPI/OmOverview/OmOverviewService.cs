using API.All.DbContexts;
using CORE.DTO;
using RegisterServicesWithReflection.Services.Base;

namespace API.All.HRM.Profile.ProfileAPI.OmOverview
{
    [ScopedRegistration]
    public class OmOverviewService: IOmOverviewService
    {

        private readonly FullDbContext _fullDbContext;

        public OmOverviewService(FullDbContext fullDbContext)
        {
            _fullDbContext = fullDbContext;
        }

        public async Task<FormatedResponse> OrganizationOverview(OmOverviewRequest request)
        {
            var people = await (


                
                
                            from v in _fullDbContext.HuEmployeeCvs

                         join e in _fullDbContext.HuEmployees on v.ID equals e.PROFILE_ID into ve
                         from veResult in ve.DefaultIfEmpty()

                         join p in _fullDbContext.HuPositions  on veResult.POSITION_ID equals p.ID into vep
                         from vepResult in vep.DefaultIfEmpty()

                         where vepResult.ORG_ID != null && request.OrgIds.Contains((long)vepResult.ORG_ID)

                         select vepResult).ToListAsync();

            return new() { InnerBody = people };

        }
    }
}
