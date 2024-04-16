using API.All.DbContexts;
using CORE.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using RegisterServicesWithReflection.Services.Base;

namespace API.All.HRM.Profile.ProfileAPI.Portal.PortalAtEntitlement
{
    [ScopedRegistration]
    public class PortalAtEntitlement : IPortalAtEntitlement
    {
        private readonly FullDbContext _fullDbContext;

        public PortalAtEntitlement(FullDbContext fullDbContext)
        {
            _fullDbContext = fullDbContext;
        }

        public async Task<FormatedResponse> GetEntitlement(string sid)
        {
            // innerBody return type: PortalAtEntitlementOutputDTO
            try
            {

                var now = DateTime.UtcNow;
                var user = await _fullDbContext.SysUsers.AsNoTracking().SingleAsync(x => x.ID == sid);
                var employeeId = user?.EMPLOYEE_ID;

                var ets = _fullDbContext.AtEntitlements.AsNoTracking().Where(et => et.YEAR == now.Year && et.MONTH == now.Month && et.EMPLOYEE_ID == employeeId);

                if (ets != null)
                {

                    if (ets.Count() == 1)
                    {

                        var r = from et in ets
                                from e in _fullDbContext.HuEmployees.AsNoTracking().Where(e => e.ID == et.EMPLOYEE_ID).DefaultIfEmpty()
                                from ol in _fullDbContext.SysOtherLists.AsNoTracking().Where(ol => ol.ID == e.WORK_STATUS_ID).DefaultIfEmpty()
                                from lt in _fullDbContext.SysOtherListTypes.AsNoTracking().Where(lt => lt.ID == ol.TYPE_ID).DefaultIfEmpty()
                                from u in _fullDbContext.SysUsers.AsNoTracking().Where(u => u.EMPLOYEE_ID == et.EMPLOYEE_ID).DefaultIfEmpty()

                                where lt.CODE == "EMP_STATUS" && ol.CODE == "ESW" && u.ID == sid && et.YEAR == now.Year && et.MONTH == now.Month

                                select new PortalAtEntitlementOutputDTO()
                                {
                                    PrevHave = et.PREV_HAVE,
                                    PrevUsed = et.PREV_USED,
                                    CurHave = et.CUR_HAVE,
                                    CurUsed = et.CUR_USED,
                                };

                        return new() { InnerBody = r };

                    }
                    else
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.NO_AT_ENTITLEMENT_RECORD_FOUND_FOR_CURRENT_EMPLOYEE_IN_CURRENT_MONTH, StatusCode = EnumStatusCode.StatusCode400 };
                    }
                }
                else
                {
                    return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.NO_AT_ENTITLEMENT_RECORD_FOUND_FOR_CURRENT_EMPLOYEE_IN_CURRENT_MONTH, StatusCode = EnumStatusCode.StatusCode400 };
                }
            }
            catch (Exception ex)
            {
                return new() { ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message, StatusCode = EnumStatusCode.StatusCode500 };
            }
        }
    }
}
