using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.InsRegimesMng
{
    public interface IInsRegimesMngRepository : IGenericRepository<INS_REGIMES_MNG, InsRegimesMngDTO>
    {
        Task<GenericPhaseTwoListResponse<InsRegimesMngDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsRegimesMngDTO> request);
        Task<FormatedResponse> GetRegimes();
        Task<FormatedResponse> GetAllGroup();
        Task<FormatedResponse> GetRegimesByGroupId(long id);
        Task<FormatedResponse> GetInforByEmployeeId(long id);
        Task<FormatedResponse> SpsTienCheDo(InsRegimesMngDTO dto);
        Task<FormatedResponse> GetAccumulateDay(InsRegimesMngDTO dto);
        Task<FormatedResponse> SpsTienBH6TH(long? empId, string? date);

    }
}

