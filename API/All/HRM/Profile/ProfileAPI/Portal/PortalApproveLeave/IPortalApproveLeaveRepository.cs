using API.DTO;
using API.DTO.PortalDTO;
using API.Entities.PORTAL;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Profile.ProfileAPI.Portal.PortalApproveLeave
{
    public interface IPortalApproveLeaveRepository : IGenericRepository<PORTAL_REGISTER_OFF, PortalApproveLeaveDTO>
    {
        Task<GenericPhaseTwoListResponse<PortalApproveLeaveDTO>> SinglePhaseQueryList(GenericQueryListDTO<PortalApproveLeaveDTO> request);
        Task<FormatedResponse> Approve(GenericUnitOfWork _uow, PortalApproveLeaveDTO dto, string sid, bool patchMode = true);
        Task<FormatedResponse> GetById(string id, PortalApproveLeaveDTO model);
        Task<FormatedResponse> GetByIdVer2(string id, PortalApproveLeaveDTO model);
        Task<FormatedResponse> GetById(long id);
        Task<FormatedResponse> ApproveHistory(string id, DateTime fromDate, DateTime toDate);
        Task<FormatedResponse> GetPortalApproveById(long id);
    }
}

