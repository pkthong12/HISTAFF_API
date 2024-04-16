using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Profile.ProfileAPI.HuBlacklist
{
    public interface IHuBlacklistRepository : IGenericRepository<HU_BLACKLIST, HuBlacklistDTO>
    {
        Task<GenericPhaseTwoListResponse<HuBlacklistDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuBlacklistDTO> request);
    }
}