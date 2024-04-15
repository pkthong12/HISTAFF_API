using API.DTO;
using API.Entities.PORTAL;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.PortalRoute
{
    public interface IPortalRouteRepository : IGenericRepository<PORTAL_ROUTE, PortalRouteDTO>
    {
        Task<GenericPhaseTwoListResponse<PortalRouteDTO>> SinglePhaseQueryList(GenericQueryListDTO<PortalRouteDTO> request);
        Task<FormatedResponse> ReadAllMini();
    }
}

