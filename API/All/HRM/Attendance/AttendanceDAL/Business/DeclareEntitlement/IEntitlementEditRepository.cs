using AttendanceDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace AttendanceDAL.Repositories
{
    public interface IEntitlementEditRepository : IRepository<AT_ENTITLEMENT_EDIT>
    {

        Task<PagedResult<EntitlementEditDTO>> GetAll(EntitlementEditDTO param);
        Task<ResultWithError> CreateAsync(EntitlementEditInputDTO param);
        Task<ResultWithError> UpdateAsync(EntitlementEditInputDTO param);
        Task<ResultWithError> Delete(List<long> ids);
        Task<ResultWithError> GetBy(long id);
        Task<ResultWithError> ExportTemplate(ParaOrg param);
        Task<ResultWithError> ImportTemplate(EntitlementEditParam param);
        Task<ResultWithError> PortalEntilmentGet();
    }
}
