using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.InsInformation
{
    public interface IInsInformationRepository : IGenericRepository<INS_INFORMATION, InsInformationDTO>
    {
        Task<GenericPhaseTwoListResponse<InsInformationDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsInformationDTO> request);
        Task<FormatedResponse> GetInforById(long id);
        Task<FormatedResponse> GetBhxhStatus();
        Task<FormatedResponse> GetBhYtStatus();
        Task<FormatedResponse> GetInsWhereHealth();
        Task<FormatedResponse> GetBhxhStatusById(long id);
        Task<FormatedResponse> GetBhYtStatusById(long id);
        Task<FormatedResponse> GetInsWhereHealthById(long id);
        Task<FormatedResponse> GetLstInsCheck(long id);
    }
}

