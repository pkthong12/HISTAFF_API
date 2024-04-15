using API.DTO;
using API.DTO.PortalDTO;
using API.Entities.PORTAL;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Profile.ProfileAPI.Portal.PortalRegisterOff
{
    public interface IPortalRegisterOffRepository : IGenericRepository<PORTAL_REGISTER_OFF, PortalRegisterOffDTO>
    {
        Task<GenericPhaseTwoListResponse<PortalRegisterOffDTO>> SinglePhaseQueryList(GenericQueryListDTO<PortalRegisterOffDTO> request);
        Task<FormatedResponse> GetTotalOtMonth(string id);
        Task<FormatedResponse> GetLeaveDay(string id);
        Task<FormatedResponse> RegisterHistory(string id, DateTime fromDate, DateTime toDate);
        Task<FormatedResponse> GetRegisterHistoryById(long id);
        Task<FormatedResponse> RegisterOff(GenericUnitOfWork _uow, DynamicDTO dto, string sid);
        Task<FormatedResponse> GetRegisterById(long id);

    }

}

