using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.InsRegimes
{
    public interface IInsRegimesRepository : IGenericRepository<INS_REGIMES, InsRegimesDTO>
    {
        Task<GenericPhaseTwoListResponse<InsRegimesDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsRegimesDTO> request);
        Task<FormatedResponse> GetInsGroup();
        Task<FormatedResponse> CreateNewCode();
        Task<FormatedResponse> GetCalDateType();
        Task<FormatedResponse> GetCalDateTypeById(long id);
    }
}

