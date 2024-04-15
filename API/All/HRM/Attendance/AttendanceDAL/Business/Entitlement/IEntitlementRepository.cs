using AttendanceDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;
using CORE.DTO;

namespace AttendanceDAL.Repositories
{
    public interface IEntitlementRepository : IRepository<AT_ENTITLEMENT>
    {
        Task<GenericPhaseTwoListResponse<AtEntitlementDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtEntitlementDTO> request);
        Task<ResultWithError> Calculate(AtEntitlementInputDTO request);
    }
}
