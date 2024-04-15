using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.TrCenter
{
    public interface ITrCenterRepository : IGenericRepository<TR_CENTER, TrCenterDTO>
    {
        Task<GenericPhaseTwoListResponse<TrCenterDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrCenterDTO> request);
        Task<FormatedResponse> CreateNewCode();
    }
}

